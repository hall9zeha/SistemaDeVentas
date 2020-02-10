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
    public partial class Buscar_Cambio : Form
    {
        VentasN objVN = new VentasN();

        public Buscar_Cambio()
        {
            InitializeComponent();
        }

        private void Buscar_Cambio_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            crearGrid();
        }
        void crearGrid()
        {
            dgvBuscarInventario.Columns.Add("Codproducto", "Codproducto");
            dgvBuscarInventario.Columns.Add("Descripción", "Descripción");
            dgvBuscarInventario.Columns.Add("Marca", "Marca");
            dgvBuscarInventario.Columns.Add("Color", "Color");
            dgvBuscarInventario.Columns.Add("TallaA", "TallaA");
            dgvBuscarInventario.Columns.Add("TallaN", "TallaN");
            dgvBuscarInventario.Columns.Add("Cantidad", "Cantidad");
            dgvBuscarInventario.Columns.Add("Importe", "Importe");
            dgvBuscarInventario.Columns.Add("CodDet.", "CodDet.");
            DataGridViewImageColumn Imagen = new DataGridViewImageColumn();
            Imagen.HeaderText = "EstadoStock";
            Imagen.Name = "EstadoStock";
            dgvBuscarInventario.Columns.Add(Imagen);

            dgvBuscarInventario.Columns[0].Width = 100;
            dgvBuscarInventario.Columns[1].Width = 100;
            dgvBuscarInventario.Columns[2].Width = 70;
            dgvBuscarInventario.Columns[3].Width = 60;
            dgvBuscarInventario.Columns[4].Width = 50;
            dgvBuscarInventario.Columns[5].Width = 50;
            dgvBuscarInventario.Columns[6].Width = 50;
            dgvBuscarInventario.Columns[7].Width = 50;
            dgvBuscarInventario.Columns[8].Width = 50;
            dgvBuscarInventario.Columns[9].Width = 100;

            dgvBuscarInventario.AllowUserToAddRows = false;
            dgvBuscarInventario.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBuscarInventario.MultiSelect = false;


        }
        void buscarPrendaCambio(string cadenaEntrada)
        {
            Image img = null;
            try
            {
                if (cadenaEntrada != string.Empty)
                {
                    int num = 0;
                    List<DetalleInventarioE> lista = objVN.BuscarPrendaCambio(cadenaEntrada);
                    dgvBuscarInventario.Rows.Clear();
                    for (int i = 0; i < lista.Count; i++)
                    {
                        if (lista[i].Stock == 0)
                        { img = Properties.Resources.circulorojo_24x24; }
                        if (lista[i].Stock >= 1)
                        { img = Properties.Resources.CirculoNaranja24x24; }
                        if (lista[i].Stock >= 10)
                        { img = Properties.Resources.circulo_verde24x24; }
                        num++;
                        string[] fila = new string[] {
                        lista[i].Codproducto,
                        lista[i].inventario.Descripción,
                        lista[i].Marca,
                        lista[i].Color,
                        lista[i].Talla_alfanum,
                        lista[i].Talla_num.ToString(),
                        lista[i].Stock.ToString(),
                        lista[i].Precio.ToString("0.00"),
                        lista[i].CodStock.ToString()


                    };
                        dgvBuscarInventario.Rows.Add(fila);
                        dgvBuscarInventario.Rows[i].Cells[9].Value = img;
                    }
                }
                else
                { dgvBuscarInventario.Rows.Clear(); }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            buscarPrendaCambio(txtBuscar.Text);
        }
    }
}
