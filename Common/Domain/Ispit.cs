using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Common.Domain
{
    /// <summary>Predstavlja ispit (teorijski ili praktični) koji kandidat polaže u okviru upisa.</summary>
    [Serializable]
    public class Ispit : IEntity
    {
        /// <summary>Jedinstveni identifikator ispita (PK).</summary>
        public int IspitId { get; set; }

        /// <summary>Identifikator upisa u okviru kojeg se polaže ispit (FK na Upis).</summary>
        public int UpisId { get; set; }

        /// <summary>Datum kada je ispit održan.</summary>
        public DateTime DatumIspita { get; set; }

        /// <summary>Tip ispita (npr. "teorijski" ili "prakticni").</summary>
        public string Tip { get; set; }

        /// <summary>Rezultat ispita (npr. "polozio" ili "pao").</summary>
        public string Rezultat { get; set; }

        /// <summary>Opcionalna napomena uz ispit.</summary>
        public string Napomena { get; set; }

        /// <inheritdoc/>
        public string TableName => "Ispit";

        /// <inheritdoc/>
        public string Values =>
            $"{UpisId}, '{DatumIspita:yyyy-MM-dd}', '{Tip}', '{Rezultat}', '{Napomena}'";

        /// <inheritdoc/>
        public string Query => $"UpisId = {UpisId}";

        /// <inheritdoc/>
        public string TableKeyColumn => "IspitId";

        /// <inheritdoc/>
        public string TableKeyQuery =>
            $"{TableKeyColumn} = {IspitId}";

        /// <inheritdoc/>
        public string Update =>
            $"UPDATE Ispit SET " +
            $"UpisId = {UpisId}, " +
            $"DatumIspita = '{DatumIspita:yyyy-MM-dd}', " +
            $"Tip = '{Tip}', " +
            $"Rezultat = '{Rezultat}', " +
            $"Napomena = '{Napomena}' " +
            $"WHERE IspitId = {IspitId}";

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
