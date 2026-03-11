using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.SystemOperation
{
    internal class UpisiKandidataSO : SystemOperationBase
    {
        private readonly Upis _upis;

        public UpisiKandidataSO(Upis upis)
        {
            _upis = upis;
        }

        public IEntity Result { get; private set; }

        protected override void ExecuteConcreteOperation()
        {
            Validate(_upis);
            Result = _broker.Add(_upis);
        }

        private void Validate(Upis upis)
        {
            if (upis == null)
            {
                throw new Exception("Podaci o upisu nisu prosledjeni.");
            }

            if (upis.KandidatId <= 0)
            {
                throw new Exception("Kandidat za upis nije izabran.");
            }

            if (upis.PaketId <= 0)
            {
                throw new Exception("Paket obuke nije izabran.");
            }

            if (upis.DatumUpisa.Date > DateTime.Now.Date)
            {
                throw new Exception("Datum upisa ne moze biti u buducnosti.");
            }

            if (string.IsNullOrWhiteSpace(upis.Status))
            {
                throw new Exception("Status upisa je obavezan.");
            }

            List<Upis> postojeciUpisi = _broker.GetAll(new Upis()).Cast<Upis>().ToList();
            Kandidat kandidatZaUpis = (Kandidat)_broker.GetEntityByID(upis.Kandidat);
            PaketObuke paketObuke = (PaketObuke)_broker.GetEntityByID(upis.Paket);

            if (kandidatZaUpis == null)
            {
                throw new Exception("Izabrani kandidat ne postoji u sistemu.");
            }
            if (!kandidatZaUpis.Aktivan)
            {
                throw new Exception("Izabrani kandidat nije aktivan.");
            }
            if (paketObuke == null)
            {
                throw new Exception("Izabrani paket obuke ne postoji u sistemu.");
            }

            if (kandidatZaUpis.Kategorija.KategorijaID != paketObuke.Kategorija.KategorijaID)
            {
                throw new Exception("Izabrani paket obuke ne odgovara kategoriji kandidata.");
            }

            bool imaAktivanUpis = postojeciUpisi.Any(u =>
                u.KandidatId == upis.KandidatId &&
                string.Equals(u.Status, "aktivan", StringComparison.OrdinalIgnoreCase));

            if (imaAktivanUpis)
            {
                throw new Exception("Kandidat je vec aktivno upisan na obuku.");
            }

            upis.Status = upis.Status.Trim().ToLower();
            upis.Kandidat = kandidatZaUpis;
            upis.Paket = paketObuke;

        }
    }
}