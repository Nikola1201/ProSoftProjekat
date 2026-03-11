using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;

namespace Common.Domain
{
    [Serializable]
    public class PaketObuke : IEntity
    {
        public int PaketId { get; set; }
        public string Naziv { get; set; }
        public Kategorija Kategorija { get; set; }
        public int BrojCasova { get; set; }
        public decimal Cena { get; set; }
        public string Opis { get; set; }

        public string TableName => "PaketObuke";

        public string Values =>
            $"'{Naziv}', '{Kategorija.KategorijaID}', {BrojCasova}, {Cena.ToString(System.Globalization.CultureInfo.InvariantCulture)}, '{Opis}'";

        public object Query =>
            $"INSERT INTO PaketObuke (Naziv, KategorijaID, BrojCasova, Cena, Opis) " +
            $"VALUES ({Values})";

        public object TableKeyColumn => "PaketId";

        public object SearchQuery =>
            $"SELECT * FROM PaketObuke WHERE Naziv LIKE '%{Naziv}%'";

        public object TableKeyQuery =>
            $" {TableKeyColumn} = {PaketId}";

        public object Update =>
            $"UPDATE PaketObuke SET " +
            $"Naziv = '{Naziv}', " +
            $"KategorijaID = '{Kategorija.KategorijaID}', " +
            $"BrojCasova = {BrojCasova}, " +
            $"Cena = {Cena.ToString(System.Globalization.CultureInfo.InvariantCulture)}, " +
            $"Opis = '{Opis}' " +
            $"WHERE PaketId = {PaketId}";

        public List<IEntity> GetReaderList(SqlDataReader reader)
        {
            var list = new List<IEntity>();
            while (reader.Read())
            {
                list.Add(new PaketObuke
                    {
                        PaketId = (int)reader["PaketId"],
                        Naziv = reader["Naziv"].ToString(),
                        Kategorija = new Kategorija()
                        {
                            KategorijaID = (int)reader["KategorijaID"],
                        },
                        BrojCasova = (int)reader["BrojCasova"],
                        Cena = (decimal)reader["Cena"],
                        Opis = reader["Opis"].ToString()
                    }
                );
            }
                
            return list;
        }

        public IEntity GetReaderResult(SqlDataReader reader)
        {
            if (reader.Read())
            {
                return new PaketObuke
                {
                    PaketId = (int)reader["PaketId"],
                    Naziv = reader["Naziv"].ToString(),
                    Kategorija = new Kategorija()
                    {
                        KategorijaID = (int)reader["KategorijaID"],
                    },
                    BrojCasova = (int)reader["BrojCasova"],
                    Cena = (decimal)reader["Cena"],
                    Opis = reader["Opis"].ToString()
                };
            }
            return null;
        }

        public override string ToString()
        {
            return $"{Naziv} ({Kategorija})";
        }
    }
}
