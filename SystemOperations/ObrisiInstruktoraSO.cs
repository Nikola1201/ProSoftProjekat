using Common.Domain;
using DBBroker;

namespace SystemOperations
{
    /// <summary>
    /// Sistemska operacija za brisanje instruktora iz sistema.
    /// </summary>
    public class ObrisiInstruktoraSO : SystemOperationBase
    {
        private Instruktor argument;

        /// <summary>Konstruktor za produkcijsku upotrebu (podrazumevani broker).</summary>
        /// <param name="argument">Instruktor za brisanje.</param>
        public ObrisiInstruktoraSO(Instruktor argument) : this(argument, null) { }

        /// <summary>Konstruktor sa injektovanim brokerom (test-friendly).</summary>
        /// <param name="argument">Instruktor za brisanje.</param>
        /// <param name="broker">Broker za pristup bazi.</param>
        public ObrisiInstruktoraSO(Instruktor argument, IBroker? broker) : base(broker)
        {
            this.argument = argument;
        }

        /// <inheritdoc/>
        protected override void ExecuteConcreteOperation()
        {
            _broker.Delete(argument);
        }
    }
}