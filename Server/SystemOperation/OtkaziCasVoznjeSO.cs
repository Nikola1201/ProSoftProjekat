using Common.Domain;
using System;
using System.Linq;

namespace Server.SystemOperation
{
    internal class OtkaziCasVoznjeSO : SystemOperationBase
    {
        private readonly CasVoznje _argument;

        public OtkaziCasVoznjeSO(CasVoznje argument)
        {
            _argument = argument;
        }

        public IEntity Result { get; private set; }

        protected override void ExecuteConcreteOperation()
        {
            if (_argument == null || _argument.CasId <= 0)
            {
                throw new Exception("Izabrani cas nije validan za otkazivanje.");
            }

            CasVoznje postojeci = _broker.GetAll(new CasVoznje())
                .Cast<CasVoznje>()
                .FirstOrDefault(c => c.CasId == _argument.CasId);

            if (postojeci == null)
            {
                throw new Exception("Izabrani cas ne postoji u sistemu.");
            }

            if (string.Equals(postojeci.Status, "otkazan", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Izabrani cas je vec otkazan.");
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
