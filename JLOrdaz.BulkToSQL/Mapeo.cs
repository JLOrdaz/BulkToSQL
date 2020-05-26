using System;
using System.Collections.Generic;
using System.Text;

namespace JLOrdaz.BulkToSQL
{
    public class Mapeo
    {
        public string Origen { get; set; }
        public string Destino { get; set; }

        public Mapeo()
        {
            Origen = string.Empty;
            Destino = string.Empty;
        }

        public Mapeo(string origen, string destino)
        {
            Origen = origen;
            Destino = destino;
        }
    }
}
