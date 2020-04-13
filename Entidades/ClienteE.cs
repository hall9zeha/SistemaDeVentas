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
        private string nroDocumento;
        public string NroDocumento {
            get {
                if (nroDocumento == "")
                {nroDocumento="Desconocido"; }
                 return nroDocumento;
                }
            set {
                if (nroDocumento == null)
                { nroDocumento = "Desconocido"; }
                nroDocumento = value; 
                }
        }
        private string nombreCliente;
        public string NombreCliente {
            get {
                if (nombreCliente == "")
                { nombreCliente = "Desconocido"; }
                 
                return nombreCliente;   
                }
            set {
                if (nombreCliente == null)
                { nombreCliente = "Desconocido"; }
                nombreCliente = value; 
                }
         }
        public string ApellidoCliente { get; set; }
        public string SexoCliente { get; set; }
        private string direccionCliente;
        public string DireccionCliente{
            get {
                if (direccionCliente == "")
                { direccionCliente = "Desconocido"; }
                return direccionCliente;
                }
            set {
                if (direccionCliente ==null)
                { direccionCliente = "Desconocido"; }
                direccionCliente = value;
                }
                
         }
        public string TelefonoCliente { get; set; }

        public string CorreoCliente { get; set; }
        public string FechaRegistro { get; set; }

        public int EstadoCliente { get; set; }

    }
}
