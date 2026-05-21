using Common.Domain;

namespace SystemOperations
{
    public class AdminLoginSO : SystemOperationBase
    {
        private readonly Admin _admin;
        public IEntity Result { get; set; }

        public AdminLoginSO(Admin argument)
        {
            _admin = argument;
        }

        protected override void ExecuteConcreteOperation()
        {
            Result = _broker.GetEntityByQuery(_admin);
        }
    }
}