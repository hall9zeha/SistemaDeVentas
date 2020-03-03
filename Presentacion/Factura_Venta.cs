﻿using System;
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
    public partial class Factura_Venta : Form
    {
        BoletaE objB = new BoletaE();
        VentasN objVN = new VentasN();
        int tipoListaUsar = 0;
        public Factura_Venta()
        {
            InitializeComponent();
        }

        void crearGrid()
        {
            dgvDetalleFactura.Columns.Add("CodDet", "CodDet");
            dgvDetalleFactura.Columns.Add("CodProducto", "CodProducto");
            dgvDetalleFactura.Columns.Add("Descripción", "Descripción");
            dgvDetalleFactura.Columns.Add("Marca", "Marca");
            dgvDetalleFactura.Columns.Add("Prc.Venta", "Prc.Venta");
            dgvDetalleFactura.Columns.Add("Cantidad", "Cantidad");
            dgvDetalleFactura.Columns.Add("Stock", "Stock");
            dgvDetalleFactura.Columns.Add("P/Art.", "P/Art.");

            dgvDetalleFactura.Columns[0].Width = 80;
            dgvDetalleFactura.Columns[1].Width = 100;
            dgvDetalleFactura.Columns[2].Width = 100;
            dgvDetalleFactura.Columns[3].Width = 70;
            dgvDetalleFactura.Columns[4].Width = 60;
            dgvDetalleFactura.Columns[5].Width = 60;
            dgvDetalleFactura.Columns[6].Width = 60;
            dgvDetalleFactura.Columns[7].Width = 60;
            
            DataGridViewCellStyle cabecera = new DataGridViewCellStyle();
            cabecera.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDetalleFactura.ColumnHeadersDefaultCellStyle = cabecera;

            dgvDetalleFactura.AllowUserToAddRows = false;
            dgvDetalleFactura.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetalleFactura.AllowUserToResizeColumns = false;


        }
        private void Factura_Venta_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            crearGrid();
            generarCodigoFactura();
            habilitarBotones(false, false, false, true, true);
        }
        void generarCodigoFactura()
        {
            string serie = "FT" + 00;
            lblFactura.Text = objVN.GenerarCodigoBoletaFactura(serie,2);

        }
        void llenarGridFactura(List<DetalleInventarioE> lista)
        {
            try
            {
                dgvDetalleFactura.Rows.Clear();
                for (int i = 0; i < lista.Count; i++)
                {
                    string[] fila = new string[] {
                        lista[i].CodStock.ToString(),
                        lista[i].Codproducto,
                        lista[i].inventario.Descripción,
                        lista[i].Marca,
                        lista[i].Precio.ToString("0.00"),
                        lista[i].Cantidad.ToString(),
                        lista[i].Stock.ToString()

                    };
                    dgvDetalleFactura.Rows.Add(fila);

                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        void contarItems()
        {
            int items = 0;
            foreach (DataGridViewRow row in dgvDetalleFactura.Rows)
            {
                items++;
            }
            lblNumItems.Text = "N° Items" + " " + items;
        }
        void montoTotal()
        {
            double subTotal = 0.0;
            foreach (DataGridViewRow row in dgvDetalleFactura.Rows)
            {
                if (row.Cells[4].Value == null) row.Cells[4].Value = 0;
                {
                    row.Cells[7].Value = Convert.ToDouble(row.Cells[4].Value) * Convert.ToDouble(row.Cells[5].Value);

                }
                if (row.Cells[5].Value == null) row.Cells[5].Value = 0;
                {
                    row.Cells[7].Value = Convert.ToDouble(row.Cells[4].Value) * Convert.ToDouble(row.Cells[5].Value);
                    subTotal += Convert.ToDouble(row.Cells[7].Value);
                    txtTotal.Text = subTotal.ToString("0.00");
                }
                
            }
            if (dgvDetalleFactura.RowCount== 0)
            {
                txtTotal.Text = "0";
            }
        }
        void guardarVenta()
        {
            try
            {
                BoletaE b = new BoletaE();
                int tipoComprobante = 2;
                b.CodVenta = lblFactura.Text;
                b.Importe_rg = Convert.ToDouble(txtTotal.Text);
                b.tipoComprobante = tipoComprobante;
                List<DetalleBoletaE> Detalle = new List<DetalleBoletaE>();
                foreach (DataGridViewRow row in dgvDetalleFactura.Rows)
                {
                    DetalleBoletaE dt = new DetalleBoletaE();
                    
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
            List<DetalleBoletaE> detalle = new List<DetalleBoletaE>();
            foreach (DataGridViewRow row in dgvDetalleFactura.Rows)
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


        private void BtnAgregarItem_Click(object sender, EventArgs e)
        {
            tipoListaUsar = 2;
            Buscar_Prenda objBuscar_Prenda = new Buscar_Prenda(tipoListaUsar);
            objBuscar_Prenda.ShowDialog();
            List<DetalleInventarioE> lista = LocalBD.Instancia.ReturnListaFactura(0, 0, 0, 0);
            llenarGridFactura(lista);
            montoTotal();
            contarItems();
            habilitarBotones(true, true, true, true, true);

        }

        private void BtnQuitarItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Quieres quitar esta prenda de la lista?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                int codProd = Convert.ToInt32(dgvDetalleFactura.CurrentRow.Cells[0].Value);
                LocalBD.Instancia.RemoverPrendaListaFactura(codProd);
                List<DetalleInventarioE> lista = LocalBD.Instancia.ReturnListaFactura(0, 0, 0, 0);
                llenarGridFactura(lista);
                montoTotal();
                contarItems();
            }
        }
        void habilitarBotones(bool nuevo, bool guardar, bool anular, bool agregar, bool quitar)
        {
            btnGuardar.Enabled = guardar;
            btnAnular.Enabled = anular;
            btnNuevo.Enabled = nuevo;
            btnAgregarItem.Enabled = agregar;
            btnQuitarItem.Enabled = quitar;
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
                objTicket.TextoExtremos("Caja # 1", "Ticket # " + lblFactura.Text);
                objTicket.LineasAsteriscos();


                objTicket.TextoIzquierda("");
                objTicket.TextoIzquierda("ATENDIÓ: Barry ");
                objTicket.TextoIzquierda("CLIENTE: PUBLICO EN GENERAL");
                objTicket.TextoIzquierda("TIPODOC:");
                objTicket.TextoIzquierda("NUM DOC:");
                objTicket.TextoIzquierda("");
                objTicket.TextoExtremos("FECHA: " + DateTime.Now.ToShortDateString(), "HORA: " + DateTime.Now.ToShortTimeString());
                objTicket.LineasAsteriscos();


                objTicket.EncabezadoTicket();
                objTicket.LineasAsteriscos();
                foreach (DataGridViewRow row in dgvDetalleFactura.Rows)
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

                foreach (DataGridViewRow row in dgvDetalleFactura.Rows)
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

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            this.Dispose();
            LocalBD.Instancia.LimpiarListaFactura();
        }

        private void TxtEfectivo_KeyPress(object sender, KeyPressEventArgs e)
        {
            calcularVuelto();
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            dgvDetalleFactura.Rows.Clear();
            lblFactura.Text = "";
            txtTotal.Clear();
            txtEfectivo.Clear();
            txtCambio.Clear();
            lblNumItems.Text = "";
            generarCodigoFactura();
            habilitarBotones(true, false, false, true, true);
        }
    }
}
