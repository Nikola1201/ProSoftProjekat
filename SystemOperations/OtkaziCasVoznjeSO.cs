using Common.Domain;
using Common.Validation;
using DBBroker;
using System;
using System.Linq;

namespace SystemOperations
{
    public class OtkaziCasVoznjeSO : SystemOperationBase
    {
        private readonly CasVoznje _argument;

        public OtkaziCasVoznjeSO(CasVoznje argument) : this(argument, null) { }
        public OtkaziCasVoznjeSO(CasVoznje argument, IBroker? broker) : base(broker)
        {
            _argument = argument;
        }

        public IEntity Result { get; private set; }

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
