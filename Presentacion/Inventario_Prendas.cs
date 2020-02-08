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
    public partial class Inventario_Prendas : Form
    {
        InventarioE objE = new InventarioE();
        InventarioN objN = new InventarioN();
        int tipoBusqueda = 0;
        Image img = null;
        public Inventario_Prendas()
        {
            InitializeComponent();
        }

        private void Inventario_Prendas_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            checkBox1.Checked = true;
            crearGrid();
            listarInventario();
            contarPrendas();

        }
        void crearGrid()
        {
            dgvInventario.Columns.Add("CodProducto", "CodProducto");
            dgvInventario.Columns.Add("Descripción", "Descripción");
            dgvInventario.Columns.Add("Marca", "Marca");
            dgvInventario.Columns.Add("PrecioVenta", "PrecioVenta");
            dgvInventario.Columns.Add("Stock", "Stock");
            DataGridViewImageColumn Imagen = new DataGridViewImageColumn();
            Imagen.HeaderText = "Estado Stock";
            Imagen.Name = "Estado Stock";
            dgvInventario.Columns.Add(Imagen);

            dgvInventario.Columns[0].Visible = true;
            dgvInventario.Columns[1].Width = 170;
            dgvInventario.Columns[2].Width = 150;
            dgvInventario.Columns[3].Width = 100;
            dgvInventario.Columns[4].Width = 100;
            dgvInventario.Columns[5].Width = 120;

            DataGridViewCellStyle cssCabecera = new DataGridViewCellStyle();
            cssCabecera.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvInventario.ColumnHeadersDefaultCellStyle = cssCabecera;

            dgvInventario.AllowUserToAddRows = false;
            dgvInventario.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInventario.AllowUserToResizeColumns = false;

        }
        void contarPrendas()
        {
            try
            {
                int stock = 0;
                foreach (DataGridViewRow row in dgvInventario.Rows)
                {
                    stock += Convert.ToInt32(row.Cells[4].Value.ToString());
                }
                lblPrendas.Text = "N° Total" + " " + stock; 
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        void buscarInventario(string filtro)
        {

            try
            {
                if (filtro != string.Empty)
                {
                    int num = 0;
                    List<InventarioE> lista = InventarioN.Instancia.MostrarInventario(tipoBusqueda, filtro);
                    dgvInventario.Rows.Clear();
                    for (int i = 0; i < lista.Count; i++)
                    {
                        num++;
                        if (lista[i].Stock >= 10)
                        { img = Properties.Resources.circulo_verde24x24; }
                        else if (lista[i].Stock <= 9 && lista[i].Stock >= 1)
                        { img = Properties.Resources.CirculoNaranja24x24; }
                        else if (lista[i].Stock == 0)
                        { img = Properties.Resources.circulorojo_24x24; }

                        string[] fila = new string[] {
                            lista[i].Codproducto,
                            lista[i].Descripción,
                            lista[i].Marca,
                            lista[i].PrecioVenta.ToString("0.00"),
                            lista[i].Stock.ToString()};
                        dgvInventario.Rows.Add(fila);
                        dgvInventario.Rows[i].Cells[5].Value = img;
                        contarPrendas();
                    }

                }
                else
                { listarInventario();
                    contarPrendas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        void listarInventario()
        {
            try
            {
                int num = 0;
                List<InventarioE> lista = objN.ListarInventarioGeneric();
                dgvInventario.Rows.Clear();
                for (int i = 0; i < lista.Count; i++)
                {
                    if (lista[i].Stock >= 10)
                    { img = Properties.Resources.circulo_verde24x24; }
                    else if (lista[i].Stock <= 9 && lista[i].Stock >= 1)
                    { img = Properties.Resources.CirculoNaranja24x24; }
                    else if (lista[i].Stock == 0)
                    { img = Properties.Resources.circulorojo_24x24; }
                    num++;
                    String[] fila = new string[] {
                             lista[i].Codproducto,
                             lista[i].Descripción,
                             lista[i].Marca,
                             lista[i].PrecioVenta.ToString("0.00"),
                             lista[i].Stock.ToString()};
                    dgvInventario.Rows.Add(fila);
                    dgvInventario.Rows[i].Cells[5].Value = img;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

      
        private void Button1_Click(object sender, EventArgs e)
        {
            string id = Convert.ToString(dgvInventario.CurrentRow.Cells[0].Value);
            Detalle_Prenda Dprenda = new Detalle_Prenda(id);
            Dprenda.Show();
        }

        private void Txtbuscar_TextChanged_1(object sender, EventArgs e)
        {
            buscarInventario(txtbuscar.Text);
        }

        private void CheckBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            tipoBusqueda = 2;
        }

        private void CheckBox2_CheckedChanged_1(object sender, EventArgs e)
        {
            tipoBusqueda = 3;
        }

        private void CheckBox3_CheckedChanged_1(object sender, EventArgs e)
        {
            tipoBusqueda = 1;
        }
    }
}
