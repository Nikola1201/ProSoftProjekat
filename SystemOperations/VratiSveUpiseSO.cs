using Common.Domain;
using DBBroker;
using System.Collections.Generic;
using System.Linq;

namespace SystemOperations
{
    public class VratiSveUpiseSO : SystemOperationBase
    {
        public List<Upis> Result { get; private set; }

        public VratiSveUpiseSO() : base() { }
        public VratiSveUpiseSO(IBroker? broker) : base(broker) { }

        protected override void ExecuteConcreteOperation()
        {
            Result = _broker.GetAll(new Upis())
                .Cast<Upis>()
                .ToList();
        }
    }
}
