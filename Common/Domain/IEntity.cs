using System.Collections.Generic;
using System.Data.Common;

namespace Common.Domain
{
    public interface IEntity
    {
        string TableName { get; }
        string TableKeyColumn { get; }
        string TableKeyQuery { get; }
        string Query { get; }
        string Values { get; }
        string Update { get; }
        List<IEntity> GetReaderList(DbDataReader reader);
        IEntity GetReaderResult(DbDataReader reader);
    }
}
