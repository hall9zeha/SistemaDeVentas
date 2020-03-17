using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Negocio;
using Entidades;
namespace Presentacion
{
    public partial class frmCambio_de_Prenda : Form
    {
        public frmCambio_de_Prenda()
        {
            InitializeComponent();
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Cambio_de_Prenda_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            crearGrid();
        }
        void crearGrid()
        {
            dgvCambioPrenda.Columns.Add("CodVenta", "CodVenta");
            dgvCambioPrenda.Columns.Add("Boleta/Factura", "Boleta/Factura");
            dgvCambioPrenda.Columns.Add("Prendas", "Prendas");
            dgvCambioPrenda.Columns.Add("Total", "Total");
            dgvCambioPrenda.Columns.Add("FechaBoleta", "FechaBoleta");
            dgvCambioPrenda.Columns.Add("Hora", "Hora");

            dgvCambioPrenda.Columns[0].Width = 70;
            dgvCambioPrenda.Columns[1].Width = 100;
            dgvCambioPrenda.Columns[2].Width = 50;
            dgvCambioPrenda.Columns[3].Width = 60;
            dgvCambioPrenda.Columns[4].Width = 70;
            dgvCambioPrenda.Columns[5].Width = 60;

            dgvCambioPrenda.AllowUserToAddRows = false;
            dgvCambioPrenda.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCambioPrenda.MultiSelect = false;

        }
        void buscarVentaBoleta(string filtro)
        {

            try
            {
                if (filtro != string.Empty)
                {
                    int num = 0;
                    List<VentasE> lista = VentasN.Instancia.BuscarVentaBoleta(filtro);
                    dgvCambioPrenda.Rows.Clear();

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
                        dgvCambioPrenda.Rows.Add(fila);

                    }
                   
                }
                else
                {
                    dgvCambioPrenda.Rows.Clear();
                   
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            buscarVentaBoleta(txtBuscar.Text);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                int idVenta = Convert.ToInt32(dgvCambioPrenda.CurrentRow.Cells[0].Value);
                string codBoleta = Convert.ToString(dgvCambioPrenda.CurrentRow.Cells[1].Value);
                frmDetalle_Cambios objDetalleCambios = new frmDetalle_Cambios(idVenta, codBoleta);
                objDetalleCambios.ShowDialog();

            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
    }
}
