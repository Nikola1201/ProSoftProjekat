using Common.Domain;
using Common.Validation;
using DBBroker;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemOperations
{
    /// <summary>
    /// Sistemska operacija za upis kandidata na paket obuke.
    /// Validira obavezne podatke, proverava da li kandidat i paket postoje,
    /// i sprečava dupli aktivni upis za istog kandidata.
    /// </summary>
    public class UpisiKandidataSO : SystemOperationBase
    {
        private readonly Upis _upis;

        /// <summary>Konstruktor za produkcijsku upotrebu (podrazumevani broker).</summary>
        /// <param name="upis">Podaci o upisu koji se kreira.</param>
        public UpisiKandidataSO(Upis upis) : this(upis, null) { }

        /// <summary>Konstruktor sa injektovanim brokerom (test-friendly).</summary>
        /// <param name="upis">Podaci o upisu koji se kreira.</param>
        /// <param name="broker">Broker za pristup bazi.</param>
        public UpisiKandidataSO(Upis upis, IBroker? broker) : base(broker)
        {
            _upis = upis;
        }

        /// <summary>Rezultat operacije — sačuvani upis.</summary>
        public IEntity Result { get; private set; }

        /// <inheritdoc/>
        protected override void ExecuteConcreteOperation()
        {
            Validate(_upis);
            Result = _broker.Add(_upis);
        }

        /// <summary>
        /// Proverava obavezna polja upisa, validnost datuma, egzistenciju kandidata i paketa,
        /// i jedinstvenost aktivnog upisa za kandidata.
        /// </summary>
        /// <param name="upis">Upis za validaciju.</param>
        /// <exception cref="ValidacijaException">Baca se ako neko od ograničenja nije zadovoljeno.</exception>
        private void Validate(Upis upis)
        {
            if (upis == null)
            {
                throw new ValidacijaException("Podaci o upisu nisu prosledjeni.");
            }

            if (upis.KandidatId <= 0)
            {
                throw new ValidacijaException("Kandidat za upis nije izabran.");
            }

            if (upis.PaketId <= 0)
            {
                throw new ValidacijaException("Paket obuke nije izabran.");
            }

            if (upis.DatumUpisa.Date > DateTime.Now.Date)
            {
                throw new ValidacijaException("Datum upisa ne moze biti u buducnosti.");
            }

            if (string.IsNullOrWhiteSpace(upis.Status))
            {
                throw new ValidacijaException("Status upisa je obavezan.");
            }

            List<Upis> postojeciUpisi = _broker.GetAll(new Upis()).Cast<Upis>().ToList();
            Kandidat kandidatZaUpis = (Kandidat)_broker.GetEntityByID(upis.Kandidat);
            PaketObuke paketObuke = (PaketObuke)_broker.GetEntityByID(upis.Paket);

            if (kandidatZaUpis == null)
            {
                throw new ValidacijaException("Izabrani kandidat ne postoji u sistemu.");
            }
            if (!kandidatZaUpis.Aktivan)
            {
                throw new ValidacijaException("Izabrani kandidat nije aktivan.");
            }
            if (paketObuke == null)
            {
                throw new ValidacijaException("Izabrani paket obuke ne postoji u sistemu.");
            }

            bool imaAktivanUpis = postojeciUpisi.Any(u =>
                u.KandidatId == upis.KandidatId &&
                string.Equals(u.Status, "aktivan", StringComparison.OrdinalIgnoreCase));

            if (imaAktivanUpis)
            {
                throw new ValidacijaException("Kandidat je vec aktivno upisan na obuku.");
            }

            upis.Status = upis.Status.Trim().ToLower();
            upis.Kandidat = kandidatZaUpis;
            upis.Paket = paketObuke;

        }
    }
}