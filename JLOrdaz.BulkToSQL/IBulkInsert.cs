using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace JLOrdaz.BulkToSQL
{
    public interface IBulkInsert<T>
    {
        List<Mapeo> Mapping { get; set; }

        SqlConnection SQLConex { get; set; }

        void PutDataIntoDB(IList<T> data, string tableDestination, int timeout);
    }
}  