using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class VentasE
    {
        public int IdVenta { get; set; }
        public string CodVenta { get; set; }

        public DateTime Fechaboleta { get; set; }
        public double Importe_rg { get; set; }
        public string Descripción { get; set; }
        public int Cantidad { get; set; }
        public double Precio_final { get; set; }
        public int EstadoVenta { get; set; }
        public string SerieVenta { get; set; }
        public string CorrelativoVenta { get; set; }
        public int TipoComprobante { get; set; }
        public int TipoPago { get; set; }
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public int TipoMoneda { get; set; }
        //campo utilidad solo para calcular 
        public double Utilidad { get; set; }
        //propiedade de tipo lista con contenido de elementos de la entidad DetalleVenta
        public List<DetalleVentasE> DetalleVenta { get; set; }
        //relaciones con DetalleVentase
        public DetalleVentasE DetalleVentas { get; set; }
        //relaciones con entidadCliente
        public ClienteE Cliente { get; set; }
        //*****************************************
        //relaciones con entidadMoneda
        public MonedaE Moneda { get; set; }
        //******************************************
        //relaciones con entidadTipoDocumento
        public TipoDocumentoE TipoDocumento { get; set; }
        //******************************************
       //relacion con entidadTipoPago
        public TipoPagoE TipPago { get; set; }
        //******************************************
    }
}
