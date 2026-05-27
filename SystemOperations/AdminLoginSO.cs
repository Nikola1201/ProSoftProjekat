using Common.Domain;
using DBBroker;

namespace SystemOperations
{
    public class AdminLoginSO : SystemOperationBase
    {
        private readonly Admin _admin;
        public IEntity Result { get; set; }

        public AdminLoginSO(Admin argument) : this(argument, null) { }
        public AdminLoginSO(Admin argument, IBroker? broker) : base(broker)
        {
            _admin = argument;
        }

        protected override void ExecuteConcreteOperation()
        {
            Result = _broker.GetEntityByQuery(_admin);
        }
    }
}