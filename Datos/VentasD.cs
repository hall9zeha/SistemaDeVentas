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
                cmd = new SqlCommand(query_GuardarVenta(), cn);
                
                cmd.Parameters.AddWithValue("@Cadxml", xml);
                cn.Open();
                resultado = cmd.ExecuteNonQuery();

                return resultado;
            }

            catch (Exception)
            { throw; }
            finally { cmd.Connection.Close(); }
        }
        

        
        private string query_GuardarVenta()
        {
            string Query = @"
                        
                        begin 
                        declare  @h int, @smsError varchar(500)
                        exec sp_xml_preparedocument @h output , @Cadxml
                        begin try
                        begin transaction
                        IF(SELECT COUNT(*) FROM OpenXML(@h,'root/tbboleta/detalle_tbboleta',1)WITH(
		                           codproducto_detalle int,
		                           cantidad int
		                           )dt INNER JOIN tbstock s on s.CodEstock=dt.codproducto_detalle WHERE s.Stock<dt.cantidad)>0
		                          BEGIN
		                           RAISERROR('Uno ó mas productos no cuentan con el stock suficiente',16,1)
		                          END
		                          INSERT INTO tbboleta(Codboleta,Fechaboleta,Importe,Importe_rg)
                                   SELECT b.codboleta,GETDATE(),b.importe, b.importe_rg
		                           FROM OpenXML(@h,'root/tbboleta',1)WITH(
		                           codboleta nvarchar(20),
		                           importe decimal(5,2),
		                           importe_rg decimal(5,2)
		                           )b
		   
		                           INSERT INTO detalle_tbboleta(Codboleta, Codproducto, CodProducto_detalle, Descripción, Cantidad, Precio_final) 
		                           SELECT dt.codboleta,dt.codproducto,dt.codproducto_detalle,dt.descripcion,dt.cantidad, dt.precio_final
		                           FROM OpenXML(@h,'root/tbboleta/detalle_tbboleta',1)WITH(
		                           codboleta nvarchar(20),
		                           codproducto nvarchar(20),
		                           codproducto_detalle int,
		                           descripcion nvarchar(max),
		                           cantidad int,
		                           precio_final decimal(5,2)
		                           )dt   
		                            
                                    update s
		                               set
		                               s.Stock=s.Stock - st.cantidad
		                               from OpenXML(@h, 'root/tbboleta/tbstock',1)with
		                               (cantidad int,
		                               codestock int)st inner join tbstock s on s.CodEstock=st.codestock

		                           IF(@@TRANCOUNT>0) COMMIT TRANSACTION
		                        END TRY

		                        BEGIN CATCH
		                         IF(@@TRANCOUNT>0)
		                           BEGIN
			                         ROLLBACK TRANSACTION
			                         SELECT @smsError = ERROR_MESSAGE()
			                         RAISERROR(@smsError,16,1)
		                           END
		                        END CATCH
	                         END

                            ";
            return Query;

        }
       

    }
}
