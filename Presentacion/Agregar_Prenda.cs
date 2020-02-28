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
using System.Data.SqlClient;
using Datos;

namespace Presentacion
{
    public partial class Agregar_Prenda : Form
    {
        InventarioE objE = new InventarioE();
        DetalleInventarioE objS = new DetalleInventarioE();
        InventarioN objN = new InventarioN();
        SqlConnection cn =  Conexion.Instancia.Conectar();

        public Agregar_Prenda()
        {
            InitializeComponent();
        }
        void crearGrid()
        {
            dgvprenda.Columns.Add("Código", "Código");
            dgvprenda.Columns.Add("Color", "Color");
            dgvprenda.Columns.Add("Talla_Alfa", "Talla_Alfa");
            dgvprenda.Columns.Add("Talla_N", "Talla_N");
            dgvprenda.Columns.Add("Cantidad", "Cantidad");

            dgvprenda.Columns[0].Width = 150;
            dgvprenda.Columns[1].Width = 100;
            dgvprenda.Columns[2].Width = 60;
            dgvprenda.Columns[3].Width = 60;
            dgvprenda.Columns[4].Width = 60;
        }
        private void Label4_Click(object sender, EventArgs e)
        {

        }

        private void Agregar_Prenda_Load(object sender, EventArgs e)
        {
            crearGrid();
            this.CenterToScreen();
            generarCodigoPrenda();
            habilitarBotones(false, false, false);
        }
        void limpiarControles()
        {
            lblcode.Text = "";
            txtdescripcion.Clear();
            txtmarca.Clear();
            txtcolor.Clear();
            txtprecio.Clear();
            txtprecioventa.Clear();
            cmbtallaalfa.Text = "";
            txttallanum.Clear();
            txtcantidad.Clear();
        }
        void generarCodigoPrenda()
        {
            lblcode.Text =objN.GenerarCodigoPrenda();
        }
    
       
        void guardarInventario()
        {
            try
            {
                objE.Codproducto = lblcode.Text;
                if (txtdescripcion.Text == "") objE.Descripción = "Desconocido"; else objE.Descripción = txtdescripcion.Text;
                if (txtmarca.Text == "") objE.Marca = "Desconocido"; else objE.Marca = txtmarca.Text;
                if (txtprecio.Text == "") objE.Precio = 0; else objE.Precio = Convert.ToDouble(txtprecio.Text);
                if (txtprecioventa.Text == "") objE.PrecioVenta = 0; else objE.PrecioVenta = Convert.ToDouble(txtprecioventa.Text);

                List<DetalleInventarioE> Detalle = new List<DetalleInventarioE>();
                foreach (DataGridViewRow row in dgvprenda.Rows)
                {
                    //la razón de utilizar una nueva instancia dentro del bucle es para que agregue todos los elementos
                    //si tomamos la variable global solo agregara un elemento
                    DetalleInventarioE s = new DetalleInventarioE();
                    s.Codproducto = row.Cells[0].Value.ToString();
                    s.Color = row.Cells[1].Value.ToString();
                    s.Talla_alfanum = row.Cells[2].Value.ToString();
                    s.Talla_num = Convert.ToInt32(row.Cells[3].Value.ToString());
                    s.Cantidad = Convert.ToInt32(row.Cells[4].Value.ToString());
                    s.Stock = Convert.ToInt32(row.Cells[4].Value.ToString());
                    Detalle.Add(s);
                }
                objE.detalleInventario = Detalle;
                int resultado = InventarioN.Instancia.GuardarPrendaInventario(objE);
                MessageBox.Show("Prendas Resitradas");
                dgvprenda.Rows.Clear();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

        }
        private void Button5_Click(object sender, EventArgs e)
        {
          
        }
       
        void agregarPrendaGrid()

        {
            if (txtcolor.Text == "") txtcolor.Text = "vacio"; else txtcolor.Text=txtcolor.Text;
            if (cmbtallaalfa.Text == "") cmbtallaalfa.Text = "vacio"; else cmbtallaalfa.Text = cmbtallaalfa.Text;
            if (txttallanum.Text == "") txttallanum.Text = "0"; else txttallanum.Text = txttallanum.Text;
            if (txtcantidad.Text == "") txtcantidad.Text = "0"; else txtcantidad.Text = txtcantidad.Text;
           dgvprenda.Rows.Add(lblcode.Text, txtcolor.Text, cmbtallaalfa.Text, txttallanum.Text, txtcantidad.Text);

        }

        private void Button2_Click(object sender, EventArgs e)
        {
           
            
            agregarPrendaGrid();
            int num = 0;
            foreach (DataGridViewRow f in dgvprenda.Rows)
            {
                num++;
            }
            if (num != 0)
            { habilitarBotones(true, true, true); }

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            limpiarControles();
            habilitarBotones(true, false, false);
            generarCodigoPrenda();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show("Quieres Registrar Todo?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    guardarInventario();
                    limpiarControles();
                    generarCodigoPrenda();
                }
                else
                {
                    MessageBox.Show("Tarea Cancelada");
                }

        }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        void habilitarBotones(bool Agregar, bool Quitar, bool Guardar)
        {
            btnAgregar.Enabled = Agregar;
            btnQuitar.Enabled = Quitar;
            btnGuardar.Enabled = Guardar;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            dgvprenda.Rows.RemoveAt(dgvprenda.CurrentRow.Index);
            int num = 0;
            foreach (DataGridViewRow f in dgvprenda.Rows)
            {
                num++;
            }
            if (num == 0)
            { habilitarBotones(true, false, false); }
        }

        private void Lblcode_Click(object sender, EventArgs e)
        {

        }
    }
}

