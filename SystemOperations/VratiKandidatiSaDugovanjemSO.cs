using Common.DTO.Izvestaji;
using DBBroker.Reports;
using System.Collections.Generic;

namespace SystemOperations
{
    public class VratiKandidatiSaDugovanjemSO : SystemOperationBase
    {
        public List<KandidatDugovanjeDto> Result { get; private set; }

        protected override void ExecuteConcreteOperation()
        {
            Result = _broker.ExecuteReport(new KandidatiSaDugovanjemReport());
        }
    }
}
