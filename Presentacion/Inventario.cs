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
using System.IO;
namespace Presentacion
{
    public partial class txtbuscar : Form
    {
        InventarioE objE = new InventarioE();
        InventarioN objN = new InventarioN();
        DataTable dt = new DataTable();

        int tipoBusqueda = 0;
        public txtbuscar()
        {
            InitializeComponent();
        }
        private void CrearGrid()
        {
            dgvInventario.Columns.Add("Código", "Codproducto");
            dgvInventario.Columns.Add("Descripción", "Descripción");
            dgvInventario.Columns.Add("Marca", "Marca");
            dgvInventario.Columns.Add("PrecioCompra", "Precio");
            dgvInventario.Columns.Add("PrecioVenta", "PrecioVenta");
            dgvInventario.Columns.Add("CantidadInicial", "Cantidad");
            dgvInventario.Columns.Add("Stock", "Stcok");


            dgvInventario.Columns[0].Visible = false;
            dgvInventario.Columns[1].Width = 170;
            dgvInventario.Columns[2].Width = 150;
            dgvInventario.Columns[3].Width = 70;
            dgvInventario.Columns[4].Width = 60;
            dgvInventario.Columns[5].Width = 60;
            dgvInventario.Columns[6].Width = 60;

            DataGridViewCellStyle cssCabecera = new DataGridViewCellStyle();
            cssCabecera.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvInventario.ColumnHeadersDefaultCellStyle = cssCabecera;

            dgvInventario.AllowUserToAddRows = false;
            dgvInventario.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInventario.MultiSelect = false;

        }

        void listarInventario()
        {
            dt=objN.ListarInventario();
            dgvInventario.DataSource = dt;

        }
      
        private void Inventario_Load(object sender, EventArgs e)
        {

            CrearGrid();
            CargarInventario();

        }

        private void Button4_Click(object sender, EventArgs e)
        {
            //dgvInventario.Rows.Clear();
            CargarInventario();
        }
        void buscarProducto(string filtro)
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
                        string[] fila = new string[] {
                            lista[i].Codproducto,
                            lista[i].Descripción,
                            lista[i].Marca,
                            lista[i].Precio.ToString(),
                            lista[i].PrecioVenta.ToString(),
                            lista[i].Cantidad.ToString(),
                            lista[i].Stock.ToString() 

                        };
                        dgvInventario.Rows.Add(fila);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            }
        void CargarInventario()
        {
            try
            {
                
                {
                    int num = 0;
                    List<InventarioE> lista = InventarioN.Instancia.ListarInventarioGeneric();
                    dgvInventario.Rows.Clear();
                    for (int i = 0; i < lista.Count; i++)
                    {
                        num++;
                        string[] fila = new string[] {
                            lista[i].Codproducto,
                            lista[i].Descripción,
                            lista[i].Marca,
                            lista[i].Precio.ToString(),
                            lista[i].PrecioVenta.ToString(),
                            lista[i].Cantidad.ToString(),
                            lista[i].Stock.ToString()
                        };
                        dgvInventario.Rows.Add(fila);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            dgvInventario.Rows.Clear();
            tipoBusqueda = 2;
        }

        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            dgvInventario.Rows.Clear();
            tipoBusqueda = 1;
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            dgvInventario.Rows.Clear();
            tipoBusqueda = 3;
        }

        private void Txtfiltro_TextChanged(object sender, EventArgs e)
        {
            buscarProducto(txtfiltro.Text);
        }

        private void DgvInventario_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
