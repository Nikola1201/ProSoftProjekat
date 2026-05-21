using Common.Domain;
using Common.Domain.Izvestaji;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace DBBroker.Reports
{
    public class KandidatiSaDugovanjemReport : IReport<KandidatDugovanjeDto>
    {
        public string Sql => @"
WITH AgregatPoKandidatu AS
(
    SELECT
        k.KandidatId,
        LTRIM(RTRIM(k.Ime + ' ' + k.Prezime)) AS PunoIme,
        k.JMBG,
        SUM(p.Cena) AS UkupnaCena,
        SUM(ISNULL(pl.Placeno, 0)) AS UkupnoPlaceno
    FROM Kandidat k
    INNER JOIN Upis u ON u.KandidatId = k.KandidatId
    INNER JOIN PaketObuke p ON p.PaketId = u.PaketId
    OUTER APPLY
    (
        SELECT SUM(pla.Iznos) AS Placeno
        FROM Placanje pla
        WHERE pla.UpisId = u.UpisId
    ) pl
    GROUP BY k.KandidatId, k.Ime, k.Prezime, k.JMBG
)
SELECT
    KandidatId,
    PunoIme,
    JMBG,
    UkupnaCena,
    UkupnoPlaceno,
    CASE WHEN UkupnaCena - UkupnoPlaceno < 0 THEN 0 ELSE UkupnaCena - UkupnoPlaceno END AS Dugovanje
FROM AgregatPoKandidatu
WHERE (UkupnaCena - UkupnoPlaceno) > 0
ORDER BY Dugovanje DESC, PunoIme;";

        public IEnumerable<SqlParameter> Parameters => Array.Empty<SqlParameter>();

        public List<KandidatDugovanjeDto> Hydrate(SqlDataReader reader)
        {
            List<KandidatDugovanjeDto> result = new List<KandidatDugovanjeDto>();
            while (reader.Read())
            {
                result.Add(new KandidatDugovanjeDto
                {
                    KandidatId = Convert.ToInt32(reader["KandidatId"]),
                    PunoIme = reader["PunoIme"].ToString(),
                    JMBG = reader["JMBG"].ToString(),
                    UkupnaCena = Convert.ToDecimal(reader["UkupnaCena"]),
                    UkupnoPlaceno = Convert.ToDecimal(reader["UkupnoPlaceno"]),
                    Dugovanje = Convert.ToDecimal(reader["Dugovanje"])
                });
            }
            return result;
        }
    }
}
