using Common.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Server.SystemOperation
{
    internal class VratiSveCasoveVoznjeSO : SystemOperationBase
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
