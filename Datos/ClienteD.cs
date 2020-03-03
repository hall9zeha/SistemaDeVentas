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


    }
}
