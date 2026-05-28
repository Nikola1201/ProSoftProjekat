using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Common.Domain
{
    /// <summary>Predstavlja upis kandidata u određeni paket obuke.</summary>
    [Serializable]
    public class Upis : IEntity
    {
        /// <summary>Jedinstveni identifikator upisa (PK).</summary>
        public int UpisId { get; set; }

        /// <summary>Identifikator kandidata koji je upisan (FK na Kandidat).</summary>
        public int KandidatId { get; set; }

        /// <summary>Identifikator paketa obuke u koji je kandidat upisan (FK na PaketObuke).</summary>
        public int PaketId { get; set; }

        /// <summary>Datum kada je kandidat upisan u paket obuke.</summary>
        public DateTime DatumUpisa { get; set; }

        /// <summary>Trenutni status upisa: "aktivan", "polozio", "pao" ili "odustao".</summary>
        public string Status { get; set; }  // 'aktivan', 'polozio', 'pao', 'odustao'

        /// <summary>Navigacioni objekat ka kandidatu koji je upisan.</summary>
        public Kandidat Kandidat { get; set; }

        /// <summary>Navigacioni objekat ka paketu obuke u koji je kandidat upisan.</summary>
        public PaketObuke Paket { get; set; }

        /// <inheritdoc/>
        public string TableName => "Upis";

        /// <inheritdoc/>
        public string Values =>
            $"{KandidatId}, {PaketId}, '{DatumUpisa:yyyy-MM-dd}', '{Status}'";

        /// <inheritdoc/>
        public string Query =>
            $"KandidatId = {KandidatId}";

        /// <inheritdoc/>
        public string TableKeyColumn => "UpisId";

        /// <inheritdoc/>
        public string TableKeyQuery =>
            $"{TableKeyColumn} = {UpisId}";

        /// <inheritdoc/>
        public string Update =>
            $"UPDATE Upis SET " +
            $"KandidatId = {KandidatId}, " +
            $"PaketId = {PaketId}, " +
            $"DatumUpisa = '{DatumUpisa:yyyy-MM-dd}', " +
            $"Status = '{Status}' " +
            $"WHERE UpisId = {UpisId}";

        /// <inheritdoc/>
        public List<IEntity> GetReaderList(DbDataReader reader)
        {
            var list = new List<IEntity>();
            while (reader.Read())
            {
                list.Add(new Upis
                    {
                        UpisId = (int)reader["UpisId"],
                        KandidatId = (int)reader["KandidatId"],
                        PaketId = (int)reader["PaketId"],
                        DatumUpisa = (DateTime)reader["DatumUpisa"],
                        Status = reader["Status"].ToString()
                    }
                );
            }

            return list;
        }

        /// <inheritdoc/>
        public IEntity GetReaderResult(DbDataReader reader)
        {
            if (reader.Read())
            {
                return new Upis
                {
                    UpisId = (int)reader["UpisId"],
                    KandidatId = (int)reader["KandidatId"],
                    PaketId = (int)reader["PaketId"],
                    DatumUpisa = (DateTime)reader["DatumUpisa"],
                    Status = reader["Status"].ToString()
                };
            }
            return null;
        }
    }
}
