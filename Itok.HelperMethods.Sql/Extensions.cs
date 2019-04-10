using System;
using System.Collections.Generic;
using System.Data;

namespace Itok.HelperMethods.Sql
{
    public static class Extensions
    {
        public static DataTable CreateDataTable<T>(this IEnumerable<T> dataList, string tableName)
        {
            if (String.IsNullOrEmpty(tableName)) throw new ArgumentNullException(tableName);
            var dt = new DataTable(tableName);
            foreach (var property in typeof(T).GetProperties())
                dt.Columns.Add(property.Name,
                    Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);

            foreach (var imageDb in dataList)
            {
                var row = dt.NewRow();
                foreach (var property in typeof(T).GetProperties())
                    row[property.Name] = property.GetValue(imageDb) ?? DBNull.Value;
                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}