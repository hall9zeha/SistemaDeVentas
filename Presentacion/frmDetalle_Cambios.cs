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
namespace Presentacion
{
    public partial class frmDetalle_Cambios : Form
    {
        VentasN objVN = new VentasN();
        DetalleVentasE objDTE = new DetalleVentasE();
        string _codBoleta = "";
        int _idVenta = 0;
        double precioUnid = 0.0;
        double montActualizado = 0.0;
        public frmDetalle_Cambios(int idVenta, string codBoleta)
        {
            InitializeComponent();
            this._codBoleta = codBoleta;
            this._idVenta = idVenta;
        }

        private void LblBoleta_Click(object sender, EventArgs e)
        {

        }

        private void Detalle_Cambios_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            crearGrid(dgvDetalleCambio);
            crearGrid(dgvPrendaCambio);
            listarDetalleBoletaCambio();
            montoTotal();
            lblBoleta.Text = _codBoleta;
            habilitarBotones(true, false, false, false, true,false);

        }

        void guardarCambioDePrenda()
        {
            try
            {
                VentasE b = new VentasE();
                List<DetalleVentasE> Detalle = new List<DetalleVentasE>();
                foreach (DataGridViewRow row in dgvDetalleCambio.Rows)
                {
                    DetalleVentasE dt = new DetalleVentasE();
                    dt.IdVenta = _idVenta;

                    dt.Codproducto = row.Cells[0].Value.ToString();
                    dt.CodProducto_detalle = Convert.ToInt32(row.Cells[9].Value.ToString());
                    dt.Descripción = row.Cells[1].Value.ToString();
                    dt.Cantidad = Convert.ToInt32(row.Cells[6].Value.ToString());
                    dt.Coddetalle = Convert.ToInt32(row.Cells[8].Value.ToString());
                    dt.Precio_final = Convert.ToDouble(row.Cells[7].Value.ToString());
                    dt.EstadoCambio = row.Cells[10].Value.ToString();

                    Detalle.Add(dt);
                }

                b.DetalleVenta = Detalle;
                b.Importe_rg = Convert.ToDouble(nuevoTotal.Text);
                b.IdVenta = _idVenta;

                int resultado = VentasN.Instancia.GuardarCambioDePrenda(b);

                MessageBox.Show("Cambio registrado ");

            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "Algo paso revisa"); }


        }


        void crearGrid(DataGridView dgv)
        {
            dgv.Columns.Add("Codproducto", "Codproducto");
            dgv.Columns.Add("Descripción", "Descripción");
            dgv.Columns.Add("Marca", "Marca");
            dgv.Columns.Add("Color", "Color");
            dgv.Columns.Add("TallaA", "TallaA");
            dgv.Columns.Add("TallaN", "TallaN");
            dgv.Columns.Add("Cantidad", "Cantidad");
            dgv.Columns.Add("Importe", "Importe");
            dgv.Columns.Add("CodDet.", "CodDet.");
            dgv.Columns.Add("CodProD", "CodProD");
            dgv.Columns.Add("E/C", "E/C");//significa Estado/Cambio.
            dgv.Columns.Add("P/C", "P/C");//significa Precio/Cambio.


            dgv.Columns[0].Width = 100;
            dgv.Columns[1].Width = 100;
            dgv.Columns[2].Width = 70;
            dgv.Columns[3].Width = 60;
            dgv.Columns[4].Width = 50;
            dgv.Columns[5].Width = 50;
            dgv.Columns[6].Width = 50;
            dgv.Columns[7].Width = 50;
            dgv.Columns[8].Width = 50;
            dgv.Columns[9].Width = 30;
            dgv.Columns[10].Width = 50;
            dgv.Columns[11].Width = 50;

            dgv.AllowUserToAddRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;


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
        void listarDetalleBoletaCambio()
        {
            try
            {
                int num = 0;
                List<DetalleVentasE> lista = objVN.ListarDetalleVentaCambio(_idVenta);
                dgvDetalleCambio.Rows.Clear();
                for (int i = 0; i < lista.Count; i++)
                {
                    num++;
                    string[] fila = new string[] {
                        lista[i].Codproducto,
                        lista[i].Descripción,
                        lista[i].Inventario.Marca,
                        lista[i].DetInventario.Color,
                        lista[i].DetInventario.Talla_alfanum,
                        lista[i].DetInventario.Talla_num.ToString(),
                        lista[i].Cantidad.ToString(),
                        lista[i].Precio_final.ToString("0.00"),
                        lista[i].Coddetalle.ToString(),
                        lista[i].CodProducto_detalle.ToString()


                    };
                    dgvDetalleCambio.Rows.Add(fila);

                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        void listarProductoCambio(List<DetalleInventarioE> lista)
        {
            try
            {
                int num = 0;

                dgvPrendaCambio.Rows.Clear();
                for (int i = 0; i < lista.Count; i++)
                {
                    num++;
                    string[] fila = new string[] {
                        lista[i].Codproducto,
                        lista[i].inventario.Descripción,
                        lista[i].Marca,
                        lista[i].Color,
                        lista[i].Talla_alfanum,
                        lista[i].Talla_num.ToString(),
                        lista[i].Cantidad.ToString(),
                        lista[i].Precio.ToString("0.00"),
                        lista[i].CodStock.ToString(),
                        lista[i].EstadoCambio= "E",
                        lista[i].MontoCambio.ToString("0.00")

                    };
                    dgvPrendaCambio.Rows.Add(fila);

                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        void montoTotal()
        {
            try
            {
                double monto = 0.0;
                foreach (DataGridViewRow row in dgvDetalleCambio.Rows)
                {
                    monto += Convert.ToDouble(row.Cells[7].Value.ToString());

                }
                txtTotal.Text = monto.ToString("0.00");
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        void montoTotalCambio()
        {
            try
            {
                double monto = 0.0;
                foreach (DataGridViewRow row in dgvPrendaCambio.Rows)
                {
                    monto += Convert.ToDouble(row.Cells[7].Value.ToString());

                }
                txtTotalCambio.Text = monto.ToString("0.00");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void montoActualizadoGuardar()
        {
            try
            {
                double total = 0.0;
                double diferencia = 0.0;
                foreach (DataGridViewRow row in dgvDetalleCambio.Rows)
                {
                    total += Convert.ToDouble(row.Cells[7].Value.ToString());
                }
                diferencia = total - Convert.ToDouble(txtTotal.Text);
                if (diferencia > 0)
                { txtDiferencia.BackColor = Color.GreenYellow; }
                txtDiferencia.Text = diferencia.ToString("0.00");
                montActualizado = Convert.ToDouble(txtDiferencia.Text) + Convert.ToDouble(txtTotal.Text);
                nuevoTotal.Text = montActualizado.ToString("0.00");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void montoCambioPagar()
        {
            double montoDif = 0.0;
            double total = 0.0;
            try
            {
                foreach (DataGridViewRow row in dgvPrendaCambio.Rows)
                {
                    montoDif += Convert.ToDouble(row.Cells[7].Value); //- Convert.ToDouble(row.Cells[10].Value);
                }

                total = montoDif - precioUnid;


                if (total < 0)
                {
                    txtTotalDif.BackColor = Color.Red;
                }
                else
                {
                    txtTotalDif.BackColor = Color.LightGreen;
                }
                txtTotalDif.Text = total.ToString("0.00");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnAgregarItem_Click(object sender, EventArgs e)
        {
            dgvDetalleCambio.CurrentRow.Cells[10].Value = "C";
            precioUnid = Convert.ToDouble(dgvDetalleCambio.CurrentRow.Cells[7].Value);
            dgvDetalleCambio.CurrentRow.Cells[7].Value = 0;
            marcarPrendaACambiar();
            frmBuscar_Cambio objBuscarCambio = new frmBuscar_Cambio(precioUnid);
            objBuscarCambio.ShowDialog();
            List<DetalleInventarioE> lista = LocalBD.Instancia.ReturnListaCambio(0, 0, 0, 0);
            listarProductoCambio(lista);
            montoTotalCambio();
            montoCambioPagar();
            habilitarBotones(true, true, true, false, true, true);

        }

        private void BtnQuitarItem_Click(object sender, EventArgs e)
        {

            dgvDetalleCambio.CurrentRow.Cells[10].Value = null;
            marcarPrendaACambiar();


        }
        void marcarPrendaACambiar()
        {
            foreach (DataGridViewRow row in dgvDetalleCambio.Rows)
            {
                if (row.Cells[10].Value == "C")
                {
                    row.DefaultCellStyle.BackColor = Color.Orange;
                }
                else if (row.Cells[10].Value == null)
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                    row.Cells[10].Value = "N";
                }
                if (row.Cells[11].Value == "E")
                {
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                }

            }
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            this.Dispose();
            LocalBD.Instancia.LimpiarListaCambio();

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvPrendaCambio.Rows)
            {
                if (row.Cells.Count > 0)
                {

                    double totalDif = Convert.ToDouble(txtTotalDif.Text);
                    if (totalDif < 0)
                    {
                        MessageBox.Show("El monto es inferior que la prenda a cambiar  ", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        agregarPrendaACambiar();

                    }
                }
                else if (row.Cells.Count == 0)
                {
                    MessageBox.Show("Advertencia", "No hay ninguna prenda para cambiar", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }



        }

        void agregarPrendaACambiar()
        {
            for (int i = 0; i < dgvPrendaCambio.RowCount; i++)
            {

                dgvDetalleCambio.Rows.Add(
                    dgvPrendaCambio.Rows[i].Cells[0].Value,
                    dgvPrendaCambio.Rows[i].Cells[1].Value,
                    dgvPrendaCambio.Rows[i].Cells[2].Value,
                    dgvPrendaCambio.Rows[i].Cells[3].Value,
                    dgvPrendaCambio.Rows[i].Cells[4].Value,
                    dgvPrendaCambio.Rows[i].Cells[5].Value,
                    dgvPrendaCambio.Rows[i].Cells[6].Value,
                    dgvPrendaCambio.Rows[i].Cells[7].Value,
                    "",
                    dgvPrendaCambio.Rows[i].Cells[8].Value,
                    dgvPrendaCambio.Rows[i].Cells[9].Value,
                    dgvPrendaCambio.Rows[i].Cells[10].Value,
                    dgvPrendaCambio.Rows[i].Cells[11].Value
                    );
            }
            dgvDetalleCambio.DefaultCellStyle.BackColor = Color.LightGreen;
            foreach (DataGridViewRow row in dgvDetalleCambio.Rows)
            {
                if (row.Cells[11].Value != null)
                {
                    row.Cells[8].Value = 0;
                }
            }

            dgvPrendaCambio.Rows.Clear();
            LocalBD.Instancia.LimpiarListaCambio();
            montoActualizadoGuardar();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Quiere realizar el cambio de Prenda?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                guardarCambioDePrenda();
                habilitarBotones(true, false, false, false, true, false);
            }
        }


        private void BtnAnular_Click(object sender, EventArgs e)
        {
            listarDetalleBoletaCambio();
            dgvPrendaCambio.Rows.Clear();
            LocalBD.Instancia.LimpiarListaCambio();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            int numPrenda = 0;
            foreach (DataGridViewRow row in dgvPrendaCambio.Rows)
            {
                numPrenda++;
            }
                if (numPrenda > 0)
                {
                    int codProd = Convert.ToInt32(dgvPrendaCambio.CurrentRow.Cells[8].Value);
                    LocalBD.Instancia.RemovePrendaListaCambio(codProd);
                    List<DetalleInventarioE> lista = LocalBD.Instancia.ReturnListaCambio(0, 0, 0, 0);
                    listarProductoCambio(lista);
                    montoTotalCambio();
                    montoCambioPagar();
                }

                else
                {
                    MessageBox.Show("No hay prendas en la lista", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {

        }

        private void BtnImprimir_Click(object sender, EventArgs e)
        {

        }
    }
}
