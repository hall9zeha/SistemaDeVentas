using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using System.Data;
using System.Data.SqlClient;
namespace Datos
{
    public class VentasD
    {
        private static readonly VentasD _instancia = new VentasD();
        public static VentasD Instancia
        {
            get{ return VentasD._instancia; }
        }
        Querys sql = new Querys();

        SqlConnection cn =  Conexion.Instancia.Conectar();
        //Método para generar un código string funcional desde la capa de negocio, aun sin funcionr desde la capa datos
        public string GenerarCodigoBoletaFactura(string serie, int tipoComprobante)
        {
            try
            {
                //string Abc = "BT00";
                
               
                SqlCommand cmd = new SqlCommand(sql.Query_GenerarCodigoBoleta_Factura(), cn);
                cmd.Parameters.AddWithValue("@tipoComprobante", tipoComprobante);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                DataRow dr;
                dr = dt.Rows[0];

                string codigoTabla = dr[0].ToString();
                int codeTablaConvert = int.Parse(codigoTabla);
                string drCeros = "";
                string numeracion = codeTablaConvert.ToString();
                for (int i = 0; i <=7  - numeracion.Length; i++)
                {
                    drCeros += "0";

                }
                drCeros += numeracion;
                string cadena = serie + "-" + drCeros;

                if (cn.State == ConnectionState.Open) cn.Close();
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                return cadena;
            }
            catch (Exception )
            { throw; }
            

        }
      

        //fin del método

        //Método para guardar venta en la tabla tbboleta y detalle_tbboleta, con validación de stock correctamente funcional      
        public int GuardarVenta(string xml)
        {
            var resultado = 0;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(sql.Query_GuardarVenta(), cn);
                
                cmd.Parameters.AddWithValue("@Cadxml", xml);
                cn.Open();
                resultado = cmd.ExecuteNonQuery();

                return resultado;
            }

            catch (Exception)
            { throw; }
            finally { cmd.Connection.Close(); }
        }

        public int AnularVenta(string xml)
        {
            var resultado = 0;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(sql.Query_AnularVenta(), cn);
                cmd.Parameters.AddWithValue("@CadXml", xml);
                cn.Open();
                resultado = cmd.ExecuteNonQuery();
                return resultado;
            }
            catch (Exception)
            { throw; }
            finally { cmd.Connection.Close(); }
        }
        public List<BoletaE> MostrarVentasSimple(String fecha)
        {
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            List<BoletaE> lista = null;
            try
            {
               
                cmd = new SqlCommand(sql.Query_MostrarVentasFecha(), cn);
                cmd.Parameters.AddWithValue("@Fechaboleta", fecha);
                cn.Open();
                dr = cmd.ExecuteReader();
                lista = new List<BoletaE>();
                while (dr.Read())
                {
                    BoletaE b = new BoletaE();

                    b.idVenta = Convert.ToInt32(dr["idVenta"].ToString());
                    b.CodVenta = dr["CodVenta"].ToString();
                    b.Cantidad = Convert.ToInt32(dr["Prendas"].ToString());
                    b.Precio_final = Convert.ToDouble(dr["Total"].ToString());
                    b.Fechaboleta = Convert.ToDateTime(dr["Fechaboleta"].ToString());
                    lista.Add(b);
                }
            }
            catch (Exception)
            { throw; }
            finally { cmd.Connection.Close(); }
            return lista;

        }
        public List<BoletaE> MostrarVentasFechaDoble(string fechaIni, string fechaFin)
        {
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            List<BoletaE> lista = null;
            try
            {

                cmd = new SqlCommand(sql.Query_MostrarVentasFechaDoble(), cn);
                cmd.Parameters.AddWithValue("@FechaBoletaIni", fechaIni);
                cmd.Parameters.AddWithValue("@FechaBoletaFin", fechaFin);
                cn.Open();
                dr = cmd.ExecuteReader();
                lista = new List<BoletaE>();
                while (dr.Read())
                {
                    BoletaE b = new BoletaE();

                    b.idVenta = Convert.ToInt32(dr["idVenta"].ToString());
                    b.CodVenta = dr["CodVenta"].ToString();
                    b.Cantidad = Convert.ToInt32(dr["Prendas"].ToString());
                    b.Precio_final = Convert.ToDouble(dr["Total"].ToString());
                    b.Fechaboleta = Convert.ToDateTime(dr["Fechaboleta"].ToString());
                    lista.Add(b);
                }

            }
            catch (Exception)
            { throw; }
            finally { cmd.Connection.Close(); }
            return lista;

        }
        public List<BoletaE>BuscarVentaBoleta(string boleta)
        {
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            List<BoletaE> lista = null;
            try
            {
                cmd = new SqlCommand(sql.Query_BuscarBoletaVenta(), cn);
                cmd.Parameters.AddWithValue("@CodVenta", boleta);
                cn.Open();
                dr = cmd.ExecuteReader();
                lista = new List<BoletaE>();
                while(dr.Read())
                {
                    BoletaE b = new BoletaE();

                    b.idVenta = Convert.ToInt32(dr["idVenta"].ToString());
                    b.CodVenta = dr["CodVenta"].ToString();
                    b.Cantidad = Convert.ToInt32(dr["Prendas"].ToString());
                    b.Precio_final = Convert.ToDouble(dr["Total"].ToString());
                    b.Fechaboleta = Convert.ToDateTime(dr["Fechaboleta"].ToString());
                    lista.Add(b);
                }
            }
            catch (Exception)
            { throw; }
            finally { cmd.Connection.Close(); }
            return lista;
        }
        public List<DetalleBoletaE> ListarDetalleBoleta(int  idVenta)
        {
            SqlCommand cmd = null;
            SqlDataReader dr=null;
            List<DetalleBoletaE> lista = null;
            try
            {
                cmd = new SqlCommand(sql.Query_ListarDetalleVenta(), cn);
                cmd.Parameters.AddWithValue("@idVenta", idVenta);
                cn.Open();
                dr = cmd.ExecuteReader();
                lista = new List<DetalleBoletaE>();
                while (dr.Read())
                {
                    DetalleBoletaE dt = new DetalleBoletaE();
                    dt.Codproducto = dr["Codproducto"].ToString();
                    dt.Descripción = dr["Descripción"].ToString();
                    InventarioE m = new InventarioE();
                    m.Marca = dr["Marca"].ToString();
                    dt.Marca = m;
                    DetalleInventarioE c = new DetalleInventarioE();
                    c.Color = dr["Color"].ToString();
                    dt.Color = c;
                    DetalleInventarioE t = new DetalleInventarioE();
                    t.Talla_alfanum = dr["Talla_alfanum"].ToString();
                    dt.Talla_alfanum = t;
                    DetalleInventarioE tn = new DetalleInventarioE();
                    tn.Talla_num = Convert.ToInt32(dr["Talla_num"].ToString());
                    dt.Talla_num = tn;
                    dt.Cantidad = Convert.ToInt32(dr["Cantidad"]);
                    dt.Precio_final = Convert.ToDouble(dr["Precio_final"].ToString());
                    dt.CodProducto_detalle = Convert.ToInt32(dr["CodProducto_detalle"].ToString());
                    lista.Add(dt);


                }

            }
            catch (Exception)
            { throw; }
            finally { cmd.Connection.Close(); }
            return lista;

        }

        public List<DetalleBoletaE> ListarDetalleBoletaCambio(int idVenta)
        {
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            List<DetalleBoletaE> lista = null;
            try
            {
                cmd = new SqlCommand(sql.Query_ListarDetalleVentaCambio(), cn);
                cmd.Parameters.AddWithValue("@idVenta", idVenta);
                cn.Open();
                dr = cmd.ExecuteReader();
                lista = new List<DetalleBoletaE>();
                while(dr.Read())
                {
                    DetalleBoletaE dt = new DetalleBoletaE();
                    dt.Codproducto = dr["Codproducto"].ToString();
                    dt.CodProducto_detalle = Convert.ToInt32(dr["CodProducto_detalle"].ToString());
                    dt.Descripción = dr["Descripción"].ToString();
                    InventarioE m = new InventarioE();
                    m.Marca = dr["Marca"].ToString();
                    dt.Marca = m;
                    DetalleInventarioE c = new DetalleInventarioE();
                    c.Color = dr["Color"].ToString();
                    dt.Color = c;
                    DetalleInventarioE t = new DetalleInventarioE();
                    t.Talla_alfanum = dr["Talla_alfanum"].ToString();
                    dt.Talla_alfanum = t;
                    DetalleInventarioE tn = new DetalleInventarioE();
                    tn.Talla_num = Convert.ToInt32(dr["Talla_num"].ToString());
                    dt.Talla_num = tn;
                    dt.Cantidad = Convert.ToInt32(dr["Cantidad"]);
                    dt.Precio_final = Convert.ToDouble(dr["Precio_final"].ToString());
                    dt.Coddetalle = Convert.ToInt32(dr["Coddetalle"].ToString());
                    lista.Add(dt);
                }

            }
            catch (Exception)
            { throw; }
            finally { cmd.Connection.Close(); }
            return lista;
        }
        
        public DetalleInventarioE TraerPrendaCambio(int codProd)
        {
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            DetalleInventarioE dt = null;
            try
            {
                cmd = new SqlCommand(sql.Query_TraerPrendaCambio(), cn);
                cmd.Parameters.AddWithValue("@CodProd",codProd);
                cn.Open();
                dr = cmd.ExecuteReader();
               
                if (dr.Read())
                {
                    dt = new DetalleInventarioE();
                    dt.Codproducto = dr["Codproducto"].ToString();
                    dt.CodStock = Convert.ToInt32(dr["CodEstock"].ToString());
                    InventarioE d = new InventarioE();
                    d.Descripción = dr["Descripción"].ToString();
                    dt.inventario = d;
                    dt.Marca = dr["Marca"].ToString();
                    dt.Color = dr["Color"].ToString();
                    dt.Talla_alfanum = dr["Talla_alfanum"].ToString();
                    dt.Talla_num = Convert.ToInt32(dr["Talla_num"].ToString());
                    dt.Stock = Convert.ToInt32(dr["Stock"].ToString());
                    dt.Precio = Convert.ToDouble(dr["PrecioVenta"].ToString());
                    


                }

            }
            catch (Exception)
            { throw; }
            finally { cmd.Connection.Close(); }
            return dt;
        }
        public List<DetalleInventarioE> BuscarPrendaCambio(string cadenaEntrada)
        {
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            List<DetalleInventarioE> lista = null;

            try
            {
                cmd = new SqlCommand(sql.Query_BuscarPrendaCambio(), cn);
                cmd.Parameters.AddWithValue("@tipobusqueda", cadenaEntrada);
                cn.Open();
                dr = cmd.ExecuteReader();
                lista = new List<DetalleInventarioE>();
                while (dr.Read())
                {
                    DetalleInventarioE dt = new DetalleInventarioE();
                    dt.Codproducto = dr["Codproducto"].ToString();
                    dt.CodStock = Convert.ToInt32(dr["CodEstock"].ToString());
                    InventarioE d = new InventarioE();
                    d.Descripción = dr["Descripción"].ToString();
                    dt.inventario = d;
                    dt.Marca = dr["Marca"].ToString();
                    dt.Color = dr["Color"].ToString();
                    dt.Talla_alfanum = dr["Talla_alfanum"].ToString();
                    dt.Talla_num = Convert.ToInt32(dr["Talla_num"].ToString());
                    dt.Stock = Convert.ToInt32(dr["Stock"].ToString());
                    dt.Precio = Convert.ToDouble(dr["PrecioVenta"].ToString());
                    lista.Add(dt);

                }
            }
            catch (Exception)
            { throw; }
            finally { cmd.Connection.Close(); }
            return lista;
        }
     
        public int GuardarCambioDePrenda(string xml )
        {
            var resultado = 0;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(sql.Query_GuardarCambioDePrenda(), cn);
                cmd.Parameters.AddWithValue("@Cadxml", xml);
               
                cn.Open();
                resultado = cmd.ExecuteNonQuery();
                return resultado;

            }
            catch (Exception)
            { throw; }
            finally { cmd.Connection.Close(); }
        }
    }
}
