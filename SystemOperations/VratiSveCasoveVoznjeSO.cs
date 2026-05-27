using Common.Domain;
using DBBroker;
using System.Collections.Generic;
using System.Linq;

namespace SystemOperations
{
    public class VratiSveCasoveVoznjeSO : SystemOperationBase
    {
        public List<CasVoznje> Result { get; private set; }

        public VratiSveCasoveVoznjeSO() : base() { }
        public VratiSveCasoveVoznjeSO(IBroker? broker) : base(broker) { }

        protected override void ExecuteConcreteOperation()
        {
            Result = _broker.GetAll(new CasVoznje())
                .Cast<CasVoznje>()
                .OrderBy(c => c.DatumCas)
                .ToList();
        }
    }
}
