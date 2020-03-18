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
    public class ClienteD
    {
        private static readonly ClienteD _instancia = new ClienteD();
        public static ClienteD Instancia
        {
            get { return ClienteD._instancia; }
        }

        SqlConnection cn = Conexion.Instancia.Conectar();
        Querys sql = new Querys();

        public int MantenimientoCliente(string xml)
        {
            var resultado = 0;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(sql.Query_MantenimientoCliente(), cn);
                cmd.Parameters.AddWithValue("@CadXml", xml);
                cn.Open();
                resultado = cmd.ExecuteNonQuery();
                return resultado;
            }
            catch (Exception)
            { throw; }
            finally { cmd.Connection.Close(); }
        }
        public DataTable CargarTipoDoc()
        {
            SqlCommand cmd = new SqlCommand(sql.Query_ListarTipoDocCliente(), cn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        public List<ClienteE> BuscarCliente(int tipoBusqueda, string filtro)
        {

            SqlCommand cmd = null;
            SqlDataReader dr = null;
            List<ClienteE> lista = null;
            try
            {
                cmd = new SqlCommand(sql.query_BuscarCliente(), cn);
                cmd.Parameters.AddWithValue("@tipoBusqueda", tipoBusqueda);
                cmd.Parameters.AddWithValue("@filtro", filtro);
                cn.Open();
                dr = cmd.ExecuteReader();
                lista = new List<ClienteE>();
                while(dr.Read())
                {

                    ClienteE c = new ClienteE();
                    c.IdCliente = Convert.ToInt32(dr["idCliente"].ToString());
                    c.DescTipDocumento = dr["nombreTipoDoc"].ToString();
                    c.NroDocumento = dr["nroDocumento"].ToString();
                    c.NombreCliente = dr["nombreCliente"].ToString();
                    c.ApellidoCliente = dr["apellidoCliente"].ToString();
                    c.SexoCliente = dr["SexoCliente"].ToString();
                    c.DireccionCliente = dr["direccionCliente"].ToString();
                    c.TelefonoCliente = dr["telefonoCliente"].ToString();
                    c.CorreoCliente = dr["correoCliente"].ToString();
                    c.FechaRegistro = dr["fechaRegistro"].ToString();
                    lista.Add(c);
                }
            }
            catch (Exception)
            { throw; }
            finally { cmd.Connection.Close(); }
            return lista;

        }
        public ClienteE TraerCliente(int idCliente, string nroDocumento)
        {
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            ClienteE c = null;
            try
            {
                cmd = new SqlCommand(sql.query_TrerCliente(), cn);
                cmd.Parameters.AddWithValue("@idCliente", idCliente);
                cmd.Parameters.AddWithValue("@nroDocumento", nroDocumento);
                cn.Open();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    c = new ClienteE();
                    c.IdCliente = Convert.ToInt32(dr["idCliente"].ToString());
                    c.DescTipDocumento = dr["nombreTipoDoc"].ToString();
                    c.NroDocumento = dr["nroDocumento"].ToString();
                    c.NombreCliente = dr["nombreCliente"].ToString();
                    c.ApellidoCliente = dr["apellidoCliente"].ToString();
                    c.SexoCliente = dr["SexoCliente"].ToString();
                    c.DireccionCliente = dr["direccionCliente"].ToString();
                    c.TelefonoCliente = dr["telefonoCliente"].ToString();
                    c.CorreoCliente = dr["correoCliente"].ToString();
                    c.FechaRegistro = dr["fechaRegistro"].ToString();
                }
            }
            catch (Exception)
            { throw; }
            finally { cmd.Connection.Close(); }
            return c;
        }
        public List<ClienteE> ListarCliente()
        {
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            List<ClienteE> lista = null;
            try
            {
                cmd = new SqlCommand(sql.query_ListarCliente(), cn);
                cn.Open();
                dr = cmd.ExecuteReader();
                lista = new List<ClienteE>();
                while (dr.Read())
                {
                    ClienteE c = new ClienteE();
                    c.IdCliente = Convert.ToInt32(dr["idCliente"].ToString());
                    c.DescTipDocumento = dr["nombreTipoDoc"].ToString();
                    c.NroDocumento = dr["nroDocumento"].ToString();
                    c.NombreCliente = dr["nombreCliente"].ToString();
                    c.ApellidoCliente = dr["apellidoCliente"].ToString();
                    c.SexoCliente = dr["SexoCliente"].ToString();
                    c.DireccionCliente = dr["direccionCliente"].ToString();
                    c.TelefonoCliente = dr["telefonoCliente"].ToString();
                    c.CorreoCliente = dr["correoCliente"].ToString();
                    c.FechaRegistro = dr["fechaRegistro"].ToString();
                    lista.Add(c);
                }
            }
            catch (Exception)
            { throw; }
            finally { cmd.Connection.Close(); }
            return lista;
        }
    }
}
