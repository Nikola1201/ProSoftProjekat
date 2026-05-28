using Common.Domain;
using Common.DTO.Izvestaji;
using Common.Validation;
using DBBroker;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemOperations
{
    /// <summary>
    /// Sistemska operacija za evidentiranje ispita kandidata.
    /// Validira zahtev, proverava egzistenciju kandidata i upisa, sprečava duplikate
    /// i automatski ažurira status upisa na <c>polozio</c> kada kandidat položi oba tipa ispita.
    /// </summary>
    public class EvidentirajIspitSO : SystemOperationBase
    {
        private static readonly HashSet<string> DozvoljeniTipovi = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "teorijski",
            "prakticni"
        };

        private static readonly HashSet<string> DozvoljeniRezultati = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "polozio",
            "pao",
            "nije_pristupio"
        };

        private readonly EvidentirajIspitRequest _request;

        /// <summary>Konstruktor za produkcijsku upotrebu (podrazumevani broker).</summary>
        /// <param name="request">Podaci o ispitu koji se evidentira.</param>
        public EvidentirajIspitSO(EvidentirajIspitRequest request) : this(request, null) { }

        /// <summary>Konstruktor sa injektovanim brokerom (test-friendly).</summary>
        /// <param name="request">Podaci o ispitu koji se evidentira.</param>
        /// <param name="broker">Broker za pristup bazi.</param>
        public EvidentirajIspitSO(EvidentirajIspitRequest request, IBroker? broker) : base(broker)
        {
            _request = request;
        }

        /// <summary>Rezultat operacije — evidentirani ispit i ažurirani status upisa.</summary>
        public EvidentirajIspitResponse Result { get; private set; }

        /// <inheritdoc/>
        protected override void ExecuteConcreteOperation()
        {
            ValidateRequest(_request);

            Kandidat kandidat = (Kandidat)_broker.GetEntityByID(new Kandidat { KandidatId = _request.KandidatId });
            if (kandidat == null)
            {
                throw new ValidacijaException("Kandidat ne postoji.");
            }

            Upis najnovijiUpis = _broker.GetEntitiesByQuery(new Upis { KandidatId = _request.KandidatId })
                .Cast<Upis>()
                .OrderByDescending(u => u.DatumUpisa)
                .ThenByDescending(u => u.UpisId)
                .FirstOrDefault();

            if (najnovijiUpis == null)
            {
                throw new ValidacijaException("Kandidat nema upis. Prvo upisite kandidata na paket.");
            }

            List<Ispit> ispitiZaUpis = _broker.GetEntitiesByQuery(new Ispit { UpisId = najnovijiUpis.UpisId })
                .Cast<Ispit>()
                .ToList();

            bool postojiIstiTipIstiDan = ispitiZaUpis.Any(i =>
                string.Equals(i.Tip, _request.Tip, StringComparison.OrdinalIgnoreCase)
                && i.DatumIspita.Date == _request.DatumIspita.Date);
            if (postojiIstiTipIstiDan)
            {
                throw new ValidacijaException("Za izabrani datum vec postoji evidentiran isti tip ispita za aktivni upis kandidata.");
            }

            bool vecPolozenIstiTip = ispitiZaUpis.Any(i =>
                string.Equals(i.Tip, _request.Tip, StringComparison.OrdinalIgnoreCase)
                && string.Equals(i.Rezultat, "polozio", StringComparison.OrdinalIgnoreCase));
            if (string.Equals(_request.Rezultat, "polozio", StringComparison.OrdinalIgnoreCase) && vecPolozenIstiTip)
            {
                throw new ValidacijaException("Kandidat je vec polozio izabrani tip ispita i ne moze ponovo biti evidentiran kao polozio.");
            }

            Ispit noviIspit = new Ispit
            {
                UpisId = najnovijiUpis.UpisId,
                DatumIspita = _request.DatumIspita.Date,
                Tip = _request.Tip,
                Rezultat = _request.Rezultat,
                Napomena = _request.Napomena ?? string.Empty
            };

            _broker.Add(noviIspit);
            ispitiZaUpis.Add(noviIspit);

            bool imaPolozenTeorijski = ispitiZaUpis.Any(i =>
                string.Equals(i.Tip, "teorijski", StringComparison.OrdinalIgnoreCase)
                && string.Equals(i.Rezultat, "polozio", StringComparison.OrdinalIgnoreCase));
            bool imaPolozenPrakticni = ispitiZaUpis.Any(i =>
                string.Equals(i.Tip, "prakticni", StringComparison.OrdinalIgnoreCase)
                && string.Equals(i.Rezultat, "polozio", StringComparison.OrdinalIgnoreCase));

            bool statusPromenjen = false;
            string noviStatus = najnovijiUpis.Status;

            if (imaPolozenTeorijski && imaPolozenPrakticni &&
                !string.Equals(najnovijiUpis.Status, "polozio", StringComparison.OrdinalIgnoreCase))
            {
                najnovijiUpis.Status = "polozio";
                _broker.Update(najnovijiUpis);
                statusPromenjen = true;
                noviStatus = "polozio";
            }

            Result = new EvidentirajIspitResponse
            {
                Ispit = noviIspit,
                UpisId = najnovijiUpis.UpisId,
                UpisStatus = noviStatus,
                StatusPromenjen = statusPromenjen,
                Poruka = "Ispit je uspesno evidentiran."
            };
        }

        /// <summary>
        /// Proverava obaveznost i ispravnost svih polja zahteva za evidentiranje ispita:
        /// kandidat, datum, tip i rezultat ispita, kao i dužinu napomene.
        /// </summary>
        /// <param name="request">Zahtev za validaciju.</param>
        /// <exception cref="ValidacijaException">Baca se ako neko od ograničenja nije zadovoljeno.</exception>
        private void ValidateRequest(EvidentirajIspitRequest request)
        {
            if (request == null)
            {
                throw new ValidacijaException("Podaci o ispitu nisu prosledjeni.");
            }

            if (request.KandidatId <= 0)
            {
                throw new ValidacijaException("Kandidat je obavezan.");
            }

            if (request.DatumIspita == DateTime.MinValue)
            {
                throw new ValidacijaException("Datum ispita je obavezan.");
            }

            if (request.DatumIspita.Date > DateTime.Now.Date)
            {
                throw new ValidacijaException("Datum ispita ne moze biti u buducnosti.");
            }

            request.Tip = (request.Tip ?? string.Empty).Trim().ToLower();
            request.Rezultat = (request.Rezultat ?? string.Empty).Trim().ToLower();
            request.Napomena = (request.Napomena ?? string.Empty).Trim();

            if (!DozvoljeniTipovi.Contains(request.Tip))
            {
                throw new ValidacijaException("Tip ispita mora biti teorijski ili prakticni.");
            }

            if (!DozvoljeniRezultati.Contains(request.Rezultat))
            {
                throw new ValidacijaException("Rezultat ispita nije validan.");
            }

            if (request.Napomena.Length > 500)
            {
                throw new ValidacijaException("Napomena moze imati najvise 500 karaktera.");
            }
        }
    }
}
