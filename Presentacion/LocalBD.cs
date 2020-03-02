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

    List<DetalleInventarioE> listaBoleta = new List<DetalleInventarioE>();
    List<DetalleInventarioE> listaBoletaCambio = new List<DetalleInventarioE>();
    List<DetalleInventarioE> listaFactura = new List<DetalleInventarioE>();

   
    private static readonly LocalBD _instancia = new LocalBD();

    public static LocalBD Instancia
    {
        get { return LocalBD._instancia; }
            }
   

    public List<DetalleInventarioE> ReturnListaBoleta(int getset, int idstock, int cantidad, double precio)
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
                    DetalleInventarioE detBoleta = InventarioN.Instancia.AgregarProductoBoleta(idstock);
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
    public List<DetalleInventarioE> ReturnListaFactura(int getset, int codProd, int cantidad, double precioUnidad)
    {
        try
        {
            if (getset ==1)
            {
                if (cantidad > 1)
                {
                    for (int i = 0; i < listaFactura.Count; i++)
                    {
                        if (listaFactura[i].CodStock == codProd)
                        {
                            listaFactura[i].Cantidad = cantidad;
                            listaFactura[i].Precio = precioUnidad;
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < listaFactura.Count; i++)
                    {
                        if (listaFactura[i].CodStock == codProd)
                        {

                            throw new ApplicationException("El Producto ya está en la lista");
                        }
                    }
                    //usamremos el mismo método agregarproductoboleta por mientras, lo reutilizaremos, si no da problemas lo renombraremos como agregarproductoventa
                    DetalleInventarioE detFactura = InventarioN.Instancia.AgregarProductoBoleta(codProd);
                    detFactura.Cantidad = cantidad;
                    detFactura.Precio = precioUnidad;
                    listaFactura.Add(detFactura);
                    

                }
                
            }
            return listaFactura;
        }
        catch (Exception)
        { throw; }

    }
    public List<DetalleInventarioE> ReturnListaCambio(int getset, int codProd, int cantidad ,double precioUnidad)
    {
        try
        {
            if (getset == 1)
            {
                if (cantidad > 1)
                {
                    for (int i = 0; i < listaBoletaCambio.Count; i++)
                    {
                        if (listaBoletaCambio[i].CodStock == codProd)
                        {
                            listaBoletaCambio[i].Stock = cantidad;
                            listaBoletaCambio[i].MontoCambio = precioUnidad;
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < listaBoletaCambio.Count; i++)
                    {
                        if (listaBoletaCambio[i].CodStock == codProd)
                        {
                            throw new ApplicationException("El producto ya esta en la lista");

                        }
                    }
                    DetalleInventarioE objInventario = VentasN.Instancia.TraerPrendaCambio(codProd);
                    objInventario.Cantidad = cantidad;
                    objInventario.MontoCambio = precioUnidad;
                    listaBoletaCambio.Add(objInventario);
                }

            }
            return listaBoletaCambio;
        }
        catch (Exception)
        { throw; }

    }
    public void RemoverPrendaListaBoleta(int idstock)
    {
        try
        {
            foreach (DetalleInventarioE p in listaBoleta)
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
    public void RemoverPrendaListaFactura(int idStock)
    {
        try
        {
            foreach (DetalleInventarioE p in listaFactura)
            {
                if (p.CodStock == idStock)
                {
                    listaFactura.Remove(p);
                    return;
                }
            }
        }
        catch (Exception)
        { throw; }
    }
    public void RemovePrendaListaCambio(int codProd)
    {
        try
        {
            foreach (DetalleInventarioE p in listaBoletaCambio)
            {
                if (p.CodStock == codProd)
                {
                    listaBoletaCambio.Remove(p);
                    return;
                }
            }

        }
        catch (Exception)
        { throw; }
    }
    public void LimpiarListaCambio()
    {
        try
        {
            listaBoletaCambio.RemoveRange(0, listaBoletaCambio.Count);

        }
        catch (Exception)
        { throw; }
    }
    public void LimpiarListaBoleta()
    {
        try
        {
            listaBoleta.RemoveRange(0, listaBoleta.Count);
        }
        catch (Exception)
        { throw; }
    }
    public void LimpiarListaFactura()
    {
        try
        {
            listaFactura.RemoveRange(0, listaFactura.Count);
        }
        catch (Exception)
        { throw; }
    }

}
