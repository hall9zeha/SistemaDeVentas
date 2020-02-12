using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class DetalleBoletaE
    {
        public int Coddetalle { get; set; }
        public string Codboleta { get; set; }
        public string Codproducto { get; set; }
        public int CodProducto_detalle { get; set; }
        public string Descripción { get; set; }
        public int Cantidad { get; set; }
        public double Precio_final { get; set; } 
        public InventarioE Marca { get; set; }
        public DetalleInventarioE Color { get; set; }
        public DetalleInventarioE Talla_alfanum { get; set; }
        public DetalleInventarioE Talla_num { get; set; }
        public BoletaE _Importe { get; set; }
        public double importe { get; set; }
        public string EstadoCambio { get; set; }


    }
}
