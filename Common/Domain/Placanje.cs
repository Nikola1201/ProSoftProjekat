using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Common.Domain
{
    [Serializable]
    public class Placanje : IEntity
    {
        public int PlacanjeId { get; set; }
        public int UpisId { get; set; }
        public decimal Iznos { get; set; }
        public DateTime DatumPlacanja { get; set; }
        public string NacinPlacanja { get; set; }  // 'gotovina', 'kartica', 'transfer'
        public string Napomena { get; set; }

        public string TableName => "Placanje";

        public string Values =>
            $"{UpisId}, {Iznos.ToString(System.Globalization.CultureInfo.InvariantCulture)}, '{DatumPlacanja:yyyy-MM-dd}', '{NacinPlacanja}', '{Napomena}'";

        public string Query => $"UpisId = {UpisId}";

        public string TableKeyColumn => "PlacanjeId";

        public string TableKeyQuery =>
            $"{TableKeyColumn} = {PlacanjeId}";

        public string Update =>
            $"UPDATE Placanje SET " +
            $"UpisId = {UpisId}, " +
            $"Iznos = {Iznos.ToString(System.Globalization.CultureInfo.InvariantCulture)}, " +
            $"DatumPlacanja = '{DatumPlacanja:yyyy-MM-dd}', " +
            $"NacinPlacanja = '{NacinPlacanja}', " +
            $"Napomena = '{Napomena}' " +
            $"WHERE PlacanjeId = {PlacanjeId}";

        public List<IEntity> GetReaderList(DbDataReader reader)
        {
            var list = new List<IEntity>();
            while (reader.Read())
                list.Add(GetReaderResult(reader));
            return list;
        }

        public IEntity GetReaderResult(DbDataReader reader)
        {
            return new Placanje
            {
                PlacanjeId = (int)reader["PlacanjeId"],
                UpisId = (int)reader["UpisId"],
                Iznos = (decimal)reader["Iznos"],
                DatumPlacanja = (DateTime)reader["DatumPlacanja"],
                NacinPlacanja = reader["NacinPlacanja"].ToString(),
                Napomena = reader["Napomena"].ToString()
            };
        }
    }
}
