using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class BoletaE
    {
        public  int idVenta { get; set; }
        public string CodVenta { get; set; }
        
        public DateTime Fechaboleta { get; set; }
        public double Importe_rg { get; set; }
        public string Descripción { get; set; }
        public int Cantidad { get; set; }
        public double Precio_final { get; set; }
        public int estadoVenta { get; set; }
        public string serieVenta { get; set; }
        public string correlativoVenta { get; set; }
        public int tipoComprobante { get; set; }
        public int tipoPago { get; set; }
        public int idCliente { get; set; }
        public int idUsuario { get; set; }
        
        public List<DetalleBoletaE> detalleBoleta { get; set; }
       
       
    }
}
