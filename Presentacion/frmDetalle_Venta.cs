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
    public partial class frmDetalle_Venta : Form
    {
        int idVenta = 0;
        string codBoleta = "";
        public frmDetalle_Venta(int idVenta,string CodBoleta)
        {
            InitializeComponent();
            this.codBoleta = CodBoleta;
            this.idVenta = idVenta;
        }

        private void Detalle_Venta_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            crearGrid();
            listarDetalleVenta();
            contarItems();
            montoTotal();
            lblBoleta.Text = codBoleta;
        }
        void contarItems()
        {
            int num = 0;
            foreach (DataGridViewRow row in dgvDetalleVenta.Rows)
            {
                num++;
            }
            lblItems.Text = "N° Items " + " " + num;
        }
        void montoTotal()
        {
            double total = 0.0;
            foreach (DataGridViewRow row in dgvDetalleVenta.Rows)
            {
                total += Convert.ToDouble(row.Cells[7].Value);
            }
            txtTotal.Text="S/." + " " + total.ToString("0.00");
        }
        void crearGrid()
        {
            dgvDetalleVenta.Columns.Add("CodProducto", "CodProducto");
            dgvDetalleVenta.Columns.Add("descripción", "descripción");
            dgvDetalleVenta.Columns.Add("Marca", "Marca");
            dgvDetalleVenta.Columns.Add("Color", "Color");
            dgvDetalleVenta.Columns.Add("TallaA", "TallaA");
            dgvDetalleVenta.Columns.Add("TallaN", "TallaN");
            dgvDetalleVenta.Columns.Add("Cantidad", "Cantidad");
            dgvDetalleVenta.Columns.Add("Importe", "Importe");
            dgvDetalleVenta.Columns.Add("Cod/D", "Cod/D");

            dgvDetalleVenta.Columns[0].Width = 70;
            dgvDetalleVenta.Columns[1].Width = 120;
            dgvDetalleVenta.Columns[2].Width = 100;
            dgvDetalleVenta.Columns[3].Width = 90;
            dgvDetalleVenta.Columns[4].Width = 60;
            dgvDetalleVenta.Columns[5].Width = 60;
            dgvDetalleVenta.Columns[6].Width = 50;
            dgvDetalleVenta.Columns[7].Width = 70;
            dgvDetalleVenta.Columns[8].Width = 50;

            dgvDetalleVenta.AllowUserToAddRows = false;
            dgvDetalleVenta.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetalleVenta.MultiSelect = false;
        }
        void listarDetalleVenta()
        {
            try
            {
                int num = 0;
                List<DetalleVentasE> lista = VentasN.Instancia.ListarDetalleBoleta(idVenta);
                dgvDetalleVenta.Rows.Clear();
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
                        lista[i].CodProducto_detalle.ToString()


                    };
                    dgvDetalleVenta.Rows.Add(fila);
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        void anularVenta()
        {
            VentasE b = new VentasE();
            //b.Codboleta = lblBoleta.Text;
            List<DetalleVentasE> detalle = new List<DetalleVentasE>();
            foreach (DataGridViewRow row in dgvDetalleVenta.Rows)
            {
                DetalleVentasE dt = new DetalleVentasE();
                dt.CodProducto_detalle = Convert.ToInt32(row.Cells[8].Value.ToString());
                dt.Cantidad = Convert.ToInt32(row.Cells[6].Value.ToString());
                detalle.Add(dt);
            }
            b.DetalleVenta = detalle;
            int resultado = VentasN.Instancia.AnularVenta(b);
            MessageBox.Show("Venta Anulada");
        }


        private void BtnAnular_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Realmente Quieres anular esta venta", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                anularVenta();
            }
            
        }
            
    }
}
