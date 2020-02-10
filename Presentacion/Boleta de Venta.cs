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
        SqlConnection cn = Conexion.Instancia.Conectar();
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
            dgvDetalleBoleta.Columns.Add("Total", "Total");
                       
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
            try
            {
                string Abc = "BO";
                string Query = @"Declare @Id Int
                                select top 1 @Id = Left(Codboleta,4) FROM tbboleta  order by Codboleta desc
                                if LEN(@Id) is null
                                begin
                                set @id = 1
                                end
                                print @id
                                Declare @Val int
                                select @Val=COUNT(*) from tbboleta where LEFT(Codboleta,4)=@id
                                if @val = 1
                                 begin
                                 set @Id = @Id+1
                                 set @Val = 1
                                 end
                                else
                                 begin
                                 set @Id = @Id
                                 set @Val = @Val +1
                                 end
 
                                select @Id As Numero,@Val As Abc";
                SqlCommand cmd = new SqlCommand(Query, cn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                DataRow dr;
                dr = dt.Rows[0];

                string codigoTabla = dr[0].ToString();
                int codeTablaConvert = int.Parse(codigoTabla);
                string drCeros = "";
                string numeracion = codeTablaConvert.ToString();
                for (int i = 0; i <= 3 - numeracion.Length; i++)
                {
                    drCeros += "0";

                }
                drCeros += numeracion;
               lblBoleta.Text = drCeros + "-" + Abc;

                if (cn.State == ConnectionState.Open) cn.Close();
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

            }
            catch (Exception)
            { throw; }
            

        }

        private void BtnAgregarItem_Click(object sender, EventArgs e)
        {
            Buscar_Prenda objBuscar_Prenda = new Buscar_Prenda();
            objBuscar_Prenda.ShowDialog();
           
            List<DetalleInventarioE> lista = LocalBD.Instancia.ReturnListaBoleta(0, 0, 0, 0);
            llenarGridBoleta(lista);
            montoTotal();
            contarItems();
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
                    LocalBD.Instancia.RemovePrendaLista(idStock);

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
                    lblBoleta.Text = "";
                    txtTotal.Clear();
                    lblNumItems.Text = "";
                    generarCodigoBoleta();
                   
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
                b.Codboleta = lblBoleta.Text;
                b.Importe_rg = Convert.ToDouble(txtTotal.Text);

                List<DetalleBoletaE> Detalle = new List<DetalleBoletaE>();
                foreach (DataGridViewRow row in dgvDetalleBoleta.Rows)
                {
                    DetalleBoletaE dt = new DetalleBoletaE();
                    dt.Codboleta = lblBoleta.Text;

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
                dgvDetalleBoleta.Rows.Clear();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
       
       
       

    }
}
