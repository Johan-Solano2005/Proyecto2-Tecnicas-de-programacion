using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaseModelo
{
    /// <summary>
    /// Modelo de Historial
    /// </summary>
    public class Historial
    {
        public string Id { get; set; }
        public string Cliente { get; set; }
        public int Cantidad { get; set; }
        public string Producto { get; set; }
        public DateTime Fecha { get; set; }

    }
}
