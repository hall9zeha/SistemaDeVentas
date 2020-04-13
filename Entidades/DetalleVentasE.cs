using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class DetalleVentasE
    {
        public int Coddetalle { get; set; }
        public int IdVenta { get; set; }
        public string Codproducto { get; set; }
        public int CodProducto_detalle { get; set; }
        public string Descripción { get; set; }
        public int Cantidad { get; set; }
        public double Precio_final { get; set; } 
        public InventarioE Inventario { get; set; }
        public DetalleInventarioE DetInventario { get; set; }
        
        public VentasE _Importe { get; set; }
        public double importe { get; set; }
        public string EstadoCambio { get; set; }


    }
}
