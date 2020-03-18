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
            dgvDetalleNotaVenta.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetalleNotaVenta.AllowUserToAddRows = false;
            dgvDetalleNotaVenta.AllowUserToResizeColumns = false;


        }

        private void FrmNota_De_Venta_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            string serie = "VE" + 00;
            lblCorrelativo.Text = objN.GenerarCodigoBoletaFactura(serie, 3);
            crearGrid();
            habilitarBotones(false, false, false, false, true, false);
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
        void montoEnLetras()
        {
            lblMontoEnletras.Text = (" SON: " + AccionesEnControles.Instancia.MontoEnLetras(txtTotal.Text) + " SOLES ").ToLower();
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
        void calcularCambio()
        {
            try
            {
                double total = 0;
                if (txtEfectivo.Text != "")
                {
                    total = Convert.ToDouble(txtEfectivo.Text) - Convert.ToDouble(txtTotal.Text);
                    txtCambio.Text = total.ToString("0.00");
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
        void buscarCliente(int idCli, string numDoc)
        {
            try
            {
                ClienteE c = null;
                c = objCliN.TraerCliente(idCli, numDoc);
                txtNombreCliente.Text = c.NombreCliente + " " + c.ApellidoCliente;
                txtNumDoc.Text = c.NroDocumento;
            }
            catch (ApplicationException)
            {
                DialogResult dr = MessageBox.Show("No se encontro nuingun cliente ¿Quiere abrir búsqueda avanzada?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    LocalBD.Instancia.Invocador(1, 3);
                    frmMantenimiento_Cliente objMCliente = new frmMantenimiento_Cliente();
                    objMCliente.Show();
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        void guardarNotaVenta()
        {
            try
            {
                int tipoComprobante = 3;
                VentasE v = new VentasE();
                v.CodVenta = lblCorrelativo.Text;
                v.Importe_rg = Convert.ToDouble(txtTotal.Text);
                v.TipoComprobante = tipoComprobante;
                List<DetalleVentasE> detalleVenta = new List<DetalleVentasE>();
                foreach (DataGridViewRow row in dgvDetalleNotaVenta.Rows)
                {
                    DetalleVentasE dt = new DetalleVentasE();
                    dt.Codproducto = row.Cells[1].Value.ToString();
                    dt.CodProducto_detalle = Convert.ToInt32(row.Cells[0].Value.ToString());
                    dt.Descripción = row.Cells[2].Value.ToString();
                    dt.Cantidad = Convert.ToInt32(row.Cells[5].Value.ToString());
                    dt.Precio_final = Convert.ToDouble(row.Cells[7].Value.ToString());
                    detalleVenta.Add(dt);
                }
                v.DetalleVenta = detalleVenta;
                int resultado = objN.GuardarVenta(v);
                MessageBox.Show("Venta registrada");
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        void anularVenta()
        {
            try
            {
                List<DetalleVentasE> detalle = new List<DetalleVentasE>();
                foreach (DataGridViewRow row in dgvDetalleNotaVenta.Rows)
                {
                    DetalleVentasE dt = new DetalleVentasE();
                    dt.CodProducto_detalle = Convert.ToInt32(row.Cells[0].Value.ToString());
                    dt.Cantidad = Convert.ToInt32(row.Cells[5].Value.ToString());
                    detalle.Add(dt);
                }
                objE.DetalleVenta = detalle;
                int resultado = objN.AnularVenta(objE);
                MessageBox.Show("Venta Anulada");
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
                List<DetalleInventarioE> lista = LocalBD.Instancia.ReturnListaNotaVenta(0, 0, 0, 0);
                llenarGridNotaVenta(lista);
                contarItems();
                montoTotal();
                montoEnLetras();
                habilitarBotones(true, true, false, false, true, true);

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
                    montoEnLetras();
                    habilitarBotones(true, true, false, false, true, true);
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

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            LocalBD.Instancia.LimpiarListaNotaVenta();
            this.Dispose();
        }

        private void BtnBuscarCliente_Click(object sender, EventArgs e)
        {
            string numDoc = txtNumDoc.Text;
            buscarCliente(0, numDoc);
        }

        private void BtnBuscarXid_Click(object sender, EventArgs e)
        {
            int idCli = LocalBD.Instancia.ReturnIdClienteNV(0, 0);
            buscarCliente(idCli, 0.ToString());
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Realizar Venta?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                guardarNotaVenta();
                imprimirTicket();
                habilitarBotones(true, false, true, true, false, false);
            }
            
        }

        private void BtnAnular_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("¿Realmente quieres anular la venta?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                anularVenta();
                habilitarBotones(true, false, false, false, false, false);
            }
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            dgvDetalleNotaVenta.Rows.Clear();
            limpiarControles();
            string serie = "VE" + 00;
            lblCorrelativo.Text = objN.GenerarCodigoBoletaFactura(serie, 3);

        }
        void limpiarControles()
        {
            txtNombreCliente.Clear();
            txtNumDoc.Clear();
            txtSubtotal.Clear();
            txtTotal.Clear();
            txtDescuento.Clear();
            lblNumItems.Text = "";
            lblCorrelativo.Text = "";

        }
        void habilitarBotones(bool nuevo, bool guardar, bool anular, bool imprimir, bool agregar, bool quitar)
        {
            btnNuevo.Enabled = nuevo;
            btnGuardar.Enabled = guardar;
            btnAnular.Enabled = anular;
            btnImprimir.Enabled = imprimir;
            btnAgregarItem.Enabled = agregar;
            btnQuitarItem.Enabled = quitar;
        }

        private void BtnImprimir_Click(object sender, EventArgs e)
        {
            habilitarBotones(true, false, false, false, false, false);
        }
        void imprimirTicket()
        {
            try
            {


                CrearTicket objTicket = new CrearTicket();
                int art = 0;
                string nomCli = "", numDoc = "";

                objTicket.AbrirCajon();



                objTicket.TextoCentro("COMPANY BARRY ZEHA");
                objTicket.TextoIzquierda("EXPEDIDO EN: LOCAL PRINCIPAL");
                objTicket.TextoIzquierda("DIREC: DIRECCION DE LA EMPRESA");
                objTicket.TextoIzquierda("TELEF: 4530000");
                objTicket.TextoIzquierda("R.F.C: XXXXXXXXX-XX");
                objTicket.TextoIzquierda("EMAIL: vmwaretars@gmail.com");
                objTicket.TextoIzquierda("");
                objTicket.TextoCentro("NOTA DE VENTA");
                objTicket.TextoExtremos("Caja # 1", "N° # " + lblCorrelativo.Text);
                objTicket.LineasAsteriscos();


                objTicket.TextoIzquierda("");
                objTicket.TextoIzquierda("ATENDIÓ: Barry ");

                if (txtNombreCliente.Text == "")
                { nomCli = "PUBLICO EN GENERAL"; }
                else
                { nomCli = txtNombreCliente.Text; }

                objTicket.TextoIzquierda("CLIENTE: " + nomCli);
                objTicket.TextoIzquierda("TIPODOC:");

                if (txtNumDoc.Text == "")
                { numDoc = ""; }
                else
                { numDoc = txtNumDoc.Text; }

                objTicket.TextoIzquierda("NUM DOC: " + numDoc);

                objTicket.TextoIzquierda("");
                objTicket.TextoExtremos("FECHA: " + DateTime.Now.ToShortDateString(), "HORA: " + DateTime.Now.ToShortTimeString());
                objTicket.LineasAsteriscos();


                objTicket.EncabezadoTicket();
                objTicket.LineasAsteriscos();
                foreach (DataGridViewRow row in dgvDetalleNotaVenta.Rows)
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
                objTicket.TextoIzquierda(lblMontoEnletras.Text);
                objTicket.LineasAsteriscos();
                objTicket.AgregarTotales("         EFECTIVO......S/", Convert.ToDecimal(txtEfectivo.Text));
                objTicket.AgregarTotales("         CAMBIO........S/", Convert.ToDecimal(txtCambio.Text)
                    );

                //Texto final del Ticket.

                foreach (DataGridViewRow row in dgvDetalleNotaVenta.Rows)
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

        private void TxtEfectivo_KeyPress(object sender, KeyPressEventArgs e)
        {
            calcularCambio();
        }
    }
}
