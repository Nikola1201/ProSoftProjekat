using Common.Domain;
using Common.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemOperations
{
    public class ZakaziCasVoznjeSO : SystemOperationBase
    {
        private readonly CasVoznje _casVoznje;

        public ZakaziCasVoznjeSO(CasVoznje casVoznje)
        {
            _casVoznje = casVoznje;
        }

        public IEntity Result { get; private set; }

        protected override void ExecuteConcreteOperation()
        {
            Validate(_casVoznje);
            Result = _broker.Add(_casVoznje);
        }

        private void Validate(CasVoznje cas)
        {
            if (cas == null)
            {
                throw new ValidacijaException("Podaci o casu voznje nisu prosledjeni.");
            }

            if (cas.UpisId <= 0)
            {
                throw new ValidacijaException("Kandidat nema validan aktivan upis za zakazivanje casa.");
            }

            if (cas.InstruktorId <= 0)
            {
                throw new ValidacijaException("Instruktor nije izabran.");
            }

            if (cas.VoziloId <= 0)
            {
                throw new ValidacijaException("Vozilo nije izabrano.");
            }

            if (cas.TrajanjMin <= 0)
            {
                throw new ValidacijaException("Trajanje casa mora biti vece od 0 minuta.");
            }

            if (cas.DatumCas < DateTime.Now)
            {
                throw new ValidacijaException("Datum i vreme casa ne mogu biti u proslosti.");
            }

            Upis upis = (Upis)_broker.GetEntityByID(new Upis { UpisId = cas.UpisId });
            if (upis == null || !string.Equals(upis.Status, "aktivan", StringComparison.OrdinalIgnoreCase))
            {
                throw new ValidacijaException("Izabrani kandidat nema aktivan upis.");
            }

            Instruktor instruktor = (Instruktor)_broker.GetEntityByID(new Instruktor { InstruktorId = cas.InstruktorId });
            if (instruktor == null || !instruktor.Aktivan)
            {
                throw new ValidacijaException("Izabrani instruktor nije aktivan.");
            }

            Vozilo vozilo = (Vozilo)_broker.GetEntityByID(new Vozilo { VoziloId = cas.VoziloId });
            if (vozilo == null || !vozilo.Aktivno)
            {
                throw new ValidacijaException("Izabrano vozilo nije aktivno.");
            }

            ValidateInstruktorOvlascen(cas.InstruktorId, vozilo.KategorijaID);

            ValidateTerminKonflikt(cas);

            if (string.IsNullOrWhiteSpace(cas.Napomena))
            {
                cas.Napomena = string.Empty;
            }
            else
            {
                cas.Napomena = cas.Napomena.Trim();
            }
            cas.Status = "zakazan";
        }

        private void ValidateInstruktorOvlascen(int instruktorId, int kategorijaId)
        {
            InstrKat veza = (InstrKat)_broker.GetEntityByID(new InstrKat
            {
                InstruktorId = instruktorId,
                KategorijaID = kategorijaId
            });

            if (veza == null || !veza.Aktivno)
            {
                throw new ValidacijaException("Izabrani instruktor nije ovlascen za kategoriju izabranog vozila.");
            }
        }

        private void ValidateTerminKonflikt(CasVoznje noviCas)
        {
            DateTime noviPocetak = noviCas.DatumCas;
            DateTime noviKraj = noviPocetak.AddMinutes(noviCas.TrajanjMin);

            List<CasVoznje> postojeciCasovi = _broker.GetAll(new CasVoznje())
                .Cast<CasVoznje>()
                .Where(c => !string.Equals(c.Status, "otkazan", StringComparison.OrdinalIgnoreCase))
                .ToList();

            bool konfliktInstruktor = postojeciCasovi.Any(c =>
                c.InstruktorId == noviCas.InstruktorId &&
                TerminSePreklapa(noviPocetak, noviKraj, c.DatumCas, c.DatumCas.AddMinutes(c.TrajanjMin)));

            if (konfliktInstruktor)
            {
                throw new ValidacijaException("Instruktor vec ima zakazan cas u izabranom terminu.");
            }

            bool konfliktVozilo = postojeciCasovi.Any(c =>
                c.VoziloId == noviCas.VoziloId &&
                TerminSePreklapa(noviPocetak, noviKraj, c.DatumCas, c.DatumCas.AddMinutes(c.TrajanjMin)));

            if (konfliktVozilo)
            {
                throw new ValidacijaException("Vozilo je vec zauzeto u izabranom terminu.");
            }
        }

        private bool TerminSePreklapa(DateTime noviPocetak, DateTime noviKraj, DateTime postojeciPocetak, DateTime postojeciKraj)
        {
            return noviPocetak < postojeciKraj && postojeciPocetak < noviKraj;
        }
    }
}
