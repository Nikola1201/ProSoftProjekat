using Common.DTO.Izvestaji;
using DBBroker;
using DBBroker.Reports;
using System.Collections.Generic;

namespace SystemOperations
{
    /// <summary>
    /// Sistemska operacija za generisanje izveštaja o kandidatima koji imaju neizmirena dugovanja.
    /// Izvršava SQL izveštaj <see cref="KandidatiSaDugovanjemReport"/> i vraća listu DTO-ova.
    /// </summary>
    public class VratiKandidatiSaDugovanjemSO : SystemOperationBase
    {
        /// <summary>Lista kandidata sa dugovanjem vraćena iz izveštaja.</summary>
        public List<KandidatDugovanjeDto> Result { get; private set; }

        /// <summary>Konstruktor za produkcijsku upotrebu (podrazumevani broker).</summary>
        public VratiKandidatiSaDugovanjemSO() : base() { }

        /// <summary>Konstruktor sa injektovanim brokerom (test-friendly).</summary>
        /// <param name="broker">Broker za pristup bazi.</param>
        public VratiKandidatiSaDugovanjemSO(IBroker? broker) : base(broker) { }

        /// <inheritdoc/>
        protected override void ExecuteConcreteOperation()
        {
            Result = _broker.ExecuteReport(new KandidatiSaDugovanjemReport());
        }
    }
}
