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
    public partial class Detalle_Cambios : Form
    {
        VentasN objVN = new VentasN();
        DetalleBoletaE objDTE = new DetalleBoletaE();
        string _codBoleta = "";
        double precioUnid = 0.0;
        double montActualizado = 0.0;
        public Detalle_Cambios(string codBoleta)
        {
            InitializeComponent();
            this._codBoleta = codBoleta;
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
        void listarDetalleBoletaCambio()
        {
            try
            {
                int num = 0;
                List<DetalleBoletaE> lista = objVN.ListarDetalleBoletaCambio(_codBoleta);
                dgvDetalleCambio.Rows.Clear();
                for (int i = 0; i < lista.Count; i++)
                {
                    num++;
                    string[] fila = new string[] {
                        lista[i].Codproducto,
                        lista[i].Descripción,
                        lista[i].Marca.Marca,
                        lista[i].Color.Color,
                        lista[i].Talla_alfanum.Talla_alfanum,
                        lista[i].Talla_num.Talla_num.ToString(),
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
                txtTotalCambio.Text =monto.ToString("0.00");
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
                txtDiferencia.Text =diferencia.ToString("0.00");
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
            Buscar_Cambio objBuscarCambio = new Buscar_Cambio(precioUnid);
            objBuscarCambio.ShowDialog();
            List<DetalleInventarioE> lista = LocalBD.Instancia.ReturnListaCambio(0, 0, 0,0);
            listarProductoCambio(lista);
            montoTotalCambio();
            montoCambioPagar();
           
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
                if (row.Cells[10].Value!=null)
                {
                    row.DefaultCellStyle.BackColor = Color.Orange;
                }
                else if (row.Cells[10].Value==null)
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                    row.Cells[10].Value = "N";
                }
                if(row.Cells[11].Value!=null)
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
           for(int i=0; i<dgvPrendaCambio.RowCount; i++)
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
            //prueba de guardado de datos
            
            registrarEntradaPrendaCambio();
            registrarSalidaPrendaCambio();
        }

        void registrarEntradaPrendaCambio()
        {
            try
            {
                for (int i = 0; i < dgvDetalleCambio.RowCount; i++)
                {
                    objDTE.EstadoCambio = dgvDetalleCambio.Rows[i].Cells[10].Value.ToString();
                    objDTE.Cantidad= Convert.ToInt32(dgvDetalleCambio.Rows[i].Cells[6].Value.ToString());
                    objDTE.Coddetalle = Convert.ToInt32(dgvDetalleCambio.Rows[i].Cells[8].Value.ToString());
                    objDTE.CodProducto_detalle= Convert.ToInt32(dgvDetalleCambio.Rows[i].Cells[9].Value.ToString());
                    objVN.RegistrarEntradaPrendaCambio(objDTE);
               
                }
            MessageBox.Show("Transaccion1 correcta, revisa porfa ");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void registrarSalidaPrendaCambio()
        {
            //try
            //{
                
                for (int i = 0; i < dgvDetalleCambio.RowCount; i++)
                {
                    objDTE.EstadoCambio = dgvDetalleCambio.Rows[i].Cells[10].Value.ToString();
                    objDTE.Codboleta = lblBoleta.Text;
                    objDTE.Codproducto = dgvDetalleCambio.Rows[i].Cells[0].Value.ToString();
                    objDTE.CodProducto_detalle = Convert.ToInt32(dgvDetalleCambio.Rows[i].Cells[9].Value.ToString());
                    objDTE.Descripción = dgvDetalleCambio.Rows[i].Cells[1].Value.ToString();
                    objDTE.Cantidad = Convert.ToInt32(dgvDetalleCambio.Rows[i].Cells[6].Value.ToString());
                    objDTE.Precio_final = Convert.ToDouble(dgvDetalleCambio.Rows[i].Cells[7].Value.ToString());
                    objDTE.CodProducto_detalle = Convert.ToInt32(dgvDetalleCambio.Rows[i].Cells[9].Value.ToString());
                    objDTE.importe = Convert.ToDouble(nuevoTotal.Text);

                objVN.RegistrarSalidaPrendaCambio(objDTE);

                }
            
            MessageBox.Show("Transaccion2 correcta, revisa porfa ");

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

        }

        private void BtnAnular_Click(object sender, EventArgs e)
        {
            listarDetalleBoletaCambio();
            dgvPrendaCambio.Rows.Clear();
            LocalBD.Instancia.LimpiarListaCambio();
        }
    }
}
