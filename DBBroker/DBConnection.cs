using System.Configuration;
using Microsoft.Data.SqlClient;

namespace DBBroker
{
    internal class DBConnection
    {
        private SqlConnection connection;
        private SqlTransaction transaction;
        public DBConnection()
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["bazaSkole"].ConnectionString);
        }
        public void OpenConnection()
        {
            connection?.Open();
        }
        public void CloseConnection()
        {
            connection?.Close();
        }
        public void BeginTransaction()
        {
            transaction = connection.BeginTransaction();
        }
        public void Commit()
        {
            transaction?.Commit();
        }
        public void Rollback()
        {
            transaction.Rollback();
        }
        public SqlCommand CreateCommand()
        {
            return new SqlCommand("", connection, transaction);
        }
    }
}