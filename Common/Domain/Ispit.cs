using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Common.Domain
{
    [Serializable]
    public class Ispit : IEntity
    {
        public int IspitId { get; set; }
        public int UpisId { get; set; }
        public DateTime DatumIspita { get; set; }
        public string Tip { get; set; }
        public string Rezultat { get; set; }
        public string Napomena { get; set; }

        public string TableName => "Ispit";

        public string Values =>
            $"{UpisId}, '{DatumIspita:yyyy-MM-dd}', '{Tip}', '{Rezultat}', '{Napomena}'";

        public string Query => $"UpisId = {UpisId}";

        public string TableKeyColumn => "IspitId";

        public string TableKeyQuery =>
            $"{TableKeyColumn} = {IspitId}";

        public string Update =>
            $"UPDATE Ispit SET " +
            $"UpisId = {UpisId}, " +
            $"DatumIspita = '{DatumIspita:yyyy-MM-dd}', " +
            $"Tip = '{Tip}', " +
            $"Rezultat = '{Rezultat}', " +
            $"Napomena = '{Napomena}' " +
            $"WHERE IspitId = {IspitId}";

        public List<IEntity> GetReaderList(SqlDataReader reader)
        {
            var list = new List<IEntity>();
            while (reader.Read())
                list.Add(GetReaderResult(reader));
            return list;
        }

        public IEntity GetReaderResult(SqlDataReader reader)
        {
            return new Ispit
            {
                IspitId = (int)reader["IspitId"],
                UpisId = (int)reader["UpisId"],
                DatumIspita = (DateTime)reader["DatumIspita"],
                Tip = reader["Tip"].ToString(),
                Rezultat = reader["Rezultat"].ToString(),
                Napomena = reader["Napomena"].ToString()
            };
        }
    }
}
