using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Entidades;
using System.Linq;
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
                string Query = @"if(@tipoBusqueda=1)
                begin
                            select i.Codproducto ,
                            i.Descripción,
                            i.Marca,
                            sum(s.Cantidad) as cantidad_inicial,
                            sum(s.Stock) as Stock, 
                            i.Precio as 'precio/unidad',
                            i.PrecioVenta
                            from tbinventario  i inner join tbstock s 
                            on s.Codproducto=i.Codproducto 
                            where i.Codproducto LIKE '%'+@valorEntrada+'%'
                            group by 
                            i.Codproducto ,i.Descripción, i.Marca, i.Precio, i.PrecioVenta order by Codproducto
                            
                end
                            else if(@tipoBusqueda=2)
                begin
                            select i.Codproducto ,
                            i.Descripción,
                            i.Marca,
                            sum(s.Cantidad) as cantidad_inicial,
                            sum(s.Stock) as Stock, 
                            i.Precio as 'precio/unidad',
                            i.PrecioVenta
                            from tbinventario  i inner join tbstock s 
                            on s.Codproducto=i.Codproducto 
                            where i.Descripción LIKE '%'+@valorEntrada+'%'
                            group by 
                            i.Codproducto ,i.Descripción, i.Marca, i.Precio, i.PrecioVenta order by Codproducto
                            
                end
                            else
                begin
                            select i.Codproducto ,
                            i.Descripción,
                            i.Marca,
                            sum(s.Cantidad) as cantidad_inicial,
                            sum(s.Stock) as Stock, 
                            i.Precio as 'precio/unidad',
                            i.PrecioVenta
                            from tbinventario  i inner join tbstock s 
                            on s.Codproducto=i.Codproducto 
                            where i.Marca LIKE '%'+@valorEntrada+'%'
                            group by 
                            i.Codproducto ,i.Descripción, i.Marca, i.Precio, i.PrecioVenta order by Codproducto
                            
                end

                            ";
                cmd = new SqlCommand(Query, cn);
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


                cmd = new SqlCommand(Query, cn);
                
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

        public void AgregarPrenda(InventarioE obj)
        {
            try
            {
                string Query = @"
                                
                                insert into tbinventario
                                (Codproducto, Descripción, Marca, Precio,PrecioVenta)  
                                Values
                                (@Codproducto,@Descripción,@Marca,@Precio, @PrecioVenta)
                                
                                   ";
                SqlCommand cmd = new SqlCommand(Query, cn);
                cmd.Parameters.AddWithValue("@Codproducto", obj.Codproducto);
                cmd.Parameters.AddWithValue("@Descripción", obj.Descripción);
                cmd.Parameters.AddWithValue("@Marca", obj.Marca);
                cmd.Parameters.AddWithValue("@Precio", obj.Precio);
                cmd.Parameters.AddWithValue("@PrecioVenta", obj.PrecioVenta);
                
                if (cn.State == ConnectionState.Open) cn.Close();
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception)
            { throw; }
        }
        public void AgregarDetallePrenda(StockE obj)
        {
            //try
            //{
                string Query = @" insert into tbstock
                                (Codproducto, Color, Talla_alfanum, Talla_num, Cantidad, Stock)
                                Values
                                (@Codproducto, @Color, @Talla_alfanum, @Talla_num, @Cantidad, @Stock)
                                ";
                SqlCommand cmd = new SqlCommand(Query, cn);
                cmd.Parameters.AddWithValue("@Codproducto", obj.Codproducto);
                cmd.Parameters.AddWithValue("@Color", obj.Color);
                cmd.Parameters.AddWithValue("@Talla_alfanum", obj.Talla_alfanum);
                cmd.Parameters.AddWithValue("@Talla_num", obj.Talla_num);
                cmd.Parameters.AddWithValue("@Cantidad", obj.Cantidad);
                cmd.Parameters.AddWithValue("@Stock", obj.Stock);
                if (cn.State == ConnectionState.Open) cn.Close();
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            //}
            //catch (Exception)
            //{ throw; }
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
                string Query = @"select 
                                  s.CodEstock,
                                  s.Codproducto,
                                i.Descripción,
                                i.Marca,
                                s.Color,
                                s.Talla_alfanum,
                                s.Talla_num,
                                s.Cantidad,
                                i.PrecioVenta,
                                s.Stock from tbstock s inner join tbinventario i on s.Codproducto= i.Codproducto where s.Codproducto=@Codproducto";
                cmd = new SqlCommand(Query, cn);
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
                string Query = @"select 
                                  s.CodEstock,
                                  s.Codproducto,
                                i.Descripción,
                                i.Marca,
                                s.Color,
                                s.Talla_alfanum,
                                s.Talla_num,
                                s.Cantidad,
                                s.Stock,
                                i.PrecioVenta,
                                s.Stock from tbstock s inner join tbinventario i on s.Codproducto= i.Codproducto where s.CodEstock=@Codproducto";
                cmd = new SqlCommand(Query, cn);
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
                cmd = new SqlCommand(sp_GuardarInventario(), cn);
                cmd.Parameters.AddWithValue("@Cadxml", xml);
                cn.Open();
                resultado = cmd.ExecuteNonQuery();
                return resultado;

            }
            catch (Exception)
            { throw; }
            finally{ cmd.Connection.Close(); }

        }

        private string sp_GuardarInventario()
        {
            string Query= @"
                    begin
                        declare @h int , @msmError varchar(500)
                        exec sp_xml_preparedocument @h output, @Cadxml
                        begin try
                        begin transaction
                        insert into  tbinventario(Codproducto, Descripción, Marca, Precio, PrecioVenta)
                        select i.codproducto, i.descripcion, i.marca, i.precio, i.precioventa
                        from openxml(@h, 'root/tbinventario', 1)with
                        (
                        codproducto nvarchar(20),
                        descripcion nvarchar(max),
                        marca nvarchar(max),
                        precio decimal(5,2),
                        precioventa decimal(5,2)
                        )i
                        insert into  tbstock(Codproducto, Color, Talla_alfanum, Talla_num, Cantidad, Stock )
                        select s.codproducto, s.color, s.talla_alfanum, s.talla_num, s.cantidad, s.stock
                        from openxml(@h, 'root/tbinventario/tbstock',1)with
                        (
                        codproducto nvarchar(20),
                        color nvarchar(max),
                        talla_alfanum nvarchar(max),
                        talla_num int ,
                        cantidad int,
                        stock int
                        )s
                        if (@@trancount>0)commit transaction
                        end try
                        begin catch
                        if(@@trancount>0)
                        begin
                        rollback transaction
                        select @msmError=ERROR_MESSAGE()
                        raiserror(@msmError,16,1)
                        end 
                        end catch
                        end
    
                    ";
            return Query;

        }

    }

}

