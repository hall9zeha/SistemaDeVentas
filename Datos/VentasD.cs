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
        public DataTable MostrarVentasSimple(string fecha)
        {
            try
            {
                string Query = @"
                            select 
                            dt.Codboleta,
                            sum(dt.Cantidad)as Prendas,
                            sum(dt.Precio_final)as Total,
                            b.Fechaboleta
                            from detalle_tbboleta dt inner join tbboleta b
                            
                            on b.Codboleta=dt.Codboleta
                            where b.Fechaboleta=@Fechaboleta
                            group by 
                            dt.Codboleta,
                            dt.Cantidad,
                            b.Fechaboleta
                            ";
                SqlCommand cmd = new SqlCommand(Query, cn);
                cmd.Parameters.AddWithValue("@Fechaboleta", fecha);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
            catch (Exception)
            { throw; }


        }

        
       
    }
}
