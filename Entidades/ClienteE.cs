using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class ClienteE
    {
        public int IdCliente { get; set; }
        public int TipoDocumento { get; set; }
        public string DescTipDocumento { get; set; }
        public string NroDocumento { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
        public string SexoCliente { get; set; }
        public string DireccionCliente{ get; set; }
        public string TelefonoCliente { get; set; }

        public string CorreoCliente { get; set; }
        public string FechaRegistro { get; set; }

        public int EstadoCliente { get; set; }

    }
}
