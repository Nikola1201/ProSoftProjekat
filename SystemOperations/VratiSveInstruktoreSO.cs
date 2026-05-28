using Common.Domain;
using DBBroker;
using System.Collections.Generic;
using System.Linq;

namespace SystemOperations
{
    /// <summary>
    /// Sistemska operacija za dohvatanje svih instruktora iz baze.
    /// </summary>
    public class VratiSveInstruktoreSO : SystemOperationBase
    {
        /// <summary>Lista instruktora vraćena iz baze.</summary>
        public List<Instruktor> Result { get; internal set; }

        /// <summary>Konstruktor za produkcijsku upotrebu (podrazumevani broker).</summary>
        public VratiSveInstruktoreSO() : base() { }

        /// <summary>Konstruktor sa injektovanim brokerom (test-friendly).</summary>
        /// <param name="broker">Broker za pristup bazi.</param>
        public VratiSveInstruktoreSO(IBroker? broker) : base(broker) { }

        /// <inheritdoc/>
        protected override void ExecuteConcreteOperation()
        {
           Result = _broker.GetAll(new Instruktor()).Cast<Instruktor>().ToList();
        }
    }
}