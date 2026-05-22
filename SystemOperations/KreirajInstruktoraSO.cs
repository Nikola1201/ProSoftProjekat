using Common.Domain;
using Common.DTO;
using Common.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemOperations
{
    public class KreirajInstruktoraSO : SystemOperationBase
    {
        private readonly KreirajInstruktoraRequest _request;

        public KreirajInstruktoraSO(KreirajInstruktoraRequest request)
        {
            _request = request;
        }

        public IEntity Result { get; internal set; }

        protected override void ExecuteConcreteOperation()
        {
            if (_request == null)
            {
                throw new ValidacijaException("Podaci za kreiranje instruktora nisu prosledjeni.");
            }

            Validate(_request.Instruktor);
            ValidateKategorija(_request.KategorijaID);

            _broker.Add(_request.Instruktor);

            Instruktor saved = (Instruktor)_broker.GetEntityByQuery(
                new Instruktor { JMBG = _request.Instruktor.JMBG });

            if (saved == null)
            {
                throw new InvalidOperationException("Instruktor nije sacuvan u bazi.");
            }

            _broker.Add(new InstrKat
            {
                InstruktorId = saved.InstruktorId,
                KategorijaID = _request.KategorijaID,
                DatumDodele = DateTime.Now,
                Aktivno = true
            });

            Result = saved;
        }

        private void ValidateKategorija(int kategorijaId)
        {
            if (kategorijaId <= 0)
            {
                throw new ValidacijaException("Kategorija je obavezna.");
            }

            Kategorija k = (Kategorija)_broker.GetEntityByID(new Kategorija { KategorijaID = kategorijaId });
            if (k == null)
            {
                throw new ValidacijaException("Izabrana kategorija ne postoji.");
            }
        }

        private void Validate(Instruktor argument)
        {
            if (argument == null)
            {
                throw new ValidacijaException("Podaci o instruktoru nisu prosledjeni.");
            }

            if (string.IsNullOrWhiteSpace(argument.Ime))
            {
                throw new ValidacijaException("Ime instruktora je obavezno.");
            }

            if (string.IsNullOrWhiteSpace(argument.Prezime))
            {
                throw new ValidacijaException("Prezime instruktora je obavezno.");
            }

            if (string.IsNullOrWhiteSpace(argument.JMBG))
            {
                throw new ValidacijaException("JMBG instruktora je obavezan.");
            }

            if (argument.JMBG.Length != 13 || !argument.JMBG.All(char.IsDigit))
            {
                throw new ValidacijaException("JMBG mora da sadrzi tacno 13 cifara.");
            }

            if (string.IsNullOrWhiteSpace(argument.Telefon))
            {
                throw new ValidacijaException("Telefon instruktora je obavezan.");
            }

            if (string.IsNullOrWhiteSpace(argument.Email))
            {
                throw new ValidacijaException("Email instruktora je obavezan.");
            }

            if (argument.DatumZaposlenja.Date > DateTime.Now.Date)
            {
                throw new ValidacijaException("Datum zaposlenja ne moze biti u buducnosti.");
            }

            List<Instruktor> instruktori = _broker.GetAll(new Instruktor()).Cast<Instruktor>().ToList();
            if (instruktori.Any(i => i.JMBG == argument.JMBG))
            {
                throw new ValidacijaException("Instruktor sa unetim JMBG vec postoji u sistemu.");
            }

            if (instruktori.Any(i => string.Equals(i.Email, argument.Email, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ValidacijaException("Instruktor sa unetom email adresom vec postoji u sistemu.");
            }
        }
    }
}
