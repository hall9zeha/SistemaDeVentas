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
    public partial class frmEditar_Prenda : Form
    {
        
        InventarioN objN = new InventarioN();
        InventarioE objE = new InventarioE();
        DetalleInventarioE objS = new DetalleInventarioE();
        AccionesEnControles objAc = new AccionesEnControles();
        string Id_Prod = "";
        int idStock = 0;
        int tipoAccion = 0;
        public frmEditar_Prenda(string Id)
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
            dgvprenda.Columns.Add("BarCode", "BarCode");
            DataGridViewImageColumn imagen = new DataGridViewImageColumn();
            imagen.HeaderText = "CrearBarCode";
            imagen.Name = "CrearBarCode";
            dgvprenda.Columns.Add(imagen);

            dgvprenda.Columns[0].Width = 60;
            dgvprenda.Columns[1].Width = 100;
            dgvprenda.Columns[2].Width = 120;
            dgvprenda.Columns[3].Width = 60;
            dgvprenda.Columns[4].Width = 60;
            dgvprenda.Columns[5].Width = 60;
            dgvprenda.Columns[6].Width = 100;
            dgvprenda.Columns[7].Width = 100;

            dgvprenda.AllowUserToAddRows = false;
            dgvprenda.AllowUserToResizeRows = false;
            dgvprenda.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvprenda.MultiSelect = false;

            DataGridViewCellStyle css = new DataGridViewCellStyle();
            css.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvprenda.ColumnHeadersDefaultCellStyle = css;

            
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
                int i = 0;
                int val = 0;
                objS.Codproducto = lblcode.Text;
                if (txtcolor.Text == "") objS.Color= "vacio"; else objS.Color = txtcolor.Text;
                if (cmbtallaalfa.Text == "") objS.Talla_alfanum = "vacio"; else objS.Talla_alfanum = cmbtallaalfa.Text;
                if (txttallanum.Text == "")objS.Talla_num = 0; else objS.Talla_num =Convert.ToInt32(txttallanum.Text);
                if (txtcantidad.Text == "") objS.Cantidad = 0; else objS.Cantidad = Convert.ToInt32(txtcantidad.Text);
                if (txtcantidad.Text == "") objS.Stock = 0; else objS.Stock = Convert.ToInt32(txtcantidad.Text);
                //método para agregar código numérico que será el código de barras de cada producto, al grid
                foreach (DataGridViewRow rows in dgvprenda.Rows)
                {
                    i++;
                    if (i > 0)
                    {
                        val++;
                    }
                }
                objS.CodigoDeBarra = lblcode.Text + val;
                //fin del método
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
        void generarCodigoDeBarra(int tipoGen)
        {
            Document doc = new Document(new iTextSharp.text.Rectangle(24, 12), 5, 5, 1, 1);
            
            try
            {
                int numPrendas = 0;
                string code = Convert.ToString(dgvprenda.CurrentRow.Cells[1].Value.ToString());
                int stock = Convert.ToInt32(dgvprenda.CurrentRow.Cells[5].Value.ToString());
                int precio = Convert.ToInt32(txtprecioventa.Text);
                string cantidad = txtcantidad.Text;
                string desc = txtdescripcion.Text;

                string marca = txtmarca.Text;
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
                if (tipoGen == 1)
                {
                    foreach (DataGridViewRow row in dgvprenda.Rows)
                    {
                        numPrendas++;
                    }
                    
                        for (int i = 0; i < numPrendas; i++)
                        {
                        for (int n = 0; n < Convert.ToInt32(dgvprenda.Rows[i].Cells[5].Value.ToString()); n++)
                        {
                            DataRow row = dt.NewRow();
                            row["ID"] = dgvprenda.Rows[i].Cells[1].Value.ToString() + i.ToString();
                            row["Price"] = "S/. " + precio.ToString("0.00");
                            row["Descripcion"] = desc;
                            row["Marca"] = marca;
                            row["Color"] = dgvprenda.Rows[i].Cells[2].Value.ToString();
                            row["Cantidad"] = cantidad;
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
                            pictureBox1.Image = img1;

                            iTextSharp.text.Image img = bc.CreateImageWithBarcode(cb, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.BaseColor.BLACK);
                            cb.SetTextMatrix(1.5f, 3.0f);
                            img.ScaleToFit(60, 5);
                            img.SetAbsolutePosition(1.5f, 1);



                            cb.AddImage(img);

                        

                    }
                    doc.Close();
                    System.Diagnostics.Process.Start(path);

                }
                else if (tipoGen == 2)
                {
                    for (int i = 0; i < stock; i++)
                    {
                        DataRow row = dt.NewRow();
                        row["ID"] = dgvprenda.CurrentRow.Cells[6].Value.ToString();
                        row["Price"] = "S/. " + precio.ToString("0.00");
                        row["Descripcion"] = desc;
                        row["Marca"] = marca;
                        row["Color"] = dgvprenda.CurrentRow.Cells[2].Value.ToString();
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
                        pictureBox1.Image = img1;

                        iTextSharp.text.Image img = bc.CreateImageWithBarcode(cb, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.BaseColor.BLACK);
                        cb.SetTextMatrix(1.5f, 3.0f);
                        img.ScaleToFit(60, 5);
                        img.SetAbsolutePosition(1.5f, 1);
                        
                        cb.AddImage(img);



                    }
                    doc.Close();
                    System.Diagnostics.Process.Start(path);

                }
            }
            
            
            catch
            {
            }
            finally { doc.Close(); }
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

                //prueba para cargar imagen de code Bar en picturebox
                System.Drawing.Image img1 = null;
                iTextSharp.text.pdf.Barcode128 bc = new Barcode128();
                bc.TextAlignment = Element.ALIGN_CENTER;
                bc.Code =row.Cells[6].Value.ToString();
                bc.StartStopText = false;
                bc.CodeType = iTextSharp.text.pdf.Barcode128.EAN13;
                bc.Extended = true;
                System.Drawing.Image bimg = bc.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White);
                img1 = bimg;
                pictureBox1.Image = img1;

                //fin de prueaba
                //Prueba de cargar codigo QR en un picture box que contiene el texto del color de cada prenda
                pictureBox2.Image=CodesMethods.Instancia.CodigoQR(row.Cells[6].Value.ToString());
                //fin de prueba
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
                System.Drawing.Image imagen = null;
                List<DetalleInventarioE> lista = objN.TraerDetallePrenda(Id_Prod);
                dgvprenda.Rows.Clear();
                for (int i = 0; i < lista.Count; i++)
                {
                    imagen = Properties.Resources.barcodes_40531;
                    num++;
                    string[] fila = new string[] {
                        lista[i].CodStock.ToString(),
                        lista[i].Codproducto,
                        lista[i].Color,
                        lista[i].Talla_alfanum,
                    lista[i].Talla_num.ToString(),
                    lista[i].Stock.ToString(),
                    lista[i].CodigoDeBarra
                    };
                    
                    dgvprenda.Rows.Add(fila);
                    dgvprenda.Rows[i].Cells[7].Value = imagen;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Realmente quiere eliminar esta prenda?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                tipoAccion = 3;
                eliminarDetallePrenda();
                listarDetallePrenda();
            }
            
        }
        void limpiarControles()
        {
            txtcolor.Clear();
            cmbtallaalfa.Text = "";
            txttallanum.Clear();
            txtcantidad.Clear();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            limpiarControles();
            
        }

        private void BtnBarCode_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Se generarán códigos de barra para todas las \n prendas de la lista con un stock mayor a 0 \n\n ¿Desea proceder? ","Importante", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            { generarCodigoDeBarra(1); }

        }

        private void Dgvprenda_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvprenda.Rows[e.RowIndex].Cells["CrearBarCode"].Selected)
                {//El número 2 que se pasa como parámetro es la opción del método que genera barcodes  de una prenda determinada 
                 //y no de toda la lista 
                    generarCodigoDeBarra(2);
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void Txtprecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                e.Handled = objAc.SoloDecimales(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Txtprecioventa_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                e.Handled = objAc.SoloDecimales(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Txttallanum_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                e.Handled = objAc.SoloNumeros(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Txtcantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                e.Handled = objAc.SoloNumeros(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
