using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Entidades;
namespace Negocio
{
   public  class StockN
    {
        private static readonly StockN _instancia = new StockN();
        public static StockN Instancia
        {
            get { return StockN._instancia; }
        }

        public List<StockE> TraerDetallePrenda(string Id)
        {
            try
            {
                List<StockE> lista = null;
                lista = InventarioD.Instancia.TraerDetallePrenda(Id);
                return lista;
            }
            catch (Exception)
            { throw; }
        }
    }
}
