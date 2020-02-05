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
        public string GenerarCodigoBoleta(string control)
        {
            try
            {
                string Abc = "BO";
                string Query = @"Declare @Id Int
                                select top 1 @Id = Left(Codboleta,4) FROM tbboleta  order by Codboleta desc
                                if LEN(@Id) is null
                                begin
                                set @id = 1
                                end
                                print @id
                                Declare @Val int
                                select @Val=COUNT(*) from tbboleta where LEFT(Codboleta,4)=@id
                                if @val = 1
                                 begin
                                 set @Id = @Id+1
                                 set @Val = 1
                                 end
                                else
                                 begin
                                 set @Id = @Id
                                 set @Val = @Val +1
                                 end
 
                                select @Id As Numero,@Val As Abc";
                SqlCommand cmd = new SqlCommand(Query, cn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                DataRow dr;
                dr = dt.Rows[0];

                string codigoTabla = dr[0].ToString();
                int codeTablaConvert = int.Parse(codigoTabla);
                string drCeros = "";
                string numeracion = codeTablaConvert.ToString();
                for (int i = 0; i <= 3 - numeracion.Length; i++)
                {
                    drCeros += "0";

                }
                drCeros += numeracion;
                control = drCeros + "-" + Abc;

                if (cn.State == ConnectionState.Open) cn.Close();
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                
            }
            catch (Exception )
            { throw; }
            return control;

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
        public List<BoletaE> MostrarVentasSimple(string fecha)
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

                    b.Codboleta = dr["Codboleta"].ToString();
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

                    b.Codboleta = dr["Codboleta"].ToString();
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
                cmd.Parameters.AddWithValue("@Codboleta", boleta);
                cn.Open();
                dr = cmd.ExecuteReader();
                lista = new List<BoletaE>();
                while(dr.Read())
                {
                    BoletaE b = new BoletaE();

                    b.Codboleta = dr["Codboleta"].ToString();
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
        public List<DetalleBoletaE> ListarDetalleBoleta(string boleta)
        {
            SqlCommand cmd = null;
            SqlDataReader dr=null;
            List<DetalleBoletaE> lista = null;
            try
            {
                cmd = new SqlCommand(sql.Query_ListarDetalleVenta(), cn);
                cmd.Parameters.AddWithValue("@Codboleta", boleta);
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
                    StockE c = new StockE();
                    c.Color = dr["Color"].ToString();
                    dt.Color = c;
                    StockE t = new StockE();
                    t.Talla_alfanum = dr["Talla_alfanum"].ToString();
                    dt.Talla_alfanum = t;
                    StockE tn = new StockE();
                    tn.Talla_num = Convert.ToInt32(dr["Talla_num"].ToString());
                    dt.Talla_num = tn;
                    dt.Cantidad = Convert.ToInt32(dr["Cantidad"]);
                    dt.Precio_final = Convert.ToDouble(dr["Precio_final"].ToString());
                    lista.Add(dt);


                }

            }
            catch (Exception)
            { throw; }
            finally { cmd.Connection.Close(); }
            return lista;

        }

        
       
    }
}
