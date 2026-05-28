using System;
using System.IO;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DBBroker
{
    /// <summary>Niskonivoski wrapper nad <see cref="SqlConnection"/> i tekućoj transakciji.</summary>
    internal class DBConnection
    {
        private SqlConnection connection;
        private SqlTransaction transaction;

        /// <summary>Inicijalizuje konekciju čitajući string iz appsettings.json (preferirano) ili App.config (fallback).</summary>
        public DBConnection()
        {
            connection = new SqlConnection(LoadConnectionString());
        }

        private static string LoadConnectionString()
        {
            var jsonPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            if (File.Exists(jsonPath))
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                    .Build();
                var fromJson = config.GetConnectionString("bazaSkole");
                if (!string.IsNullOrWhiteSpace(fromJson)) return fromJson;
            }
            var legacy = System.Configuration.ConfigurationManager.ConnectionStrings["bazaSkole"]?.ConnectionString;
            if (!string.IsNullOrWhiteSpace(legacy)) return legacy;
            throw new InvalidOperationException("Connection string 'bazaSkole' nije pronađen ni u appsettings.json ni u App.config.");
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
