using Common.Domain;
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
                throw new Exception("Podaci o instruktoru nisu prosledjeni.");
            }

            if (string.IsNullOrWhiteSpace(argument.Ime))
            {
                throw new Exception("Ime instruktora je obavezno.");
            }

            if (string.IsNullOrWhiteSpace(argument.Prezime))
            {
                throw new Exception("Prezime instruktora je obavezno.");
            }

            if (string.IsNullOrWhiteSpace(argument.JMBG))
            {
                throw new Exception("JMBG instruktora je obavezan.");
            }

            if (argument.JMBG.Length != 13 || !argument.JMBG.All(char.IsDigit))
            {
                throw new Exception("JMBG mora da sadrzi tacno 13 cifara.");
            }

            if (string.IsNullOrWhiteSpace(argument.Telefon))
            {
                throw new Exception("Telefon instruktora je obavezan.");
            }

            if (string.IsNullOrWhiteSpace(argument.Email))
            {
                throw new Exception("Email instruktora je obavezan.");
            }

            if (argument.DatumZaposlenja.Date > DateTime.Now.Date)
            {
                throw new Exception("Datum zaposlenja ne moze biti u buducnosti.");
            }

            List<Instruktor> instruktori = _broker.GetAll(new Instruktor()).Cast<Instruktor>().ToList();
            if (instruktori.Any(i => i.JMBG == argument.JMBG))
            {
                throw new Exception("Instruktor sa unetim JMBG vec postoji u sistemu.");
            }

            if (instruktori.Any(i => string.Equals(i.Email, argument.Email, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception("Instruktor sa unetom email adresom vec postoji u sistemu.");
            }
        }
    }
}
