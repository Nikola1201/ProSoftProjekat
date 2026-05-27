using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Common.Domain
{
    [Serializable]
    public class CasVoznje : IEntity
    {
        public int CasId { get; set; }
        public int UpisId { get; set; }
        public int InstruktorId { get; set; }
        public int VoziloId { get; set; }
        public DateTime DatumCas { get; set; }
        public int TrajanjMin { get; set; }
        public string Status { get; set; }  // 'zakazan', 'odrzan', 'otkazan'
        public string Napomena { get; set; }

        public string TableName => "CasVoznje";

        public string Values =>
            $"{UpisId}, {InstruktorId}, {VoziloId}, '{DatumCas:yyyy-MM-dd HH:mm}', {TrajanjMin}, '{Status}', '{Napomena}'";

        public string Query => $"UpisId = {UpisId}";

        public string TableKeyColumn => "CasId";

        public string TableKeyQuery =>
            $"{TableKeyColumn} = {CasId}";

        public string Update =>
            $"UPDATE CasVoznje SET " +
            $"UpisId = {UpisId}, " +
            $"InstruktorId = {InstruktorId}, " +
            $"VoziloId = {VoziloId}, " +
            $"DatumCas = '{DatumCas:yyyy-MM-dd HH:mm}', " +
            $"TrajanjMin = {TrajanjMin}, " +
            $"Status = '{Status}', " +
            $"Napomena = '{Napomena}' " +
            $"WHERE CasId = {CasId}";

        public List<IEntity> GetReaderList(DbDataReader reader)
        {
            var list = new List<IEntity>();
            while (reader.Read())
                list.Add(GetReaderResult(reader));
            return list;
        }

        public IEntity GetReaderResult(DbDataReader reader)
        {
            return new CasVoznje
            {
                CasId = (int)reader["CasId"],
                UpisId = (int)reader["UpisId"],
                InstruktorId = (int)reader["InstruktorId"],
                VoziloId = (int)reader["VoziloId"],
                DatumCas = (DateTime)reader["DatumCas"],
                TrajanjMin = (int)reader["TrajanjMin"],
                Status = reader["Status"].ToString(),
                Napomena = reader["Napomena"].ToString()
            };
        }
    }
}
