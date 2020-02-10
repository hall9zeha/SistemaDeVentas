using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entidades;
using Negocio;
namespace Presentacion
{
    public partial class Detalle_Cambios : Form
    {
        VentasN objVN = new VentasN();
        string _codBoleta = "";
        public Detalle_Cambios(string codBoleta)
        {
            InitializeComponent();
            this._codBoleta = codBoleta;
        }

        private void LblBoleta_Click(object sender, EventArgs e)
        {

        }

        private void Detalle_Cambios_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            crearGrid();
            listarDetalleBoletaCambio();
            montoTotal();
            lblBoleta.Text = _codBoleta;

        }
        void crearGrid()
        {
            dgvDetalleCambio.Columns.Add("Codproducto", "Codproducto");
            dgvDetalleCambio.Columns.Add("Descripción", "Descripción");
            dgvDetalleCambio.Columns.Add("Marca", "Marca");
            dgvDetalleCambio.Columns.Add("Color", "Color");
            dgvDetalleCambio.Columns.Add("TallaA", "TallaA");
            dgvDetalleCambio.Columns.Add("TallaN", "TallaN");
            dgvDetalleCambio.Columns.Add("Cantidad", "Cantidad");
            dgvDetalleCambio.Columns.Add("Importe", "Importe");
            dgvDetalleCambio.Columns.Add("CodDet.", "CodDet.");
            dgvDetalleCambio.Columns.Add("E/C", "E/C");//significa Estado/Cambio.

            dgvDetalleCambio.Columns[0].Width = 100;
            dgvDetalleCambio.Columns[1].Width = 100;
            dgvDetalleCambio.Columns[2].Width = 70;
            dgvDetalleCambio.Columns[3].Width = 60;
            dgvDetalleCambio.Columns[4].Width = 50;
            dgvDetalleCambio.Columns[5].Width = 50;
            dgvDetalleCambio.Columns[6].Width = 50;
            dgvDetalleCambio.Columns[7].Width = 50;
            dgvDetalleCambio.Columns[8].Width = 50;
            dgvDetalleCambio.Columns[9].Width = 30;

            dgvDetalleCambio.AllowUserToAddRows = false;
            dgvDetalleCambio.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetalleCambio.MultiSelect = false;


        }
        void listarDetalleBoletaCambio()
        {
            try
            {
                int num = 0;
                List<DetalleBoletaE> lista = objVN.ListarDetalleBoletaCambio(_codBoleta);
                dgvDetalleCambio.Rows.Clear();
                for (int i = 0; i < lista.Count; i++)
                {
                    num++;
                    string[] fila = new string[] {
                        lista[i].Codproducto,
                        lista[i].Descripción,
                        lista[i].Marca.Marca,
                        lista[i].Color.Color,
                        lista[i].Talla_alfanum.Talla_alfanum,
                        lista[i].Talla_num.Talla_num.ToString(),
                        lista[i].Cantidad.ToString(),
                        lista[i].Precio_final.ToString("0.00"),
                        lista[i].Coddetalle.ToString()


                    };
                    dgvDetalleCambio.Rows.Add(fila);
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        void montoTotal()
        {
            try
            {
                double monto = 0.0;
                foreach (DataGridViewRow row in dgvDetalleCambio.Rows)
                {
                    monto += Convert.ToDouble(row.Cells[7].Value.ToString());
                    
                }
                txtTotal.Text = "S/." + " " + monto.ToString("0.00");
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void BtnAgregarItem_Click(object sender, EventArgs e)
        {
            dgvDetalleCambio.CurrentRow.Cells[9].Value = "C";
            marcarPrendaACambiar();
            Buscar_Cambio objBuscarCambio = new Buscar_Cambio();
            objBuscarCambio.ShowDialog();
        }

        private void BtnQuitarItem_Click(object sender, EventArgs e)
        {

            dgvDetalleCambio.CurrentRow.Cells[9].Value = null;
            marcarPrendaACambiar();
        }
        void marcarPrendaACambiar()
        {
            foreach (DataGridViewRow row in dgvDetalleCambio.Rows)
            {
                if (row.Cells[9].Value != null)
                {
                    row.DefaultCellStyle.BackColor = Color.Orange;
                }
                else if(row.Cells[9].Value == null)
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                }
            }
        }
    }
}
