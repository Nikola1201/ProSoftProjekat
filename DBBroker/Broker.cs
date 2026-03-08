namespace DBBroker
{
    public class Broker
    {
        private DBConnection _connection;

        public Broker()
        {
            _connection = new DBConnection();
        }
        public void Rollback()
        {
            _connection.Rollback();
        }

        public void Commit()
        {
            _connection.Commit();
        }

        public void BeginTransaction()
        {
            _connection.BeginTransaction();
        }

        public void CloseConnection()
        {
            _connection.CloseConnection();
        }

        public void OpenConnection()
        {
            _connection.OpenConnection();
        }
    }
}
