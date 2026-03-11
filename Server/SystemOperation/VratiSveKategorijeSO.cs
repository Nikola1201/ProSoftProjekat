using Server.SystemOperation;
using System.Collections.Generic;
using System.Linq;
using Common.Domain;

namespace Server
{
    internal class VratiSveKategorijeSO : SystemOperationBase
    {
        public List<Kategorija> Result { get; set; }

        protected override void ExecuteConcreteOperation()
        {
            Result = _broker.GetAll(new Kategorija()).Cast<Kategorija>().ToList();
        }
    }
}