using DBBroker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.SystemOperation
{
    public abstract class SystemOperationBase
    {
        protected Broker _broker;

        public SystemOperationBase()
        {
            _broker = new Broker();
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
            catch (Exception ex)
            {
                _broker.Rollback();
                throw ex;
            }
            finally
            {
                _broker.CloseConnection();
            }
        }
        protected abstract void ExecuteConcreteOperation();
    }
}
