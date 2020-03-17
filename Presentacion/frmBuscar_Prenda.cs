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
    public partial class frmBuscar_Prenda : Form
    {
        InventarioE objE = new InventarioE();
        InventarioN objN = new InventarioN();
        int tipoBusqueda = 0;
        int tipoListaUsar = 0;
        Image img = null;
        public frmBuscar_Prenda(int tipoListaUsar)
        {
            InitializeComponent();
            this.tipoListaUsar = tipoListaUsar;
        }

        private void Buscar_Prenda_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            checkBox1.Checked = true;
            crearGrid();
            listarInventario();

        }
        void crearGrid()
        {
            dgvBuscarInventario.Columns.Add("CodProducto", "CodProducto");
            dgvBuscarInventario.Columns.Add("Descripción", "Descripción");
            dgvBuscarInventario.Columns.Add("Marca", "Marca");
            dgvBuscarInventario.Columns.Add("PrecioVenta", "PrecioVenta");
            dgvBuscarInventario.Columns.Add("Stock", "Stock");
            DataGridViewImageColumn Imagen = new DataGridViewImageColumn();
            Imagen.HeaderText = "Estado Stock";
            Imagen.Name = "Estado Stock";
            dgvBuscarInventario.Columns.Add(Imagen);

            dgvBuscarInventario.Columns[0].Visible = true;
            dgvBuscarInventario.Columns[1].Width = 170;
            dgvBuscarInventario.Columns[2].Width = 150;
            dgvBuscarInventario.Columns[3].Width = 100;
            dgvBuscarInventario.Columns[4].Width = 100;
            dgvBuscarInventario.Columns[5].Width = 120;

            DataGridViewCellStyle cssCabecera = new DataGridViewCellStyle();
            cssCabecera.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvBuscarInventario.ColumnHeadersDefaultCellStyle = cssCabecera;

            dgvBuscarInventario.AllowUserToAddRows = false;
            dgvBuscarInventario.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBuscarInventario.AllowUserToResizeColumns = false;

        }
        void buscarInventario(string filtro)
        {
            
            try
            {
                if (filtro != string.Empty)
                {
                    int num = 0;
                    List<InventarioE> lista = InventarioN.Instancia.MostrarInventario(tipoBusqueda, filtro);
                    dgvBuscarInventario.Rows.Clear();
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
                        dgvBuscarInventario.Rows.Add(fila);
                        dgvBuscarInventario.Rows[i].Cells[5].Value = img;
                    }

                }
                else
                { listarInventario(); }
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
                dgvBuscarInventario.Rows.Clear();
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
                    dgvBuscarInventario.Rows.Add(fila);
                    dgvBuscarInventario.Rows[i].Cells[5].Value = img;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Txtbuscar_TextChanged(object sender, EventArgs e)
        {
            buscarInventario(txtbuscar.Text);
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            tipoBusqueda = 2;
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            tipoBusqueda = 3;
        }

        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            tipoBusqueda = 1;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
          string id = Convert.ToString(dgvBuscarInventario.CurrentRow.Cells[0].Value);
            frmDetalle_Prenda Dprenda = new frmDetalle_Prenda(id, tipoListaUsar);
            Dprenda.Show();
           
            
            
        }
    }
}
