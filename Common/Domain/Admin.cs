using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Common.Domain
{
    [Serializable]
    public class Admin : IEntity
    {
        public int AdminId { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Username { get; set; }
        public string Lozinka { get; set; }
        public string Email { get; set; }
        public DateTime DatumKreiranja { get; set; }

        public string TableName => "Admin";

        public string Values =>
            $"'{Ime}', '{Prezime}', '{Username}', '{Lozinka}', '{Email}'";

        public object Query =>
            $"[Username] = '{Username}' and [Lozinka] = '{Lozinka}'";

        public object TableKeyColumn => "AdminId";

        public object SearchQuery =>
            $"SELECT * FROM Admin WHERE Ime LIKE '%{Ime}%' OR Prezime LIKE '%{Prezime}%'";

        public object TableKeyQuery =>
            $"SELECT * FROM Admin WHERE AdminId = {AdminId}";

        public object Update =>
            $"UPDATE Admin SET " +
            $"Ime = '{Ime}', " +
            $"Prezime = '{Prezime}', " +
            $"Username = '{Username}', " +
            $"Lozinka = '{Lozinka}', " +
            $"Email = '{Email}' " +
            $"WHERE AdminId = {AdminId}";

        public List<IEntity> GetReaderList(SqlDataReader reader)
        {
            var list = new List<IEntity>();
            while (reader.Read())
                list.Add(GetReaderResult(reader));
            return list;
        }

        public IEntity GetReaderResult(SqlDataReader reader)
        {
            return new Admin
            {
                AdminId = (int)reader["AdminId"],
                Ime = reader["Ime"].ToString(),
                Prezime = reader["Prezime"].ToString(),
                Username = reader["Username"].ToString(),
                Lozinka = reader["Lozinka"].ToString(),
                Email = reader["Email"].ToString(),
                DatumKreiranja = (DateTime)reader["DatumKreiranja"]
            };
        }
    }
}
