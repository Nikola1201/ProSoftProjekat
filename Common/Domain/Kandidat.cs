using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Common.Domain
{
    /// <summary>Predstavlja kandidata upisanog u auto-školu.</summary>
    [Serializable]
    public class Kandidat : IEntity
    {
        /// <summary>
        /// Jedinstveni identifikator kandidata (PK).
        /// Dozvoljene vrednosti: auto-generisan PK; ne validira se.
        /// </summary>
        public int KandidatId { get; set; }

        /// <summary>
        /// Ime kandidata. Obavezno.
        /// Dozvoljene vrednosti: obavezno; 1–50 karaktera. Validira <see cref="Common.Validation.KandidatValidator"/>.
        /// </summary>
        public string Ime { get; set; }

        /// <summary>
        /// Prezime kandidata. Obavezno.
        /// Dozvoljene vrednosti: obavezno; 1–50 karaktera. Validira <see cref="Common.Validation.KandidatValidator"/>.
        /// </summary>
        public string Prezime { get; set; }

        /// <summary>
        /// Jedinstveni matični broj građana kandidata.
        /// Dozvoljene vrednosti: obavezno; tačno 13 cifara. Validira <see cref="Common.Validation.KandidatValidator"/>.
        /// </summary>
        public string JMBG { get; set; }

        /// <summary>
        /// Kontakt telefon kandidata.
        /// Dozvoljene vrednosti: obavezno; 6–20 cifara uz opcioni vodeći '+'. Validira <see cref="Common.Validation.KandidatValidator"/>.
        /// </summary>
        public string Telefon { get; set; }

        /// <summary>
        /// Email adresa kandidata.
        /// Dozvoljene vrednosti: obavezno; ispravan email format. Validira <see cref="Common.Validation.KandidatValidator"/>.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Adresa stanovanja kandidata.
        /// Dozvoljene vrednosti: obavezno; 1–100 karaktera. Validira <see cref="Common.Validation.KandidatValidator"/>.
        /// </summary>
        public string Adresa { get; set; }

        /// <summary>
        /// Datum upisa kandidata u auto-školu.
        /// Dozvoljene vrednosti: obavezno; ne sme biti podrazumevani datum. Validira <see cref="Common.Validation.KandidatValidator"/>.
        /// </summary>
        public DateTime DatumUpisa { get; set; }

        /// <summary>
        /// Označava da li je kandidat trenutno aktivan u sistemu.
        /// Dozvoljene vrednosti: true ili false; ne validira se.
        /// </summary>
        public bool Aktivan { get; set; }

        /// <summary>Spojeno ime i prezime za prikaz u korisničkom interfejsu.</summary>
        public string PunoIme => $"{Ime} {Prezime}".Trim();

        /// <inheritdoc/>
        public string TableName => "Kandidat";

        /// <inheritdoc/>
        public string Values =>
            $"'{Ime}', '{Prezime}', '{JMBG}', '{Telefon}', '{Email}', " +
            $"'{Adresa}', '{DatumUpisa:yyyy-MM-dd}', {(Aktivan ? 1 : 0)}";

        /// <inheritdoc/>
        public string Query => $"JMBG = '{JMBG}'";

        /// <inheritdoc/>
        public string TableKeyColumn => "KandidatId";

        /// <inheritdoc/>
        public string TableKeyQuery =>
            $"{TableKeyColumn} = {KandidatId}";

        /// <inheritdoc/>
        public string Update =>
            $"UPDATE Kandidat SET " +
            $"Ime = '{Ime}', " +
            $"Prezime = '{Prezime}', " +
            $"JMBG = '{JMBG}', " +
            $"Telefon = '{Telefon}', " +
            $"Email = '{Email}', " +
            $"Adresa = '{Adresa}', " +
            $"DatumUpisa = '{DatumUpisa:yyyy-MM-dd}', " +
            $"Aktivan = {(Aktivan ? 1 : 0)} " +
            $"WHERE KandidatId = {KandidatId}";

        /// <inheritdoc/>
        public List<IEntity> GetReaderList(DbDataReader reader)
        {
            var list = new List<IEntity>();
            while (reader.Read())
            {
                list.Add(new Kandidat
                    {
                        KandidatId = (int)reader["KandidatId"],
                        Ime = reader["Ime"].ToString(),
                        Prezime = reader["Prezime"].ToString(),
                        JMBG = reader["JMBG"].ToString(),
                        Telefon = reader["Telefon"].ToString(),
                        Email = reader["Email"].ToString(),
                        Adresa = reader["Adresa"].ToString(),
                        DatumUpisa = (DateTime)reader["DatumUpisa"],
                        Aktivan = (bool)reader["Aktivan"]
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
                return new Kandidat
                {
                    KandidatId = (int)reader["KandidatId"],
                    Ime = reader["Ime"].ToString(),
                    Prezime = reader["Prezime"].ToString(),
                    JMBG = reader["JMBG"].ToString(),
                    Telefon = reader["Telefon"].ToString(),
                    Email = reader["Email"].ToString(),
                    Adresa = reader["Adresa"].ToString(),
                    DatumUpisa = (DateTime)reader["DatumUpisa"],
                    Aktivan = (bool)reader["Aktivan"]
                };
            }
            return null;
        }

        /// <summary>Tekstualna reprezentacija kandidata: "Ime Prezime (JMBG)" ili samo puno ime ako JMBG nije postavljen.</summary>
        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(JMBG) ? PunoIme : $"{PunoIme} ({JMBG})";
        }
    }
}
