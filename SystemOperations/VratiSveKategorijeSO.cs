using System.Collections.Generic;
using System.Linq;
using Common.Domain;
using DBBroker;

namespace SystemOperations
{
    /// <summary>
    /// Sistemska operacija za dohvatanje svih kategorija vozačkih dozvola iz baze.
    /// </summary>
    public class VratiSveKategorijeSO : SystemOperationBase
    {
        /// <summary>Lista kategorija vraćena iz baze.</summary>
        public List<Kategorija> Result { get; set; }

        /// <summary>Konstruktor za produkcijsku upotrebu (podrazumevani broker).</summary>
        public VratiSveKategorijeSO() : base() { }

        /// <summary>Konstruktor sa injektovanim brokerom (test-friendly).</summary>
        /// <param name="broker">Broker za pristup bazi.</param>
        public VratiSveKategorijeSO(IBroker? broker) : base(broker) { }

        /// <inheritdoc/>
        protected override void ExecuteConcreteOperation()
        {
            Result = _broker.GetAll(new Kategorija()).Cast<Kategorija>().ToList();
        }
    }
}