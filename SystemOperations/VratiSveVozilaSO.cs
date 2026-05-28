using Common.Domain;
using DBBroker;
using System.Collections.Generic;
using System.Linq;

namespace SystemOperations
{
    /// <summary>
    /// Sistemska operacija za dohvatanje svih aktivnih vozila iz baze.
    /// Vozila sa oznakom <c>Aktivno = false</c> nisu uključena u rezultat.
    /// </summary>
    public class VratiSveVozilaSO : SystemOperationBase
    {
        /// <summary>Lista aktivnih vozila vraćena iz baze.</summary>
        public List<Vozilo> Result { get; private set; }

        /// <summary>Konstruktor za produkcijsku upotrebu (podrazumevani broker).</summary>
        public VratiSveVozilaSO() : base() { }

        /// <summary>Konstruktor sa injektovanim brokerom (test-friendly).</summary>
        /// <param name="broker">Broker za pristup bazi.</param>
        public VratiSveVozilaSO(IBroker? broker) : base(broker) { }

        /// <inheritdoc/>
        protected override void ExecuteConcreteOperation()
        {
            Result = _broker.GetAll(new Vozilo())
                .Cast<Vozilo>()
                .Where(v => v.Aktivno)
                .ToList();
        }
    }
}
