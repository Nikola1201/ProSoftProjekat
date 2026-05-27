using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Common.Domain
{
    [Serializable]
    public class Instruktor : IEntity
    {
        public int InstruktorId { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string JMBG { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public DateTime DatumZaposlenja { get; set; }
        public bool Aktivan { get; set; }
        public string PunoIme => $"{Ime} {Prezime}".Trim();

        public string TableName => "Instruktor";

        public string Values =>
            $"'{Ime}', '{Prezime}', '{JMBG}', '{Telefon}', '{Email}', '{DatumZaposlenja:yyyy-MM-dd}',{(Aktivan ? 1 : 0)}";

        public string Query => $"JMBG = '{JMBG}'";

        public string TableKeyColumn => "InstruktorId";

        public string TableKeyQuery =>
            $"{TableKeyColumn} = {InstruktorId}";

        public string Update =>
            $"UPDATE Instruktor SET " +
            $"Ime = '{Ime}', " +
            $"Prezime = '{Prezime}', " +
            $"JMBG = '{JMBG}', " +
            $"Telefon = '{Telefon}', " +
            $"Email = '{Email}', " +
            $"DatumZaposlenja = '{DatumZaposlenja:yyyy-MM-dd}', " +
            $"Aktivan = {(Aktivan ? 1 : 0)} " +
            $"WHERE InstruktorId = {InstruktorId}";


        public List<IEntity> GetReaderList(DbDataReader reader)
        {
            var list = new List<IEntity>();
            while (reader.Read())
            {
                list.Add(new Instruktor
                {
                    InstruktorId = (int)reader["InstruktorId"],
                    Ime = reader["Ime"].ToString(),
                    Prezime = reader["Prezime"].ToString(),
                    JMBG = reader["JMBG"].ToString(),
                    Telefon = reader["Telefon"].ToString(),
                    Email = reader["Email"].ToString(),
                    DatumZaposlenja = (DateTime)reader["DatumZaposlenja"],
                    Aktivan = (bool)reader["Aktivan"]
                });
            }
            return list;
        }

        public IEntity GetReaderResult(DbDataReader reader)
        {
            if (reader.Read())
            {
                return new Instruktor
                {
                    InstruktorId = (int)reader["InstruktorId"],
                    Ime = reader["Ime"].ToString(),
                    Prezime = reader["Prezime"].ToString(),
                    JMBG = reader["JMBG"].ToString(),
                    Telefon = reader["Telefon"].ToString(),
                    Email = reader["Email"].ToString(),
                    DatumZaposlenja = (DateTime)reader["DatumZaposlenja"],
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
