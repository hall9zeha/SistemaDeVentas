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
    public partial class Cambio_de_Prenda : Form
    {
        public Cambio_de_Prenda()
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
            dgvCambioPrenda.Columns.Add("Codboleta", "Codboleta");
            dgvCambioPrenda.Columns.Add("Prendas", "Prendas");
            dgvCambioPrenda.Columns.Add("Total", "Total");
            dgvCambioPrenda.Columns.Add("FechaBoleta", "FechaBoleta");

            dgvCambioPrenda.Columns[0].Width = 70;
            dgvCambioPrenda.Columns[1].Width = 60;
            dgvCambioPrenda.Columns[2].Width = 100;
            dgvCambioPrenda.Columns[3].Width = 100;

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
                    List<BoletaE> lista = VentasN.Instancia.BuscarVentaBoleta(filtro);
                    dgvCambioPrenda.Rows.Clear();

                    for (int i = 0; i < lista.Count; i++)
                    {
                        num++;
                        string[] fila = new string[] {
                        lista[i].Codboleta,
                        lista[i].Cantidad.ToString(),
                        lista[i].Precio_final.ToString("0.00"),
                        lista[i].Fechaboleta.ToString("dd-MM-yy")};
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
                string codBoleta = Convert.ToString(dgvCambioPrenda.CurrentRow.Cells[0].Value);
                Detalle_Cambios objDetalleCambios = new Detalle_Cambios(codBoleta);
                objDetalleCambios.ShowDialog();

            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
    }
}
