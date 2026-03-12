
using Common.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Server.SystemOperation
{
    internal class VratiSveUpisaneKandidateSO : SystemOperationBase
    {
        public VratiSveUpisaneKandidateSO()
        {
        }

        public List<Kandidat> Result { get; internal set; }

        protected override void ExecuteConcreteOperation()
        {
            Result = _broker.GetAll(new Kandidat()).Cast<Kandidat>().ToList();
        }
    }
}