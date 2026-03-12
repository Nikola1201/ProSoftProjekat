using System;
using System.Collections.Generic;
using System.Data.SqlClient;

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

        public object Query =>
            $"KandidatID = {KandidatId}";

        public object TableKeyColumn => "UpisId";

        public object SearchQuery =>
            $"SELECT * FROM Upis WHERE KandidatId = {KandidatId}";

        public object TableKeyQuery =>
            $"{TableKeyColumn} = {UpisId}";

        public object Update =>
            $"UPDATE Upis SET " +
            $"KandidatId = {KandidatId}, " +
            $"PaketId = {PaketId}, " +
            $"DatumUpisa = '{DatumUpisa:yyyy-MM-dd}', " +
            $"Status = '{Status}' " +
            $"WHERE UpisId = {UpisId}";

        public List<IEntity> GetReaderList(SqlDataReader reader)
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

        public IEntity GetReaderResult(SqlDataReader reader)
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
