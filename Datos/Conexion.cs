using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Datos
{
    public class Conexion
    {
        //creamos la instancia para acceder a los métodos de la clase desde todo el proyecto
        private static readonly Conexion _instancia= new Conexion();
        
        public static Conexion Instancia
        {
            get { return Conexion._instancia; }
        }
        //************

        //método para conectarnos a la base de datos

        public SqlConnection Conectar()
        {
            try
            {
                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = "data source=./; initial catalog=STORE; integrated security=true";
                return cn;
            }
            catch (Exception)
            { throw; }
        }
        //

    }
}
