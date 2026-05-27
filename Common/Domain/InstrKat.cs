using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Common.Domain
{
    [Serializable]
    public class InstrKat : IEntity
    {
        public int InstruktorId { get; set; }
        public int KategorijaID { get; set; }
        public DateTime DatumDodele { get; set; }
        public bool Aktivno { get; set; }

        public string TableName => "InstrKat";

        public string Values =>
            $"{InstruktorId}, {KategorijaID}, '{DatumDodele:yyyy-MM-dd HH:mm:ss}', {(Aktivno ? 1 : 0)}";

        public string Query => $"InstruktorId = {InstruktorId}";

        public string TableKeyColumn => "InstruktorId, KategorijaID";

        public string TableKeyQuery =>
            $"InstruktorId = {InstruktorId} AND KategorijaID = {KategorijaID}";

        public string Update =>
            $"UPDATE InstrKat SET " +
            $"DatumDodele = '{DatumDodele:yyyy-MM-dd HH:mm:ss}', " +
            $"Aktivno = {(Aktivno ? 1 : 0)} " +
            $"WHERE InstruktorId = {InstruktorId} AND KategorijaID = {KategorijaID}";

        public List<IEntity> GetReaderList(DbDataReader reader)
        {
            var list = new List<IEntity>();
            while (reader.Read())
            {
                list.Add(new InstrKat
                {
                    InstruktorId = (int)reader["InstruktorId"],
                    KategorijaID = (int)reader["KategorijaID"],
                    DatumDodele = (DateTime)reader["DatumDodele"],
                    Aktivno = (bool)reader["Aktivno"]
                });
            }
            return list;
        }

        public IEntity GetReaderResult(DbDataReader reader)
        {
            if (reader.Read())
            {
                return new InstrKat
                {
                    InstruktorId = (int)reader["InstruktorId"],
                    KategorijaID = (int)reader["KategorijaID"],
                    DatumDodele = (DateTime)reader["DatumDodele"],
                    Aktivno = (bool)reader["Aktivno"]
                };
            }
            return null;
        }
    }
}