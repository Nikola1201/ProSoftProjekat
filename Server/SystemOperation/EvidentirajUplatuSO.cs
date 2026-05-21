using Common.Domain;
using Common.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Server.SystemOperation
{
    internal class EvidentirajUplatuSO : SystemOperationBase
    {
        private static readonly HashSet<string> DozvoljeniNaciniPlacanja = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "gotovina",
            "kartica",
            "transfer"
        };

        private readonly EvidentirajUplatuRequest _request;

        public EvidentirajUplatuSO(EvidentirajUplatuRequest request)
        {
            _request = request;
        }

        public EvidentirajUplatuResponse Result { get; private set; }

        protected override void ExecuteConcreteOperation()
        {
            ValidateRequest(_request);

            Upis ciljniUpis;
            if (_request.UpisId.HasValue && _request.UpisId.Value > 0)
            {
                ciljniUpis = _broker.GetUpisById(_request.UpisId.Value);
                if (ciljniUpis == null || ciljniUpis.KandidatId != _request.KandidatId)
                {
                    throw new ValidacijaException("Izabrani upis ne postoji za zadatog kandidata.");
                }
            }
            else
            {
                if (!_broker.KandidatPostoji(_request.KandidatId))
                {
                    throw new ValidacijaException("Kandidat ne postoji.");
                }
                ciljniUpis = _broker.GetNajnovijiUpisZaKandidata(_request.KandidatId);
                if (ciljniUpis == null)
                {
                    throw new ValidacijaException("Kandidat nema upis. Prvo upisite kandidata na paket.");
                }
            }

            decimal preostaloPre = _broker.GetPreostaloDugovanjeZaUpis(ciljniUpis.UpisId);

            if (_request.Iznos > preostaloPre)
            {
                throw new ValidacijaException(
                    string.Format(CultureInfo.InvariantCulture,
                        "Iznos uplate ({0:N2} RSD) prelazi preostalo dugovanje ({1:N2} RSD).",
                        _request.Iznos, preostaloPre));
            }

            Placanje novoPlacanje = new Placanje
            {
                UpisId = ciljniUpis.UpisId,
                Iznos = _request.Iznos,
                DatumPlacanja = _request.DatumPlacanja.Date,
                NacinPlacanja = _request.NacinPlacanja,
                Napomena = _request.Napomena ?? string.Empty
            };
            _broker.Add(novoPlacanje);

            decimal preostaloPosle = preostaloPre - _request.Iznos;
            if (preostaloPosle < 0m) preostaloPosle = 0m;

            Result = new EvidentirajUplatuResponse
            {
                Placanje = novoPlacanje,
                UpisId = ciljniUpis.UpisId,
                PreostaloDugovanje = preostaloPosle,
                Poruka = "Uplata je uspesno evidentirana."
            };
        }

        private void ValidateRequest(EvidentirajUplatuRequest request)
        {
            if (request == null)
            {
                throw new ValidacijaException("Podaci o uplati nisu prosledjeni.");
            }

            if (request.KandidatId <= 0 && (!request.UpisId.HasValue || request.UpisId.Value <= 0))
            {
                throw new ValidacijaException("Kandidat ili upis su obavezni.");
            }

            if (request.Iznos <= 0m)
            {
                throw new ValidacijaException("Iznos uplate mora biti veci od nule.");
            }

            if (request.DatumPlacanja == DateTime.MinValue)
            {
                throw new ValidacijaException("Datum uplate je obavezan.");
            }

            if (request.DatumPlacanja.Date > DateTime.Now.Date)
            {
                throw new ValidacijaException("Datum uplate ne moze biti u buducnosti.");
            }

            request.NacinPlacanja = (request.NacinPlacanja ?? string.Empty).Trim().ToLower();
            request.Napomena = (request.Napomena ?? string.Empty).Trim();

            if (!DozvoljeniNaciniPlacanja.Contains(request.NacinPlacanja))
            {
                throw new ValidacijaException("Nacin placanja mora biti gotovina, kartica ili transfer.");
            }

            if (request.Napomena.Length > 500)
            {
                throw new ValidacijaException("Napomena moze imati najvise 500 karaktera.");
            }
        }
    }
}
