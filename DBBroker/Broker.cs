using Common.Domain;
using Common.Domain.Izvestaji;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace DBBroker
{
    public class Broker
    {
        private DBConnection _connection;

        public Broker()
        {
            _connection = new DBConnection();
        }

        public void OpenConnection()
        {
            _connection.OpenConnection();
        }

        public void CloseConnection()
        {
            _connection.CloseConnection();
        }

        public void BeginTransaction()
        {
            _connection.BeginTransaction();
        }

        public void Commit()
        {
            _connection.Commit();
        }

        public void Rollback()
        {
            _connection.Rollback();
        }

        public IEntity Add(IEntity entity)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = $"INSERT INTO {entity.TableName} VALUES ({entity.Values})";
            command.ExecuteNonQuery();
            command.Dispose();
            return entity;
        }

        public void Update(IEntity entity)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = entity.Update;
            command.ExecuteNonQuery();
            command.Dispose();
        }

        public void Delete(IEntity entity)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = $"DELETE FROM {entity.TableName} WHERE {entity.TableKeyQuery}";
            command.ExecuteNonQuery();
            command.Dispose();
        }

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
