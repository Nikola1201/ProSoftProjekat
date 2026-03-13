using Common.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Server.SystemOperation
{
    internal class VratiSveInstruktoreSO : SystemOperationBase
    {
        public List<Instruktor> Result { get; internal set; }

        protected override void ExecuteConcreteOperation()
        {
           Result = _broker.GetAll(new Instruktor()).Cast<Instruktor>().ToList();
        }
    }
}