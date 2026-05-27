using Common.Domain;
using DBBroker;
using System.Collections.Generic;
using System.Linq;

namespace SystemOperations
{
    public class VratiSvePaketeObukeSO : SystemOperationBase
    {
        public List<PaketObuke> Result { get; private set; }

        public VratiSvePaketeObukeSO() : base() { }
        public VratiSvePaketeObukeSO(IBroker? broker) : base(broker) { }

        protected override void ExecuteConcreteOperation()
        {
            Result = _broker.GetAll(new PaketObuke()).Cast<PaketObuke>().ToList();
        }
    }
}
