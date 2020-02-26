using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
namespace Presentacion
{
    public class CrearTicket
    {
        StringBuilder linea = new StringBuilder();
        int maxCar = 40, cortar;

        public string LineasGuion()
        {
            string guion = "";

            for (int i = 0; i < maxCar; i++)
            {
                guion += "-";
            }
            return linea.AppendLine(guion).ToString();
        }
        public string LineasAsteriscos()
        {
            string asterisco = "";
            for (int i = 0; i < maxCar; i++)
            {
                asterisco += "*";
            }
            return linea.AppendLine(asterisco).ToString();
        }

        public string LineasIgual()
        {
            string igual = "";
            for (int i = 0; i < maxCar; i++)
            {
                igual += "=";
            }
            return linea.AppendLine(igual).ToString();
        }

        public void EncabezadoTicket()
        {
            linea.AppendLine("ARTÍCULO            |CANT|PRECIO|IMPORTE");

        }
        public void TextoIzquierda(string texto)
        {
            if (texto.Length > maxCar)
            {
                int caracterActual = 0;
                for (int longitudTexto = texto.Length; longitudTexto > maxCar; longitudTexto -= maxCar)
                {
                    linea.AppendLine(texto.Substring(caracterActual, maxCar));
                    caracterActual += maxCar;
                }
                linea.AppendLine(texto.Substring(caracterActual, texto.Length - caracterActual));
            }
            else
            {
                linea.AppendLine(texto);
            }
        }
        public void TextoDerecha(string texto)
        {
            if (texto.Length > maxCar)
            {
                int caracterActual = 0;
                for (int longitudTexto = texto.Length; longitudTexto > maxCar; longitudTexto -= maxCar)
                {
                    linea.AppendLine(texto.Substring(caracterActual, maxCar));
                    caracterActual += maxCar;
                }
                string espacios = "";
                for (int i = 0; i < (maxCar + texto.Substring(caracterActual, texto.Length - caracterActual).Length); i++)
                {
                    espacios += " ";
                }
                linea.AppendLine(espacios + texto.Substring(caracterActual, texto.Length - caracterActual));
            }
            else
            {
                string espacios = "";
                for (int i = 0; i < (maxCar - texto.Length); i++)
                {
                    espacios += " ";
                }
                linea.AppendLine(espacios + texto);
            }
        }
        public void TextoCentro(string texto)
        {
            if (texto.Length > maxCar)
            {
                int caracterActual = 0;
                for (int longitudTexto = texto.Length; longitudTexto > maxCar; longitudTexto -= maxCar)
                {
                    linea.AppendLine(texto.Substring(caracterActual, maxCar));
                    caracterActual += maxCar;
                }
                string espacios = "";
                int centrar = (maxCar - texto.Substring(caracterActual, texto.Length - caracterActual).Length) / 2;

                for (int i = 0; i < centrar; i++)
                {
                    espacios += " ";
                }
                linea.AppendLine(espacios + texto.Substring(caracterActual, texto.Length - caracterActual));
            }
            else
            {
                string espacios = "";
                int centrar = (maxCar - texto.Length) / 2;
                for (int i = 0; i < centrar; i++)
                {
                    espacios += " ";
                }
                linea.AppendLine(espacios + texto);
            }
        }
        public void TextoExtremos(string textoIzquierda, string textoDerecha)
        {
            string textIzq, textDer, textoCompleto = "", espacios = "";
            if (textoIzquierda.Length > 18)
            {
                cortar = textoIzquierda.Length - 18;
                textIzq = textoIzquierda.Remove(18, cortar);
            }
            else
            {
                textIzq = textoIzquierda;
            }
            textoCompleto = textIzq;
            if (textoDerecha.Length > 20)
            {
                cortar = textoDerecha.Length - 20;
                textDer = textoDerecha.Remove(20, cortar);
            }
            else
            {
                textDer = textoDerecha;
            }
            int nroEspacios = maxCar - (textIzq.Length + textDer.Length);
            for (int i = 0; i < nroEspacios; i++)
            {
                espacios += " ";
            }
            textoCompleto += espacios + textoDerecha;
            linea.AppendLine(textoCompleto);
        }
        public void AgregarTotales(string texto, decimal total)
        {
            string resumen, valor, textoCompleto, espacios = "";
            if (texto.Length > 25)
            {
                cortar = texto.Length - 25;
                resumen = texto.Remove(25, cortar);
            }
            else
            {
                resumen = texto;
            }
            textoCompleto = resumen;
            valor = total.ToString("#,#.00");
            int nroEspacios = maxCar - (resumen.Length + valor.Length);
            for (int i = 0; i < nroEspacios; i++)
            {
                espacios += " ";
            }
            textoCompleto += espacios + valor;
            linea.AppendLine(textoCompleto);
        }

        public void AgregarArticulo(string articulo, int cantidad, decimal precio, decimal importe)
        {
            if (cantidad.ToString().Length <= 5 && precio.ToString().Length <= 7 && importe.ToString().Length < 8)
            {
                string elemento = "", espacios = "";
                bool bandera = false;
                int nroEspacios = 0;
                if (articulo.Length > 20)
                {
                    nroEspacios = (5 - cantidad.ToString().Length);
                    espacios = "";
                    for (int i = 0; i < nroEspacios; i++)
                    {
                        espacios += " ";
                    }
                    elemento += espacios + cantidad.ToString();
                    nroEspacios = (7 - precio.ToString().Length);
                    espacios = "";
                    for (int i = 0; i < nroEspacios; i++)
                    {
                        espacios += " ";
                    }
                    elemento += espacios + precio.ToString();
                    nroEspacios = (8 - importe.ToString().Length);
                    espacios = "";
                    for (int i = 0; i < nroEspacios; i++)
                    {
                        espacios += " ";
                    }
                    elemento += espacios + importe.ToString();
                    int caracterActual = 0;
                    for (int longitudTexto = articulo.Length; longitudTexto > 20; longitudTexto -= 20)
                    {
                        if (bandera == false)
                        {
                            linea.AppendLine(articulo.Substring(caracterActual, 20) + elemento);
                        }
                        else
                        {
                            linea.AppendLine(articulo.Substring(caracterActual, 20));
                            caracterActual += 20;
                        }
                    }
                    linea.AppendLine(articulo.Substring(caracterActual, articulo.Length - caracterActual));

                }
                else
                {
                    for (int i = 0; i < (20 - articulo.Length); i++)
                    {
                        espacios += " ";
                    }
                    elemento = articulo + espacios;

                    nroEspacios = (5 - cantidad.ToString().Length);
                    espacios = "";
                    for (int i = 0; i < nroEspacios; i++)
                    {
                        espacios += " ";
                    }
                    elemento += espacios + cantidad.ToString();
                    nroEspacios = (7 - precio.ToString().Length);
                    espacios = "";
                    for (int i = 0; i < nroEspacios; i++)
                    {
                        espacios += " ";
                    }
                    elemento += espacios + precio.ToString();
                    nroEspacios = (8 - importe.ToString().Length);
                    espacios = "";
                    for (int i = 0; i < nroEspacios; i++)
                    {
                        espacios += " ";
                    }
                    elemento += espacios + importe.ToString();
                    linea.AppendLine(elemento);
                }
            }
            else
            {
                linea.AppendLine("los valores ingresados para esta fila");
                linea.AppendLine("superan las columnas soportada por ésta");
                throw new Exception("los valores ingresados para algunas filas \nsuperan las columnas soportadas por ésta");
            }



        }

        public void CortarTicket()
        {
            linea.AppendLine("\x1B" + "m");
            linea.AppendLine("\x1B" + "d" + "\x09");
        }

        public void AbrirCajon()
        {
            linea.AppendLine("\x1B" + "p" + "\x00" + "\x0F" + "\x96");
        }

        public void ImprimirTicket(string impresora)
        {
            RawPrinterHelper.SendStringToPrinter(impresora, linea.ToString());
            linea.Clear();
        }


    }
    public class RawPrinterHelper
    {
        // Structure and API declarions:
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        // SendBytesToPrinter()
        // When the function is given a printer name and an unmanaged array
        // of bytes, the function sends those bytes to the print queue.
        // Returns true on success, false on failure.
        public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
        {
            Int32 dwError = 0, dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // Assume failure unless you specifically succeed.

            di.pDocName = "Ticket de Venta";//Este es el nombre con el que guarda el archivo en caso de no imprimir a la impresora fisica.
            di.pDataType = "RAW";//de tipo texto plano

            // Open the printer.
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                // Start a document.
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    // Start a page.
                    if (StartPagePrinter(hPrinter))
                    {
                        // Write your bytes.
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
            // If you did not succeed, GetLastError may give more information
            // about why not.
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
            }
            return bSuccess;
        }

        public static bool SendStringToPrinter(string szPrinterName, string szString)
        {
            IntPtr pBytes;
            Int32 dwCount;
            // How many characters are in the string?
            dwCount = szString.Length;
            // Assume that the printer is expecting ANSI text, and then convert
            // the string to ANSI text.
            pBytes = Marshal.StringToCoTaskMemAnsi(szString);
            // Send the converted ANSI string to the printer.
            SendBytesToPrinter(szPrinterName, pBytes, dwCount);
            Marshal.FreeCoTaskMem(pBytes);
            return true;
        }
    }
}
