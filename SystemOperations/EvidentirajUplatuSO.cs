using Common.Domain;
using Common.DTO.Izvestaji;
using Common.Validation;
using DBBroker;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SystemOperations
{
    /// <summary>
    /// Sistemska operacija za evidentiranje uplate po osnovu upisa.
    /// Validira zahtev, pronalazi ciljni upis (eksplicitno ili po poslednjem aktivnom),
    /// proverava da iznos ne prelazi preostalo dugovanje i kreira novi zapis o plaćanju.
    /// </summary>
    public class EvidentirajUplatuSO : SystemOperationBase
    {
        private static readonly HashSet<string> DozvoljeniNaciniPlacanja = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "gotovina",
            "kartica",
            "transfer"
        };

        private readonly EvidentirajUplatuRequest _request;

        /// <summary>Konstruktor za produkcijsku upotrebu (podrazumevani broker).</summary>
        /// <param name="request">Podaci o uplati koja se evidentira.</param>
        public EvidentirajUplatuSO(EvidentirajUplatuRequest request) : this(request, null) { }

        /// <summary>Konstruktor sa injektovanim brokerom (test-friendly).</summary>
        /// <param name="request">Podaci o uplati koja se evidentira.</param>
        /// <param name="broker">Broker za pristup bazi.</param>
        public EvidentirajUplatuSO(EvidentirajUplatuRequest request, IBroker? broker) : base(broker)
        {
            _request = request;
        }

        /// <summary>Rezultat operacije — evidentirano plaćanje i preostalo dugovanje.</summary>
        public EvidentirajUplatuResponse Result { get; private set; }

        /// <inheritdoc/>
        protected override void ExecuteConcreteOperation()
        {
            ValidateRequest(_request);

            Upis ciljniUpis;
            if (_request.UpisId.HasValue && _request.UpisId.Value > 0)
            {
                ciljniUpis = (Upis)_broker.GetEntityByID(new Upis { UpisId = _request.UpisId.Value });
                if (ciljniUpis == null || ciljniUpis.KandidatId != _request.KandidatId)
                {
                    throw new ValidacijaException("Izabrani upis ne postoji za zadatog kandidata.");
                }
            }
            else
            {
                Kandidat kandidat = (Kandidat)_broker.GetEntityByID(new Kandidat { KandidatId = _request.KandidatId });
                if (kandidat == null)
                {
                    throw new ValidacijaException("Kandidat ne postoji.");
                }
                ciljniUpis = _broker.GetEntitiesByQuery(new Upis { KandidatId = _request.KandidatId })
                    .Cast<Upis>()
                    .OrderByDescending(u => u.DatumUpisa)
                    .ThenByDescending(u => u.UpisId)
                    .FirstOrDefault();
                if (ciljniUpis == null)
                {
                    throw new ValidacijaException("Kandidat nema upis. Prvo upisite kandidata na paket.");
                }
            }

            decimal preostaloPre = IzracunajPreostaloDugovanje(ciljniUpis);

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

        /// <summary>
        /// Izračunava preostalo dugovanje za dati upis:
        /// cena paketa umanjena za sumu svih dosadašnjih plaćanja.
        /// </summary>
        /// <param name="upis">Upis za koji se računa dugovanje.</param>
        /// <returns>Preostalo dugovanje u RSD; minimalno <c>0</c>.</returns>
        private decimal IzracunajPreostaloDugovanje(Upis upis)
        {
            PaketObuke paket = (PaketObuke)_broker.GetEntityByID(new PaketObuke { PaketId = upis.PaketId });
            if (paket == null)
            {
                return 0m;
            }

            decimal placeno = _broker.GetEntitiesByQuery(new Placanje { UpisId = upis.UpisId })
                .Cast<Placanje>()
                .Sum(p => p.Iznos);

            decimal preostalo = paket.Cena - placeno;
            return preostalo < 0m ? 0m : preostalo;
        }

        /// <summary>
        /// Proverava obaveznost i ispravnost svih polja zahteva za evidentiranje uplate:
        /// identifikator kandidata/upisa, iznos, datum i način plaćanja, kao i dužinu napomene.
        /// </summary>
        /// <param name="request">Zahtev za validaciju.</param>
        /// <exception cref="ValidacijaException">Baca se ako neko od ograničenja nije zadovoljeno.</exception>
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
