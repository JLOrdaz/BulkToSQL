using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace JLOrdaz.BulkToSQL
{
    internal static class ListToDatatable
    {
        /// <summary>
        /// Converts a list of objects to a DataTable.
        /// </summary>
        /// <typeparam name="T">The type of objects in the list.</typeparam>
        /// <param name="data">The list of objects to convert.</param>
        /// <returns>A DataTable representation of the list.</returns>
        internal static DataTable ToDataTable<T>(this IEnumerable<T> data)
        {
            ArgumentNullException.ThrowIfNull(data);

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new(typeof(T).Name);

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }

            return table;
        }
    }
}
