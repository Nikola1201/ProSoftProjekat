using Common.DTO.Izvestaji;
using DBBroker;
using DBBroker.Reports;
using System.Collections.Generic;

namespace SystemOperations
{
    public class VratiKandidatiSaDugovanjemSO : SystemOperationBase
    {
        public List<KandidatDugovanjeDto> Result { get; private set; }

        public VratiKandidatiSaDugovanjemSO() : base() { }
        public VratiKandidatiSaDugovanjemSO(IBroker? broker) : base(broker) { }

        protected override void ExecuteConcreteOperation()
        {
            Result = _broker.ExecuteReport(new KandidatiSaDugovanjemReport());
        }
    }
}
