using System;
using System.Collections.Generic;
using System.Data.Common;

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

        public string Query => $"Naziv = '{Naziv}'";

        public string TableKeyColumn => "PaketId";

        public string TableKeyQuery =>
            $"{TableKeyColumn} = {PaketId}";

        public string Update =>
            $"UPDATE PaketObuke SET " +
            $"Naziv = '{Naziv}', " +
            $"KategorijaID = '{Kategorija.KategorijaID}', " +
            $"BrojCasova = {BrojCasova}, " +
            $"Cena = {Cena.ToString(System.Globalization.CultureInfo.InvariantCulture)}, " +
            $"Opis = '{Opis}' " +
            $"WHERE PaketId = {PaketId}";

        public List<IEntity> GetReaderList(DbDataReader reader)
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

        public IEntity GetReaderResult(DbDataReader reader)
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
