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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Globalization;

namespace Presentacion
{
    public partial class Boleta_de_Venta : Form
    {
        VentasE objB = new VentasE();
        VentasN objVN = new VentasN();
        AccionesEnControles objAc = new AccionesEnControles();
        DataTable dt = new DataTable();
        int tipoListaUsar = 0;
        double igv = 0.18;
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
            
            //los métodos de llenado de los comboBox, estan en la clase Acciones en controles, para ser reutilizados, ya que 
            //serán llamados en tres formularios diferentes ,boleta, factura y nota de ventas, asi dejamos de escribir código redundante en cada formulario 
            objAc.LlenarCboMoneda(this.gbCliente);
            objAc.LlenarCboTipoPago(this.cboTipoPago);
            objAc.LlenarCboTipoDoc(this.cboTipDoc);
            //*******************************************
            int idCliente = LocalBD.Instancia.ReturnIdCliente(0, 0);
        }
       
        void montoTotal()
        {
            double subTotal = 0.0;
            try
            {
                double iva = 0;
                double granTotal = 0;
                foreach (DataGridViewRow row in dgvDetalleBoleta.Rows)
                {
                    if (row.Cells[4].Value == null) row.Cells[4].Value = 0;
                    { row.Cells[7].Value = Convert.ToDouble(row.Cells[4].Value) * Convert.ToDouble(row.Cells[5].Value); }
                    if (row.Cells[5].Value == null) row.Cells[5].Value = 0;
                    { row.Cells[7].Value = Convert.ToDouble(row.Cells[4].Value) * Convert.ToDouble(row.Cells[5].Value);
                        subTotal += Convert.ToDouble(row.Cells[7].Value);
                        iva = subTotal * igv;
                        granTotal = subTotal + iva;
                        txtTotal.Text = granTotal.ToString("0.00");
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
            string serie = "BO" + 00;
            lblBoleta.Text = objVN.GenerarCodigoBoletaFactura(serie,1);           

        }
        void montoEnLetras()
        {
            lblMontoEnletras.Text = ("SON: " + AccionesEnControles.Instancia.MontoEnLetras(txtTotal.Text) + "SOLES").ToLower();
        }

        private void BtnAgregarItem_Click(object sender, EventArgs e)
        {
            tipoListaUsar = 1;
            frmBuscar_Prenda objBuscar_Prenda = new frmBuscar_Prenda(tipoListaUsar);
            objBuscar_Prenda.ShowDialog();
           
            List<DetalleInventarioE> lista = LocalBD.Instancia.ReturnListaBoleta(0, 0, 0, 0);
            llenarGridBoleta(lista);
            montoTotal();
            contarItems();
            habilitarBotones(true, true, true,true,true);
            montoEnLetras();
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
        void buscarCliente(int idCli,string nroDocumento)
        {

            try
            {
                ClienteE c = null;
                c = ClienteN.Instancia.TraerCliente(idCli, nroDocumento);
                txtNombreCliente.Text = c.NombreCliente;
                txtDireccionCliente.Text = c.DireccionCliente;
                txtNumDoc.Text = c.NroDocumento;
                cboTipDoc.Text = c.DescTipDocumento;
                int i = LocalBD.Instancia.ReturnIdCliente(1, c.IdCliente);

            }
            catch (Exception)
            { throw; }

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
                    montoEnLetras();

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
                    //imprimirTicket();
                    //habilitarBotones(true, false, true, false, false);
                   
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
                VentasE b = new VentasE();
                int tipoComprobante = 1;
                b.CodVenta = lblBoleta.Text;
                b.Importe_rg = Convert.ToDouble(txtTotal.Text);
                b.TipoComprobante = tipoComprobante;
                b.TipoPago = Convert.ToInt32(cboTipoPago.SelectedValue);
                b.IdCliente = LocalBD.Instancia.ReturnIdCliente(0,0);
                b.TipoMoneda = Convert.ToInt32(CboMoneda.SelectedValue);
                //a implementar cuando se haga el login b.IdUsuario = 0;
                List<DetalleVentasE> Detalle = new List<DetalleVentasE>();
                foreach (DataGridViewRow row in dgvDetalleBoleta.Rows)
                {
                    DetalleVentasE dt = new DetalleVentasE();
                    

                    dt.Codproducto = row.Cells[1].Value.ToString();
                    dt.CodProducto_detalle = Convert.ToInt32(row.Cells[0].Value.ToString());
                    dt.Descripción = row.Cells[2].Value.ToString();
                    dt.Cantidad = Convert.ToInt32(row.Cells[5].Value.ToString());
                    dt.Precio_final = Convert.ToDouble(row.Cells[4].Value.ToString());
                    Detalle.Add(dt);
                }
                
                b.DetalleVenta = Detalle;
                int resultado = VentasN.Instancia.GuardarVenta(b);
                MessageBox.Show("Venta Registrada");
                imprimirTicket();
                habilitarBotones(true, false, true, false, false);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message);
                habilitarBotones(true, true, false, true, true);
            }
        }
        void anularVenta()
        {
            
            List<DetalleVentasE> detalle = new List<DetalleVentasE>();
            foreach (DataGridViewRow row in dgvDetalleBoleta.Rows)
            {
                DetalleVentasE dt = new DetalleVentasE();
                dt.CodProducto_detalle = Convert.ToInt32(row.Cells[0].Value.ToString());
                dt.Cantidad = Convert.ToInt32(row.Cells[5].Value.ToString());
                detalle.Add(dt);
            }
            objB.DetalleVenta = detalle;
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
                objTicket.TextoIzquierda("TELEF: 00000000");
                objTicket.TextoIzquierda("R.F.C: XXXXXXXXX-XX");
                objTicket.TextoIzquierda("EMAIL: vmwaretars@gmail.com");
                objTicket.TextoIzquierda("");
                objTicket.TextoCentro("BOLETA DE VENTA");
                objTicket.TextoExtremos("Caja # 1", "N° # " + lblBoleta.Text);
                objTicket.DibujarLineas("*");

                
                objTicket.TextoIzquierda("");
                objTicket.TextoIzquierda("ATENDIÓ: Barry " );
                string nomCli = "";
                if (txtNombreCliente.Text == "") { nomCli = "Público en general"; }
                else { nomCli = txtNombreCliente.Text; }
                objTicket.TextoIzquierda("CLIENTE: "+nomCli);
                objTicket.TextoIzquierda("TIPODOC: " + cboTipDoc.SelectedText);
                string numDoc = "";
                if (txtNumDoc.Text == "") { numDoc = ""; }
                else { numDoc = txtNumDoc.Text; }
                objTicket.TextoIzquierda("NUM DOC: "+numDoc);
                objTicket.TextoIzquierda("");
                objTicket.TextoExtremos("FECHA: " + DateTime.Now.ToShortDateString(), "HORA: " + DateTime.Now.ToShortTimeString());
                objTicket.DibujarLineas("*");

                
                objTicket.EncabezadoTicket();
                objTicket.DibujarLineas("*");
                foreach (DataGridViewRow row in dgvDetalleBoleta.Rows)
                {
                    objTicket.AgregarArticulo(
                        row.Cells[2].Value.ToString(),
                        int.Parse(row.Cells[5].Value.ToString()),
                        decimal.Parse(row.Cells[4].Value.ToString()),
                        decimal.Parse(row.Cells[7].Value.ToString())

                        );
                }

                objTicket.AgregarTotales("         SUBTOTAL......S/" ,Convert.ToDecimal(txtTotal.Text));
                decimal iva =Convert.ToDecimal(Decimal.Parse(txtTotal.Text) * (decimal)igv);
                objTicket.AgregarTotales("         IVA...........S/", Convert.ToDecimal(txtTotal.Text) + iva);//La M indica que es un decimal en C#
                objTicket.AgregarTotales("         TOTAL.........S/", Convert.ToDecimal(txtTotal.Text));
                objTicket.TextoIzquierda("");
                if (txtEfectivo.Text == "")
                { txtEfectivo.Text = "0"; }
                if (txtCambio.Text == "")
                { txtCambio.Text = "0"; }
                objTicket.TextoIzquierda(lblMontoEnletras.Text);
                objTicket.DibujarLineas("*");
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

        private void BtnBuscarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                string numDoc=txtNumDoc.Text;
                buscarCliente(0, numDoc);
                

            }
            catch (ApplicationException)
            {
                DialogResult dr = MessageBox.Show("No se encontro el cliente ¿Quieres abrir la búsqueda avanzada?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    LocalBD.Instancia.Invocador(1, 1);
                    frmMantenimiento_Cliente objMCliente = new frmMantenimiento_Cliente();
                    objMCliente.ShowDialog();
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }

        private void BtnBuscarXid_Click(object sender, EventArgs e)
        {
            try
            {
               
                int idCli = LocalBD.Instancia.ReturnIdCliente(0, 0);
                buscarCliente(idCli, 0.ToString());
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DgvDetalleBoleta_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void BtnImprimir_Click(object sender, EventArgs e)
        {
            ticketEnPdf();
        }

        //Método para crear el ticket en pdf
        private void ticketEnPdf()
        {
            TicketPdf tic = new TicketPdf();
            
            tic.HeaderImage = $"C:\\Users\\HALL9000\\Documents\\GitHub\\N.E.N.A\\Presentacion\\Resources\\nena.png";

            tic.AddHeaderLine("COMPANY BARRY ZEHA");
            tic.AddHeaderLine("");
            tic.AddHeaderLine("EXPEDIDO EN LOCAL PRINCIPAL");
            tic.AddHeaderLine("DIREC: DIRECCION DE LA EMPRESA");
            tic.AddHeaderLine("TELEF: 4530000");
            tic.AddHeaderLine("R.F.C: XXXXXXXXX-XX");
            tic.AddHeaderLine("EMAIL: vmwaretars@gmail.com");
            tic.AddHeaderLine("");
            tic.AddHeaderLine("BOLETA DE VENTA");
            tic.AddHeaderLine("");
            tic.AddSubHeaderLine("Caja # 1  N° # " + lblBoleta.Text);
           
            tic.AddHeaderLine("ATENDIÓ: Barry ");
            tic.AddHeaderLine("CLIENTE: PUBLICO EN GENERAL");
            tic.AddHeaderLine("TIPODOC:");
            tic.AddHeaderLine("NUM DOC:");
            tic.AddSubHeaderLine("");
            tic.AddSubHeaderLine("FECHA: " + DateTime.Now.ToShortDateString() + " HORA: " + DateTime.Now.ToShortTimeString());
            foreach (DataGridViewRow row in dgvDetalleBoleta.Rows)
            {
                tic.AddItem(
                    row.Cells[5].Value.ToString(),
                    row.Cells[2].Value.ToString(),
                    String.Format(new CultureInfo("es-PE"), "{0:C}",
                    Convert.ToDouble(row.Cells[5].Value.ToString()) *
                    Convert.ToDouble(row.Cells[7].Value.ToString()))


                    );
                
            }
            double iva = 0;
            tic.AddTotal("SUBTOTAL", String.Format(new CultureInfo("es-PE"), "{0:C}",
           txtTotal.Text));
            tic.AddTotal("IVA", String.Format(new CultureInfo("es-PE"), "{0:C}",
                iva=Double.Parse(txtTotal.Text) * igv));
            tic.AddTotal("TOTAL", String.Format(new CultureInfo("es-PE"), "{0:C}",
                Double.Parse(txtTotal.Text) + iva));
            tic.AddTotal("DESCUENTO", String.Format(new CultureInfo("es-PE"), "{0:C}",
                -0));
            tic.AddTotal("GRAN TOTAL", String.Format(new CultureInfo("es-PE"), "{0:C}",
                Double.Parse(txtTotal.Text) + iva));
            tic.AddTotal("", "");//Ponemos un total 
                                    //en blanco que sirve de espacio 
            tic.AddTotal("RECIBIDO", String.Format(new CultureInfo("es-PE"), "{0:C}",
                txtEfectivo.Text));
            tic.AddTotal("CAMBIO", String.Format(new CultureInfo("es-PE"), "{0:C}",
                txtCambio.Text));
            tic.AddTotal("", "");//Ponemos un total 
                                 //en blanco que sirve de espacio 
            foreach (DataGridViewRow row1 in dgvDetalleBoleta.Rows)
            {
                tic.CadenaQR = Convert.ToString(row1.Cells[2].Value.ToString()) + Convert.ToString(row1.Cells[7].Value.ToString());
            }
            //tic.CadenaQR=Convert.ToString(dgvDetalleBoleta.CurrentRow.Cells[2].Value.ToString()) + txtTotal.Text;
            tic.AddFooterLine("Gracias por su compra, cualquier reclamo");
            tic.AddFooterLine("deberá presentar este comprobante de com");
            tic.AddFooterLine("pra, boleta de venta conforma a resoluci");
            tic.AddFooterLine("on de Barry Zeha Developer,vuelva pronto");
            tic.Path = $"E:\\PDFS\\";
            tic.FileName = "Ticket.pdf";
            tic.Print();
        }
        //Fin del método
    }
}
