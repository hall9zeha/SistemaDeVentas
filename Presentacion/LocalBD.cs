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
    List<DetalleInventarioE> listaCambio = new List<DetalleInventarioE>();
    List<DetalleInventarioE> listaFactura = new List<DetalleInventarioE>();
    List<DetalleInventarioE> listaNotaVenta = new List<DetalleInventarioE>();
    int _idCliente = 0, _idClienteFact=0, _idClienteNV=0;
    int invocador = 0;
   
    private static readonly LocalBD _instancia = new LocalBD();

    public static LocalBD Instancia
    {
        get { return LocalBD._instancia; }
            }

    public int ReturnIdCliente(int getset, int idCliente)
    {
        try
        {
            if (getset == 1)
            {
                _idCliente = idCliente;

            }
            return _idCliente;
        }
        catch (Exception)
        { throw; }
    }
    public int ReturnIdClienteFact(int getset, int idCliente)
    {
        try
        {
            if (getset == 1)
            {
                _idClienteFact = idCliente;
            }
            return _idClienteFact;
        }
        catch (Exception)
        { throw; }
        
    }
    public int ReturnIdClienteNV(int getset, int idCliente)
    {
        try
        {
            if (getset == 1)
            {
                _idClienteNV = idCliente;
            }
            return _idClienteNV;
        }
        catch (Exception)
        { throw; }
    }
    public int Invocador(int getset, int frm)
    {
        try
        {
            if (getset == 1)
                invocador = frm;
        }
        catch (Exception)
        { throw; }
        return invocador;
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
    public List<DetalleInventarioE> ReturnListaNotaVenta(int getset, int codProd, int cantidad, double precioUnidad)
    {
        try
        {
            if (getset == 1)
            {
                if (cantidad > 1)
                {
                    for (int i = 0; i < listaNotaVenta.Count; i++)
                    {
                        if (listaNotaVenta[i].CodStock == codProd)
                        {
                            listaNotaVenta[i].Precio = precioUnidad;
                            listaNotaVenta[i].Cantidad = cantidad;
                            break;

                        }
                    }
                }
                else
                {
                    for (int i = 0; i < listaNotaVenta.Count; i++)
                    {
                        if (listaNotaVenta[i].CodStock == codProd)
                        {
                            throw new ApplicationException("El producto ya está en la lista");
                        }
                    }
                }
                DetalleInventarioE detNotaVenta = InventarioN.Instancia.AgregarProductoBoleta(codProd);
                detNotaVenta.Cantidad = cantidad;
                detNotaVenta.Precio = precioUnidad;
                listaNotaVenta.Add(detNotaVenta);
                

            }
            return listaNotaVenta;
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
                    for (int i = 0; i < listaCambio.Count; i++)
                    {
                        if (listaCambio[i].CodStock == codProd)
                        {
                            listaCambio[i].Stock = cantidad;
                            listaCambio[i].MontoCambio = precioUnidad;
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < listaCambio.Count; i++)
                    {
                        if (listaCambio[i].CodStock == codProd)
                        {
                            throw new ApplicationException("El producto ya esta en la lista");

                        }
                    }
                    DetalleInventarioE objInventario = VentasN.Instancia.TraerPrendaCambio(codProd);
                    objInventario.Cantidad = cantidad;
                    objInventario.MontoCambio = precioUnidad;
                    listaCambio.Add(objInventario);
                }

            }
            return listaCambio;
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

    public void RemoverPrendaListaNotaVenta(int codProd)
    {
        try
        {
            foreach (DetalleInventarioE p in listaNotaVenta)
            {
                if (p.CodStock == codProd)
                {
                    listaNotaVenta.Remove(p);
                    return;
                }
            }

        }
        catch (Exception)
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
            foreach (DetalleInventarioE p in listaCambio)
            {
                if (p.CodStock == codProd)
                {
                    listaCambio.Remove(p);
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
            listaCambio.RemoveRange(0, listaCambio.Count);

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

    public void LimpiarListaNotaVenta()
    {
        try
        {
            listaNotaVenta.RemoveRange(0, listaNotaVenta.Count);
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
