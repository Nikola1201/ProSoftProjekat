using Common.Domain;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Common.DTO.Izvestaji;

namespace DBBroker
{
    /// <summary>Konkretna implementacija <see cref="IBroker"/> nad <see cref="DBConnection"/>.</summary>
    public class Broker : IBroker
    {
        private DBConnection _connection;

        /// <summary>Inicijalizuje broker sa novom DB konekcijom.</summary>
        public Broker()
        {
            _connection = new DBConnection();
        }

        /// <inheritdoc/>
        public void OpenConnection()
        {
            _connection.OpenConnection();
        }

        /// <inheritdoc/>
        public void CloseConnection()
        {
            _connection.CloseConnection();
        }

        /// <inheritdoc/>
        public void BeginTransaction()
        {
            _connection.BeginTransaction();
        }

        /// <inheritdoc/>
        public void Commit()
        {
            _connection.Commit();
        }

        /// <inheritdoc/>
        public void Rollback()
        {
            _connection.Rollback();
        }

        /// <inheritdoc/>
        public IEntity Add(IEntity entity)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = $"INSERT INTO {entity.TableName} VALUES ({entity.Values})";
            command.ExecuteNonQuery();
            command.Dispose();
            return entity;
        }

        /// <inheritdoc/>
        public void Update(IEntity entity)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = entity.Update;
            command.ExecuteNonQuery();
            command.Dispose();
        }

        /// <inheritdoc/>
        public void Delete(IEntity entity)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = $"DELETE FROM {entity.TableName} WHERE {entity.TableKeyQuery}";
            command.ExecuteNonQuery();
            command.Dispose();
        }

        /// <inheritdoc/>
        public List<IEntity> GetAll(IEntity entity)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {entity.TableName}";
            SqlDataReader reader = command.ExecuteReader();
            List<IEntity> entities = entity.GetReaderList(reader);
            reader.Close();
            command.Dispose();
            return entities;
        }

        /// <inheritdoc/>
        public IEntity GetEntityByID(IEntity entity)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {entity.TableName} WHERE {entity.TableKeyQuery}";
            SqlDataReader reader = command.ExecuteReader();
            IEntity result = entity.GetReaderResult(reader);
            reader.Close();
            command.Dispose();
            return result;
        }

        /// <inheritdoc/>
        public IEntity GetEntityByQuery(IEntity entity)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {entity.TableName} WHERE {entity.Query}";
            SqlDataReader reader = command.ExecuteReader();
            IEntity result = entity.GetReaderResult(reader);
            reader.Close();
            command.Dispose();
            return result;
        }

        /// <inheritdoc/>
        public List<IEntity> GetEntitiesByQuery(IEntity entity)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {entity.TableName} WHERE {entity.Query}";
            SqlDataReader reader = command.ExecuteReader();
            List<IEntity> entities = entity.GetReaderList(reader);
            reader.Close();
            command.Dispose();
            return entities;
        }

        /// <inheritdoc/>
        public List<T> ExecuteReport<T>(IReport<T> report)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = report.Sql;
            foreach (SqlParameter parameter in report.Parameters)
            {
                command.Parameters.Add(parameter);
            }
            SqlDataReader reader = command.ExecuteReader();
            List<T> rows = report.Hydrate(reader);
            reader.Close();
            command.Dispose();
            return rows;
        }
    }
}
