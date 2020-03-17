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
            dgvVentas.Columns.Add("CodBoleta/Factura", "CodBoleta/Factura");
            dgvVentas.Columns.Add("Prendas", "Prendas");
            dgvVentas.Columns.Add("Total", "Total");
            dgvVentas.Columns.Add("FechaBoleta", "FechaBoleta");
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
            cargarVentasFechaDoble(pickerFecha1.Text,pickerFecha2.Text);
        }
        void cargarVentasFechaSimple(String fecha)
        {

            try
            {
               
               
                    int num = 0;
                    List<VentasE> lista = VentasN.Instancia.MostrarVentasSimple(fecha);
                    dgvVentas.Rows.Clear();

                    for (int i = 0; i < lista.Count; i++)
                    {
                        num++;
                        string[] fila = new string[] {
                        lista[i].idVenta.ToString(),
                        lista[i].CodVenta,
                        lista[i].Cantidad.ToString(),
                        lista[i].Precio_final.ToString("0.00"),
                        lista[i].Fechaboleta.ToString("dd-MM-yy"),
                        lista[i].Fechaboleta.ToString("HH:mm:ss")};
                        dgvVentas.Rows.Add(fila);

                    }

                contarItems();
                montoVentas();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        void cargarVentasFechaDoble(string fechaIni, string fechaFin)
        {

            try
            {
               
                int num = 0;
                List<VentasE> lista = VentasN.Instancia.MostrarVentasFechaDoble(fechaIni, fechaFin);
                dgvVentas.Rows.Clear();

                for (int i = 0; i < lista.Count; i++)
                {
                    num++;
                    string[] fila = new string[] {
                        lista[i].idVenta.ToString(),
                        lista[i].CodVenta,
                        lista[i].Cantidad.ToString(),
                        lista[i].Precio_final.ToString("0.00"),
                        lista[i].Fechaboleta.ToString("dd-MM-yy"),
                        lista[i].Fechaboleta.ToString("HH:mm:ss")};
                    dgvVentas.Rows.Add(fila);
                }
                contarItems();
                montoVentas();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        void buscarVentaBoleta(string filtro)
        {
            
            try
            {
                if (filtro != string.Empty)
                {
                    int num = 0;
                    List<VentasE> lista = VentasN.Instancia.BuscarVentaBoleta(filtro);
                    dgvVentas.Rows.Clear();

                    for (int i = 0; i < lista.Count; i++)
                    {
                        num++;
                        string[] fila = new string[] {
                        lista[i].idVenta.ToString(),
                        lista[i].CodVenta,
                        lista[i].Cantidad.ToString(),
                        lista[i].Precio_final.ToString("0.00"),
                        lista[i].Fechaboleta.ToString("dd-MM-yy"),
                        lista[i].Fechaboleta.ToString("HH:mm:ss")};
                        dgvVentas.Rows.Add(fila);

                    }
                    contarItems();
                    montoVentas();
                }
                else
                { dgvVentas.Rows.Clear();
                    contarItems();
                    montoVentas();
                }
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
            cargarVentasFechaSimple(pickerFecha.Value.ToString("yyyy/MM/dd"));
        }

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
           
            buscarVentaBoleta(txtBuscar.Text);
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
