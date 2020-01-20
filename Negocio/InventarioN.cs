using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Entidades;
using Datos;

namespace Negocio
{
    public class InventarioN
    {
        InventarioE objE = new InventarioE();
        InventarioD objD = new InventarioD();
        private static readonly InventarioN _instancia = new InventarioN();
        public static InventarioN Instancia
        {
            get { return InventarioN._instancia; }
        }

        public DataTable ListarInventario()
        {
            return InventarioN.Instancia.ListarInventario();
        }

        public List<InventarioE> MostrarInventario(int tipoBusqueda, string valorEntrada)
        {
            try
            {
                List<InventarioE> lista = null;
                lista = InventarioD.Instancia.MostrarInventario(tipoBusqueda, valorEntrada);
                if (lista == null) throw new ApplicationException("No se encontraton registros");
                return lista;

            }
            catch (Exception)
            { throw; }
        }
        public List<InventarioE> ListarInventarioGeneric()
        {
            //try
            //{
                List<InventarioE> lista = InventarioD.Instancia.ListarInventarioGeneric();
                return lista;

            //}
            //catch (Exception)
            //{ throw; }
        }

    }
}
