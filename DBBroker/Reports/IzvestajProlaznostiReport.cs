using Common.DTO.Izvestaji;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace DBBroker.Reports
{
    public class IzvestajProlaznostiReport : IReport<IzvestajProlaznostiStavkaDto>
    {
        private readonly IzvestajProlaznostiKriterijum _kriterijum;

        public IzvestajProlaznostiReport(IzvestajProlaznostiKriterijum kriterijum)
        {
            _kriterijum = kriterijum;
        }

        public string Sql => @"
WITH KandidatAgregat AS
(
    SELECT
        k.KandidatId,
        k.Ime,
        k.Prezime,
        k.Jmbg,
        kat.NazivKategorije AS Kategorija,
        SUM(CASE WHEN i.Tip = 'teorijski' THEN 1 ELSE 0 END) AS BrojPokusajaTeorijski,
        SUM(CASE WHEN i.Tip = 'prakticni' THEN 1 ELSE 0 END) AS BrojPokusajaPrakticni,
        MAX(CASE WHEN i.Tip = 'teorijski' AND i.Rezultat = 'polozio' THEN 1 ELSE 0 END) AS ImaPolozenTeorijski,
        MAX(CASE WHEN i.Tip = 'prakticni' AND i.Rezultat = 'polozio' THEN 1 ELSE 0 END) AS ImaPolozenPrakticni,
        MAX(i.DatumIspita) AS DatumPoslednjegIspita
    FROM Kandidat k
    INNER JOIN Upis u ON u.KandidatId = k.KandidatId
    INNER JOIN PaketObuke p ON p.PaketId = u.PaketId
    INNER JOIN Kategorija kat ON kat.KategorijaID = p.KategorijaID
    LEFT JOIN Ispit i
        ON i.UpisId = u.UpisId
        AND i.DatumIspita >= @DatumOd
        AND i.DatumIspita < @DatumDoExclusive
    WHERE
        kat.NazivKategorije = @Kategorija
        AND (@IncludeOnlyAktivanUpis = 0 OR u.Status = 'aktivan')
    GROUP BY
        k.KandidatId,
        k.Ime,
        k.Prezime,
        k.Jmbg,
        kat.NazivKategorije
),
KandidatStatusi AS
(
    SELECT
        KandidatId,
        Ime,
        Prezime,
        Jmbg,
        Kategorija,
        BrojPokusajaTeorijski,
        BrojPokusajaPrakticni,
        DatumPoslednjegIspita,
        CASE
            WHEN @TipIspita = 2 AND ImaPolozenTeorijski = 1 AND ImaPolozenPrakticni = 1 THEN 0
            WHEN @TipIspita = 0 AND ImaPolozenTeorijski = 1 THEN 0
            WHEN @TipIspita = 1 AND ImaPolozenPrakticni = 1 THEN 0
            WHEN
                (
                    CASE
                        WHEN @TipIspita = 0 THEN BrojPokusajaTeorijski
                        WHEN @TipIspita = 1 THEN BrojPokusajaPrakticni
                        ELSE BrojPokusajaTeorijski + BrojPokusajaPrakticni
                    END
                ) > 0 THEN 1
            ELSE 2
        END AS StatusKod
    FROM KandidatAgregat
)
SELECT
    KandidatId,
    Ime,
    Prezime,
    Jmbg,
    Kategorija,
    BrojPokusajaTeorijski,
    BrojPokusajaPrakticni,
    DatumPoslednjegIspita,
    StatusKod
FROM KandidatStatusi
WHERE @IncludeNoData = 1 OR StatusKod <> 2
ORDER BY Prezime, Ime;";

        public IEnumerable<SqlParameter> Parameters
        {
            get
            {
                yield return new SqlParameter("@DatumOd", _kriterijum.DatumOd.Date);
                yield return new SqlParameter("@DatumDoExclusive", _kriterijum.DatumDo.Date.AddDays(1));
                yield return new SqlParameter("@Kategorija", _kriterijum.Kategorija);
                yield return new SqlParameter("@TipIspita", (int)_kriterijum.TipIspita);
                yield return new SqlParameter("@IncludeNoData", _kriterijum.IncludeNoData ? 1 : 0);
                yield return new SqlParameter("@IncludeOnlyAktivanUpis", _kriterijum.IncludeOnlyAktivanUpis ? 1 : 0);
            }
        }

        public List<IzvestajProlaznostiStavkaDto> Hydrate(SqlDataReader reader)
        {
            List<IzvestajProlaznostiStavkaDto> stavke = new List<IzvestajProlaznostiStavkaDto>();
            while (reader.Read())
            {
                stavke.Add(new IzvestajProlaznostiStavkaDto
                {
                    KandidatId = Convert.ToInt32(reader["KandidatId"]),
                    Ime = reader["Ime"].ToString(),
                    Prezime = reader["Prezime"].ToString(),
                    Jmbg = reader["Jmbg"].ToString(),
                    Kategorija = reader["Kategorija"].ToString(),
                    Status = (StatusProlaznosti)Convert.ToInt32(reader["StatusKod"]),
                    DatumPoslednjegIspita = reader["DatumPoslednjegIspita"] == DBNull.Value
                        ? (DateTime?)null
                        : Convert.ToDateTime(reader["DatumPoslednjegIspita"]),
                    BrojPokusajaTeorijski = Convert.ToInt32(reader["BrojPokusajaTeorijski"]),
                    BrojPokusajaPrakticni = Convert.ToInt32(reader["BrojPokusajaPrakticni"])
                });
            }
            return stavke;
        }
    }
}
