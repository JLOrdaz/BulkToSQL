using System;
using System.Collections.Generic;
using System.Text;

namespace JLOrdaz.BulkToSQL
{
    /// <summary>
    /// Represents a mapping between an origin and a destination.
    /// </summary>
    public class Mapeo
    {
        /// <summary>
        /// Gets or sets the origin value of the mapping.
        /// </summary>
        public string Origen { get; set; }

        /// <summary>
        /// Gets or sets the destination value of the mapping.
        /// </summary>
        public string Destino { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mapeo"/> class with default values.
        /// </summary>
        public Mapeo()
        {
            Origen = string.Empty;
            Destino = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mapeo"/> class with specified origin and destination values.
        /// </summary>
        /// <param name="origen">The origin value.</param>
        /// <param name="destino">The destination value.</param>
        public Mapeo(string origen, string destino)
        {
            Origen = origen;
            Destino = destino;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mapeo"/> class where the origin and destination are the same.
        /// </summary>
        /// <param name="origenIgualdestino">The value to be used for both origin and destination.</param>
        public Mapeo(string origenIgualdestino)
        {
            Origen = origenIgualdestino;
            Destino = origenIgualdestino;
        }
    }
}
