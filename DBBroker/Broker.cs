using Common.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

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

        public IEntity GetEntityByID(IEntity entity)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {entity.TableName} WHERE {entity.TableKeyQuery}";
            SqlDataReader reader = command.ExecuteReader();
            entity = entity.GetReaderResult(reader);
            reader.Close();
            command.Dispose();
            return entity;
        }
        public IEntity GetEntityByQuery(IEntity entity)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {entity.TableName} WHERE {entity.Query}";
            SqlDataReader reader = command.ExecuteReader();
            entity = entity.GetReaderResult(reader);
            reader.Close();
            command.Dispose();
            return entity;
        }

        public IEntity Add(IEntity entity)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = $"INSERT INTO {entity.TableName} VALUES ({entity.Values})";
            command.ExecuteNonQuery();
            command.Dispose();
            return entity;

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

        public void Delete(IEntity entity)
        {
            SqlCommand cmd = _connection.CreateCommand();
            cmd.CommandText = $"delete from {entity.TableName} where {entity.TableKeyQuery} ";
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }
    }
}
