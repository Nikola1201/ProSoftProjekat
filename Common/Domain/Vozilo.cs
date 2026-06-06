using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Common.Domain
{
    /// <summary>Predstavlja vozilo koje se koristi za obuku kandidata u auto-školi.</summary>
    [Serializable]
    public class Vozilo : IEntity
    {
        /// <summary>Jedinstveni identifikator vozila (PK). Dozvoljene vrednosti: auto-generisan PK; ne validira se.</summary>
        public int VoziloId { get; set; }

        /// <summary>Marka vozila (npr. "Volkswagen", "Renault"). Dozvoljene vrednosti: obavezno; 1–30 karaktera. Validira <see cref="Common.Validation.VoziloValidator"/>.</summary>
        public string Marka { get; set; }

        /// <summary>Model vozila (npr. "Golf", "Clio"). Dozvoljene vrednosti: obavezno; 1–30 karaktera. Validira <see cref="Common.Validation.VoziloValidator"/>.</summary>
        public string Model { get; set; }

        /// <summary>Godina proizvodnje vozila. Dozvoljene vrednosti: celobrojno, između 1950 i 2100. Validira <see cref="Common.Validation.VoziloValidator"/>.</summary>
        public int Godiste { get; set; }

        /// <summary>Registarski broj (tablica) vozila. Jedinstven u sistemu. Dozvoljene vrednosti: obavezno; 1–15 karaktera. Validira <see cref="Common.Validation.VoziloValidator"/>.</summary>
        public string Tablica { get; set; }

        /// <summary>Identifikator kategorije kojoj vozilo pripada (FK na Kategorija). Dozvoljene vrednosti: strani ključ; mora biti veće od nule. Validira <see cref="Common.Validation.VoziloValidator"/>.</summary>
        public int KategorijaID { get; set; }

        /// <summary>Označava da li je vozilo trenutno aktivno i dostupno za obuku. Dozvoljene vrednosti: true ili false; ne validira se.</summary>
        public bool Aktivno { get; set; }

        /// <inheritdoc/>
        public string TableName => "Vozilo";

        /// <inheritdoc/>
        public string Values =>
            $"'{Marka}', '{Model}', {Godiste}, '{Tablica}', {KategorijaID}";

        /// <inheritdoc/>
        public string Query => $"Tablica = '{Tablica}'";

        /// <inheritdoc/>
        public string TableKeyColumn => "VoziloId";

        /// <inheritdoc/>
        public string TableKeyQuery =>
            $"{TableKeyColumn} = {VoziloId}";

        /// <inheritdoc/>
        public string Update =>
            $"UPDATE Vozilo SET " +
            $"Marka = '{Marka}', " +
            $"Model = '{Model}', " +
            $"Godiste = {Godiste}, " +
            $"Tablica = '{Tablica}', " +
            $"KategorijaID = {KategorijaID}, " +
            $"Aktivno = {(Aktivno ? 1 : 0)} " +
            $"WHERE VoziloId = {VoziloId}";

        /// <inheritdoc/>
        public List<IEntity> GetReaderList(DbDataReader reader)
        {
            var list = new List<IEntity>();
            while (reader.Read())
            {
                list.Add(new Vozilo
                {
                    VoziloId = (int)reader["VoziloId"],
                    Marka = reader["Marka"].ToString(),
                    Model = reader["Model"].ToString(),
                    Godiste = (int)reader["Godiste"],
                    Tablica = reader["Tablica"].ToString(),
                    KategorijaID = (int)reader["KategorijaID"],
                    Aktivno = (bool)reader["Aktivno"]
                });
            }
            return list;
        }

        /// <inheritdoc/>
        public IEntity GetReaderResult(DbDataReader reader)
        {
            if (reader.Read())
            {
                return new Vozilo
                {
                    VoziloId = (int)reader["VoziloId"],
                    Marka = reader["Marka"].ToString(),
                    Model = reader["Model"].ToString(),
                    Godiste = (int)reader["Godiste"],
                    Tablica = reader["Tablica"].ToString(),
                    KategorijaID = (int)reader["KategorijaID"],
                    Aktivno = (bool)reader["Aktivno"]
                };
            }
            return null;
        }

        /// <summary>Tekstualna reprezentacija vozila: "Marka Model (Tablica) - Kategorija ID".</summary>
        public override string ToString()
        {
            return $"{Marka} {Model} ({Tablica}) - Kategorija {KategorijaID}";
        }
    }
}
