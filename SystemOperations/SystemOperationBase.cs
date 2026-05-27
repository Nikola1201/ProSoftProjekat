using DBBroker;

namespace SystemOperations
{
    public abstract class SystemOperationBase
    {
        protected readonly IBroker _broker;

        protected SystemOperationBase() : this(null) { }

        protected SystemOperationBase(IBroker? broker)
        {
            _broker = broker ?? new Broker();
        }

        public void ExecuteTemplate()
        {
            try
            {
                _broker.OpenConnection();
                _broker.BeginTransaction();
                ExecuteConcreteOperation();
                _broker.Commit();
            }
            catch
            {
                _broker.Rollback();
                throw;
            }
            finally
            {
                _broker.CloseConnection();
            }
        }

        protected abstract void ExecuteConcreteOperation();
    }
}
