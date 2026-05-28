using System.Data;
using System.Data.Common;

namespace Tests.Helpers
{
    public static class DataReaderBuilder
    {
        public static DbDataReader From(DataTable table) => table.CreateDataReader();

        public static DataTable Empty(params (string Name, System.Type Type)[] columns)
        {
            var table = new DataTable();
            foreach (var (name, type) in columns)
                table.Columns.Add(name, type);
            return table;
        }
    }
}
