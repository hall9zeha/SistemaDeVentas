using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Entidades;

namespace Datos
{
    public class InventarioD
    {
        private static readonly InventarioD _instancia = new InventarioD();
        public static InventarioD Instancia
        {
            get { return InventarioD._instancia; }
        }

        SqlConnection cn = Conexion.Instancia.Conectar();
        Querys sql = new Querys();
        //método simple usando datatables para listar el contenido de la tabla inventario y stock
        public DataTable ListarInventario()
        {
            try
            {
                string Query = @"select i.Codproducto ,
                            i.Descripción,
                            i.Marca,sum(s.Cantidad) as cantidad_inicial,
                            sum(s.Stock) as Stock, 
                            i.Precio as 'precio/unidad',
                            i.PrecioVenta
                            from tbinventario  i inner join tbstock s 
                            on s.Codproducto=i.Codproducto 
                            group by 
                            i.Codproducto ,i.Descripción, i.Marca, i.Precio, i.PrecioVenta order by Codproducto";
                SqlCommand cmd = new SqlCommand(Query, cn);
                //Ya que no estamos utilizando procedimientos almacenados la siguiente línea de código no es necesaria
                //cmd.CommandType = CommandType.StoredProcedure;
                //*****************************************************************************************************
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            { throw; }
        }
        //creamos este método de listar el inventario porque es más flexible y nos permite mas dinamismo al filtrar por diferentes parámetros
        public List<InventarioE> MostrarInventario(int tipoBusqueda, string valorEntrada)
        {
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            List<InventarioE> lista = null;
            try
            {
               
                cmd = new SqlCommand(sql.Query_MostrarInventario(), cn);
                cmd.Parameters.AddWithValue("@tipoBusqueda", tipoBusqueda);
                cmd.Parameters.AddWithValue("@valorEntrada", valorEntrada);
                cn.Open();
                dr = cmd.ExecuteReader();
                lista = new List<InventarioE>();
                while (dr.Read())
                {
                    InventarioE i = new InventarioE();
                    i.Codproducto = dr["Codproducto"].ToString();
                    i.Descripción = dr["Descripción"].ToString();
                    i.Marca = dr["Marca"].ToString();
                    i.Cantidad = Convert.ToInt32(dr["cantidad_inicial"].ToString());
                    i.Stock = Convert.ToInt32(dr["Stock"].ToString());
                    i.Precio = Convert.ToDouble(dr["precio/unidad"].ToString());
                    i.PrecioVenta = Convert.ToDouble(dr["PrecioVenta"].ToString());
                    lista.Add(i);
                }

            }

            catch (Exception)
            { throw; }
            finally { cmd.Connection.Close(); }
            return lista;
        }
        //Método para listar el contenido de la tabla tbinventario, usando una lista, nos da mas características para trabajar con una colección de datos 
        public List<InventarioE> ListarInventarioGeneric()
        {
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            List<InventarioE> lista = null;
           
            try
            {
               
                cmd = new SqlCommand(sql.Query_ListarInventarioGeneric(), cn);
                
            cn.Open();
            dr = cmd.ExecuteReader();
            lista = new List<InventarioE>();
            while (dr.Read())
            {
                InventarioE i = new InventarioE();
                i.Codproducto = dr["Codproducto"].ToString();
                i.Descripción = dr["Descripción"].ToString();
                i.Marca = dr["Marca"].ToString();
                i.Cantidad = Convert.ToInt32(dr["cantidad_inicial"].ToString());
                i.Stock = Convert.ToInt32(dr["Stock"].ToString());
                i.Precio = Convert.ToDouble(dr["precio/unidad"].ToString());
                i.PrecioVenta = Convert.ToDouble(dr["PrecioVenta"].ToString());
                lista.Add(i);
            }

            }

            catch (Exception)
            { throw; }
            finally { cmd.Connection.Close(); }
            return lista;
        }

      
        public void GenerarCodigoPrenda(string variable)
        {
            try
            {
                string Query = @"Declare @Id Int
                                select top 1 @Id = Left(Codproducto,4) FROM tbinventario  order by Codproducto desc
                                if LEN(@Id) is null
                                begin
                                set @id = 1
                                end
                                print @id
                                Declare @Val int
                                select @Val=COUNT(*) from tbinventario where LEFT(Codproducto,4)=@id
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
                variable=string.Concat("000", dr[0].ToString(),"-", "PT" );
                
                if (cn.State == ConnectionState.Open) cn.Close();
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception)
            { throw; }
            
        }
        public List<StockE> TraerDetallePrenda(string Id)
        {
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            List<StockE> lista = null;
            try
            {
               cmd = new SqlCommand(sql.Query_TraerDetallePrenda(), cn);
                cmd.Parameters.AddWithValue("@Codproducto", Id);
                cn.Open();
                dr = cmd.ExecuteReader();
                lista = new List<StockE>();
                while (dr.Read())
                {
                    StockE objS = new StockE();
                    objS.CodStock = Convert.ToInt32(dr["CodEstock"].ToString());
                    objS.Codproducto = dr["Codproducto"].ToString();
                    InventarioE i = new InventarioE();
                    i.Descripción= dr["Descripción"].ToString();
                    objS.inventario = i;
                    //objS.Descripción = dr["Descripción"].ToString();
                    objS.Marca = dr["Marca"].ToString();
                    objS.Color = dr["Color"].ToString();
                    objS.Talla_alfanum = dr["Talla_alfanum"].ToString();
                    objS.Talla_num = Convert.ToInt32(dr["Talla_num"].ToString());
                    objS.Stock = Convert.ToInt32(dr["Stock"].ToString());
                    objS.Precio = Convert.ToDouble(dr["PrecioVenta"].ToString());
                    lista.Add(objS);

                }
            }
            catch (Exception)
            { throw; }
            finally { cmd.Connection.Close(); }
            return lista;

        }

        public StockE AgregarProductoBoleta(int Id)
        {
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            StockE objS = null;
            try
            {
                cmd = new SqlCommand(sql.Query_AgregarProductoBoleta(), cn);
                cmd.Parameters.AddWithValue("@Codproducto", Id);
                cn.Open();
                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    objS = new StockE();
                    objS.CodStock = Convert.ToInt32(dr["CodEstock"].ToString());
                    objS.Codproducto = dr["Codproducto"].ToString();
                    InventarioE i = new InventarioE();
                    i.Descripción = dr["Descripción"].ToString();
                    objS.inventario = i;
                    //objS.Descripción = dr["Descripción"].ToString();
                    objS.Marca = dr["Marca"].ToString();
                    objS.Color = dr["Color"].ToString();
                    objS.Talla_alfanum = dr["Talla_alfanum"].ToString();
                    objS.Talla_num = Convert.ToInt32(dr["Talla_num"].ToString());
                    objS.Cantidad = Convert.ToInt32(dr["Cantidad"].ToString());
                    objS.Stock = Convert.ToInt32(dr["Stock"].ToString());
                    objS.Precio = Convert.ToDouble(dr["PrecioVenta"].ToString());


                }
            }
            catch (Exception)
            { throw; }
            finally { cmd.Connection.Close(); }
            return objS;

        }
        public InventarioE TraerInventario(string Id)
        {
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            InventarioE objI = null;
            try
            {
                string Query = @"select  * from tbinventario where Codproducto=@Codproducto";
                cmd = new SqlCommand(Query, cn);
                cmd.Parameters.AddWithValue("@Codproducto", Id);
                cn.Open();
                dr = cmd.ExecuteReader();
                
                while(dr.Read())
                {
                     objI= new InventarioE();
                    objI.Codproducto = dr["Codproducto"].ToString();
                    objI.Descripción = dr["Descripción"].ToString();
                    objI.Marca = dr["Marca"].ToString();
                    objI.Precio = Convert.ToDouble(dr["Precio"].ToString());
                    objI.PrecioVenta = Convert.ToDouble(dr["PrecioVenta"].ToString());
                    
                }
            }
            catch (Exception)
            { throw; }
            finally { cmd.Connection.Close(); }
            return objI;
        }
        public void EditarPrenda(InventarioE objI)
        {
            try
            {
                string Query = @"update tbinventario set
                                Descripción=@Descripción,
                                Marca=@Marca,
                                Precio=@Precio,
                                PrecioVenta=@PrecioVenta
                                where Codproducto=@Codproducto";
                SqlCommand cmd = new SqlCommand(Query, cn);
                cmd.Parameters.AddWithValue("@Descripción", objI.Descripción);
                cmd.Parameters.AddWithValue("@Marca", objI.Marca);
                cmd.Parameters.AddWithValue("@Precio", objI.Precio);
                cmd.Parameters.AddWithValue("@PrecioVenta", objI.PrecioVenta);
                cmd.Parameters.AddWithValue("@Codproducto", objI.Codproducto);
                if (cn.State == ConnectionState.Open) cn.Close();
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception)
            { throw; }
        }
        public void EditarDetallePrenda(StockE objS)
        {
            try
            {
                string Query = @"update tbstock set
                               Color=@Color,
                               Talla_alfanum=@Talla_alfanum,
                                Talla_num=@Talla_num,
                                Cantidad=@Cantidad,
                                Stock=@Stock
                                where CodEstock=@CodEstock";
                SqlCommand cmd = new SqlCommand(Query, cn);
                cmd.Parameters.AddWithValue("@Color", objS.Color);
                cmd.Parameters.AddWithValue("@Talla_alfanum", objS.Talla_alfanum);
                cmd.Parameters.AddWithValue("@Talla_num", objS.Talla_num);
                cmd.Parameters.AddWithValue("@Cantidad", objS.Cantidad);
                cmd.Parameters.AddWithValue("@Stock", objS.Cantidad);
                cmd.Parameters.AddWithValue("@CodEstock", objS.CodStock);
                if (cn.State == ConnectionState.Open) cn.Close();
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception)
            { throw; }
        }
        public int RegistrarInventario(string xml)
        {
            var resultado = 0;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(sql.Query_GuardarInventario(), cn);
                cmd.Parameters.AddWithValue("@Cadxml", xml);
                cn.Open();
                resultado = cmd.ExecuteNonQuery();
                return resultado;

            }
            catch (Exception)
            { throw; }
            finally{ cmd.Connection.Close(); }

        }

       
    }

}

