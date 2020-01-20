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
        //método simple usando datatables para listar el contenido de la tabla inventario y stock
        public DataTable ListarInventario()
        {
            try
            {
                string query = @"select i.Codproducto ,
                            i.Descripción,
                            i.Marca,sum(s.Cantidad) as cantidad_inicial,
                            sum(s.Stock) as Stock, 
                            i.Precio as 'precio/unidad',
                            i.PrecioVenta
                            from tbinventario  i inner join tbstock s 
                            on s.Codproducto=i.Codproducto 
                            group by 
                            i.Codproducto ,i.Descripción, i.Marca, i.Precio, i.PrecioVenta order by Codproducto";
                SqlCommand cmd = new SqlCommand(query, cn);
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
                string query = @"if(@tipoBusqueda=1)
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
                cmd = new SqlCommand(query, cn);
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

        public List<InventarioE> ListarInventarioGeneric()
        {
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            List<InventarioE> lista = null;
           
            try
            {
                string query = @"select i.Codproducto ,
                            i.Descripción,
                            i.Marca,sum(s.Cantidad) as cantidad_inicial,
                            sum(s.Stock) as Stock, 
                            i.Precio as 'precio/unidad',
                            i.PrecioVenta
                            from tbinventario  i inner join tbstock s 
                            on s.Codproducto=i.Codproducto 
                            group by 
                            i.Codproducto ,i.Descripción, i.Marca, i.Precio, i.PrecioVenta order by Codproducto";


                cmd = new SqlCommand(query, cn);
                
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

    }

}

