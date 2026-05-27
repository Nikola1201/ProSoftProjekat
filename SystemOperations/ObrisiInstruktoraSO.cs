using Common.Domain;
using DBBroker;

namespace SystemOperations
{
    public class ObrisiInstruktoraSO : SystemOperationBase
    {
        private Instruktor argument;

        public ObrisiInstruktoraSO(Instruktor argument) : this(argument, null) { }
        public ObrisiInstruktoraSO(Instruktor argument, IBroker? broker) : base(broker)
        {
            this.argument = argument;
        }

        protected override void ExecuteConcreteOperation()
        {
            _broker.Delete(argument);
        }
    }
}