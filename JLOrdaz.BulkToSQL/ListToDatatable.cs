using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;

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
            // Get the properties of the type T
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            // Create columns in the DataTable for each property
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            // Populate the DataTable with data from the list
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
    }
}
