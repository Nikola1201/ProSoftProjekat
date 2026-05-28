using Common.DTO.Izvestaji;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace DBBroker.Reports
{
    /// <summary>
    /// Implementacija izveštaja dugovanja kandidata za zadati period i kategoriju.
    /// Sabira cene upisa i uplate i izračunava preostalo dugovanje po kandidatu.
    /// </summary>
    public class IzvestajDugovanjaReport : IReport<IzvestajDugovanjaStavkaDto>
    {
        private readonly IzvestajDugovanjaKriterijum _kriterijum;

        /// <summary>Inicijalizuje izveštaj sa zadatim kriterijumima pretrage.</summary>
        /// <param name="kriterijum">Kriterijumi koji definišu period, kategoriju i filtere.</param>
        public IzvestajDugovanjaReport(IzvestajDugovanjaKriterijum kriterijum)
        {
            _kriterijum = kriterijum;
        }

        /// <inheritdoc/>
        public string Sql => @"
WITH DugovanjaKandidata AS
(
    SELECT
        k.KandidatId,
        k.Ime,
        k.Prezime,
        k.Jmbg,
        kat.NazivKategorije AS Kategorija,
        COUNT(DISTINCT u.UpisId) AS BrojUpisa,
        SUM(p.Cena) AS UkupnaCenaObuke,
        SUM(ISNULL(pl.Placeno, 0)) AS UkupnoPlaceno,
        MAX(pl.PoslednjaUplata) AS DatumPoslednjeUplate
    FROM Kandidat k
    INNER JOIN Upis u ON u.KandidatId = k.KandidatId
    INNER JOIN PaketObuke p ON p.PaketId = u.PaketId
    INNER JOIN Kategorija kat ON kat.KategorijaID = p.KategorijaID
    OUTER APPLY
    (
        SELECT
            SUM(pla.Iznos) AS Placeno,
            MAX(pla.DatumPlacanja) AS PoslednjaUplata
        FROM Placanje pla
        WHERE
            pla.UpisId = u.UpisId
            AND pla.DatumPlacanja < @DatumDoExclusive
    ) pl
    WHERE
        kat.NazivKategorije = @Kategorija
        AND u.DatumUpisa >= @DatumOd
        AND u.DatumUpisa < @DatumDoExclusive
        AND (@IncludeOnlyAktivanUpis = 0 OR u.Status = 'aktivan')
    GROUP BY
        k.KandidatId,
        k.Ime,
        k.Prezime,
        k.Jmbg,
        kat.NazivKategorije
)
SELECT
    KandidatId,
    Ime,
    Prezime,
    Jmbg,
    Kategorija,
    BrojUpisa,
    UkupnaCenaObuke,
    UkupnoPlaceno,
    CASE WHEN UkupnaCenaObuke - UkupnoPlaceno < 0 THEN 0 ELSE UkupnaCenaObuke - UkupnoPlaceno END AS Dugovanje,
    CASE WHEN UkupnaCenaObuke - UkupnoPlaceno <= 0 THEN 'Izmireno' ELSE 'Duguje' END AS StatusDuga,
    DatumPoslednjeUplate
FROM DugovanjaKandidata
WHERE
    @IncludeBezDuga = 1
    OR (UkupnaCenaObuke - UkupnoPlaceno) > 0
ORDER BY Prezime, Ime;";

        /// <inheritdoc/>
        public IEnumerable<SqlParameter> Parameters
        {
            get
            {
                yield return new SqlParameter("@DatumOd", _kriterijum.DatumOd.Date);
                yield return new SqlParameter("@DatumDoExclusive", _kriterijum.DatumDo.Date.AddDays(1));
                yield return new SqlParameter("@Kategorija", _kriterijum.Kategorija);
                yield return new SqlParameter("@IncludeBezDuga", _kriterijum.IncludeBezDuga ? 1 : 0);
                yield return new SqlParameter("@IncludeOnlyAktivanUpis", _kriterijum.IncludeOnlyAktivanUpis ? 1 : 0);
            }
        }

        /// <inheritdoc/>
        public List<IzvestajDugovanjaStavkaDto> Hydrate(SqlDataReader reader)
        {
            List<IzvestajDugovanjaStavkaDto> rezultat = new List<IzvestajDugovanjaStavkaDto>();
            while (reader.Read())
            {
                rezultat.Add(new IzvestajDugovanjaStavkaDto
                {
                    KandidatId = Convert.ToInt32(reader["KandidatId"]),
                    Ime = reader["Ime"].ToString(),
                    Prezime = reader["Prezime"].ToString(),
                    Jmbg = reader["Jmbg"].ToString(),
                    Kategorija = reader["Kategorija"].ToString(),
                    BrojUpisa = Convert.ToInt32(reader["BrojUpisa"]),
                    UkupnaCenaObuke = Convert.ToDecimal(reader["UkupnaCenaObuke"]),
                    UkupnoPlaceno = Convert.ToDecimal(reader["UkupnoPlaceno"]),
                    Dugovanje = Convert.ToDecimal(reader["Dugovanje"]),
                    StatusDuga = reader["StatusDuga"].ToString(),
                    DatumPoslednjeUplate = reader["DatumPoslednjeUplate"] == DBNull.Value
                        ? (DateTime?)null
                        : Convert.ToDateTime(reader["DatumPoslednjeUplate"])
                });
            }
            return rezultat;
        }
    }
}
