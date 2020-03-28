using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
namespace Presentacion
{
    public class TicketPdf
    {
        public TicketPdf()
        {
            myDocument.AddAuthor("TyroDeveloper");
            myDocument.AddCreator("BARRY ZEA");
            myDocument.AddTitle("Ticket de Venta");
        }
        //metodo para crear ticket con itextSharp
        PdfWriter writer = null;
        PdfContentByte cb = null;
        ArrayList headerLines = new ArrayList();
        ArrayList subHeaderLines = new ArrayList();
        ArrayList items = new ArrayList();
        ArrayList totales = new ArrayList();
        ArrayList footerLines = new ArrayList();
        private string headerImage = "";
        bool _DrawItemHeaders = true;
        int count = 0;
        string cadenaQR = "";
        string path = "";
        string file_name = "";
        int maxChar = 40;
        int maxCharDescription = 20;
        int imageHeight = 0;
        float leftMargin = 0;
        float topMargin = 5;
        static int fontSize = 7;
        static BaseFont bfCourier = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, false);
        static Font font = new Font(bfCourier, 12, Font.ITALIC, BaseColor.BLACK);
        Document myDocument = new
            Document(PageSize.LETTER); //Aqui se ponen todos los objetos
        string line = "";
        #region Properties

        public String Path
        {
            get { return path; }
            set { path = value; }
        }
        public String CadenaQR
        {
            get { return cadenaQR; }
            set { cadenaQR = value; }
        }

        public String FileName
        {
            get { return file_name; }
            set { file_name = value; }
        }

        public String FullFileName
        {
            get { return (String.Format("{0}{1}", path, file_name)); }
        }
        public String HeaderImage
        {
            get { return headerImage; }
            set { if (headerImage != value) headerImage = value; }
        }

        public int MaxChar
        {
            get { return maxChar; }
            set { if (value != maxChar) maxChar = value; }
        }

        public bool DrawItemHeaders
        {
            set { _DrawItemHeaders = value; }
        }

        public int MaxCharDescription
        {
            get { return maxCharDescription; }
            set { if (value != maxCharDescription) maxCharDescription = value; }
        }

        public int FontSize
        {
            get { return fontSize; }
            set { if (value != fontSize) fontSize = value; }
        }

        public Font FontName
        {
            get { return font; }
            set { if (value != font) font = value; }
        }

        #endregion

        public void AddHeaderLine(string line)
        {
            headerLines.Add(line);
        }

        public void AddSubHeaderLine(string line)
        {
            subHeaderLines.Add(line);
        }

        public void AddItem(string cantidad, string item, string price)
        {
            TicketOrderItem newItem = new TicketOrderItem('?');
            items.Add(newItem.GenerateItem(cantidad, item, price));
        }

        public void AddTotal(string name, string price)
        {
            TicketOrderTotal newTotal = new TicketOrderTotal('?');
            totales.Add(newTotal.GenerateTotal(name, price));
        }

        public void AddFooterLine(string line)
        {
            footerLines.Add(line);
        }

        private string AlignRightText(int lenght)
        {
            string espacios = "";
            int spaces = maxChar - lenght;
            for (int x = 0; x < spaces; x++)
                espacios += " ";
            return espacios;
        }

        private string DottedLine()
        {
            string dotted = "";
            for (int x = 0; x < maxChar; x++)
                dotted += "=";
            return dotted;
        }

        public bool Print()
        {
            try
            {
                //aqui para generar el PDF
                writer = PdfWriter.GetInstance(myDocument,
                    new FileStream(path + file_name, FileMode.Create));
                myDocument.Open();
                cb = writer.DirectContent;
                cb.SetFontAndSize(font.BaseFont, fontSize);
                cb.BeginText();
                DrawImage();
                DrawHeader();
                DrawSubHeader();
                DrawItems();
                DrawTotales();
                DrawFooter();
                DrawQR();
                cb.EndText();
                myDocument.Close();
                System.Diagnostics.Process.Start(path + FileName);

                return true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private float YPosition()
        {
            return (myDocument.PageSize.Height -
                (topMargin + (count * font.CalculatedSize + imageHeight)));
        }

        private void DrawImage()
        {
            try
            {
                if ((headerImage != null) && (headerImage != ""))
                {
                    if (File.Exists(headerImage))
                    {
                        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(headerImage);
                        double height = ((double)logo.Height / 58) * 15;
                        imageHeight = (int)Math.Round(height) + 4;
                        logo.SetAbsolutePosition(75,
                            myDocument.PageSize.Height - imageHeight);
                        logo.ScaleToFit(logo.Width, imageHeight);
                        myDocument.Add(logo);
                    }
                }
            }
            catch (Exception ex) { throw (ex); }
        }
        public void DrawQR()
        {
            iTextSharp.text.pdf.BarcodeQRCode qrcode = new BarcodeQRCode(cadenaQR, 100, 100, null);
            iTextSharp.text.Image imgQR = qrcode.GetImage();
            double height = ((double)imgQR.Height / 20) * 15;
            imageHeight = (int)Math.Round(height) + 3;
            imgQR.SetAbsolutePosition(40,
                            myDocument.PageSize.Height - (myDocument.PageSize.Height - imageHeight));
            //imgQR.SetAbsolutePosition(40,
            //   120);1
            //imgQR.ScaleToFit(imgQR.Width, imageHeight);

            myDocument.Add(imgQR);
        }
        private void DrawHeader()
        {
            try
            {
                foreach (string header in headerLines)
                {
                    if (header.Length > maxChar)
                    {
                        int currentChar = 0;
                        int headerLenght = header.Length;
                        while (headerLenght > maxChar)
                        {
                            line = header.Substring(currentChar, maxChar);
                            cb.SetTextMatrix(leftMargin, YPosition());
                            cb.ShowText(line);
                            count++;
                            currentChar += maxChar;
                            headerLenght -= maxChar;
                        }
                        line = header;
                        cb.SetTextMatrix(leftMargin, YPosition());
                        cb.ShowText(line.Substring(currentChar,
                            line.Length - currentChar));
                        count++;
                    }
                    else
                    {
                        line = header;
                        cb.SetTextMatrix(leftMargin, YPosition());
                        cb.ShowText(line);
                        count++;
                    }
                }
                DrawEspacio();
            }
            catch (Exception ex) { throw (ex); }
        }

        private void DrawSubHeader()
        {
            try
            {
                line = DottedLine();
                cb.SetTextMatrix(leftMargin, YPosition());
                cb.ShowText(line);
                DrawEspacio();
                foreach (string subHeader in subHeaderLines)
                {
                    if (subHeader.Length > maxChar)
                    {
                        int currentChar = 0;
                        int subHeaderLenght = subHeader.Length;
                        while (subHeaderLenght > maxChar)
                        {
                            line = subHeader;
                            cb.SetTextMatrix(leftMargin, YPosition());
                            cb.ShowText(line.Substring(currentChar, maxChar));
                            count++;
                            currentChar += maxChar;
                            subHeaderLenght -= maxChar;
                        }
                        line = subHeader;
                        cb.SetTextMatrix(leftMargin, YPosition());
                        cb.ShowText(line.Substring(currentChar,
                            line.Length - currentChar));
                        count++;
                        line = DottedLine();
                        cb.SetTextMatrix(leftMargin, YPosition());
                        cb.ShowText(line);
                        DrawEspacio();
                    }
                    else
                    {
                        line = subHeader;
                        cb.SetTextMatrix(leftMargin, YPosition());
                        cb.ShowText(line);
                        count++;
                        line = DottedLine();
                        cb.SetTextMatrix(leftMargin, YPosition());
                        cb.ShowText(line);
                        count++;
                    }
                }
                DrawEspacio();
            }
            catch (Exception ex) { throw (ex); }
        }

        private void DrawItems()
        {
            TicketOrderItem ordIt = new TicketOrderItem('?');
            if (_DrawItemHeaders)
            {
                cb.SetTextMatrix(leftMargin, YPosition());
                cb.ShowText("ARTÍCULO            |CANT|PRECIO|IMPORTE");
            }
            count++;
            DrawEspacio();
            foreach (string item in items)
            {
                line = ordIt.GetItemCantidad(item);
                cb.SetTextMatrix(leftMargin, YPosition());
                cb.ShowText(line);
                line = ordIt.GetItemPrice(item);
                line = AlignRightText(line.Length) + line;
                cb.SetTextMatrix(leftMargin, YPosition());
                cb.ShowText(line);
                string name = ordIt.GetItemName(item);
                leftMargin = 0;
                if (name.Length > maxCharDescription)
                {
                    int currentChar = 0;
                    int itemLenght = name.Length;
                    while (itemLenght > maxCharDescription)
                    {
                        line = ordIt.GetItemName(item);
                        cb.SetTextMatrix(leftMargin, YPosition());
                        cb.ShowText("      " + line.Substring(currentChar,
                            maxCharDescription));
                        count++;
                        currentChar += maxCharDescription;
                        itemLenght -= maxCharDescription;
                    }
                    line = ordIt.GetItemName(item);
                    cb.SetTextMatrix(leftMargin, YPosition());
                    cb.ShowText("      " + line.Substring(currentChar,
                        maxCharDescription));
                    count++;
                }
                else
                {
                    cb.SetTextMatrix(leftMargin, YPosition());
                    cb.ShowText("      " + ordIt.GetItemName(item));
                    count++;
                }
            }

            leftMargin = 0;
            DrawEspacio();
            line = DottedLine();
            cb.SetTextMatrix(leftMargin, YPosition());
            cb.ShowText(line);
            count++;
            DrawEspacio();
        }

        private void DrawTotales()
        {
            TicketOrderTotal ordTot = new TicketOrderTotal('?');
            foreach (string total in totales)
            {
                line = ordTot.GetTotalCantidad(total);
                line = AlignRightText(line.Length) + line;
                cb.SetTextMatrix(leftMargin, YPosition());
                cb.ShowText(line);
                leftMargin = 0;
                line = "" + ordTot.GetTotalName(total);
                cb.SetTextMatrix(leftMargin, YPosition());
                cb.ShowText(line);
                count++;
            }
            leftMargin = 0;
            DrawEspacio();
            DrawEspacio();
        }

        private void DrawFooter()
        {
            foreach (string footer in footerLines)
            {
                if (footer.Length > maxChar)
                {
                    int currentChar = 0;
                    int footerLenght = footer.Length;
                    while (footerLenght > maxChar)
                    {
                        line = footer;
                        cb.SetTextMatrix(leftMargin, YPosition());
                        cb.ShowText(line.Substring(currentChar, maxChar));
                        count++;
                        currentChar += maxChar;
                        footerLenght -= maxChar;
                    }
                    line = footer;
                    cb.SetTextMatrix(leftMargin, YPosition());
                    cb.ShowText(line.Substring(currentChar, maxChar));
                    count++;
                }
                else
                {
                    line = footer;
                    cb.SetTextMatrix(leftMargin, YPosition());
                    cb.ShowText(line);
                    count++;
                }
            }
            leftMargin = 0;
            DrawEspacio();
        }

        private void DrawEspacio()
        {
            line = "";
            cb.SetTextMatrix(leftMargin, YPosition());
            cb.SetFontAndSize(font.BaseFont, fontSize);
            cb.ShowText(line);
            count++;
        }

       
    }

    public class TicketOrderItem
    {
        char[] delimitador = new char[] { '?' };
        public TicketOrderItem(char delimit)
        {
            delimitador = new char[] { delimit };
        }

        public string GetItemCantidad(string TicketOrderItem)
        {
            string[] delimitado = TicketOrderItem.Split(delimitador);
            return delimitado[0];
        }

        public string GetItemName(string TicketOrderItem)
        {
            string[] delimitado = TicketOrderItem.Split(delimitador);
            return delimitado[1];
        }

        public string GetItemPrice(string TicketOrderItem)
        {
            string[] delimitado = TicketOrderItem.Split(delimitador);
            return delimitado[2];
        }

        public string GenerateItem(string cantidad,
            string itemName, string price)
        {
            return cantidad + delimitador[0] +
                itemName + delimitador[0] + price;
        }
    }

    public class TicketOrderTotal
    {
        char[] delimitador = new char[] { '?' };
        public TicketOrderTotal(char delimit)
        {
            delimitador = new char[] { delimit };
        }

        public string GetTotalName(string totalItem)
        {
            string[] delimitado = totalItem.Split(delimitador);
            return delimitado[0];
        }

        public string GetTotalCantidad(string totalItem)
        {
            string[] delimitado = totalItem.Split(delimitador);
            return delimitado[1];
        }

        public string GenerateTotal(string totalName,
            string price)
        {
            return totalName + delimitador[0] + price;
        }
        //metodo para crear ticket con itextSharp
    }
}
