using Common.Domain;

namespace SystemOperations
{
    public class ObrisiInstruktoraSO : SystemOperationBase
    {
        private Instruktor argument;

        public ObrisiInstruktoraSO(Instruktor argument)
        {
            this.argument = argument;
        }

        protected override void ExecuteConcreteOperation()
        {
            _broker.Delete(argument);
        }
    }
}