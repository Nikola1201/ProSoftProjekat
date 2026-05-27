using System;
using System.Collections.Generic;
using System.Data.Common;

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

        public string Values => $"'{NazivKategorije}'";

        public string Query => $"NazivKategorije = '{NazivKategorije}'";

        public string TableKeyColumn => "KategorijaID";

        public string TableKeyQuery => $"{TableKeyColumn} = {KategorijaID}";

        public string Update =>
            $"UPDATE Kategorija SET NazivKategorije = '{NazivKategorije}' WHERE KategorijaID = {KategorijaID}";

        public List<IEntity> GetReaderList(DbDataReader reader)
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

        public IEntity GetReaderResult(DbDataReader reader)
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
