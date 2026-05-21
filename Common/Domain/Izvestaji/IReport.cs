using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Common.Domain.Izvestaji
{
    public interface IReport<T>
    {
        string Sql { get; }
        IEnumerable<SqlParameter> Parameters { get; }
        List<T> Hydrate(SqlDataReader reader);
    }
}
