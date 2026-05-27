using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Common.Domain
{
    [Serializable]
    public class Upis : IEntity
    {
        public int UpisId { get; set; }
        public int KandidatId { get; set; }
        public int PaketId { get; set; }
        public DateTime DatumUpisa { get; set; }
        public string Status { get; set; }  // 'aktivan', 'polozio', 'pao', 'odustao'
        public Kandidat Kandidat { get; set; }
        public PaketObuke Paket { get; set; }
        public string TableName => "Upis";

        public string Values =>
            $"{KandidatId}, {PaketId}, '{DatumUpisa:yyyy-MM-dd}', '{Status}'";

        public string Query =>
            $"KandidatId = {KandidatId}";

        public string TableKeyColumn => "UpisId";

        public string TableKeyQuery =>
            $"{TableKeyColumn} = {UpisId}";

        public string Update =>
            $"UPDATE Upis SET " +
            $"KandidatId = {KandidatId}, " +
            $"PaketId = {PaketId}, " +
            $"DatumUpisa = '{DatumUpisa:yyyy-MM-dd}', " +
            $"Status = '{Status}' " +
            $"WHERE UpisId = {UpisId}";

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
