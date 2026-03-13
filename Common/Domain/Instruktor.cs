using System;
using System.Collections.Generic;
using System.Data.SqlClient;

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

        public string TableName => "Instruktor";

        public string Values =>
            $"'{Ime}', '{Prezime}', '{JMBG}', '{Telefon}', '{Email}', '{DatumZaposlenja:yyyy-MM-dd}',{(Aktivan ? 1 : 0)}";

        public object Query => throw new NotImplementedException();

        public object TableKeyColumn => "InstruktorId";

        public object SearchQuery =>
            $"SELECT * FROM Instruktor WHERE Ime LIKE '%{Ime}%' OR Prezime LIKE '%{Prezime}%'";

        public object TableKeyQuery =>
            $"SELECT * FROM Instruktor WHERE InstruktorId = {InstruktorId}";

        public object Update =>
            $"UPDATE Instruktor SET " +
            $"Ime = '{Ime}', " +
            $"Prezime = '{Prezime}', " +
            $"JMBG = '{JMBG}', " +
            $"Telefon = '{Telefon}', " +
            $"Email = '{Email}', " +
            $"DatumZaposlenja = '{DatumZaposlenja:yyyy-MM-dd}', " +
            $"Aktivan = {(Aktivan ? 1 : 0)} " +
            $"WHERE InstruktorId = {InstruktorId}";

        public List<IEntity> GetReaderList(SqlDataReader reader)
        {
            var list = new List<IEntity>();
            while (reader.Read())
                list.Add(GetReaderResult(reader));
            return list;
        }

        public IEntity GetReaderResult(SqlDataReader reader)
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
    }
}
