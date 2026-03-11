using Common.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Server.SystemOperation
{
    internal class VratiSvePaketeObukeSO : SystemOperationBase
    {
        public List<PaketObuke> Result { get; private set; }

        protected override void ExecuteConcreteOperation()
        {
            Result = _broker.GetAll(new PaketObuke()).Cast<PaketObuke>().ToList();
        }
    }
}