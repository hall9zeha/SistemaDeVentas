using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp;
//using iTextSharp.text;
using System.IO;
//using iTextSharp.text.pdf;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;

namespace Presentacion
{
    class CodesMethods
    {
        private static readonly CodesMethods _instancia = new CodesMethods();
        public static CodesMethods Instancia
        {
            get { return CodesMethods._instancia; }
        }

        public System.Drawing.Image CodigoQR(string cadena)
        {
            try
            {

                QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                QrCode qrCode = new QrCode();
                qrEncoder.TryEncode(cadena, out qrCode);

                GraphicsRenderer renderer =new GraphicsRenderer(new FixedCodeSize(400, QuietZoneModules.Zero), Brushes.Black, Brushes.White);
                MemoryStream ms = new MemoryStream();
                renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);
                var imageTemporal = new Bitmap(ms);
                var imagen = new Bitmap(imageTemporal, new Size(new Point(100, 100)));
                return imagen;
               
            }
            catch (Exception )
            {
                throw;
            }

        }
       
    }
}
