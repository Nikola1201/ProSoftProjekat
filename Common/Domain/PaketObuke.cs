using System.Collections.Generic;
using System.Data.SqlClient;

namespace Common.Domain
{
    public class PaketObuke : IEntity
    {
        public int PaketId { get; set; }
        public string Naziv { get; set; }
        public string Kategorija { get; set; }  // 'A', 'B', 'C'
        public int BrojCasova { get; set; }
        public decimal Cena { get; set; }
        public string Opis { get; set; }

        public string TableName => "PaketObuke";

        public string Values =>
            $"'{Naziv}', '{Kategorija}', {BrojCasova}, {Cena.ToString(System.Globalization.CultureInfo.InvariantCulture)}, '{Opis}'";

        public object Query =>
            $"INSERT INTO PaketObuke (Naziv, Kategorija, BrojCasova, Cena, Opis) " +
            $"VALUES ({Values})";

        public object TableKeyColumn => "PaketId";

        public object SearchQuery =>
            $"SELECT * FROM PaketObuke WHERE Naziv LIKE '%{Naziv}%'";

        public object TableKeyQuery =>
            $"SELECT * FROM PaketObuke WHERE PaketId = {PaketId}";

        public object Update =>
            $"UPDATE PaketObuke SET " +
            $"Naziv = '{Naziv}', " +
            $"Kategorija = '{Kategorija}', " +
            $"BrojCasova = {BrojCasova}, " +
            $"Cena = {Cena.ToString(System.Globalization.CultureInfo.InvariantCulture)}, " +
            $"Opis = '{Opis}' " +
            $"WHERE PaketId = {PaketId}";

        public List<IEntity> GetReaderList(SqlDataReader reader)
        {
            var list = new List<IEntity>();
            while (reader.Read())
                list.Add(GetReaderResult(reader));
            return list;
        }

        public IEntity GetReaderResult(SqlDataReader reader)
        {
            return new PaketObuke
            {
                PaketId = (int)reader["PaketId"],
                Naziv = reader["Naziv"].ToString(),
                Kategorija = reader["Kategorija"].ToString(),
                BrojCasova = (int)reader["BrojCasova"],
                Cena = (decimal)reader["Cena"],
                Opis = reader["Opis"].ToString()
            };
        }
    }
}
