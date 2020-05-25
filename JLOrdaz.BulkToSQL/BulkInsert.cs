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

        public void PutDataIntoDB(IList<T> data, string tableDestination)
        {
            DataTable dt = data.ToDataTable<T>();

            if (SQLConex.State != ConnectionState.Open)
                SQLConex.Open();

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(SQLConex))
            {
                bulkCopy.DestinationTableName = tableDestination;
                bulkCopy.BulkCopyTimeout = 120;

                for (int i = 0; i < Mapping.Count; i++)
                {
                    bulkCopy.ColumnMappings.Add(Mapping[i].Origen, Mapping[i].Destino);
                }
                bulkCopy.WriteToServer(dt);
            }
        }

    }
}
