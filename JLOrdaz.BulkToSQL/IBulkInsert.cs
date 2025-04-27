using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace JLOrdaz.BulkToSQL
{
    /// <summary>
    /// Interface for bulk inserting data into a SQL Server database.
    /// </summary>
    public interface IBulkInsert<T>
    {
        /// <summary>
        /// Clears the temporary table in the database.
        /// </summary>
        /// <param name="tabla">The name of the temporary table to clear.</param>
        void LimpiarTablaTemporal(string tabla);

        /// <summary>
        /// Inserts a list of data into the specified database table.
        /// </summary>
        /// <param name="data">The list of data to insert.</param>
        /// <param name="tableDestination">The name of the destination table.</param>
        /// <param name="timeout">The timeout duration for the operation.</param>
        /// <param name="truncateTable">Indicates whether to truncate the table before inserting data.</param>
        void PutDataIntoDB(IList<T> data, string tableDestination, int timeout, bool truncateTable = false);

        /// <summary>
        /// Asynchronously inserts a list of data into the specified database table.
        /// </summary>
        /// <param name="data">The list of data to insert.</param>
        /// <param name="tableDestination">The name of the destination table.</param>
        /// <param name="timeout">The timeout duration for the operation.</param>
        /// <param name="truncateTable">Indicates whether to truncate the table before inserting data.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task PutDataIntoDBAsync(IList<T> data, string tableDestination, int timeout, bool truncateTable = false);
    }
}  