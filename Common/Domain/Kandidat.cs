using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Common.Domain
{
    /// <summary>Predstavlja kandidata upisanog u auto-školu.</summary>
    [Serializable]
    public class Kandidat : IEntity
    {
        /// <summary>Jedinstveni identifikator kandidata (PK).</summary>
        public int KandidatId { get; set; }

        /// <summary>Ime kandidata. Obavezno.</summary>
        public string Ime { get; set; }

        /// <summary>Prezime kandidata. Obavezno.</summary>
        public string Prezime { get; set; }

        /// <summary>Jedinstveni matični broj građana kandidata.</summary>
        public string JMBG { get; set; }

        /// <summary>Kontakt telefon kandidata.</summary>
        public string Telefon { get; set; }

        /// <summary>Email adresa kandidata.</summary>
        public string Email { get; set; }

        /// <summary>Adresa stanovanja kandidata.</summary>
        public string Adresa { get; set; }

        /// <summary>Datum upisa kandidata u auto-školu.</summary>
        public DateTime DatumUpisa { get; set; }

        /// <summary>Označava da li je kandidat trenutno aktivan u sistemu.</summary>
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
