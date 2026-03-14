using Common.Domain;
using System;
using System.Collections.Generic;

namespace Server.SystemOperation
{
    internal class EvidentirajIspitSO : SystemOperationBase
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

        public EvidentirajIspitSO(EvidentirajIspitRequest request)
        {
            _request = request;
        }

        public EvidentirajIspitResponse Result { get; private set; }

        protected override void ExecuteConcreteOperation()
        {
            ValidateRequest(_request);

            if (!_broker.KandidatPostoji(_request.KandidatId))
            {
                throw new Exception("Kandidat ne postoji.");
            }

            Upis najnovijiUpis = _broker.GetNajnovijiUpisZaKandidata(_request.KandidatId);
            if (najnovijiUpis == null)
            {
                throw new Exception("Kandidat nema upis. Prvo upisite kandidata na paket.");
            }

            if (_broker.PostojiIspitIstogTipaIstogDana(najnovijiUpis.UpisId, _request.Tip, _request.DatumIspita))
            {
                throw new Exception("Za izabrani datum vec postoji evidentiran isti tip ispita za aktivni upis kandidata.");
            }

            if (string.Equals(_request.Rezultat, "polozio", StringComparison.OrdinalIgnoreCase)
                && _broker.ImaPolozenIspitZaTip(najnovijiUpis.UpisId, _request.Tip))
            {
                throw new Exception("Kandidat je vec polozio izabrani tip ispita i ne moze ponovo biti evidentiran kao polozio.");
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

            bool imaPolozenTeorijski = _broker.ImaPolozenIspitZaTip(najnovijiUpis.UpisId, "teorijski");
            bool imaPolozenPrakticni = _broker.ImaPolozenIspitZaTip(najnovijiUpis.UpisId, "prakticni");

            bool statusPromenjen = false;
            string noviStatus = najnovijiUpis.Status;

            if (imaPolozenTeorijski && imaPolozenPrakticni &&
                !string.Equals(najnovijiUpis.Status, "polozio", StringComparison.OrdinalIgnoreCase))
            {
                _broker.AzurirajStatusUpisa(najnovijiUpis.UpisId, "polozio");
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

        private void ValidateRequest(EvidentirajIspitRequest request)
        {
            if (request == null)
            {
                throw new Exception("Podaci o ispitu nisu prosledjeni.");
            }

            if (request.KandidatId <= 0)
            {
                throw new Exception("Kandidat je obavezan.");
            }

            if (request.DatumIspita == DateTime.MinValue)
            {
                throw new Exception("Datum ispita je obavezan.");
            }

            if (request.DatumIspita.Date > DateTime.Now.Date)
            {
                throw new Exception("Datum ispita ne moze biti u buducnosti.");
            }

            request.Tip = (request.Tip ?? string.Empty).Trim().ToLower();
            request.Rezultat = (request.Rezultat ?? string.Empty).Trim().ToLower();
            request.Napomena = (request.Napomena ?? string.Empty).Trim();

            if (!DozvoljeniTipovi.Contains(request.Tip))
            {
                throw new Exception("Tip ispita mora biti teorijski ili prakticni.");
            }

            if (!DozvoljeniRezultati.Contains(request.Rezultat))
            {
                throw new Exception("Rezultat ispita nije validan.");
            }

            if (request.Napomena.Length > 500)
            {
                throw new Exception("Napomena moze imati najvise 500 karaktera.");
            }
        }
    }
}
