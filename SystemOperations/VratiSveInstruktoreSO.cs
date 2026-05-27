using Common.Domain;
using DBBroker;
using System.Collections.Generic;
using System.Linq;

namespace SystemOperations
{
    public class VratiSveInstruktoreSO : SystemOperationBase
    {
        public List<Instruktor> Result { get; internal set; }

        public VratiSveInstruktoreSO() : base() { }
        public VratiSveInstruktoreSO(IBroker? broker) : base(broker) { }

        protected override void ExecuteConcreteOperation()
        {
           Result = _broker.GetAll(new Instruktor()).Cast<Instruktor>().ToList();
        }
    }
}