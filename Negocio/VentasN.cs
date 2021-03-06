﻿using System;
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

        public string GenerarCodigoBoletaFactura(string serie, int tipoComprobante)
        {
            return objV.GenerarCodigoBoletaFactura(serie, tipoComprobante);
        }
       
        public int GuardarVenta(VentasE b)
        {
            try
            {
                if (b.IdCliente == 0) throw new ApplicationException("Debe de seleccionar un Cliente");
                if (b.DetalleVenta.Count == 0) throw new ApplicationException("Debe de Agregar un producto como mínimo");
               
                String Cadxml = "";
                Cadxml += "<tbboleta ";
                Cadxml += "codventa='" + b.CodVenta + "' ";
                Cadxml += "importe='" + b.Importe_rg + "' ";
                Cadxml += "tipocomprobante='" + b.TipoComprobante + "' ";
                Cadxml += "tipopago='" + b.TipoPago + "' ";
                Cadxml += "idcliente='" + b.IdCliente + "' ";
                Cadxml += "idUsuario='" + b.IdUsuario + "' ";
                Cadxml += "tipomoneda='" + b.TipoMoneda + "' ";
                Cadxml += "importe_rg='" + b.Importe_rg + "'>";
                

                foreach (DetalleVentasE dt in b.DetalleVenta)
                {
                    Cadxml += "<detalle_tbboleta ";
                    //Cadxml += "codboleta='" + dt.Codboleta + "' ";
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
        public int AnularVenta(VentasE b)
        {
            try
            {
                String CadXml = "";
                CadXml += "<tbboleta ";
                CadXml += "idventa='" + b.IdVenta + "'>";
                foreach (DetalleVentasE dt in b.DetalleVenta)
                {
                    CadXml += "<tbstock ";
                    CadXml += "codestock='" + dt.CodProducto_detalle + "' ";
                    CadXml += "stock='" + dt.Cantidad + "'/>";

                    CadXml += "<detalle_tbboleta ";
                    CadXml += "idventa='" + b.IdVenta + "'/>";
                }
                CadXml += "</tbboleta>";
                CadXml = "<root>" + CadXml + "</root>";
                int i = VentasD.Instancia.AnularVenta(CadXml);
                if (i <= 0) throw new ApplicationException("Ocurrió un error en la transacción");
                return i;
            }
            catch (Exception)
            { throw; }

        }
        

        
        public List<VentasE> BuscarVenta(string boleta)
        {
            List<VentasE> lista = VentasD.Instancia.BuscarVenta(boleta);
            return lista;
        }
        public List<VentasE> ListarVentas()
        {
            List<VentasE> lista = VentasD.Instancia.ListarVentas();
            return lista;
        }
        public List<VentasE> ListarVentasYUtilidades(int tipBusqueda, string fechaSimple, string fechaDoble,string numComprobante)
        {
            try
            {
                List<VentasE> lista = VentasD.Instancia.ListarVentasYUtilidades(tipBusqueda, fechaSimple, fechaDoble,numComprobante);
               // if (lista.Count <= 0) throw new ApplicationException("No se encontraron registros para el reporte");
                return lista;
            }
            catch (Exception)
            { throw; }
        }
        public List<TipoPagoE> ListarTipoPago()
        {
            List<TipoPagoE> lista = objV.ListarTipoPago();
            return lista;

        }
        public List<MonedaE> ListarMoneda()
        {

            List<MonedaE> lista = objV.ListarMoneda();
            return lista;

        }

        public VentasE ListarDetalleVenta(int idVenta)
        {
            VentasE v= objV.ListarDetalleVenta(idVenta);
            return v;
        }
        public List<DetalleVentasE> ListarDetalleVentaCambio(int idVenta)
        {
          List<DetalleVentasE> lista = objV.ListarDetalleVentaCambio(idVenta);
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
       
        public int GuardarCambioDePrenda(VentasE b)
        {
            try
            {
                string Cadxml = "";
                Cadxml += "<tbboleta ";
                Cadxml += "idventa='" + b.IdVenta + "' ";
                Cadxml += "importe_rg='" + b.Importe_rg + "'>";
              //  Cadxml += "estadocambio='" + b.EstadoCambio + "'>";


                foreach (DetalleVentasE dt in b.DetalleVenta)
                {
                    Cadxml += "<detalle_tbboleta ";
                    Cadxml += "idventa='" + dt.IdVenta + "' ";
                    Cadxml += "codproducto='" + dt.Codproducto + "' ";
                    Cadxml += "codproducto_detalle='" + dt.CodProducto_detalle + "' ";
                    Cadxml += "descripcion='" + dt.Descripción + "' ";
                    Cadxml += "cantidad='" + dt.Cantidad + "' ";
                    

                    Cadxml += "precio_final='" + dt.Precio_final.ToString().Replace(",", ".") + "' ";
                    Cadxml += "estadocambio='" + dt.EstadoCambio + "'/>";

                    Cadxml += "<detalle_tbboleta ";
                    Cadxml += "Dcantidad='" + dt.Cantidad + "' ";
                    Cadxml += "Dcoddetalle='" + dt.Coddetalle + "' ";
                    Cadxml += "Destadocambio='" + dt.EstadoCambio + "'/>";

                    Cadxml += "<tbstock ";
                    Cadxml += "cantidad='" + dt.Cantidad + "' ";
                    Cadxml += "codestock='" + dt.CodProducto_detalle + "' ";
                    Cadxml += "estadocambio='" + dt.EstadoCambio + "'/>";

                    Cadxml += "<tbstock ";
                    Cadxml += "Scantidad='" + dt.Cantidad + "' ";
                    Cadxml += "Scodstock='" + dt.CodProducto_detalle + "' ";
                    Cadxml += "Sestadocambio='" + dt.EstadoCambio + "'/>";
                }



                Cadxml += "</tbboleta>";
                Cadxml = "<root>" + Cadxml + "</root>";
                int resultado = objV.GuardarCambioDePrenda(Cadxml);
                if (resultado <= 0) throw new ApplicationException("Ha ocurrido un error revisa el código porfavor");
                return resultado;

        }
            catch (Exception)
            { throw; }
}
        //FIN métodos de prueba para el módulo devolucion
    }
}
