using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class DetalleInventarioE
    {
        public int CodStock { get; set; }
        public string Codproducto { get; set; }
        public InventarioE inventario { get; set; }
        public string Marca{ get; set; }
        //public string Descripción { get; set; }
        public string Color { get; set; }
        public string Talla_alfanum { get; set; }
        public int Talla_num { get; set; }

        public double Precio { get; set; }
        public int Cantidad { get; set; }
        public int Stock { get; set; }
        public string CodigoDeBarra { get; set; }
        public string EstadoCambio { get; set; }
        public double MontoCambio { get; set; }
    }
}
