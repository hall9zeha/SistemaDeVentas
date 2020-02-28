using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Entidades;
using Datos;

namespace Negocio
{
    public class InventarioN
    {
        InventarioE objE = new InventarioE();
        InventarioD objD = new InventarioD();
        private static readonly InventarioN _instancia = new InventarioN();
        public static InventarioN Instancia
        {
            get { return InventarioN._instancia; }
        }

        public DataTable ListarInventario()
        {
            return InventarioN.Instancia.ListarInventario();
        }

        public List<InventarioE> MostrarInventario(int tipoBusqueda, string valorEntrada)
        {
            try
            {
                List<InventarioE> lista = null;
                lista = InventarioD.Instancia.MostrarInventario(tipoBusqueda, valorEntrada);
                if (lista == null) throw new ApplicationException("No se encontraton registros");
                return lista;

            }
            catch (Exception)
            { throw; }
        }
        public List<InventarioE> ListarInventarioGeneric()
        {
            try
            {
                List<InventarioE> lista = InventarioD.Instancia.ListarInventarioGeneric();
                return lista;

            }
            catch (Exception)
            { throw; }
        }

        public string GenerarCodigoPrenda()
        {
           return  objD.GenerarCodigoPrenda();
        }
        public List<DetalleInventarioE> TraerDetallePrenda(string Id)
        {
            try
            {
                List<DetalleInventarioE> lista = null;
                lista = InventarioD.Instancia.TraerDetallePrenda(Id);
                return lista;
            }
            catch (Exception)
            { throw; }
        }


        public DetalleInventarioE AgregarProductoBoleta(int id)
        {
            try
            {
                DetalleInventarioE objS = InventarioD.Instancia.AgregarProductoBoleta(id);
                return objS;
            }
            catch (Exception)
            { throw; }
        }
        public int GuardarPrendaInventario(InventarioE i)
        {
            try
            {
                String CadXml = "";
                CadXml += "<tbinventario ";
                CadXml += "codproducto='" + i.Codproducto + "' ";
                CadXml += "descripcion='" + i.Descripción + "' ";
                CadXml += "marca='" + i.Marca + "' ";
                CadXml += "precio='" + i.Precio + "' ";
                CadXml += "precioventa='" + i.PrecioVenta + "'>";

                foreach (DetalleInventarioE s in i.detalleInventario)
                {
                    CadXml += "<tbstock ";
                    CadXml += "codproducto='" + s.Codproducto + "' ";
                    CadXml += "color='" + s.Color + "' ";
                    CadXml += "talla_alfanum='" + s.Talla_alfanum + "' ";
                    CadXml += "talla_num='" + s.Talla_num + "' ";
                    CadXml += "cantidad='" + s.Cantidad + "' ";
                    CadXml += "stock='" + s.Stock + "'/>";
                }
                CadXml += "</tbinventario>";
                CadXml = "<root>" + CadXml + "</root>";
                int result = InventarioD.Instancia.GuardarPrendaInventario(CadXml);
                if (result <= 0) throw new ApplicationException("Hubo un error en la tansaccion");
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int MantenimientoDetalleInventario(DetalleInventarioE s, int tipoaccion)
        {
            try
            {
                string Cadxml = "";

                Cadxml += "<tbstock ";
                Cadxml += "codestock='" + s.CodStock + "' ";
                Cadxml += "codproducto='" + s.Codproducto + "' ";
                Cadxml += "color='" + s.Color + "' ";
                Cadxml += "talla_alfanum='" + s.Talla_alfanum + "' ";
                Cadxml += "talla_num='" + s.Talla_num + "' ";
                Cadxml += "cantidad='" + s.Cantidad + "' ";
                Cadxml += "stock='" + s.Stock + "' ";
                Cadxml += "tipoaccion='" + tipoaccion + "'/>";
                Cadxml = "<root>" + Cadxml + "</root>";
                int result = objD.MantenimientoDetalleInventario(Cadxml);
                if (result <= 0) throw new ApplicationException("Hubo un error en la transacción");
                return result;

            }
            
            catch (Exception)
            { throw; }
        }
        public InventarioE TraerInventario(string Id)
        {
            try
            {
                InventarioE prenda = null;
                prenda = InventarioD.Instancia.TraerInventario(Id);
                return prenda;
            }
            catch (Exception)
            { throw; }
        }
        public void EditarPrenda(InventarioE objI)
        {
            objD.EditarPrenda(objI);
        }
        public int EliminarPrenda(InventarioE objI)
        {
            String CadXml = "";
            CadXml += "<tbinventario ";
            CadXml += "codproducto ='" + objI.Codproducto + "'/>";
            CadXml = "<root>" + CadXml + "</root>";
            int resultado = InventarioD.Instancia.EliminarPrenda(CadXml);
            if (resultado <= 0) throw new ApplicationException("Hubo un error en la transacción");
            return resultado;
        }
      

    }
}
