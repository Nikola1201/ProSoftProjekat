using System.Collections.Generic;
using Microsoft.Data.SqlClient;

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
        List<IEntity> GetReaderList(SqlDataReader reader);
        IEntity GetReaderResult(SqlDataReader reader);
    }
}
