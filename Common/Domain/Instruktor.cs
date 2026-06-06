using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Common.Domain
{
    /// <summary>Predstavlja instruktora vožnje zaposlenog u auto-školi.</summary>
    [Serializable]
    public class Instruktor : IEntity
    {
        /// <summary>Jedinstveni identifikator instruktora (PK). Dozvoljene vrednosti: auto-generisan PK; ne validira se.</summary>
        public int InstruktorId { get; set; }

        /// <summary>Ime instruktora. Obavezno. Dozvoljene vrednosti: obavezno; 1–50 karaktera. Validira <see cref="Common.Validation.InstruktorValidator"/>.</summary>
        public string Ime { get; set; }

        /// <summary>Prezime instruktora. Obavezno. Dozvoljene vrednosti: obavezno; 1–50 karaktera. Validira <see cref="Common.Validation.InstruktorValidator"/>.</summary>
        public string Prezime { get; set; }

        /// <summary>Jedinstveni matični broj građana instruktora. Dozvoljene vrednosti: obavezno; tačno 13 cifara. Validira <see cref="Common.Validation.InstruktorValidator"/>.</summary>
        public string JMBG { get; set; }

        /// <summary>Kontakt telefon instruktora. Dozvoljene vrednosti: obavezno; 6–20 cifara uz opcioni vodeći '+'. Validira <see cref="Common.Validation.InstruktorValidator"/>.</summary>
        public string Telefon { get; set; }

        /// <summary>Email adresa instruktora. Dozvoljene vrednosti: obavezno; ispravan email format. Validira <see cref="Common.Validation.InstruktorValidator"/>.</summary>
        public string Email { get; set; }

        /// <summary>Datum kada je instruktor zaposlen u auto-školi. Dozvoljene vrednosti: obavezno; ne sme biti podrazumevani datum. Validira <see cref="Common.Validation.InstruktorValidator"/>.</summary>
        public DateTime DatumZaposlenja { get; set; }

        /// <summary>Označava da li je instruktor trenutno aktivan (zaposlen). Dozvoljene vrednosti: true ili false; ne validira se.</summary>
        public bool Aktivan { get; set; }

        /// <summary>Spojeno ime i prezime za prikaz u korisničkom interfejsu.</summary>
        public string PunoIme => $"{Ime} {Prezime}".Trim();

        /// <inheritdoc/>
        public string TableName => "Instruktor";

        /// <inheritdoc/>
        public string Values =>
            $"'{Ime}', '{Prezime}', '{JMBG}', '{Telefon}', '{Email}', '{DatumZaposlenja:yyyy-MM-dd}',{(Aktivan ? 1 : 0)}";

        /// <inheritdoc/>
        public string Query => $"JMBG = '{JMBG}'";

        /// <inheritdoc/>
        public string TableKeyColumn => "InstruktorId";

        /// <inheritdoc/>
        public string TableKeyQuery =>
            $"{TableKeyColumn} = {InstruktorId}";

        /// <inheritdoc/>
        public string Update =>
            $"UPDATE Instruktor SET " +
            $"Ime = '{Ime}', " +
            $"Prezime = '{Prezime}', " +
            $"JMBG = '{JMBG}', " +
            $"Telefon = '{Telefon}', " +
            $"Email = '{Email}', " +
            $"DatumZaposlenja = '{DatumZaposlenja:yyyy-MM-dd}', " +
            $"Aktivan = {(Aktivan ? 1 : 0)} " +
            $"WHERE InstruktorId = {InstruktorId}";


        /// <inheritdoc/>
        public List<IEntity> GetReaderList(DbDataReader reader)
        {
            var list = new List<IEntity>();
            while (reader.Read())
            {
                list.Add(new Instruktor
                {
                    InstruktorId = (int)reader["InstruktorId"],
                    Ime = reader["Ime"].ToString(),
                    Prezime = reader["Prezime"].ToString(),
                    JMBG = reader["JMBG"].ToString(),
                    Telefon = reader["Telefon"].ToString(),
                    Email = reader["Email"].ToString(),
                    DatumZaposlenja = (DateTime)reader["DatumZaposlenja"],
                    Aktivan = (bool)reader["Aktivan"]
                });
            }
            return list;
        }

        /// <inheritdoc/>
        public IEntity GetReaderResult(DbDataReader reader)
        {
            if (reader.Read())
            {
                return new Instruktor
                {
                    InstruktorId = (int)reader["InstruktorId"],
                    Ime = reader["Ime"].ToString(),
                    Prezime = reader["Prezime"].ToString(),
                    JMBG = reader["JMBG"].ToString(),
                    Telefon = reader["Telefon"].ToString(),
                    Email = reader["Email"].ToString(),
                    DatumZaposlenja = (DateTime)reader["DatumZaposlenja"],
                    Aktivan = (bool)reader["Aktivan"]
                };
            }
            return null;
        }

        /// <summary>Tekstualna reprezentacija instruktora: "Ime Prezime (JMBG)" ili samo puno ime ako JMBG nije postavljen.</summary>
        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(JMBG) ? PunoIme : $"{PunoIme} ({JMBG})";
        }
    }
}
