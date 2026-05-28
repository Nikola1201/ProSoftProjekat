using Common.Domain;
using DBBroker;
using System.Collections.Generic;
using System.Linq;

namespace SystemOperations
{
    /// <summary>
    /// Sistemska operacija za dohvatanje svih paketa obuke iz baze.
    /// </summary>
    public class VratiSvePaketeObukeSO : SystemOperationBase
    {
        /// <summary>Lista paketa obuke vraćena iz baze.</summary>
        public List<PaketObuke> Result { get; private set; }

        /// <summary>Konstruktor za produkcijsku upotrebu (podrazumevani broker).</summary>
        public VratiSvePaketeObukeSO() : base() { }

        /// <summary>Konstruktor sa injektovanim brokerom (test-friendly).</summary>
        /// <param name="broker">Broker za pristup bazi.</param>
        public VratiSvePaketeObukeSO(IBroker? broker) : base(broker) { }

        /// <inheritdoc/>
        protected override void ExecuteConcreteOperation()
        {
            Result = _broker.GetAll(new PaketObuke()).Cast<PaketObuke>().ToList();
        }
    }
}
