using Common.Domain;
using System.Collections.Generic;
using System.Linq;

namespace SystemOperations
{
    public class VratiSveUpiseSO : SystemOperationBase
    {
        public List<Upis> Result { get; private set; }

        protected override void ExecuteConcreteOperation()
        {
            Result = _broker.GetAll(new Upis())
                .Cast<Upis>()
                .ToList();
        }
    }
}
