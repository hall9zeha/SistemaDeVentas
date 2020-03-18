using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using Datos;
using System.Data;
namespace Negocio
{
    public class ClienteN
    {

        ClienteD objCli = new ClienteD();
        private static readonly ClienteN _instancia = new ClienteN();
        public static ClienteN Instancia
        {
            get { return ClienteN._instancia;  }
        }
        public int MantenimientoCliente(ClienteE c, int tipoaccion)
        {
            try
            {
                String CadXml = "";
                CadXml += "<tbClientes ";
                CadXml += "idcliente='" + c.IdCliente + "' ";
                CadXml += "tipodocumento='" + c.TipoDocumento + "' ";
                CadXml += "nrodocumento='" + c.NroDocumento + "' ";
                CadXml += "nombrecliente='" + c.NombreCliente + "' ";
                CadXml += "apellidocliente='" + c.ApellidoCliente + "' ";
                CadXml += "sexocliente='" + c.SexoCliente + "' ";
                CadXml += "direccioncliente='" + c.DireccionCliente + "' ";
                CadXml += "telefonocliente='" + c.TelefonoCliente + "' ";
                CadXml += "correocliente='" + c.CorreoCliente + "' ";
                CadXml += "tipoaccion='" + tipoaccion + "'/>";
                CadXml = "<root>" + CadXml + "</root>";
                int resultado = ClienteD.Instancia.MantenimientoCliente(CadXml);
                if (resultado <= 0) throw new ApplicationException("Ha ocurrido un error en la transacción");
                return resultado;
            }
            catch (Exception)
            { throw; }

        }
        public DataTable CargarTipoDoc()
        {
            return objCli.CargarTipoDoc();
        }
        public List<ClienteE> BuscarCliente(int tipoBusqueda, string filtro)
        {
            List<ClienteE> lista = objCli.BuscarCliente(tipoBusqueda, filtro);
            return lista;
        }
        public ClienteE TraerCliente(int idCliente, string nroDocumento)
        {
            try
            {
                ClienteE c = objCli.TraerCliente(idCliente, nroDocumento);
                if (c == null) throw new ApplicationException("Ha ocurrido un error en la transacción");
                return c;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<ClienteE> ListarCliente()
        {
            List<ClienteE>lista= objCli.ListarCliente();
            if (lista == null) throw new ApplicationException("Ha ocurrido un error en la transacción");
            return lista;
        }
    }
}
