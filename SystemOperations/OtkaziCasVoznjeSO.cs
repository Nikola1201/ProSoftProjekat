using Common.Domain;
using Common.Validation;
using DBBroker;
using System;
using System.Linq;

namespace SystemOperations
{
    /// <summary>
    /// Sistemska operacija za otkazivanje časa vožnje.
    /// Menja status časa u <c>otkazan</c> i opciono beleži napomenu.
    /// Baca izuzetak ako čas ne postoji ili je već otkazan.
    /// </summary>
    public class OtkaziCasVoznjeSO : SystemOperationBase
    {
        private readonly CasVoznje _argument;

        /// <summary>Konstruktor za produkcijsku upotrebu (podrazumevani broker).</summary>
        /// <param name="argument">Čas vožnje za otkazivanje (mora imati validan <c>CasId</c>).</param>
        public OtkaziCasVoznjeSO(CasVoznje argument) : this(argument, null) { }

        /// <summary>Konstruktor sa injektovanim brokerom (test-friendly).</summary>
        /// <param name="argument">Čas vožnje za otkazivanje (mora imati validan <c>CasId</c>).</param>
        /// <param name="broker">Broker za pristup bazi.</param>
        public OtkaziCasVoznjeSO(CasVoznje argument, IBroker? broker) : base(broker)
        {
            _argument = argument;
        }

        /// <summary>Rezultat operacije — ažurirani čas vožnje sa statusom <c>otkazan</c>.</summary>
        public IEntity Result { get; private set; }

        /// <inheritdoc/>
        protected override void ExecuteConcreteOperation()
        {
            if (_argument == null || _argument.CasId <= 0)
            {
                throw new ValidacijaException("Izabrani cas nije validan za otkazivanje.");
            }

            CasVoznje postojeci = _broker.GetAll(new CasVoznje())
                .Cast<CasVoznje>()
                .FirstOrDefault(c => c.CasId == _argument.CasId);

            if (postojeci == null)
            {
                throw new ValidacijaException("Izabrani cas ne postoji u sistemu.");
            }

            if (string.Equals(postojeci.Status, "otkazan", StringComparison.OrdinalIgnoreCase))
            {
                throw new ValidacijaException("Izabrani cas je vec otkazan.");
            }

            postojeci.Status = "otkazan";
            if (!string.IsNullOrWhiteSpace(_argument.Napomena))
            {
                postojeci.Napomena = _argument.Napomena.Trim();
            }
            else if (string.IsNullOrWhiteSpace(postojeci.Napomena))
            {
                postojeci.Napomena = string.Empty;
            }

            _broker.Update(postojeci);
            Result = postojeci;
        }
    }
}
