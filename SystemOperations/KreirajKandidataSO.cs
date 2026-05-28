using Common.Domain;
using Common.Validation;
using DBBroker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemOperations
{
    /// <summary>
    /// Sistemska operacija za kreiranje novog kandidata.
    /// Validira sve obavezne podatke i jedinstvenost JMBG i email adrese pre upisa.
    /// </summary>
    public class KreirajKandidataSO : SystemOperationBase
    {
        private readonly Kandidat _kandidat;

        /// <summary>Rezultat operacije — sačuvan kandidat.</summary>
        public IEntity Result { get; set; }

        /// <summary>Konstruktor za produkcijsku upotrebu (podrazumevani broker).</summary>
        /// <param name="kandidat">Kandidat za upis.</param>
        public KreirajKandidataSO(Kandidat kandidat) : this(kandidat, null) { }

        /// <summary>Konstruktor sa injektovanim brokerom (test-friendly).</summary>
        /// <param name="kandidat">Kandidat za upis.</param>
        /// <param name="broker">Broker za pristup bazi.</param>
        public KreirajKandidataSO(Kandidat kandidat, IBroker? broker) : base(broker)
        {
            _kandidat = kandidat;
        }

        /// <inheritdoc/>
        protected override void ExecuteConcreteOperation()
        {
            Validate(_kandidat);
            Result = _broker.Add(_kandidat);
        }

        /// <summary>
        /// Proverava strukturna i vrednosna ograničenja kandidata,
        /// kao i jedinstvenost JMBG i email adrese u sistemu.
        /// </summary>
        /// <param name="kandidat">Kandidat za validaciju.</param>
        /// <exception cref="ValidacijaException">Baca se ako neko od ograničenja nije zadovoljeno.</exception>
        private void Validate(Kandidat kandidat)
        {
            if (kandidat == null)
            {
                throw new ValidacijaException("Podaci o kandidatu nisu prosledjeni.");
            }

            if (string.IsNullOrWhiteSpace(kandidat.Ime))
            {
                throw new ValidacijaException("Ime kandidata je obavezno.");
            }

            if (string.IsNullOrWhiteSpace(kandidat.Prezime))
            {
                throw new ValidacijaException("Prezime kandidata je obavezno.");
            }

            if (string.IsNullOrWhiteSpace(kandidat.JMBG))
            {
                throw new ValidacijaException("JMBG kandidata je obavezan.");
            }

            if (kandidat.JMBG.Length != 13 || !kandidat.JMBG.All(char.IsDigit))
            {
                throw new ValidacijaException("JMBG mora da sadrzi tacno 13 cifara.");
            }

            if (string.IsNullOrWhiteSpace(kandidat.Telefon))
            {
                throw new ValidacijaException("Telefon kandidata je obavezan.");
            }

            if (string.IsNullOrWhiteSpace(kandidat.Email))
            {
                throw new ValidacijaException("Email kandidata je obavezan.");
            }

            if (string.IsNullOrWhiteSpace(kandidat.Adresa))
            {
                throw new ValidacijaException("Adresa kandidata je obavezna.");
            }

            if (kandidat.DatumUpisa.Date > DateTime.Now.Date)
            {
                throw new ValidacijaException("Datum upisa ne moze biti u buducnosti.");
            }

            List<Kandidat> kandidati = _broker.GetAll(new Kandidat()).Cast<Kandidat>().ToList();
            if (kandidati.Any(k => k.JMBG == kandidat.JMBG))
            {
                throw new ValidacijaException("Kandidat sa unetim JMBG vec postoji u sistemu.");
            }

            if (kandidati.Any(k => string.Equals(k.Email, kandidat.Email, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ValidacijaException("Kandidat sa unetom email adresom vec postoji u sistemu.");
            }
        }
    }
}
