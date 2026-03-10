using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Common.Domain
{
    [Serializable]
    public class Kategorija : IEntity
    {
        private string _nazivKategorije;

        public int KategorijaID { get; set; }

        public string NazivKategorije
        {
            get => _nazivKategorije;
            set
            {
                if (value != null && value.Length > 2)
                {
                    throw new ArgumentException("Naziv kategorije moze imati najvise 2 karaktera.");
                }

                _nazivKategorije = value;
            }
        }

        public string TableName => "Kategorija";

        public string Values => $"('{NazivKategorije}')";

        public object Query => $"NazivKategorije = '{NazivKategorije}'";

        public object TableKeyColumn => "KategorijaID";

        public object SearchQuery =>
            $"SELECT * FROM Kategorija WHERE NazivKategorije LIKE '%{NazivKategorije ?? string.Empty}%'";

        public object TableKeyQuery => $"SELECT * FROM Kategorija WHERE KategorijaID = {KategorijaID}";

        public object Update =>
            $"UPDATE Kategorija SET NazivKategorije = '{NazivKategorije}' WHERE KategorijaID = {KategorijaID}";

        public List<IEntity> GetReaderList(SqlDataReader reader)
        {
            var list = new List<IEntity>();
            while (reader.Read())
            {
                list.Add(new Kategorija
                {
                    KategorijaID = (int)reader["KategorijaID"],
                    NazivKategorije = reader["NazivKategorije"].ToString()
                });
            }

            return list;
        }

        public IEntity GetReaderResult(SqlDataReader reader)
        {
            if (reader.Read())
            {
                return new Kategorija
                {
                    KategorijaID = (int)reader["KategorijaID"],
                    NazivKategorije = reader["NazivKategorije"].ToString()
                };
            }

            return null;
        }

        public override string ToString()
        {
            return NazivKategorije;
        }
    }
}
