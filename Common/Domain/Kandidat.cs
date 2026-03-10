using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Common.Domain
{
    [Serializable]
    public class Kandidat : IEntity
    {
        public int KandidatId { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string JMBG { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string Adresa { get; set; }
        public DateTime DatumUpisa { get; set; }
        public Kategorija Kategorija { get; set; }
        public bool Aktivan { get; set; }

        public string TableName => "Kandidat";

        public string Values =>
            $"'{Ime}', '{Prezime}', '{JMBG}', '{Telefon}', '{Email}', " +
            $"'{Adresa}', '{DatumUpisa:yyyy-MM-dd}', 1, '{Kategorija.KategorijaID}'";

        public object Query =>
            $"INSERT INTO Kandidat (Ime, Prezime, JMBG, Telefon, Email, Adresa, DatumUpisa, Kategorija) " +
            $"VALUES ({Values})";

        public object TableKeyColumn => "KandidatId";

        public object SearchQuery =>
            $"SELECT * FROM Kandidat WHERE Ime LIKE '%{Ime}%' OR Prezime LIKE '%{Prezime}%'";

        public object TableKeyQuery =>
            $"SELECT * FROM Kandidat WHERE KandidatId = {KandidatId}";

        public object Update =>
            $"UPDATE Kandidat SET " +
            $"Ime = '{Ime}', " +
            $"Prezime = '{Prezime}', " +
            $"JMBG = '{JMBG}', " +
            $"Telefon = '{Telefon}', " +
            $"Email = '{Email}', " +
            $"Adresa = '{Adresa}', " +
            $"DatumUpisa = '{DatumUpisa:yyyy-MM-dd}', " +
            $"Kategorija = '{Kategorija}', " +
            $"Aktivan = {(Aktivan ? 1 : 0)} " +
            $"WHERE KandidatId = {KandidatId}";

        public List<IEntity> GetReaderList(SqlDataReader reader)
        {
            var list = new List<IEntity>();
            while (reader.Read())
                list.Add(GetReaderResult(reader));
            return list;
        }

        public IEntity GetReaderResult(SqlDataReader reader)
        {
            return new Kandidat
            {
                KandidatId = (int)reader["KandidatId"],
                Ime = reader["Ime"].ToString(),
                Prezime = reader["Prezime"].ToString(),
                JMBG = reader["JMBG"].ToString(),
                Telefon = reader["Telefon"].ToString(),
                Email = reader["Email"].ToString(),
                Adresa = reader["Adresa"].ToString(),
                DatumUpisa = (DateTime)reader["DatumUpisa"],
                //Kategorija = reader["Kategorija"].ToString(),
                Aktivan = (bool)reader["Aktivan"]
            };
        }
    }
}
