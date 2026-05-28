using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Common.Domain
{
    /// <summary>Predstavlja plaćanje vezano za određeni upis kandidata.</summary>
    [Serializable]
    public class Placanje : IEntity
    {
        /// <summary>Jedinstveni identifikator plaćanja (PK).</summary>
        public int PlacanjeId { get; set; }

        /// <summary>Identifikator upisa na koji se odnosi ovo plaćanje (FK na Upis).</summary>
        public int UpisId { get; set; }

        /// <summary>Iznos plaćanja u dinarima.</summary>
        public decimal Iznos { get; set; }

        /// <summary>Datum kada je plaćanje izvršeno.</summary>
        public DateTime DatumPlacanja { get; set; }

        /// <summary>Način plaćanja: "gotovina", "kartica" ili "transfer".</summary>
        public string NacinPlacanja { get; set; }  // 'gotovina', 'kartica', 'transfer'

        /// <summary>Opcionalna napomena uz plaćanje.</summary>
        public string Napomena { get; set; }

        /// <inheritdoc/>
        public string TableName => "Placanje";

        /// <inheritdoc/>
        public string Values =>
            $"{UpisId}, {Iznos.ToString(System.Globalization.CultureInfo.InvariantCulture)}, '{DatumPlacanja:yyyy-MM-dd}', '{NacinPlacanja}', '{Napomena}'";

        /// <inheritdoc/>
        public string Query => $"UpisId = {UpisId}";

        /// <inheritdoc/>
        public string TableKeyColumn => "PlacanjeId";

        /// <inheritdoc/>
        public string TableKeyQuery =>
            $"{TableKeyColumn} = {PlacanjeId}";

        /// <inheritdoc/>
        public string Update =>
            $"UPDATE Placanje SET " +
            $"UpisId = {UpisId}, " +
            $"Iznos = {Iznos.ToString(System.Globalization.CultureInfo.InvariantCulture)}, " +
            $"DatumPlacanja = '{DatumPlacanja:yyyy-MM-dd}', " +
            $"NacinPlacanja = '{NacinPlacanja}', " +
            $"Napomena = '{Napomena}' " +
            $"WHERE PlacanjeId = {PlacanjeId}";

        /// <inheritdoc/>
        public List<IEntity> GetReaderList(DbDataReader reader)
        {
            var list = new List<IEntity>();
            while (reader.Read())
                list.Add(GetReaderResult(reader));
            return list;
        }

        /// <inheritdoc/>
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
