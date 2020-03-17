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
    public partial class frmNota_De_Venta : Form
    {
        VentasN objN = new VentasN();
        VentasE objE = new VentasE();
        ClienteN objCliN = new ClienteN();
        DataTable dt = new DataTable();
        
        public frmNota_De_Venta()
        {
            InitializeComponent();
        }
        void crearGrid()
        {
            dgvDetalleNotaVenta.Columns.Add("CodDet", "CodDet");
            dgvDetalleNotaVenta.Columns.Add("CodProducto", "CodProducto");
            dgvDetalleNotaVenta.Columns.Add("Descripción", "Descripción");
            dgvDetalleNotaVenta.Columns.Add("Marca", "Marca");
            dgvDetalleNotaVenta.Columns.Add("PrecioVenta", "PrecioVenta");
            dgvDetalleNotaVenta.Columns.Add("Cantidad", "Cantidad");
            dgvDetalleNotaVenta.Columns.Add("Stock", "Stock");
            dgvDetalleNotaVenta.Columns.Add("P/Art", "P/Art");

            dgvDetalleNotaVenta.Columns[0].Width = 50;
            dgvDetalleNotaVenta.Columns[1].Width = 80;
            dgvDetalleNotaVenta.Columns[2].Width = 100;
            dgvDetalleNotaVenta.Columns[3].Width = 80;
            dgvDetalleNotaVenta.Columns[4].Width = 60;
            dgvDetalleNotaVenta.Columns[5].Width = 60;
            dgvDetalleNotaVenta.Columns[6].Width = 50;
            dgvDetalleNotaVenta.Columns[7].Width = 70;

            DataGridViewCellStyle css = new DataGridViewCellStyle();
            css.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDetalleNotaVenta.DefaultCellStyle = css;
            dgvDetalleNotaVenta.SelectionMode= DataGridViewSelectionMode.FullRowSelect;
            dgvDetalleNotaVenta.AllowUserToAddRows = false;
            dgvDetalleNotaVenta.AllowUserToResizeColumns = false;


        }

        private void FrmNota_De_Venta_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            string serie = "VE" + 00;
            lblCorrelativo.Text = objN.GenerarCodigoBoletaFactura(serie, 3);
            crearGrid();
        }
        void llenarGridNotaVenta(List<DetalleInventarioE> lista)
        {
            try
            {
                dgvDetalleNotaVenta.Rows.Clear();
               for (int i = 0; i < lista.Count; i++)
                {
                    string[] fila = new string[] {
                        lista[i].CodStock.ToString(),
                        lista[i].Codproducto,
                        lista[i].inventario.Descripción,
                        lista[i].Marca,
                        lista[i].Precio.ToString(),
                        lista[i].Cantidad.ToString(),
                        lista[i].Stock.ToString(),
                        
                    };
                    dgvDetalleNotaVenta.Rows.Add(fila);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void montoTotal()
        {
            try
            {
                double total = 0;
                foreach (DataGridViewRow row in dgvDetalleNotaVenta.Rows)
                {
                    if (row.Cells[4].Value == null) row.Cells[4].Value = 0;
                    {
                        row.Cells[7].Value = Convert.ToDouble(row.Cells[4].Value) * Convert.ToDouble(row.Cells[5].Value);
                    }
                    if (row.Cells[5].Value == null) row.Cells[5].Value = 0;
                    {
                        row.Cells[7].Value = Convert.ToDouble(row.Cells[4].Value) * Convert.ToDouble(row.Cells[5].Value);
                        total += Convert.ToDouble(row.Cells[7].Value);
                    }
                    txtSubtotal.Text = total.ToString("0.00");
                    txtTotal.Text = total.ToString("0.00");

                  
                }
                if (dgvDetalleNotaVenta.RowCount == 0)
                {
                    txtTotal.Text = "0";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void contarItems()
        {
            try
            {
                int num = 0;
                foreach (DataGridViewRow row in dgvDetalleNotaVenta.Rows)
                {
                    num++;
                }
                lblNumItems.Text = "N° Items " + num;  
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        private void BtnAgregarItem_Click(object sender, EventArgs e)
        {
            try
            {
                int tipoListaUsar = 3;
                frmBuscar_Prenda objBuscarPrenda = new frmBuscar_Prenda(tipoListaUsar);
                objBuscarPrenda.ShowDialog();
                List<DetalleInventarioE> lista=LocalBD.Instancia.ReturnListaNotaVenta(0, 0, 0, 0);
                llenarGridNotaVenta(lista);
                contarItems();
                montoTotal();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnQuitarItem_Click(object sender, EventArgs e)
        {
            try
            {
                int codProd = Convert.ToInt32(dgvDetalleNotaVenta.CurrentRow.Cells[0].Value);
                DialogResult dr = MessageBox.Show("Quieres Quitar el producto de la lista?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    LocalBD.Instancia.RemoverPrendaListaNotaVenta(codProd);
                    List<DetalleInventarioE> lista = LocalBD.Instancia.ReturnListaNotaVenta(0, 0, 0, 0);
                    llenarGridNotaVenta(lista);
                    contarItems();
                    montoTotal();
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void DgvDetalleNotaVenta_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DgvDetalleNotaVenta_KeyUp(object sender, KeyEventArgs e)
        {
            montoTotal();
        }
    }
}
