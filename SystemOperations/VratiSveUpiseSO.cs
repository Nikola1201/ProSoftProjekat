using Common.Domain;
using DBBroker;
using System.Collections.Generic;
using System.Linq;

namespace SystemOperations
{
    /// <summary>
    /// Sistemska operacija za dohvatanje svih upisa iz baze.
    /// </summary>
    public class VratiSveUpiseSO : SystemOperationBase
    {
        /// <summary>Lista upisa vraćena iz baze.</summary>
        public List<Upis> Result { get; private set; }

        /// <summary>Konstruktor za produkcijsku upotrebu (podrazumevani broker).</summary>
        public VratiSveUpiseSO() : base() { }

        /// <summary>Konstruktor sa injektovanim brokerom (test-friendly).</summary>
        /// <param name="broker">Broker za pristup bazi.</param>
        public VratiSveUpiseSO(IBroker? broker) : base(broker) { }

        /// <inheritdoc/>
        protected override void ExecuteConcreteOperation()
        {
            Result = _broker.GetAll(new Upis())
                .Cast<Upis>()
                .ToList();
        }
    }
}
