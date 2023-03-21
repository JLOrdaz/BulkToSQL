using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JLOrdaz.BulkToSQL
{
    public class BulkInsert<T> : IBulkInsert<T>
    {

        public List<Mapeo> Mapping { get; set; }
        public SqlConnection SQLConex { get; set; }

        public BulkInsert(SqlConnection sqlConex, List<Mapeo> mapeos)
        {
            SQLConex = sqlConex;
            Mapping = mapeos;
        }


        private string ValidarObjetos()
        {
            if (Mapping is null)
            {
                return "Mapping is null";
            }

            if (SQLConex is null)
            {
                return "SQLConex is null";
            }

            return "OK";
        }

        private void ValidarOpenConexion()
        {
            if (SQLConex.State != ConnectionState.Open)
                SQLConex.Open();
        }


        private void LimpiarTablaTemporal(string tabla)
        {
            using (SqlCommand cmd = SQLConex.CreateCommand())
            {
                cmd.CommandText = $"Truncate Table {tabla}";
                cmd.ExecuteNonQuery();
            }
        }


        public void PutDataIntoDB(IList<T> data, string tableDestination, int timeout = 120, bool truncateTable = false)
        {

            var resp = ValidarObjetos();
            if (resp == "OK")
            {
                ValidarOpenConexion();
                DataTable dt = data.ToDataTable<T>();

                //validar borrar tabla
                if (truncateTable)
                {
                    using (SqlCommand cmd = SQLConex.CreateCommand())
                    {
                        cmd.CommandText = $"Truncate Table {tableDestination}";
                        cmd.ExecuteNonQuery();
                    }
                }

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(SQLConex))
                {
                    bulkCopy.DestinationTableName = tableDestination;
                    bulkCopy.BulkCopyTimeout = timeout;

                    for (int i = 0; i < Mapping.Count; i++)
                    {
                        bulkCopy.ColumnMappings.Add(Mapping[i].Origen, Mapping[i].Destino);
                    }
                    bulkCopy.WriteToServer(dt);
                }
            }
            else
            {
                throw new Exception(resp);
            }

        }

        public async Task PutDataIntoDBAsync(IList<T> data, string tableDestination, int timeout, bool truncateTable = false)
        {
            var resp = ValidarObjetos();
            if (resp == "OK")
            {
                ValidarOpenConexion();
                DataTable dt = data.ToDataTable<T>();

                //validar borrar tabla
                if (truncateTable)
                {
                    using (SqlCommand cmd = SQLConex.CreateCommand())
                    {
                        cmd.CommandText = $"Truncate Table {tableDestination}";
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(SQLConex))
                {
                    bulkCopy.DestinationTableName = tableDestination;
                    bulkCopy.BulkCopyTimeout = timeout;

                    for (int i = 0; i < Mapping.Count; i++)
                    {
                        bulkCopy.ColumnMappings.Add(Mapping[i].Origen, Mapping[i].Destino);
                    }
                    await bulkCopy.WriteToServerAsync(dt);
                }
            }
            else
            {
                throw new Exception(resp);
            }

        }
    }
}
