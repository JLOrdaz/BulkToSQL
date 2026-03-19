using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace JLOrdaz.BulkToSQL
{
    /// <summary>
    /// Executes bulk inserts against SQL Server using a connection and column mappings.
    /// </summary>
    public class BulkInsert<T> : IBulkInsert<T>
    {
        /// <summary>
        /// Gets or sets the source-to-destination column mappings used by bulk copy.
        /// </summary>
        public List<Mapeo> Mapping { get; set; }

        /// <summary>
        /// Gets or sets the SQL connection used for bulk insert operations.
        /// </summary>
        public SqlConnection SQLConex { get; set; }

        /// <summary>
        /// Initializes a new bulk insert helper for the given SQL connection and mappings.
        /// </summary>
        /// <param name="sqlConex">The openable SQL connection to use.</param>
        /// <param name="mapeos">The column mappings for the destination table.</param>
        public BulkInsert(SqlConnection sqlConex, List<Mapeo> mapeos)
        {
            SQLConex = sqlConex ?? throw new ArgumentNullException(nameof(sqlConex));
            Mapping = mapeos ?? throw new ArgumentNullException(nameof(mapeos));
        }

        private void EnsureConnectionOpen()
        {
            if (SQLConex.State != ConnectionState.Open)
            {
                SQLConex.Open();
            }
        }

        private async Task EnsureConnectionOpenAsync()
        {
            if (SQLConex.State != ConnectionState.Open)
            {
                await SQLConex.OpenAsync().ConfigureAwait(false);
            }
        }

        private void TruncateTable(string tableName)
        {
            using SqlCommand cmd = SQLConex.CreateCommand();
            cmd.CommandText = $"Truncate Table {tableName}";
            cmd.ExecuteNonQuery();
        }

        private async Task TruncateTableAsync(string tableName)
        {
            using SqlCommand cmd = SQLConex.CreateCommand();
            cmd.CommandText = $"Truncate Table {tableName}";
            await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }

        private SqlBulkCopy CreateBulkCopy(string tableDestination, int timeout)
        {
            SqlBulkCopy bulkCopy = new(SQLConex, SqlBulkCopyOptions.FireTriggers, null)
            {
                DestinationTableName = tableDestination,
                BulkCopyTimeout = timeout
            };

            foreach (Mapeo mapping in Mapping)
            {
                bulkCopy.ColumnMappings.Add(mapping.Origen, mapping.Destino);
            }

            return bulkCopy;
        }

        /// <inheritdoc />
        public void LimpiarTablaTemporal(string tabla)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(tabla);
            EnsureConnectionOpen();
            TruncateTable(tabla);
        }


        /// <inheritdoc />
        public void PutDataIntoDB(IList<T> data, string tableDestination, int timeout = 120, bool truncateTable = false)
        {
            ArgumentNullException.ThrowIfNull(data);
            ArgumentException.ThrowIfNullOrWhiteSpace(tableDestination);

            EnsureConnectionOpen();
            DataTable dt = data.ToDataTable();

            if (truncateTable)
            {
                TruncateTable(tableDestination);
            }

            using SqlBulkCopy bulkCopy = CreateBulkCopy(tableDestination, timeout);
            bulkCopy.WriteToServer(dt);
        }

        /// <inheritdoc />
        public async Task PutDataIntoDBAsync(IList<T> data, string tableDestination, int timeout, bool truncateTable = false)
        {
            ArgumentNullException.ThrowIfNull(data);
            ArgumentException.ThrowIfNullOrWhiteSpace(tableDestination);

            await EnsureConnectionOpenAsync().ConfigureAwait(false);
            DataTable dt = data.ToDataTable();

            if (truncateTable)
            {
                await TruncateTableAsync(tableDestination).ConfigureAwait(false);
            }

            using SqlBulkCopy bulkCopy = CreateBulkCopy(tableDestination, timeout);
            await bulkCopy.WriteToServerAsync(dt).ConfigureAwait(false);
        }
    }
}
