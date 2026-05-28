using DBBroker;

namespace SystemOperations
{
    /// <summary>
    /// Template Method bazna klasa za sve sistemske operacije.
    /// Otvara konekciju, započinje transakciju, poziva <see cref="ExecuteConcreteOperation"/>,
    /// commit-uje na uspeh, rollback-uje na izuzetak.
    /// </summary>
    public abstract class SystemOperationBase
    {
        /// <summary>Broker koji konkretna operacija koristi za pristup bazi.</summary>
        protected readonly IBroker _broker;

        /// <summary>Inicijalizuje operaciju sa podrazumevanim <see cref="Broker"/>-om.</summary>
        protected SystemOperationBase() : this(null) { }

        /// <summary>Inicijalizuje operaciju sa eksplicitnim brokerom (test-friendly).</summary>
        /// <param name="broker">Broker za injektovanje; <see langword="null"/> = novi <see cref="Broker"/>.</param>
        protected SystemOperationBase(IBroker? broker)
        {
            _broker = broker ?? new Broker();
        }

        /// <summary>
        /// Izvršava operaciju u okviru transakcije.
        /// Izuzetak unutar konkretne operacije dovodi do rollback-a.
        /// </summary>
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

        /// <summary>Konkretna logika sistemske operacije — implementira je svaka <c>*SO</c> klasa.</summary>
        protected abstract void ExecuteConcreteOperation();
    }
}
