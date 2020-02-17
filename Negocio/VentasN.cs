using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Entidades;
using System.Data;
namespace Negocio
{
    public class VentasN
    {

        VentasD objV = new VentasD();

        private static readonly VentasN _instancia = new VentasN();
        public static VentasN Instancia
        {
            get { return VentasN._instancia; }

        }

        public string GenerarCodigoBoleta()
        {
            return objV.GenerarCodigoBoleta();
        }
       
        public int GuardarVenta(BoletaE b)
        {
            try
            {
               
                String Cadxml = "";
                Cadxml += "<tbboleta ";
                Cadxml += "codboleta='" + b.Codboleta + "' ";
                Cadxml += "importe='" + b.Importe_rg + "' ";
                Cadxml += "importe_rg='" + b.Importe_rg + "'>";
                

                foreach (DetalleBoletaE dt in b.detalleBoleta)
                {
                    Cadxml += "<detalle_tbboleta ";
                    Cadxml += "codboleta='" + dt.Codboleta + "' ";
                    Cadxml += "codproducto='" + dt.Codproducto + "' ";
                    Cadxml += "codproducto_detalle='" + dt.CodProducto_detalle + "' ";
                    Cadxml += "descripcion='" + dt.Descripción + "' ";
                    Cadxml += "cantidad='" + dt.Cantidad + "' ";
                    Cadxml += "precio_final='" + dt.Precio_final.ToString().Replace(",", ".") + "'/>";
                    Cadxml += "<tbstock ";
                    Cadxml += "cantidad='" + dt.Cantidad + "' ";
                    Cadxml += "codestock='" + dt.CodProducto_detalle + "'/>";
                    
                }
                Cadxml += "</tbboleta>";
                Cadxml = "<root>" + Cadxml + "</root>";
                int i = VentasD.Instancia.GuardarVenta(Cadxml);
                if (i <= 0) throw new ApplicationException("Ocurrio un error al guardar venta actual");
                return i;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public List<BoletaE> MostrarVentasSimple(string fecha)
        {
            List<BoletaE> lista = VentasD.Instancia.MostrarVentasSimple(fecha);
            return lista;
        }

        public List<BoletaE> MostrarVentasFechaDoble(string fechaIni, string fechaFin)
        {
            List<BoletaE> lista = VentasD.Instancia.MostrarVentasFechaDoble(fechaIni,fechaFin);
            return lista;
        }
        public List<BoletaE> BuscarVentaBoleta(string boleta)
        {
            List<BoletaE> lista = VentasD.Instancia.BuscarVentaBoleta(boleta);
            return lista;
        }
        public List<DetalleBoletaE> ListarDetalleBoleta(string boleta)
        {
            List<DetalleBoletaE> lista = objV.ListarDetalleBoleta(boleta);
            return lista;
        }
        public List<DetalleBoletaE> ListarDetalleBoletaCambio(string boleta)
        {
          List<DetalleBoletaE> lista = objV.ListarDetalleBoletaCambio(boleta);
            return lista;
        }
        //INICIO métodos de prueba para el módulo devolucion
        public DetalleInventarioE TraerPrendaCambio(int codProd)
        {
            DetalleInventarioE dt = objV.TraerPrendaCambio(codProd);
            return dt;
        }
        public List<DetalleInventarioE> BuscarPrendaCambio(string cadenaEntrada)
        {
            List<DetalleInventarioE> lista = objV.BuscarPrendaCambio(cadenaEntrada);
            return lista;
        }
        public void RegistrarEntradaPrendaCambio(DetalleBoletaE obj)
        {
            objV.RegistrarEntradaPrendaCambio(obj);

        }
        public void RegistrarSalidaPrendaCambio(DetalleBoletaE obj)
        {
            objV.RegistrarSalidaPrendaCambio(obj);
        }

        public int GuardarCambioDePrenda(BoletaE b)
        {
            try
            {
                string Cadxml = "";
                //Cadxml += "<detalle_tbboleta ";
                //Cadxml += "cantidad='" + b.Cantidad + "' ";
                //Cadxml += "coddetalle='" + b.CodDetalle + "' ";
                //Cadxml += "estadocambio='" + b.EstadoCambio + "'/>";

                //Cadxml += "<tbstock ";
                //Cadxml += "cantidad='" + b.Cantidad + "' ";
                //Cadxml += "codstock='" + b.CodProducto_detalle + "' ";
                //Cadxml += "estadocambio='" + b.EstadoCambio + "'/>";

                Cadxml += "<tbboleta ";
                Cadxml += "codboleta='" + b.Codboleta + "' ";
                Cadxml += "importe_rg='" + b.Importe_rg + "' ";
                Cadxml += "estadocambio='" + b.EstadoCambio + "'>";


                foreach (DetalleBoletaE dt in b.detalleBoleta)
                {
                    Cadxml += "<detalle_tbboleta ";
                    Cadxml += "codboleta='" + dt.Codboleta + "' ";
                    Cadxml += "codproducto='" + dt.Codproducto + "' ";
                    Cadxml += "codproducto_detalle='" + dt.CodProducto_detalle + "' ";
                    Cadxml += "descripcion='" + dt.Descripción + "' ";
                    Cadxml += "cantidad='" + dt.Cantidad + "' ";
                    

                    Cadxml += "precio_final='" + dt.Precio_final.ToString().Replace(",", ".") + "' ";
                    Cadxml += "estadocambio='" + dt.EstadoCambio + "'/>";

                    Cadxml += "<detalle_tbboleta2 ";
                    Cadxml += "Dcantidad='" + dt.Cantidad + "' ";
                    Cadxml += "Dcoddetalle='" + dt.Coddetalle + "' ";
                    Cadxml += "Destadocambio='" + dt.EstadoCambio + "'/>";

                    Cadxml += "<tbstock ";
                    Cadxml += "cantidad='" + dt.Cantidad + "' ";
                    Cadxml += "codestock='" + dt.CodProducto_detalle + "' ";
                    Cadxml += "estadocambio='" + dt.EstadoCambio + "'/>";

                    Cadxml += "<tbstock2 ";
                    Cadxml += "Scantidad='" + dt.Cantidad + "' ";
                    Cadxml += "Scodstock='" + dt.CodProducto_detalle + "' ";
                    Cadxml += "Sestadocambio='" + dt.EstadoCambio + "'/>";
                }



                Cadxml += "</tbboleta>";
                Cadxml = "<root>" + Cadxml + "</root>";
                int resultado = objV.GuardarCambioDePrenda(Cadxml);
                if (resultado <= 0) throw new ApplicationException("Haocurrido un error revisa el código porfavor");
                return resultado;

        }
            catch (Exception)
            { throw; }
}
        //FIN métodos de prueba para el módulo devolucion
    }
}
