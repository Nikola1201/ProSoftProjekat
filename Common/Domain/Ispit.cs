using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Common.Domain
{
    /// <summary>Predstavlja ispit (teorijski ili praktični) koji kandidat polaže u okviru upisa.</summary>
    [Serializable]
    public class Ispit : IEntity
    {
        /// <summary>Jedinstveni identifikator ispita (PK). Dozvoljene vrednosti: auto-generisan PK; ne validira se.</summary>
        public int IspitId { get; set; }

        /// <summary>Identifikator upisa u okviru kojeg se polaže ispit (FK na Upis). Dozvoljene vrednosti: strani ključ; mora biti veće od nule. Validira <see cref="Common.Validation.IspitValidator"/>.</summary>
        public int UpisId { get; set; }

        /// <summary>Datum kada je ispit održan. Dozvoljene vrednosti: obavezno; ne sme biti podrazumevani datum. Validira <see cref="Common.Validation.IspitValidator"/>.</summary>
        public DateTime DatumIspita { get; set; }

        /// <summary>Tip ispita (npr. "teorijski" ili "prakticni"). Dozvoljene vrednosti: jedna od: teorijski, prakticni. Validira <see cref="Common.Validation.IspitValidator"/>.</summary>
        public string Tip { get; set; }

        /// <summary>Rezultat ispita (npr. "polozio" ili "pao"). Dozvoljene vrednosti: jedna od: polozio, pao. Validira <see cref="Common.Validation.IspitValidator"/>.</summary>
        public string Rezultat { get; set; }

        /// <summary>Opcionalna napomena uz ispit. Dozvoljene vrednosti: opciono; najviše 200 karaktera. Validira <see cref="Common.Validation.IspitValidator"/>.</summary>
        public string Napomena { get; set; }

        /// <inheritdoc/>
        public string TableName => "Ispit";

        /// <inheritdoc/>
        public string Values =>
            $"{UpisId}, '{DatumIspita:yyyy-MM-dd}', '{Tip}', '{Rezultat}', '{Napomena}'";

        /// <inheritdoc/>
        public string Query => $"UpisId = {UpisId}";

        /// <inheritdoc/>
        public string TableKeyColumn => "IspitId";

        /// <inheritdoc/>
        public string TableKeyQuery =>
            $"{TableKeyColumn} = {IspitId}";

        /// <inheritdoc/>
        public string Update =>
            $"UPDATE Ispit SET " +
            $"UpisId = {UpisId}, " +
            $"DatumIspita = '{DatumIspita:yyyy-MM-dd}', " +
            $"Tip = '{Tip}', " +
            $"Rezultat = '{Rezultat}', " +
            $"Napomena = '{Napomena}' " +
            $"WHERE IspitId = {IspitId}";

        /// <inheritdoc/>
        public List<IEntity> GetReaderList(DbDataReader reader)
        {
            var list = new List<IEntity>();
            while (reader.Read())
                list.Add(GetReaderResult(reader));
            return list;
        }

        /// <inheritdoc/>
        public IEntity GetReaderResult(DbDataReader reader)
        {
            return new Ispit
            {
                IspitId = (int)reader["IspitId"],
                UpisId = (int)reader["UpisId"],
                DatumIspita = (DateTime)reader["DatumIspita"],
                Tip = reader["Tip"].ToString(),
                Rezultat = reader["Rezultat"].ToString(),
                Napomena = reader["Napomena"].ToString()
            };
        }
    }
}
