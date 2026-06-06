using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Common.Domain
{
    /// <summary>Predstavlja paket obuke koji auto-škola nudi kandidatima (kombinacija kategorije, broja časova i cene).</summary>
    [Serializable]
    public class PaketObuke : IEntity
    {
        /// <summary>Jedinstveni identifikator paketa obuke (PK). Dozvoljene vrednosti: auto-generisan PK; ne validira se.</summary>
        public int PaketId { get; set; }

        /// <summary>Naziv paketa obuke (npr. "Osnovna B kategorija"). Dozvoljene vrednosti: obavezno; 1–50 karaktera. Validira <see cref="Common.Validation.PaketObukeValidator"/>.</summary>
        public string Naziv { get; set; }

        /// <summary>Kategorija vozačke dozvole na koju se paket odnosi. Dozvoljene vrednosti: obavezno; referenca na kategoriju (ne sme biti null). Validira <see cref="Common.Validation.PaketObukeValidator"/>.</summary>
        public Kategorija Kategorija { get; set; }

        /// <summary>Ukupan broj časova vožnje predviđen paketom. Dozvoljene vrednosti: celobrojno, između 1 i 200. Validira <see cref="Common.Validation.PaketObukeValidator"/>.</summary>
        public int BrojCasova { get; set; }

        /// <summary>Cena paketa obuke u dinarima. Dozvoljene vrednosti: decimalno; mora biti veće od nule. Validira <see cref="Common.Validation.PaketObukeValidator"/>.</summary>
        public decimal Cena { get; set; }

        /// <summary>Opis sadržaja i posebnosti paketa obuke. Dozvoljene vrednosti: opciono; najviše 500 karaktera. Validira <see cref="Common.Validation.PaketObukeValidator"/>.</summary>
        public string Opis { get; set; }

        /// <inheritdoc/>
        public string TableName => "PaketObuke";

        /// <inheritdoc/>
        public string Values =>
            $"'{Naziv}', '{Kategorija.KategorijaID}', {BrojCasova}, {Cena.ToString(System.Globalization.CultureInfo.InvariantCulture)}, '{Opis}'";

        /// <inheritdoc/>
        public string Query => $"Naziv = '{Naziv}'";

        /// <inheritdoc/>
        public string TableKeyColumn => "PaketId";

        /// <inheritdoc/>
        public string TableKeyQuery =>
            $"{TableKeyColumn} = {PaketId}";

        /// <inheritdoc/>
        public string Update =>
            $"UPDATE PaketObuke SET " +
            $"Naziv = '{Naziv}', " +
            $"KategorijaID = '{Kategorija.KategorijaID}', " +
            $"BrojCasova = {BrojCasova}, " +
            $"Cena = {Cena.ToString(System.Globalization.CultureInfo.InvariantCulture)}, " +
            $"Opis = '{Opis}' " +
            $"WHERE PaketId = {PaketId}";

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <summary>Tekstualna reprezentacija paketa: "Naziv (Kategorija)".</summary>
        public override string ToString()
        {
            return $"{Naziv} ({Kategorija})";
        }
    }
}
