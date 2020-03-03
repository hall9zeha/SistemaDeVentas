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
                CadXml += "idcliente='" + c.idCliente + "' ";
                CadXml += "tipodocumento='" + c.tipoDocumento + "' ";
                CadXml += "nrodocumento='" + c.nroDocumento + "' ";
                CadXml += "nombrecliente='" + c.nombreCliente + "' ";
                CadXml += "apellidocliente='" + c.apellidoCliente + "' ";
                CadXml += "sexocliente='" + c.sexoCliente + "' ";
                CadXml += "direccioncliente='" + c.direccionCliente + "' ";
                CadXml += "telefonocliente='" + c.telefonoCliente + "' ";
                CadXml += "correocliente='" + c.correoCliente + "' ";
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
    }
}
