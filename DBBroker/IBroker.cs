using System.Collections.Generic;
using Common.Domain;
using Common.DTO.Izvestaji;

namespace DBBroker
{
    public interface IBroker
    {
        void OpenConnection();
        void CloseConnection();
        void BeginTransaction();
        void Commit();
        void Rollback();

        IEntity Add(IEntity entity);
        void Update(IEntity entity);
        void Delete(IEntity entity);
        IEntity GetEntityByID(IEntity entity);
        IEntity GetEntityByQuery(IEntity entity);
        List<IEntity> GetAll(IEntity entity);
        List<IEntity> GetEntitiesByQuery(IEntity entity);
        List<T> ExecuteReport<T>(IReport<T> report);
    }
}
