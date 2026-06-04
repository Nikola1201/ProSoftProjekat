using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Common.Domain
{
    /// <summary>Predstavlja kategoriju vozačke dozvole (npr. "B", "C", "A").</summary>
    [Serializable]
    public class Kategorija : IEntity
    {
        private string _nazivKategorije;

        /// <summary>Jedinstveni identifikator kategorije (PK). Dozvoljene vrednosti: auto-generisan PK; ne validira se.</summary>
        public int KategorijaID { get; set; }

        /// <summary>
        /// Naziv kategorije vozačke dozvole. Maksimalna dužina su 2 karaktera.
        /// Baca <see cref="ArgumentException"/> ako vrednost prelazi 2 karaktera.
        /// Dozvoljene vrednosti: obavezno; 1–2 karaktera (npr. "A", "B", "C", "AM", "B1"). Validira setter (ArgumentException za > 2) i <see cref="Common.Validation.KategorijaValidator"/>.
        /// </summary>
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

        /// <inheritdoc/>
        public string TableName => "Kategorija";

        /// <inheritdoc/>
        public string Values => $"'{NazivKategorije}'";

        /// <inheritdoc/>
        public string Query => $"NazivKategorije = '{NazivKategorije}'";

        /// <inheritdoc/>
        public string TableKeyColumn => "KategorijaID";

        /// <inheritdoc/>
        public string TableKeyQuery => $"{TableKeyColumn} = {KategorijaID}";

        /// <inheritdoc/>
        public string Update =>
            $"UPDATE Kategorija SET NazivKategorije = '{NazivKategorije}' WHERE KategorijaID = {KategorijaID}";

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <summary>Tekstualna reprezentacija kategorije: naziv kategorije.</summary>
        public override string ToString()
        {
            return NazivKategorije;
        }
    }
}
