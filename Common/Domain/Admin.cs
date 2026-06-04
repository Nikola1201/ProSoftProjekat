using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Common.Domain
{
    /// <summary>Predstavlja administratora sistema auto-škole.</summary>
    [Serializable]
    public class Admin : IEntity
    {
        /// <summary>Jedinstveni identifikator administratora (PK). Dozvoljene vrednosti: auto-generisan PK; ne validira se.</summary>
        public int AdminId { get; set; }

        /// <summary>Ime administratora. Dozvoljene vrednosti: obavezno; 1–50 karaktera. Validira <see cref="Common.Validation.AdminValidator"/>.</summary>
        public string Ime { get; set; }

        /// <summary>Prezime administratora. Dozvoljene vrednosti: obavezno; 1–50 karaktera. Validira <see cref="Common.Validation.AdminValidator"/>.</summary>
        public string Prezime { get; set; }

        /// <summary>Korisničko ime za prijavu na sistem. Dozvoljene vrednosti: obavezno; 3–30 karaktera; bez razmaka. Validira <see cref="Common.Validation.AdminValidator"/>.</summary>
        public string Username { get; set; }

        /// <summary>Lozinka administratora. Pažnja: čuva se u čistom tekstu — videti CLAUDE.md §11 #6. Dozvoljene vrednosti: obavezno; 6–100 karaktera. Validira <see cref="Common.Validation.AdminValidator"/>.</summary>
        public string Lozinka { get; set; }

        /// <summary>Email adresa administratora. Dozvoljene vrednosti: obavezno; ispravan email format. Validira <see cref="Common.Validation.AdminValidator"/>.</summary>
        public string Email { get; set; }

        /// <summary>Datum i vreme kada je nalog kreiran. Dozvoljene vrednosti: obavezno; ne sme biti podrazumevani datum. Validira <see cref="Common.Validation.AdminValidator"/>.</summary>
        public DateTime DatumKreiranja { get; set; }

        /// <inheritdoc/>
        public string TableName => "Admin";

        /// <inheritdoc/>
        public string Values =>
            $"'{Ime}', '{Prezime}', '{Username}', '{Lozinka}', '{Email}'";

        /// <inheritdoc/>
        public string Query =>
            $"[Username] = '{Username}' and [Lozinka] = '{Lozinka}'";

        /// <inheritdoc/>
        public string TableKeyColumn => "AdminId";

        /// <inheritdoc/>
        public string TableKeyQuery => $"{TableKeyColumn} = {AdminId}";

        /// <inheritdoc/>
        public string Update =>
            $"UPDATE Admin SET " +
            $"Ime = '{Ime}', " +
            $"Prezime = '{Prezime}', " +
            $"Username = '{Username}', " +
            $"Lozinka = '{Lozinka}', " +
            $"Email = '{Email}' " +
            $"WHERE AdminId = {AdminId}";

        /// <inheritdoc/>
        public List<IEntity> GetReaderList(DbDataReader reader)
        {
            var list = new List<IEntity>();
            Admin a = new Admin();
            while (reader.Read())
            {
                list.Add(new Admin
                    {
                        AdminId = (int)reader["AdminId"],
                        Ime = reader["Ime"].ToString(),
                        Prezime = reader["Prezime"].ToString(),
                        Username = reader["Username"].ToString(),
                        Lozinka = reader["Lozinka"].ToString(),
                        Email = reader["Email"].ToString(),
                        DatumKreiranja = (DateTime)reader["DatumKreiranja"]
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
                return new Admin
                {
                    AdminId = (int)reader["AdminId"],
                    Ime = reader["Ime"].ToString(),
                    Prezime = reader["Prezime"].ToString(),
                    Username = reader["Username"].ToString(),
                    Lozinka = reader["Lozinka"].ToString(),
                    Email = reader["Email"].ToString(),
                    DatumKreiranja = (DateTime)reader["DatumKreiranja"]
                };
            }
            return null;
        }
    }
}
