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
    public partial class frmDetalle_Prenda : Form
    {
        InventarioE objE = new InventarioE();
        InventarioN objN = new InventarioN();
        string Id = "";
        int tipoListaUsar = 0;
        Image img = null;
        public frmDetalle_Prenda(string id ,int tipoListaUsar)
        {
            InitializeComponent();
            this.Id = id;
            this.tipoListaUsar = tipoListaUsar;
        }
        void CrearGrid()
        {
            dgvdetalle.Columns.Add("CodStock", "CodStock");
            dgvdetalle.Columns.Add("CodProducto", "CodProducto");
            dgvdetalle.Columns.Add("Descripción", "Descripción");
            dgvdetalle.Columns.Add("Marca", "Marca");
            dgvdetalle.Columns.Add("Color", "Color");
            dgvdetalle.Columns.Add("Talla_A", "Talla_A");
            dgvdetalle.Columns.Add("Talla_Num", "Talla_Num");
            dgvdetalle.Columns.Add("Precio", "Precio");
            dgvdetalle.Columns.Add("Stock", "Stock");

            dgvdetalle.Columns[0].Width = 60;
            dgvdetalle.Columns[1].Width = 100;
            dgvdetalle.Columns[2].Width = 160;
            dgvdetalle.Columns[3].Width = 120;
            dgvdetalle.Columns[4].Width = 100;
            dgvdetalle.Columns[5].Width = 60;
            dgvdetalle.Columns[6].Width = 60;
            dgvdetalle.Columns[7].Width = 60;
            dgvdetalle.Columns[8].Width = 60;

            DataGridViewImageColumn imagen = new DataGridViewImageColumn();
            imagen.HeaderText="EstadoEstock";
            imagen.Name = "EstadoEstock";
            dgvdetalle.Columns.Add(imagen);

            dgvdetalle.AllowUserToAddRows = false;
            dgvdetalle.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvdetalle.AllowUserToResizeColumns = false;


        }
        void listarDetallePrenda()
        {
            try
            {

                int num = 0;
                List<DetalleInventarioE> lista = InventarioN.Instancia.TraerDetallePrenda(Id);
                dgvdetalle.Rows.Clear();
                for (int i = 0; i < lista.Count; i++)
                {
                    num++;
                    if (lista[i].Stock>= 10)
                    { img = Properties.Resources.circulo_verde24x24; }
                   else if (lista[i].Stock <= 9 && lista[i].Stock>=1)
                    { img = Properties.Resources.CirculoNaranja24x24; }
                    else if (lista[i].Stock  ==0)
                    { img = Properties.Resources.circulorojo_24x24; }

                    string[] fila = new string[] {
                                lista[i].CodStock.ToString(),
                                lista[i].Codproducto,
                                lista[i].inventario.Descripción,
                                lista[i].Marca,
                                lista[i].Color,
                                lista[i].Talla_alfanum,
                                lista[i].Talla_num.ToString(),
                                lista[i].Precio.ToString("0.00"),
                                lista[i].Stock.ToString()};
                    dgvdetalle.Rows.Add(fila);
                    dgvdetalle.Rows[i].Cells[9].Value = img;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Detalle_Prenda_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            txtpventa.Enabled = false;
            CrearGrid();
            listarDetallePrenda();
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            agregarProductoLista();
            
        }

        private void Dgvdetalle_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                var row = (sender as DataGridView).CurrentRow;
                txtpventa.Text = row.Cells[7].Value.ToString();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

        }
        void agregarProductoLista()
        {
            try
            {

                int idStock = Convert.ToInt32(dgvdetalle.CurrentRow.Cells[0].Value);

                int stock = Convert.ToInt32(dgvdetalle.CurrentRow.Cells[8].Value);
                if (stock >= 1 && tipoListaUsar == 1)
                {
                    if (txtreg.Text != "")
                    {

                        double precio = Convert.ToDouble(txtreg.Text);
                        LocalBD.Instancia.ReturnListaBoleta(1, idStock, 1, precio);
                        MessageBox.Show("Prenda Agregada");
                        txtreg.Clear();
                    }
                    else if (txtreg.Text == "")
                    {
                        double pventa = Convert.ToDouble(txtpventa.Text);

                        LocalBD.Instancia.ReturnListaBoleta(1, idStock, 1, pventa);
                        MessageBox.Show("Prenda Agregada");
                    }
                }


                else if (stock >= 1 && tipoListaUsar == 2)
                {
                    if (txtreg.Text != "")
                    {

                        double precio = Convert.ToDouble(txtreg.Text);
                        LocalBD.Instancia.ReturnListaFactura(1, idStock, 1, precio);
                        MessageBox.Show("Prenda Agregada");
                        txtreg.Clear();
                    }
                    else if (txtreg.Text == "")
                    {
                        double pventa = Convert.ToDouble(txtpventa.Text);

                        LocalBD.Instancia.ReturnListaFactura(1, idStock, 1, pventa);
                        MessageBox.Show("Prenda Agregada");
                    }
                }

                else if (stock >= 1 && tipoListaUsar == 3)
                {
                    if (txtreg.Text != "")
                    {
                        double precio = Convert.ToDouble(txtreg.Text);
                        LocalBD.Instancia.ReturnListaNotaVenta(1, idStock, 1, precio);
                        MessageBox.Show("Prenda Agregada");
                        txtreg.Clear();
                    }
                    else if (txtreg.Text == "")
                    {
                        double pventa = Convert.ToDouble(txtpventa.Text);
                        LocalBD.Instancia.ReturnListaNotaVenta(1, idStock, 1, pventa);
                        MessageBox.Show("Prenda Agregada");
                    }
                }
                else
                { MessageBox.Show("Cantidad Insuficiente"); }
            }

            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
    }
}
