using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Datos;
using Entidades;
using Negocio;

namespace Presentacion
{
    public partial class frmVentas : Form

    {

        VentasN objV = new VentasN();
        DataTable dt = new DataTable();
        public frmVentas()
        {
            InitializeComponent();
            //contarItems();
        }
        void crearGrid()
        {
            dgvVentas.Columns.Add("IdVenta", "IdVenta");
            dgvVentas.Columns.Add("CodComprobante", "CodComprobante");
            dgvVentas.Columns.Add("Prendas", "Prendas");
            dgvVentas.Columns.Add("Total", "Total");
            dgvVentas.Columns.Add("FechaVenta", "FechaVenta");
            dgvVentas.Columns.Add("Hora", "Hora");
            dgvVentas.Columns[0].Width = 70;
            dgvVentas.Columns[1].Width = 180;
            dgvVentas.Columns[2].Width = 60;
            dgvVentas.Columns[3].Width = 100;
            dgvVentas.Columns[4].Width = 100;
            dgvVentas.Columns[5].Width = 60;

            dgvVentas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVentas.MultiSelect = false;

        }
        void removerColumnas()
        {
            
            dgvVentas.Columns.Remove("Codboleta");
            dgvVentas.Columns.Remove("Prendas");
            dgvVentas.Columns.Remove("Total");
            dgvVentas.Columns.Remove("FechaBoleta");
        }
        private void Ventas_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            crearGrid();

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            
            cargarVentasYUtilidades(2, pickerFecha1.Text, pickerFecha2.Text,0.ToString());
        }
        void cargarVentasYUtilidades(int tipBusqueda, string fechaSimple, string fechaDoble,string numComprobante)
        {
            double descuento = 0.0, total = 0, boleta = 0.0, factura = 0.0, notaventa = 0.0,
                   efectivo = 0.0, tarjetacred = 0.0, contrareembolso = 0.0, deposito = 0.0, dolares = 0.0, inversion = 0.0, totalUtilidades = 0.0;
            try
            {
               List<VentasE>lista = VentasN.Instancia.ListarVentasYUtilidades(tipBusqueda, fechaSimple, fechaDoble,numComprobante);
                dgvVentas.Rows.Clear();
                for (int i = 0; i < lista.Count; i++)
                {
                    //Parte que ira al grid
                    string[] fila = new string[] {
                        lista[i].IdVenta.ToString(),
                        lista[i].CodVenta,
                        lista[i].DetalleVentas.Cantidad.ToString(),
                        lista[i].Precio_final.ToString("0.00"),
                        lista[i].Fechaboleta.ToString("dd-MM-yy"),
                        lista[i].Fechaboleta.ToString("HH:mm:ss")};
                    dgvVentas.Rows.Add(fila);

                    //Esta parte irá al reporte
                    total += lista[i].Precio_final;
                    inversion += lista[i].DetalleVentas.Inventario.Precio;
                    totalUtilidades += lista[i].Utilidad;
                    if (lista[i].TipoComprobante == 1) boleta += lista[i].Precio_final;
                    else if (lista[i].TipoComprobante == 2) factura += lista[i].Precio_final;
                    else if (lista[i].TipoComprobante == 3) notaventa += lista[i].Precio_final;

                    if (lista[i].TipoPago == 1) efectivo += lista[i].Precio_final;
                    else if (lista[i].TipoPago == 2) tarjetacred += lista[i].Precio_final;
                    else if (lista[i].TipoPago == 3) contrareembolso += lista[i].Precio_final;
                    else if (lista[i].TipoPago == 4) deposito += lista[i].Precio_final;

                }
                contarItems();
                montoVentas();

                lblTotal.Text = string.Format("S/ ") + total.ToString("0.00"); lblBoleta.Text = boleta.ToString("0.00"); lblFactura.Text = factura.ToString("0.00");
                lblNotaventa.Text = notaventa.ToString("0.00"); lblEfectivo.Text = efectivo.ToString("0.00"); lblTarjetacredito.Text = tarjetacred.ToString("0.00");
                lblContrarembolso.Text = contrareembolso.ToString("0.00"); lbldeposito.Text = deposito.ToString("0.00"); lblsoles.Text = "S/ " + total.ToString("0.00"); lbldolares.Text = "$ " + dolares.ToString("0.00");
                lbldescuento.Text = descuento.ToString("0.00");

                /*CÁLCULO DE UTILIDADES*/
                lblImporteInversion.Text = "S/ " + inversion.ToString("0.00");
                lblTotalUtilidades.Text = "S/ " + totalUtilidades.ToString("0.00");
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        
        void contarItems()
        {
            int num = 0;
            foreach (DataGridViewRow row in dgvVentas.Rows)
            {
                num++;
            }
            lblItems.Text = " " + " " + num; 
        }
        void montoVentas()
        {
            double total = 0.0;
            try
            {
                foreach (DataGridViewRow row in dgvVentas.Rows)
                {
                    total += Convert.ToDouble(row.Cells[3].Value.ToString());
                }
                lblMonto.Text = "S/." + total.ToString("0.00");

            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void PickerFecha_ValueChanged(object sender, EventArgs e)
        {
            
            cargarVentasYUtilidades(1, pickerFecha.Value.ToString("yyyy/MM/dd"), 0.ToString(),0.ToString());
        }

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
           
            
            cargarVentasYUtilidades(3, 0.ToString(), 0.ToString(), txtBuscar.Text);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            int idVenta = Convert.ToInt32(dgvVentas.CurrentRow.Cells[0].Value);
            string CodBoleta = Convert.ToString(dgvVentas.CurrentRow.Cells[1].Value);
            
            frmDetalle_Venta objDetalle_Venta = new frmDetalle_Venta(idVenta,CodBoleta);
            objDetalle_Venta.ShowDialog();
        }
    }
}
