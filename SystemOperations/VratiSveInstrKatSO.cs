using Common.Domain;
using DBBroker;
using System.Collections.Generic;
using System.Linq;

namespace SystemOperations
{
    public class VratiSveInstrKatSO : SystemOperationBase
    {
        public List<InstrKat> Result { get; internal set; }

        public VratiSveInstrKatSO() : base() { }
        public VratiSveInstrKatSO(IBroker? broker) : base(broker) { }

        protected override void ExecuteConcreteOperation()
        {
            Result = _broker.GetAll(new InstrKat()).Cast<InstrKat>().ToList();
        }
    }
}
