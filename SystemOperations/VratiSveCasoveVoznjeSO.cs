using Common.Domain;
using DBBroker;
using System.Collections.Generic;
using System.Linq;

namespace SystemOperations
{
    /// <summary>
    /// Sistemska operacija za dohvatanje svih časova vožnje iz baze,
    /// sortirani rastuće po datumu i vremenu časa.
    /// </summary>
    public class VratiSveCasoveVoznjeSO : SystemOperationBase
    {
        /// <summary>Lista časova vožnje vraćena iz baze, sortirana po datumu časa.</summary>
        public List<CasVoznje> Result { get; private set; }

        /// <summary>Konstruktor za produkcijsku upotrebu (podrazumevani broker).</summary>
        public VratiSveCasoveVoznjeSO() : base() { }

        /// <summary>Konstruktor sa injektovanim brokerom (test-friendly).</summary>
        /// <param name="broker">Broker za pristup bazi.</param>
        public VratiSveCasoveVoznjeSO(IBroker? broker) : base(broker) { }

        /// <inheritdoc/>
        protected override void ExecuteConcreteOperation()
        {
            Result = _broker.GetAll(new CasVoznje())
                .Cast<CasVoznje>()
                .OrderBy(c => c.DatumCas)
                .ToList();
        }
    }
}
