using Common.Domain;
using Common.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.SystemOperation
{
    internal class KreirajInstruktoraSO : SystemOperationBase
    {
        private Instruktor argument;

        public KreirajInstruktoraSO(Instruktor argument)
        {
            this.argument = argument;
        }

        public IEntity Result { get; internal set; }

        protected override void ExecuteConcreteOperation()
        {
            Validate(argument);
            Result = _broker.Add(argument);
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
