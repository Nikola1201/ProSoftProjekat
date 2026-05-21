using Common.Domain;
using System.Collections.Generic;
using System.Linq;

namespace SystemOperations
{
    public class VratiSveCasoveVoznjeSO : SystemOperationBase
    {
        public List<CasVoznje> Result { get; private set; }

        protected override void ExecuteConcreteOperation()
        {
            Result = _broker.GetAll(new CasVoznje())
                .Cast<CasVoznje>()
                .OrderBy(c => c.DatumCas)
                .ToList();
        }
    }
}
