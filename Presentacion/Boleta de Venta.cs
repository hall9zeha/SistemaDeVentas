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
using System.Data.SqlClient;
namespace Presentacion
{
    public partial class Boleta_de_Venta : Form
    {
        BoletaE objB = new BoletaE();
        VentasN objVN = new VentasN();
        int tipoListaUsar = 0;

        public Boleta_de_Venta()
        {
            InitializeComponent();
        }
        void crearGrid()
        {
            dgvDetalleBoleta.Columns.Add("CodDet", "CodDet");
            dgvDetalleBoleta.Columns.Add("CodProducto", "CodProducto");
            dgvDetalleBoleta.Columns.Add("Descripción", "Descripción");
            dgvDetalleBoleta.Columns.Add("Marca", "Marca");
            dgvDetalleBoleta.Columns.Add("PrecioVenta", "PrecioVenta");
            dgvDetalleBoleta.Columns.Add("Cantidad", "Cantidad");
            dgvDetalleBoleta.Columns.Add("Stock", "Stock");
            dgvDetalleBoleta.Columns.Add("P/Art.", "P/Art.");
                       
            dgvDetalleBoleta.Columns[0].Visible = true;
            dgvDetalleBoleta.Columns[1].Width = 80;
            dgvDetalleBoleta.Columns[2].Width = 130;
            dgvDetalleBoleta.Columns[3].Width = 100;
            dgvDetalleBoleta.Columns[4].Width = 60;
            dgvDetalleBoleta.Columns[5].Width = 60;
            dgvDetalleBoleta.Columns[6].Width = 60;
            dgvDetalleBoleta.Columns[7].Width = 60;
            DataGridViewCellStyle cssCabecera = new DataGridViewCellStyle();
            cssCabecera.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDetalleBoleta.ColumnHeadersDefaultCellStyle = cssCabecera;

            dgvDetalleBoleta.AllowUserToAddRows = false;
            dgvDetalleBoleta.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetalleBoleta.AllowUserToResizeColumns = false;

        }

        private void Boleta_de_Venta_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            
            crearGrid();
            generarCodigoBoleta();
            habilitarBotones(false, false, false, true, true);
            
        }
        void montoTotal()
        {
            double subTotal = 0.0;
            try
            {
                foreach (DataGridViewRow row in dgvDetalleBoleta.Rows)
                {
                    if (row.Cells[4].Value == null) row.Cells[4].Value = 0;
                    { row.Cells[7].Value = Convert.ToDouble(row.Cells[4].Value) * Convert.ToDouble(row.Cells[5].Value); }
                    if (row.Cells[5].Value == null) row.Cells[5].Value = 0;
                    { row.Cells[7].Value = Convert.ToDouble(row.Cells[4].Value) * Convert.ToDouble(row.Cells[5].Value);
                        subTotal += Convert.ToDouble(row.Cells[7].Value);
                        txtTotal.Text = subTotal.ToString("0.00");
                    }
                }
                if (dgvDetalleBoleta.RowCount == 0)
                {
                    txtTotal.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        void generarCodigoBoleta()
        {
            string serie = "BO00";
            lblBoleta.Text = objVN.GenerarCodigoBoletaFactura(serie,1);           

        }

        private void BtnAgregarItem_Click(object sender, EventArgs e)
        {
            tipoListaUsar = 1;
            Buscar_Prenda objBuscar_Prenda = new Buscar_Prenda(tipoListaUsar);
            objBuscar_Prenda.ShowDialog();
           
            List<DetalleInventarioE> lista = LocalBD.Instancia.ReturnListaBoleta(0, 0, 0, 0);
            llenarGridBoleta(lista);
            montoTotal();
            contarItems();
            habilitarBotones(true, true, true,true,true);
        }

        void  llenarGridBoleta(List<DetalleInventarioE> lista)
        {

            try
            {
                dgvDetalleBoleta.Rows.Clear();
                for (int i = 0; i < lista.Count; i++)
                {
                    string[] fila = new string[] {
                                lista[i].CodStock.ToString(),
                                lista[i].Codproducto,
                                lista[i].inventario.Descripción,
                                lista[i].Marca,
                                //lista[i].Color,
                                //lista[i].Talla_alfanum,
                                //lista[i].Talla_num.ToString(),
                                lista[i].Precio.ToString("0.00"),
                                lista[i].Cantidad.ToString(),
                                lista[i].Stock.ToString()};
                    dgvDetalleBoleta.Rows.Add(fila);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        void contarItems()
        {
            int num = 0;
            foreach (DataGridViewRow filas in dgvDetalleBoleta.Rows)
            {
                num++;
            }
            lblNumItems.Text = "N° Items" + " " + num;
        }

        private void BtnQuitarItem_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show("Quieres quitar esta prenda de la lista?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    int idStock = Convert.ToInt32(dgvDetalleBoleta.CurrentRow.Cells[0].Value);
                    LocalBD.Instancia.RemoverPrendaListaBoleta(idStock);

                    List<DetalleInventarioE> lista = LocalBD.Instancia.ReturnListaBoleta(0, 0, 0, 0);
                    llenarGridBoleta(lista);
                    montoTotal();
                    contarItems();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void DgvDetalleBoleta_KeyUp(object sender, KeyEventArgs e)
        {
            montoTotal();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {

            try
            {
                DialogResult dr = MessageBox.Show("Realizar Venta?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    
                    guardarVenta();
                    imprimirTicket();
                    habilitarBotones(true, false, true, false, false);
                   
                }
                else
                { MessageBox.Show("Tarea Cancelada"); }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

           

        }
        void guardarVenta()
        {
            try
            {
                BoletaE b = new BoletaE();
                int tipoComprobante = 1;
                b.CodVenta = lblBoleta.Text;
                b.Importe_rg = Convert.ToDouble(txtTotal.Text);
                b.tipoComprobante = tipoComprobante;
                List<DetalleBoletaE> Detalle = new List<DetalleBoletaE>();
                foreach (DataGridViewRow row in dgvDetalleBoleta.Rows)
                {
                    DetalleBoletaE dt = new DetalleBoletaE();
                    //dt.Codboleta = lblBoleta.Text;

                    dt.Codproducto = row.Cells[1].Value.ToString();
                    dt.CodProducto_detalle = Convert.ToInt32(row.Cells[0].Value.ToString());
                    dt.Descripción = row.Cells[2].Value.ToString();
                    dt.Cantidad = Convert.ToInt32(row.Cells[5].Value.ToString());
                    dt.Precio_final = Convert.ToDouble(row.Cells[4].Value.ToString());
                    Detalle.Add(dt);
                }

                b.detalleBoleta = Detalle;
                int resultado = VentasN.Instancia.GuardarVenta(b);
                MessageBox.Show("Venta Registrada");

            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        void anularVenta()
        {
            //objB.Codboleta = lblBoleta.Text;
            List<DetalleBoletaE> detalle = new List<DetalleBoletaE>();
            foreach (DataGridViewRow row in dgvDetalleBoleta.Rows)
            {
                DetalleBoletaE dt = new DetalleBoletaE();
                dt.CodProducto_detalle = Convert.ToInt32(row.Cells[0].Value.ToString());
                dt.Cantidad = Convert.ToInt32(row.Cells[5].Value.ToString());
                detalle.Add(dt);
            }
            objB.detalleBoleta = detalle;
            int resultado = VentasN.Instancia.AnularVenta(objB);
            MessageBox.Show("Venta Anulada");
        }

        void imprimirTicket()
        {
            try
            {
                

                CrearTicket objTicket = new CrearTicket();
                int art = 0;
                objTicket.AbrirCajon();


               
                objTicket.TextoCentro("COMPANY BARRY ZEHA");
                objTicket.TextoIzquierda("EXPEDIDO EN: LOCAL PRINCIPAL");
                objTicket.TextoIzquierda("DIREC: DIRECCION DE LA EMPRESA");
                objTicket.TextoIzquierda("TELEF: 4530000");
                objTicket.TextoIzquierda("R.F.C: XXXXXXXXX-XX");
                objTicket.TextoIzquierda("EMAIL: vmwaretars@gmail.com");
                objTicket.TextoIzquierda("");
                objTicket.TextoExtremos("Caja # 1", "Ticket # " + lblBoleta.Text);
                objTicket.LineasAsteriscos();

                
                objTicket.TextoIzquierda("");
                objTicket.TextoIzquierda("ATENDIÓ: Barry " );
                objTicket.TextoIzquierda("CLIENTE: PUBLICO EN GENERAL");
                objTicket.TextoIzquierda("TIPODOC:");
                objTicket.TextoIzquierda("NUM DOC:");
                objTicket.TextoIzquierda("");
                objTicket.TextoExtremos("FECHA: " + DateTime.Now.ToShortDateString(), "HORA: " + DateTime.Now.ToShortTimeString());
                objTicket.LineasAsteriscos();

                
                objTicket.EncabezadoTicket();
                objTicket.LineasAsteriscos();
                foreach (DataGridViewRow row in dgvDetalleBoleta.Rows)
                {
                    objTicket.AgregarArticulo(
                        row.Cells[2].Value.ToString(),
                        int.Parse(row.Cells[5].Value.ToString()),
                        decimal.Parse(row.Cells[4].Value.ToString()),
                        decimal.Parse(row.Cells[7].Value.ToString())

                        );
                }
                objTicket.AgregarTotales("         SUBTOTAL......S/", 100);
                objTicket.AgregarTotales("         IVA...........S/", 20M);//La M indica que es un decimal en C#
                objTicket.AgregarTotales("         TOTAL.........S/", Convert.ToDecimal(txtTotal.Text));
                objTicket.TextoIzquierda("");
                if (txtEfectivo.Text == "")
                { txtEfectivo.Text = "0"; }
                if (txtCambio.Text == "")
                { txtCambio.Text = "0"; }
                objTicket.AgregarTotales("         EFECTIVO......S/", Convert.ToDecimal(txtEfectivo.Text));
                objTicket.AgregarTotales("         CAMBIO........S/", Convert.ToDecimal(txtCambio.Text)
                    );

                //Texto final del Ticket.

                foreach (DataGridViewRow row in dgvDetalleBoleta.Rows)
                {
                   
                    art += Convert.ToInt32(row.Cells[5].Value);
                }
                objTicket.TextoIzquierda("");
                objTicket.TextoIzquierda("ARTÍCULOS VENDIDOS: " + art.ToString());
                objTicket.TextoIzquierda("");
                objTicket.TextoCentro("¡GRACIAS POR SU COMPRA!");
                objTicket.CortarTicket();
                objTicket.ImprimirTicket("Microsoft XPS Document Writer");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void BtnSalir_Click(object sender, EventArgs e)
        {
            this.Dispose();
            LocalBD.Instancia.LimpiarListaBoleta();
        }

        private void TxtEfectivo_TextChanged(object sender, EventArgs e)
        {

        }
        void calcularVuelto()
        {
            double cambio = 0;
            try
            {
                if (txtEfectivo.Text != "")
                {
                    cambio = Convert.ToDouble(txtEfectivo.Text) - Convert.ToDouble(txtTotal.Text);
                    txtCambio.Text = cambio.ToString("#,#.00");
                }
                else 
                {
                    txtCambio.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtEfectivo_KeyPress(object sender, KeyPressEventArgs e)
        {
            calcularVuelto();
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            dgvDetalleBoleta.Rows.Clear();
            lblBoleta.Text = "";
            txtTotal.Clear();
            txtEfectivo.Clear();
            txtCambio.Clear();
            lblNumItems.Text = "";
            generarCodigoBoleta();
            habilitarBotones(true, false, false, true, true);
        }
        void habilitarBotones(bool nuevo, bool guardar, bool anular, bool agregar, bool quitar)
        {
            btnGuardar.Enabled = guardar;
            btnAnular.Enabled = anular;
            btnNuevo.Enabled = nuevo;
            btnAgregarItem.Enabled = agregar;
            btnQuitarItem.Enabled = quitar;
        }

        private void BtnAnular_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Realmente Quieres anular esta venta", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                anularVenta();
                habilitarBotones(true, false, false, false, false);
            }
            
        }
    }
}
