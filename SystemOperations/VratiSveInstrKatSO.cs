using Common.Domain;
using DBBroker;
using System.Collections.Generic;
using System.Linq;

namespace SystemOperations
{
    /// <summary>
    /// Sistemska operacija za dohvatanje svih veza između instruktora i kategorija (InstrKat) iz baze.
    /// </summary>
    public class VratiSveInstrKatSO : SystemOperationBase
    {
        /// <summary>Lista veza instruktor–kategorija vraćena iz baze.</summary>
        public List<InstrKat> Result { get; internal set; }

        /// <summary>Konstruktor za produkcijsku upotrebu (podrazumevani broker).</summary>
        public VratiSveInstrKatSO() : base() { }

        /// <summary>Konstruktor sa injektovanim brokerom (test-friendly).</summary>
        /// <param name="broker">Broker za pristup bazi.</param>
        public VratiSveInstrKatSO(IBroker? broker) : base(broker) { }

        /// <inheritdoc/>
        protected override void ExecuteConcreteOperation()
        {
            Result = _broker.GetAll(new InstrKat()).Cast<InstrKat>().ToList();
        }
    }
}
