using System.Collections.Generic;
using System.Linq;
using Common.Domain;
using DBBroker;

namespace SystemOperations
{
    public class VratiSveKategorijeSO : SystemOperationBase
    {
        public List<Kategorija> Result { get; set; }

        public VratiSveKategorijeSO() : base() { }
        public VratiSveKategorijeSO(IBroker? broker) : base(broker) { }

        protected override void ExecuteConcreteOperation()
        {
            Result = _broker.GetAll(new Kategorija()).Cast<Kategorija>().ToList();
        }
    }
}