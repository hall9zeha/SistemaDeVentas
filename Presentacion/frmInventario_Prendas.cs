using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Windows.Forms;
using Negocio;
using Entidades;
namespace Presentacion
{
    public partial class frmInventario_Prendas : Form
    {
        InventarioE objE = new InventarioE();
        InventarioN objN = new InventarioN();
        int tipoBusqueda = 0;
        System.Drawing.Image img = null;
        public frmInventario_Prendas()
        {
            InitializeComponent();
        }

        private void Inventario_Prendas_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
          
            crearGrid();
            //listarInventario();
            
            contarPrendas();

        }
        void crearGrid()
        {
            dgvInventario.Columns.Add("CodProducto", "CodProducto");
            dgvInventario.Columns.Add("Descripción", "Descripción");
            dgvInventario.Columns.Add("Marca", "Marca");
            dgvInventario.Columns.Add("Color", "Color");
            dgvInventario.Columns.Add("Tall/A", "Talla/A");
            dgvInventario.Columns.Add("Talla/N", "Talla/N");
            dgvInventario.Columns.Add("Stock", "Stock");
            dgvInventario.Columns.Add("Precio/Compra", "Precio/Compra");
            dgvInventario.Columns.Add("PrecioVenta", "PrecioVenta");
            dgvInventario.Columns.Add("BarCode", "BarCode");
            
            DataGridViewImageColumn Imagen = new DataGridViewImageColumn();
            Imagen.HeaderText = "Estado Stock";
            Imagen.Name = "Estado Stock";
            dgvInventario.Columns.Add(Imagen);

            dgvInventario.Columns[0].Width = 100;
            dgvInventario.Columns[1].Width = 170;
            dgvInventario.Columns[2].Width = 150;
            dgvInventario.Columns[3].Width = 80;
            dgvInventario.Columns[4].Width = 60;
            dgvInventario.Columns[5].Width = 60;
            dgvInventario.Columns[6].Width = 50;
            dgvInventario.Columns[7].Width = 80;
            dgvInventario.Columns[8].Width = 60;
            dgvInventario.Columns[9].Width = 60;
            dgvInventario.Columns[10].Width = 90;
            DataGridViewCellStyle cssCabecera = new DataGridViewCellStyle();
            cssCabecera.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvInventario.ColumnHeadersDefaultCellStyle = cssCabecera;

            dgvInventario.AllowUserToAddRows = false;
            dgvInventario.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInventario.AllowUserToResizeColumns = false;

        }
        void contarPrendas()
        {
            try
            {
                int stock = 0;
                foreach (DataGridViewRow row in dgvInventario.Rows)
                {
                    stock += Convert.ToInt32(row.Cells[6].Value.ToString());
                }
                lblPrendas.Text = "N° Total" + " " + stock; 
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        void buscarInventario(string filtro)
        {

            try
            {
                if (filtro != string.Empty)
                {
                    
                    List<InventarioE> lista = InventarioN.Instancia.ListarBusquedaSimpleInventario( filtro);
                    dgvInventario.Rows.Clear();
                    for (int i = 0; i < lista.Count; i++)
                    {
                        
                        if (lista[i].DtInventario.Stock >= 10)
                        { img = Properties.Resources.circulo_verde24x24; }
                        else if (lista[i].DtInventario.Stock <= 9 && lista[i].DtInventario.Stock >= 1)
                        { img = Properties.Resources.CirculoNaranja24x24; }
                        else if (lista[i].DtInventario.Stock == 0)
                        { img = Properties.Resources.circulorojo_24x24; }

                        string[] fila = new string[] {
                            lista[i].Codproducto,
                            lista[i].Descripción,
                            lista[i].Marca,
                            lista[i].DtInventario.Color,
                            lista[i].DtInventario.Talla_alfanum,
                            lista[i].DtInventario.Talla_num.ToString(),
                            lista[i].DtInventario.Stock.ToString(),
                            lista[i].Precio.ToString("0.00"),
                            lista[i].PrecioVenta.ToString("0.00"),
                            lista[i].DtInventario.CodigoDeBarra
                        };
                        dgvInventario.Rows.Add(fila);
                        dgvInventario.Rows[i].Cells[10].Value = img;
                        contarPrendas();
                    }

                }
                else
                {
                    listarInventario();
                    contarPrendas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        void listarInventario()
        {
            try
            {
                            

                    List<InventarioE> lista = InventarioN.Instancia.ListarInventario();
                    dgvInventario.Rows.Clear();
                    for (int i = 0; i < lista.Count; i++)
                    {

                        if (lista[i].DtInventario.Stock >= 10)
                        { img = Properties.Resources.circulo_verde24x24; }
                        else if (lista[i].DtInventario.Stock <= 9 && lista[i].DtInventario.Stock >= 1)
                        { img = Properties.Resources.CirculoNaranja24x24; }
                        else if (lista[i].DtInventario.Stock == 0)
                        { img = Properties.Resources.circulorojo_24x24; }

                        string[] fila = new string[] {
                            lista[i].Codproducto,
                            lista[i].Descripción,
                            lista[i].Marca,
                            lista[i].DtInventario.Color,
                            lista[i].DtInventario.Talla_alfanum,
                            lista[i].DtInventario.Talla_num.ToString(),
                            lista[i].DtInventario.Stock.ToString(),
                            lista[i].Precio.ToString("0.00"),
                            lista[i].PrecioVenta.ToString("0.00"),
                            lista[i].DtInventario.CodigoDeBarra
                        };
                        dgvInventario.Rows.Add(fila);
                        dgvInventario.Rows[i].Cells[10].Value = img;
                        contarPrendas();
                    }

             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void generarBarCodes()
        {
            Document doc = new Document(new iTextSharp.text.Rectangle(24, 12), 5, 5, 1, 1);

            try
            {
                int numPrendas = 0;
               
                
                string path = $"E:\\PDFS\\codeBar.pdf";
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
                doc.Open();

                DataTable dt = new DataTable();
                dt.Columns.Add("ID");
                dt.Columns.Add("Price");
                dt.Columns.Add("Color");
                dt.Columns.Add("Cantidad");
                dt.Columns.Add("Descripcion");
                dt.Columns.Add("Marca");

                foreach (DataGridViewRow row in dgvInventario.Rows)
                {
                    numPrendas++;
                }

                for (int i = 0; i < numPrendas; i++)
                {
                    for (int n = 0; n <  Convert.ToInt32(dgvInventario.Rows[i].Cells[6].Value.ToString()); n++)
                    {
                        DataRow row = dt.NewRow();
                        row["ID"] = dgvInventario.Rows[i].Cells[0].Value.ToString() + i.ToString();
                        row["Price"] = "S/. " + dgvInventario.Rows[i].Cells[8].Value.ToString();
                        row["Descripcion"] = dgvInventario.Rows[i].Cells[1].Value.ToString();
                        row["Marca"] = dgvInventario.Rows[i].Cells[2].Value.ToString();
                        row["Color"] = dgvInventario.Rows[i].Cells[3].Value.ToString();
                        row["Cantidad"] = dgvInventario.Rows[i].Cells[6].Value.ToString();
                        dt.Rows.Add(row);
                    }
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
                    cb1.ShowText(dt.Rows[i]["Descripcion"].ToString());
                    cb1.EndText();

                    PdfContentByte cb2 = writer.DirectContent;
                    BaseFont bf2 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb2.SetFontAndSize(bf2, 1.3f);
                    cb2.BeginText();
                    cb2.SetTextMatrix(1.2f, 7.5f);
                    cb2.ShowText(dt.Rows[i]["Marca"].ToString());
                    cb2.EndText();

                    PdfContentByte cb3 = writer.DirectContent;
                    BaseFont bf3 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb3.SetFontAndSize(bf3, 1.3f);
                    cb3.BeginText();
                    cb3.SetTextMatrix(1.2f, 6.3f);
                    cb3.ShowText(dt.Rows[i]["Color"].ToString());
                    cb3.EndText();

                    PdfContentByte cb4 = writer.DirectContent;
                    BaseFont bf4 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb4.SetFontAndSize(bf4, 1.3f);
                    cb4.BeginText();
                    cb4.SetTextMatrix(17.5f, 1.0f);
                    cb4.ShowText(dt.Rows[i]["Price"].ToString());
                    cb4.EndText();


                    iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                    iTextSharp.text.pdf.Barcode128 bc = new Barcode128();
                    bc.TextAlignment = Element.ALIGN_LEFT;
                    bc.Code = dt.Rows[i]["ID"].ToString();
                    bc.StartStopText = false;
                    bc.CodeType = iTextSharp.text.pdf.Barcode128.EAN13;
                    bc.Extended = true;

                    System.Drawing.Image bimg = bc.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White);
                    img1 = bimg;
                    //pictureBox1.Image = img1;

                    iTextSharp.text.Image img = bc.CreateImageWithBarcode(cb, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.BaseColor.BLACK);
                    cb.SetTextMatrix(1.5f, 3.0f);
                    img.ScaleToFit(60, 5);
                    img.SetAbsolutePosition(1.5f, 1);



                    cb.AddImage(img);



                }
                doc.Close();
                MessageBox.Show("Codigos de Barra creados");
                System.Diagnostics.Process.Start(path);



            }
            catch (Exception e)
            { MessageBox.Show(e.Message); }
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            string id = Convert.ToString(dgvInventario.CurrentRow.Cells[0].Value);
            frmDetalle_Prenda Dprenda = new frmDetalle_Prenda(id,0);
            Dprenda.Show();
        }

        private void Txtbuscar_TextChanged_1(object sender, EventArgs e)
        {
            buscarInventario(txtbuscar.Text);
        }

        private void CheckBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            tipoBusqueda = 2;
        }

        private void CheckBox2_CheckedChanged_1(object sender, EventArgs e)
        {
            tipoBusqueda = 3;
        }

        private void CheckBox3_CheckedChanged_1(object sender, EventArgs e)
        {
            tipoBusqueda = 1;
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            listarInventario();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show( "Se generarán códigos de barra para \n todos los productos de la lista \n \n Quiere proceder? ","Importante", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            { generarBarCodes(); }
            
            
        }
    }
}
