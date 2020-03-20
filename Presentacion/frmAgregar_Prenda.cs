using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data.SqlClient;
using Datos;
using Negocio;
using Entidades;

namespace Presentacion
{
    public partial class frmAgregar_Prenda : Form
    {
        InventarioE objE = new InventarioE();
        DetalleInventarioE objS = new DetalleInventarioE();
        InventarioN objN = new InventarioN();
        SqlConnection cn =  Conexion.Instancia.Conectar();

        public frmAgregar_Prenda()
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
        void genCodigoDeBarra()
        {

            Document doc = new Document(new iTextSharp.text.Rectangle(24, 12), 5, 5, 1, 1);

            try
            {
                int numPrendas = 0;
                string code = Convert.ToString(dgvprenda.CurrentRow.Cells[0].Value.ToString());
                int precio = Convert.ToInt32(txtprecioventa.Text);
                string[] color = { }; /*Convert.ToString(dgvprenda.CurrentRow.Cells[1].Value.ToString());*/
                string cantidad = txtcantidad.Text;
                string desc = txtdescripcion.Text;
                
                    for (int i=0; i < dgvprenda.RowCount; i++)
                    {
                        color = new string[] { dgvprenda.Rows[i].Cells[1].Value.ToString() };
                  
                }
                

                string Marca = txtmarca.Text;
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/codes.pdf", FileMode.Create));
                doc.Open();

                DataTable dt = new DataTable();
                dt.Columns.Add("ID");
                dt.Columns.Add("Price");
                dt.Columns.Add("Color");
                dt.Columns.Add("Cantidad");
                dt.Columns.Add("Descripcion");
                dt.Columns.Add("Marca");
                foreach (DataGridViewRow row in dgvprenda.Rows)
                {
                    numPrendas++;
                }
                for (int i = 0; i < numPrendas; i++)
                {
                    DataRow row = dt.NewRow();
                    row["ID"] = code + i.ToString();
                    row["Price"] = "S/. " + precio.ToString("0.00");
                    row["Descripcion"] = desc;
                    row["Marca"] = Marca;
                    row["Color"] = dgvprenda.Rows[i].Cells[1].Value.ToString();
                    row["Cantidad"] = cantidad;
                    dt.Rows.Add(row);
                }
                System.Drawing.Image img1 = null;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i != 0)
                        doc.NewPage();
                    PdfContentByte cb1 = writer.DirectContent;
                    BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_BOLDITALIC, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                    cb1.SetFontAndSize(bf, 2.0f);
                    cb1.BeginText();
                    cb1.SetTextMatrix(1.2f, 9.5f);
                    cb1.ShowText(dt.Rows[i]["Descripcion"].ToString() );
                    cb1.EndText();

                    PdfContentByte cb4 = writer.DirectContent;
                    BaseFont bf3 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb4.SetFontAndSize(bf3, 1.3f);
                    cb4.BeginText();
                    cb4.SetTextMatrix(1.2f, 7.5f);
                    cb4.ShowText(dt.Rows[i]["Marca"].ToString());
                    cb4.EndText();


                    PdfContentByte cb2 = writer.DirectContent;
                    BaseFont bf1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb2.SetFontAndSize(bf1, 1.3f);
                    cb2.BeginText();
                    cb2.SetTextMatrix(1.2f, 6.3f);
                    cb2.ShowText(dt.Rows[i]["color"].ToString());
                    cb2.EndText();


                    PdfContentByte cb3 = writer.DirectContent;
                    BaseFont bf2 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb3.SetFontAndSize(bf2, 1.3f);
                    cb3.BeginText();
                    cb3.SetTextMatrix(17.5f, 1.0f);
                    cb3.ShowText(dt.Rows[i]["Price"].ToString());
                    cb3.EndText();




                    iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                    iTextSharp.text.pdf.Barcode128 bc = new Barcode128();
                    bc.TextAlignment = Element.ALIGN_LEFT;
                    bc.Code = dt.Rows[i]["ID"].ToString();
                    bc.StartStopText = false;
                    bc.CodeType = iTextSharp.text.pdf.Barcode128.EAN13;
                    bc.Extended = true;
                   //mostrando la imagen del producto en un picturebox
                    System.Drawing.Image bimg = bc.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White);
                    img1 = bimg;
                    //fin de la propiedad
                    pictureBox1.Image = img1;

                    iTextSharp.text.Image img = bc.CreateImageWithBarcode(cb, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.BaseColor.BLACK);

                    cb.SetTextMatrix(1.5f, 3.0f);
                    img.ScaleToFit(60, 5);
                    img.SetAbsolutePosition(1.5f, 1);
                    cb.AddImage(img);

                }

                ////////////////////***********************************//////////////////////


                doc.Close();
                System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/codes.pdf");
                //MessageBox.Show("el archivo se guardo en escritorio con el nombre de codes.pdf");
            }
            catch
            {
            }
            finally
            {
                doc.Close();
            }
        }
        private void Lblcode_Click(object sender, EventArgs e)
        {

        }

        private void Button2_Click_1(object sender, EventArgs e)
        {
            genCodigoDeBarra();
        }
    }
}

