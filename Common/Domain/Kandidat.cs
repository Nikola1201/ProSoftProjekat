using System;
using System.Collections.Generic;
using System.Data.Common;

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
        public bool Aktivan { get; set; }
        public string PunoIme => $"{Ime} {Prezime}".Trim();

        public string TableName => "Kandidat";

        public string Values =>
            $"'{Ime}', '{Prezime}', '{JMBG}', '{Telefon}', '{Email}', " +
            $"'{Adresa}', '{DatumUpisa:yyyy-MM-dd}', {(Aktivan ? 1 : 0)}";

        public string Query => $"JMBG = '{JMBG}'";

        public string TableKeyColumn => "KandidatId";

        public string TableKeyQuery =>
            $"{TableKeyColumn} = {KandidatId}";

        public string Update =>
            $"UPDATE Kandidat SET " +
            $"Ime = '{Ime}', " +
            $"Prezime = '{Prezime}', " +
            $"JMBG = '{JMBG}', " +
            $"Telefon = '{Telefon}', " +
            $"Email = '{Email}', " +
            $"Adresa = '{Adresa}', " +
            $"DatumUpisa = '{DatumUpisa:yyyy-MM-dd}', " +
            $"Aktivan = {(Aktivan ? 1 : 0)} " +
            $"WHERE KandidatId = {KandidatId}";

        public List<IEntity> GetReaderList(DbDataReader reader)
        {
            var list = new List<IEntity>();
            while (reader.Read())
            {
                list.Add(new Kandidat 
                    {
                        KandidatId = (int)reader["KandidatId"],
                        Ime = reader["Ime"].ToString(),
                        Prezime = reader["Prezime"].ToString(),
                        JMBG = reader["JMBG"].ToString(),
                        Telefon = reader["Telefon"].ToString(),
                        Email = reader["Email"].ToString(),
                        Adresa = reader["Adresa"].ToString(),
                        DatumUpisa = (DateTime)reader["DatumUpisa"],
                        Aktivan = (bool)reader["Aktivan"]
                    }
                );
            }
            return list;
        }

        public IEntity GetReaderResult(DbDataReader reader)
        {
            if (reader.Read())
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
                    Aktivan = (bool)reader["Aktivan"]
                };
            }
            return null;
        }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(JMBG) ? PunoIme : $"{PunoIme} ({JMBG})";
        }
    }
}
