using Common.Domain;
using DBBroker;
using System.Collections.Generic;
using System.Linq;

namespace SystemOperations
{
    public class VratiSveVozilaSO : SystemOperationBase
    {
        public List<Vozilo> Result { get; private set; }

        public VratiSveVozilaSO() : base() { }
        public VratiSveVozilaSO(IBroker? broker) : base(broker) { }

        protected override void ExecuteConcreteOperation()
        {
            Result = _broker.GetAll(new Vozilo())
                .Cast<Vozilo>()
                .Where(v => v.Aktivno)
                .ToList();
        }
    }
}
