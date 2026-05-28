using System.Configuration;
using Microsoft.Data.SqlClient;

namespace DBBroker
{
    /// <summary>Niskonivoski wrapper nad <see cref="SqlConnection"/> i tekućoj transakciji.</summary>
    internal class DBConnection
    {
        private SqlConnection connection;
        private SqlTransaction transaction;

        /// <summary>Inicijalizuje konekciju čitajući connection string iz konfiguracije.</summary>
        public DBConnection()
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["bazaSkole"].ConnectionString);
        }

        /// <summary>Otvara konekciju ka bazi.</summary>
        public void OpenConnection()
        {
            connection?.Open();
        }

        /// <summary>Zatvara konekciju ka bazi.</summary>
        public void CloseConnection()
        {
            connection?.Close();
        }

        /// <summary>Započinje novu transakciju nad otvorenom konekcijom.</summary>
        public void BeginTransaction()
        {
            transaction = connection.BeginTransaction();
        }

        /// <summary>Potvrđuje (commit) trenutnu transakciju.</summary>
        public void Commit()
        {
            transaction?.Commit();
        }

        /// <summary>Poništava (rollback) trenutnu transakciju.</summary>
        public void Rollback()
        {
            transaction.Rollback();
        }

        /// <summary>Kreira SQL komandu vezanu za tekuću konekciju i transakciju.</summary>
        /// <returns>Nova <see cref="SqlCommand"/> instanca.</returns>
        public SqlCommand CreateCommand()
        {
            return new SqlCommand("", connection, transaction);
        }
    }
}
