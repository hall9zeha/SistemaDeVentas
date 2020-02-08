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
    public partial class Editar_Prenda : Form
    {
        
        InventarioN objN = new InventarioN();
        InventarioE objE = new InventarioE();
        StockE objS = new StockE();
        string Id_Prod = "";
        int idStock = 0;
        int tipoAccion = 0;
        public Editar_Prenda(string Id)
        {
            InitializeComponent();
            this.Id_Prod = Id;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            editarPrenda();
            traerInventario();
        }
        void crearGrid()
        {
            dgvprenda.Columns.Add("CodStock", "CodStock");
            dgvprenda.Columns.Add("CodProducto", "CodProducto");
            dgvprenda.Columns.Add("Color", "Color");
            dgvprenda.Columns.Add("Talla_A", "Talla_A");
            dgvprenda.Columns.Add("Talla_Num", "Talla_Num");
            dgvprenda.Columns.Add("Stock", "Stock");

            dgvprenda.Columns[0].Width = 60;
            dgvprenda.Columns[1].Width = 100;
            dgvprenda.Columns[2].Width = 120;
            dgvprenda.Columns[3].Width = 60;
            dgvprenda.Columns[4].Width = 60;
            dgvprenda.Columns[5].Width = 60;
            
        }

       
        void traerInventario()
        {

            InventarioE obj;
            obj=InventarioN.Instancia.TraerInventario(this.Id_Prod);
            lblcode.Text = obj.Codproducto;
            txtdescripcion.Text = obj.Descripción;
            txtmarca.Text = obj.Marca;
            txtprecio.Text = obj.Precio.ToString();
            txtprecioventa.Text = obj.PrecioVenta.ToString();

        }
        void editarPrenda()
        {
            try
            {
                if (txtdescripcion.Text == "") objE.Descripción = "Vacío";else objE.Descripción = txtdescripcion.Text;
                if (txtmarca.Text == "") objE.Marca = "Vacío"; else objE.Marca = txtmarca.Text;
                if (txtprecio.Text == "") objE.Precio = 0;else objE.Precio = Convert.ToDouble(txtprecio.Text);
                if (txtprecioventa.Text == "") objE.PrecioVenta = 0; else objE.PrecioVenta = Convert.ToDouble(txtprecioventa.Text);
                objE.Codproducto = lblcode.Text;
                objN.EditarPrenda(objE);
                MessageBox.Show("Editado Correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void agregarDetallePrenda()
        {
            try
            {
                objS.Codproducto = lblcode.Text;
                if (txtcolor.Text == "") objS.Color= "vacio"; else objS.Color = txtcolor.Text;
                if (cmbtallaalfa.Text == "") objS.Talla_alfanum = "vacio"; else objS.Talla_alfanum = cmbtallaalfa.Text;
                if (txttallanum.Text == "")objS.Talla_num = 0; else objS.Talla_num =Convert.ToInt32(txttallanum.Text);
                if (txtcantidad.Text == "") objS.Cantidad = 0; else objS.Cantidad = Convert.ToInt32(txtcantidad.Text);
                if (txtcantidad.Text == "") objS.Stock = 0; else objS.Stock = Convert.ToInt32(txtcantidad.Text);
                objN.MantenimientoDetalleInventario(objS, tipoAccion);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void editarDetallePrenda()
        {
            try
            {
                if (txtcolor.Text == "") objS.Color = "Vacío"; else objS.Color = txtcolor.Text;
                if (cmbtallaalfa.Text == "") objS.Talla_alfanum = "Vacío"; else objS.Talla_alfanum = cmbtallaalfa.Text;
                if (txttallanum.Text == "") objS.Talla_num = 0; else objS.Talla_num = Convert.ToInt32(txttallanum.Text);
                if (txtcantidad.Text == "") objS.Cantidad = 0; else objS.Cantidad = Convert.ToInt32(txtcantidad.Text);
                if (txtcantidad.Text == "") objS.Stock = 0; else objS.Stock = Convert.ToInt32(txtcantidad.Text);
                idStock = Convert.ToInt32(dgvprenda.CurrentRow.Cells[0].Value);
                
                objS.CodStock = idStock;
                objN.MantenimientoDetalleInventario(objS, tipoAccion);
                MessageBox.Show("Detalles Editados Correctamente");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void eliminarDetallePrenda()
        {
            try
            {
                idStock = Convert.ToInt32(dgvprenda.CurrentRow.Cells[0].Value);
                objS.CodStock = idStock;
                objN.MantenimientoDetalleInventario(objS, tipoAccion);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }   
        }

        private void Editar_Prenda_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            crearGrid();
            listarDetallePrenda();
            traerInventario();
            
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            tipoAccion = 1;
            agregarDetallePrenda();
            listarDetallePrenda();
        }

        private void Dgvprenda_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                var row = (sender as DataGridView).CurrentRow;
                txtcolor.Text = row.Cells[2].Value.ToString();
                cmbtallaalfa.Text = row.Cells[3].Value.ToString();
                txttallanum.Text = row.Cells[4].Value.ToString();
                txtcantidad.Text = row.Cells[5].Value.ToString();

            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }


        private void Button2_Click(object sender, EventArgs e)
        {
           
            tipoAccion = 2;
            editarDetallePrenda();
            listarDetallePrenda();


        }
        void listarDetallePrenda()
        {
            try
            {
                int num = 0;
                List<StockE> lista = objN.TraerDetallePrenda(Id_Prod);
                dgvprenda.Rows.Clear();
                for (int i = 0; i < lista.Count; i++)
                {
                    num++;
                    string[] fila = new string[] {
                        lista[i].CodStock.ToString(),
                        lista[i].Codproducto,
                        lista[i].Color,
                        lista[i].Talla_alfanum,
                    lista[i].Talla_num.ToString(),
                    lista[i].Cantidad.ToString()};
                    dgvprenda.Rows.Add(fila);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            tipoAccion = 3;
            eliminarDetallePrenda();
            listarDetallePrenda();
        }
    }
}
