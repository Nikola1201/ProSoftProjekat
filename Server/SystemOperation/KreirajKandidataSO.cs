using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.SystemOperation
{
    internal class KreirajKandidataSO : SystemOperationBase
    {
        private readonly Kandidat _kandidat;
        public IEntity Result { get; set; }

        public KreirajKandidataSO(Kandidat kandidat)
        {
            _kandidat = kandidat;
        }
        protected override void ExecuteConcreteOperation()
        {
            Validate(_kandidat);
            Result = _broker.Add(_kandidat);
        }

        private void Validate(Kandidat kandidat)
        {
            if (kandidat == null)
            {
                throw new Exception("Podaci o kandidatu nisu prosledjeni.");
            }

            if (string.IsNullOrWhiteSpace(kandidat.Ime))
            {
                throw new Exception("Ime kandidata je obavezno.");
            }

            if (string.IsNullOrWhiteSpace(kandidat.Prezime))
            {
                throw new Exception("Prezime kandidata je obavezno.");
            }

            if (string.IsNullOrWhiteSpace(kandidat.JMBG))
            {
                throw new Exception("JMBG kandidata je obavezan.");
            }

            if (kandidat.JMBG.Length != 13 || !kandidat.JMBG.All(char.IsDigit))
            {
                throw new Exception("JMBG mora da sadrzi tacno 13 cifara.");
            }

            if (string.IsNullOrWhiteSpace(kandidat.Telefon))
            {
                throw new Exception("Telefon kandidata je obavezan.");
            }

            if (string.IsNullOrWhiteSpace(kandidat.Email))
            {
                throw new Exception("Email kandidata je obavezan.");
            }

            if (string.IsNullOrWhiteSpace(kandidat.Adresa))
            {
                throw new Exception("Adresa kandidata je obavezna.");
            }

            if (kandidat.DatumUpisa.Date > DateTime.Now.Date)
            {
                throw new Exception("Datum upisa ne moze biti u buducnosti.");
            }

            List<Kandidat> kandidati = _broker.GetAll(new Kandidat()).Cast<Kandidat>().ToList();
            if (kandidati.Any(k => k.JMBG == kandidat.JMBG))
            {
                throw new Exception("Kandidat sa unetim JMBG vec postoji u sistemu.");
            }

            if (kandidati.Any(k => string.Equals(k.Email, kandidat.Email, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception("Kandidat sa unetom email adresom vec postoji u sistemu.");
            }
        }
    }
}
