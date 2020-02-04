using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio;
using Entidades;
namespace Presentacion
{
  
    }
   public  class LocalBD
    {

    List<StockE> listaBoleta = new List<StockE>();
   
    private static readonly LocalBD _instancia = new LocalBD();

    public static LocalBD Instancia
    {
        get { return LocalBD._instancia; }
            }
   

    public List<StockE> ReturnListaBoleta(int getset, int idstock, int cantidad, double precio)
    {
        try
        {
            if (getset == 1)
            {
                if (cantidad > 1 )
                {
                    for (int i = 0; i < listaBoleta.Count; i++)
                    {
                        if (listaBoleta[i].CodStock == idstock)
                        {
                            listaBoleta[i].Cantidad = cantidad;
                            listaBoleta[i].Precio = precio;
                            break;
                        }

                    }
                }
              
                else
                {
                    for (int i = 0; i < listaBoleta.Count; i++)
                    {
                        if (listaBoleta[i].CodStock == idstock)
                        {
                            throw new ApplicationException("El producto ya está en la lista");
                        }
                    }
                    StockE detBoleta = InventarioN.Instancia.AgregarProductoBoleta(idstock);
                    detBoleta.Cantidad = cantidad;
                    detBoleta.Precio = precio;
                    listaBoleta.Add(detBoleta);
                }
            }
            return listaBoleta;
        }
        catch (Exception)
        { throw; }
    }
    public void RemovePrendaLista(int idstock)
    {
        try
        {
            foreach (StockE p in listaBoleta)
            {
                if (p.CodStock == idstock)
                {
                    listaBoleta.Remove(p);
                    return;
                }
            }
        }
        catch (Exception )
        {
            throw;
        }
    }

}
