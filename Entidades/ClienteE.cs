using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class ClienteE
    {
        public int idCliente { get; set; }
        public int tipoDocumento { get; set; }
        public string descTipDocumento { get; set; }
        public string nroDocumento { get; set; }
        public string nombreCliente { get; set; }
        public string apellidoCliente { get; set; }
        public string sexoCliente { get; set; }
        public string direccionCliente{ get; set; }
        public string telefonoCliente { get; set; }

        public string correoCliente { get; set; }
        public string fechaRegistro { get; set; }

        public int estadoCliente { get; set; }

    }
}
