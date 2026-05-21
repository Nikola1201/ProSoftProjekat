using Common.Domain;
using System.Collections.Generic;

namespace Server.SystemOperation
{
    internal class VratiKandidatiSaDugovanjemSO : SystemOperationBase
    {
        public List<KandidatDugovanjeDto> Result { get; private set; }

        protected override void ExecuteConcreteOperation()
        {
            Result = _broker.VratiKandidatiSaDugovanjem();
        }
    }
}
