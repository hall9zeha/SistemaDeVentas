using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Entidades;
namespace Negocio
{
   public  class DetalleInventarioN
    {
        private static readonly DetalleInventarioN _instancia = new DetalleInventarioN();
        public static DetalleInventarioN Instancia
        {
            get { return DetalleInventarioN._instancia; }
        }

        public List<DetalleInventarioE> TraerDetallePrenda(string Id)
        {
            try
            {
                List<DetalleInventarioE> lista = null;
                lista = InventarioD.Instancia.TraerDetallePrenda(Id);
                return lista;
            }
            catch (Exception)
            { throw; }
        }
    }
}
