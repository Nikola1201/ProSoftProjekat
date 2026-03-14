using Common.Domain;
using Common.Domain.Izvestaji;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DBBroker
{
    public class Broker
    {
        private DBConnection _connection;

        public Broker()
        {
            _connection = new DBConnection();
        }
        public void Rollback()
        {
            _connection.Rollback();
        }

        public void Commit()
        {
            _connection.Commit();
        }

        public void BeginTransaction()
        {
            _connection.BeginTransaction();
        }

        public void CloseConnection()
        {
            _connection.CloseConnection();
        }

        public void OpenConnection()
        {
            _connection.OpenConnection();
        }

        public IEntity GetEntityByID(IEntity entity)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {entity.TableName} WHERE {entity.TableKeyQuery}";
            SqlDataReader reader = command.ExecuteReader();
            entity = entity.GetReaderResult(reader);
            reader.Close();
            command.Dispose();
            return entity;
        }
        public IEntity GetEntityByQuery(IEntity entity)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {entity.TableName} WHERE {entity.Query}";
            SqlDataReader reader = command.ExecuteReader();
            entity = entity.GetReaderResult(reader);
            reader.Close();
            command.Dispose();
            return entity;
        }

        public IEntity Add(IEntity entity)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = $"INSERT INTO {entity.TableName} VALUES ({entity.Values})";
            command.ExecuteNonQuery();
            command.Dispose();
            return entity;

        }
        public List<IEntity> GetAll(IEntity entity)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {entity.TableName}";
            SqlDataReader reader = command.ExecuteReader();
            List<IEntity> entities = entity.GetReaderList(reader);
            reader.Close();
            command.Dispose();

            return entities;

        }
        public List<IEntity> GetEntitiesByQuery(IEntity entity)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {entity.TableName} WHERE {entity.Query}";
            SqlDataReader reader = command.ExecuteReader();
            List<IEntity> entities = entity.GetReaderList(reader);
            reader.Close();
            command.Dispose();

            return entities;
        }

        public void Delete(IEntity entity)
        {
            SqlCommand cmd = _connection.CreateCommand();
            cmd.CommandText = $"delete from {entity.TableName} where {entity.TableKeyQuery} ";
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        public void Update(IEntity entity)
        {
            SqlCommand cmd = _connection.CreateCommand();
            cmd.CommandText = entity.Update.ToString();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        public List<IzvestajProlaznostiStavkaDto> KreirajIzvestajProlaznosti(IzvestajProlaznostiKriterijum kriterijum)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = @"
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
                CASE
                    WHEN @TipIspita = 2 AND ImaPolozenTeorijski = 1 AND ImaPolozenPrakticni = 1 THEN 'Polozio'
                    WHEN @TipIspita = 0 AND ImaPolozenTeorijski = 1 THEN 'Polozio'
                    WHEN @TipIspita = 1 AND ImaPolozenPrakticni = 1 THEN 'Polozio'
                    WHEN
                        (
                            CASE
                                WHEN @TipIspita = 0 THEN BrojPokusajaTeorijski
                                WHEN @TipIspita = 1 THEN BrojPokusajaPrakticni
                                ELSE BrojPokusajaTeorijski + BrojPokusajaPrakticni
                            END
                        ) > 0 THEN 'Pao'
                    ELSE 'UToku'
                END AS Status
            FROM KandidatAgregat
            WHERE
                @IncludeNoData = 1
                OR
                (
                    CASE
                        WHEN @TipIspita = 2 AND ImaPolozenTeorijski = 1 AND ImaPolozenPrakticni = 1 THEN 'Polozio'
                        WHEN @TipIspita = 0 AND ImaPolozenTeorijski = 1 THEN 'Polozio'
                        WHEN @TipIspita = 1 AND ImaPolozenPrakticni = 1 THEN 'Polozio'
                        WHEN
                            (
                                CASE
                                    WHEN @TipIspita = 0 THEN BrojPokusajaTeorijski
                                    WHEN @TipIspita = 1 THEN BrojPokusajaPrakticni
                                    ELSE BrojPokusajaTeorijski + BrojPokusajaPrakticni
                                END
                            ) > 0 THEN 'Pao'
                        ELSE 'UToku'
                    END
                ) <> 'UToku'
            ORDER BY Prezime, Ime;";

            command.Parameters.AddWithValue("@DatumOd", kriterijum.DatumOd.Date);
            command.Parameters.AddWithValue("@DatumDoExclusive", kriterijum.DatumDo.Date.AddDays(1));
            command.Parameters.AddWithValue("@Kategorija", kriterijum.Kategorija);
            command.Parameters.AddWithValue("@TipIspita", (int)kriterijum.TipIspita);
            command.Parameters.AddWithValue("@IncludeNoData", kriterijum.IncludeNoData ? 1 : 0);
            command.Parameters.AddWithValue("@IncludeOnlyAktivanUpis", kriterijum.IncludeOnlyAktivanUpis ? 1 : 0);

            List<IzvestajProlaznostiStavkaDto> rezultat = new List<IzvestajProlaznostiStavkaDto>();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                rezultat.Add(new IzvestajProlaznostiStavkaDto
                {
                    KandidatId = Convert.ToInt32(reader["KandidatId"]),
                    Ime = reader["Ime"].ToString(),
                    Prezime = reader["Prezime"].ToString(),
                    Jmbg = reader["Jmbg"].ToString(),
                    Kategorija = reader["Kategorija"].ToString(),
                    Status = reader["Status"].ToString(),
                    DatumPoslednjegIspita = reader["DatumPoslednjegIspita"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["DatumPoslednjegIspita"]),
                    BrojPokusajaTeorijski = Convert.ToInt32(reader["BrojPokusajaTeorijski"]),
                    BrojPokusajaPrakticni = Convert.ToInt32(reader["BrojPokusajaPrakticni"])
                });
            }

            reader.Close();
            command.Dispose();
            return rezultat;
        }

        public List<IzvestajDugovanjaStavkaDto> KreirajIzvestajDugovanja(IzvestajDugovanjaKriterijum kriterijum)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = @"
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

            command.Parameters.AddWithValue("@DatumOd", kriterijum.DatumOd.Date);
            command.Parameters.AddWithValue("@DatumDoExclusive", kriterijum.DatumDo.Date.AddDays(1));
            command.Parameters.AddWithValue("@Kategorija", kriterijum.Kategorija);
            command.Parameters.AddWithValue("@IncludeBezDuga", kriterijum.IncludeBezDuga ? 1 : 0);
            command.Parameters.AddWithValue("@IncludeOnlyAktivanUpis", kriterijum.IncludeOnlyAktivanUpis ? 1 : 0);

            List<IzvestajDugovanjaStavkaDto> rezultat = new List<IzvestajDugovanjaStavkaDto>();
            SqlDataReader reader = command.ExecuteReader();
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
                    DatumPoslednjeUplate = reader["DatumPoslednjeUplate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["DatumPoslednjeUplate"])
                });
            }

            reader.Close();
            command.Dispose();
            return rezultat;
        }

        public bool KandidatPostoji(int kandidatId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT COUNT(1) FROM Kandidat WHERE KandidatId = @KandidatId";
            command.Parameters.AddWithValue("@KandidatId", kandidatId);

            int count = Convert.ToInt32(command.ExecuteScalar());
            command.Dispose();
            return count > 0;
        }

        public Upis GetNajnovijiUpisZaKandidata(int kandidatId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = @"
            SELECT TOP 1 UpisId, KandidatId, PaketId, DatumUpisa, Status
            FROM Upis
            WHERE KandidatId = @KandidatId
            ORDER BY DatumUpisa DESC, UpisId DESC";
            command.Parameters.AddWithValue("@KandidatId", kandidatId);

            SqlDataReader reader = command.ExecuteReader();
            Upis upis = null;

            if (reader.Read())
            {
                upis = new Upis
                {
                    UpisId = (int)reader["UpisId"],
                    KandidatId = (int)reader["KandidatId"],
                    PaketId = (int)reader["PaketId"],
                    DatumUpisa = (DateTime)reader["DatumUpisa"],
                    Status = reader["Status"].ToString()
                };
            }

            reader.Close();
            command.Dispose();
            return upis;
        }

        public bool PostojiIspitIstogTipaIstogDana(int upisId, string tip, DateTime datumIspita)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = @"
SELECT COUNT(1)
FROM Ispit
WHERE UpisId = @UpisId
  AND Tip = @Tip
  AND CAST(DatumIspita AS date) = @DatumIspita";

            command.Parameters.AddWithValue("@UpisId", upisId);
            command.Parameters.AddWithValue("@Tip", tip);
            command.Parameters.AddWithValue("@DatumIspita", datumIspita.Date);

            int count = Convert.ToInt32(command.ExecuteScalar());
            command.Dispose();
            return count > 0;
        }

        public bool ImaPolozenIspitZaTip(int upisId, string tip)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = @"
SELECT COUNT(1)
FROM Ispit
WHERE UpisId = @UpisId
  AND Tip = @Tip
  AND Rezultat = 'polozio'";

            command.Parameters.AddWithValue("@UpisId", upisId);
            command.Parameters.AddWithValue("@Tip", tip);

            int count = Convert.ToInt32(command.ExecuteScalar());
            command.Dispose();
            return count > 0;
        }

        public void AzurirajStatusUpisa(int upisId, string status)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "UPDATE Upis SET Status = @Status WHERE UpisId = @UpisId";
            command.Parameters.AddWithValue("@Status", status);
            command.Parameters.AddWithValue("@UpisId", upisId);
            command.ExecuteNonQuery();
            command.Dispose();
        }

    }
}
