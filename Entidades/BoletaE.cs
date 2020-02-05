using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class BoletaE
    {
        public string Codboleta { get; set; }
        
        public DateTime Fechaboleta { get; set; }
        public double Importe_rg { get; set; }
        public string Codproducto { get; set; }
        public int CodProducto_detalle { get; set; }
        public string Descripción { get; set; }
        public int Cantidad { get; set; }
        public double Precio_final { get; set; }
        public DetalleBoletaE DetalleBoleta { get; set; }
        public List<DetalleBoletaE> detalleBoleta { get; set; }
    }
}
