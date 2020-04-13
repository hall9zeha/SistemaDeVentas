using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class InventarioE
    {
        public string Codproducto { get; set; }
        public string Descripción { get; set; }
        public string Marca { get; set; }
        public double Precio { get; set; }
        public double PrecioVenta { get; set; }
       
        public int Cantidad { get; set; }
        public int Stock { get;set; }
        public List<DetalleInventarioE> detalleInventario { get; set; }
        public DetalleInventarioE DtInventario { get; set; }

    }
}
