using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entidades;
using Negocio;
namespace Presentacion
{
    class AccionesEnControles
    {
        private static readonly AccionesEnControles _instancia = new AccionesEnControles();
        public static AccionesEnControles Instancia
        {
            get { return AccionesEnControles._instancia; }

        }
        VentasN objVN = new VentasN();
        #region montoEnLetras
        public string MontoEnLetras(string monto)
        {
            string result, dec = "";
            Int64 entero;
            int decimales;
            double nro;

            try
            {
                nro = Convert.ToDouble(monto);

            }
            catch
            {
                return "";
            }
            entero = Convert.ToInt64(Math.Truncate(nro));
            decimales = Convert.ToInt32(Math.Round((nro - entero) * 100, 2));
            if (decimales > 0)
            {
                dec = " CON " + decimales.ToString() + "/100";
            }
            if (dec == "") dec = " CON 00/100 ";
            result = ConvertInText(Convert.ToDouble(entero)) + dec;
            return result;


        }
        private string ConvertInText(double value)
        {
            string Num2Text = "";
            value = Math.Truncate(value);
            if (value == 0) Num2Text = "CERO";
            else if (value == 1) Num2Text = "UNO";
            else if (value == 2) Num2Text = "DOS";
            else if (value == 3) Num2Text = "TRES";
            else if (value == 4) Num2Text = "CUATRO";
            else if (value == 5) Num2Text = "CINCO";
            else if (value == 6) Num2Text = "SEIS";
            else if (value == 7) Num2Text = "SIETE";
            else if (value == 8) Num2Text = "OCHO";
            else if (value == 9) Num2Text = "NUEVE";
            else if (value == 10) Num2Text = "DIEZ";
            else if (value == 11) Num2Text = "ONCE";
            else if (value == 12) Num2Text = "DOCE";
            else if (value == 13) Num2Text = "TRECE";
            else if (value == 14) Num2Text = "CATORCE";
            else if (value == 15) Num2Text = "QUINCE";
            else if (value < 20) Num2Text = "DIECI" + ConvertInText(value - 10);
            else if (value == 20) Num2Text = "VEINTE";
            else if (value < 30) Num2Text = "VEINTI" + ConvertInText(value - 20);
            else if (value == 30) Num2Text = "TREINTA";
            else if (value == 40) Num2Text = "CUARENTA";
            else if (value == 50) Num2Text = "CINCUENTA";
            else if (value == 60) Num2Text = "SESENTA";
            else if (value == 70) Num2Text = "SETENTA";
            else if (value == 80) Num2Text = "OCHENTA";
            else if (value == 90) Num2Text = "NOVENTA";
            else if (value < 100) Num2Text = ConvertInText(Math.Truncate(value / 10) * 10) + " Y " + ConvertInText(value % 10);
            else if (value == 100) Num2Text = "CIEN";
            else if (value < 200) Num2Text = "CIENTO " + ConvertInText(value - 100);
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = ConvertInText(Math.Truncate(value / 100)) + "CIENTOS";
            else if (value == 500) Num2Text = "QUINIENTOS";
            else if (value == 700) Num2Text = "SETECIENTOS";
            else if (value == 900) Num2Text = "NOVECIENTOS";
            else if (value < 1000) Num2Text = ConvertInText(Math.Truncate(value / 100) * 100) + " " + ConvertInText(value % 100);
            else if (value == 1000) Num2Text = "MIL";
            else if (value < 2000) Num2Text = "MIL " + ConvertInText(value % 1000);
            else if (value < 1000000)
            {
                Num2Text = ConvertInText(Math.Truncate(value / 1000)) + " MIL";
                if ((value % 1000) > 0) Num2Text = Num2Text + " " + ConvertInText(value % 1000);
            }

            else if (value == 1000000) Num2Text = "UN MILLON";
            else if (value < 2000000) Num2Text = "UN MILLON " + ConvertInText(value % 1000000);
            else if (value < 1000000000000)
            {
                Num2Text = ConvertInText(Math.Truncate(value / 1000000)) + " MILLONES ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + ConvertInText(value - Math.Truncate(value / 1000000) * 1000000);
            }

            else if (value == 1000000000000) Num2Text = "UN BILLON";
            else if (value < 2000000000000) Num2Text = "UN BILLON " + ConvertInText(value - Math.Truncate(value / 1000000000000) * 1000000000000);

            else
            {
                Num2Text = ConvertInText(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + ConvertInText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            }
            return Num2Text;

        }
        #endregion montoEnLetras
        //llenando un control combobox que este dentro de un control groupbox recorre los controles
        //e identifica el combobox con el nombre indicado y lo llena con la coleccion que traigamos, sea List o dataTable
        public void LlenarCboMoneda(Control control)
        {
            try
            {
                foreach (Control CboMoneda in control.Controls)
                {
                    if (CboMoneda is ComboBox)
                    {
                        if (CboMoneda.Name == "CboMoneda")
                        {
                            List<MonedaE> lista = objVN.ListarMoneda();
                            ((ComboBox)CboMoneda).ValueMember = "IdMoneda";
                            ((ComboBox)CboMoneda).DisplayMember = "Descripcion";
                            ((ComboBox)CboMoneda).DataSource = lista;

                        }
                    }
                }
            }
            catch (ApplicationException) { throw; }
            catch
            { throw; }
        }
        //En este otro método no recorremos el groupbox en el cual están los controles, solo definimos la condición
        //de existencia del control comboBox y le pasamos el nombre del mismo a nuestro método para que lo cargue
        public void LlenarCboTipoPago(Control control)
        {
            try
            {
                //Control CboTipoPago=null;
                if (control is ComboBox)
                {
                    if (control.Name == "cboTipoPago")
                    {
                        List<TipoPagoE> lista = objVN.ListarTipoPago();
                        ((ComboBox)control).ValueMember = "IdTipoPago";
                        ((ComboBox)control).DisplayMember = "Descripcion";
                        ((ComboBox)control).DataSource = lista;

                    }
                }
            }
            catch (ApplicationException)
            { throw; }
            catch { throw; }
        }
        //Los métodos anteriores llenaban el control ComboBox con una coleccion de datos tipo List<T>, pero ahora lo haremos con un DataTable
        public void LlenarCboTipoDoc(Control control)
        {
            try
            {
                if (control is ComboBox)
                {
                    if (control.Name == "cboTipDoc")
                    {
                        DataTable dt = new DataTable();
                        dt = ClienteN.Instancia.CargarTipoDoc();
                        ((ComboBox)control).ValueMember = "idTipoDoc";
                        ((ComboBox)control).DisplayMember = "AbreviaturaNombre";
                        ((ComboBox)control).DataSource = dt;
                    }
                }
            }
            catch (ApplicationException)
            { throw; }
            catch
            { throw; }
        }
        //Método para validar campos y solo permitir números
        public Boolean SoloNumeros(KeyPressEventArgs e)
        {
            try
            {
               Boolean result;
                string cadena = "1234567890" + (char)5;
                if (cadena.Contains(e.KeyChar) || e.KeyChar == 8)
                {
                    result = false;
                }
                else
                {
                    MessageBox.Show("Solo se permiten números", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    result = true;
                }
                return result;
            }
            catch (Exception )
            {
                throw;
            }
           
        }
        public Boolean SoloDecimales(KeyPressEventArgs e)
        {
            try
            {
                Boolean result;
                string cadena = "1234567890." + (char)5;
                if (cadena.Contains(e.KeyChar) || e.KeyChar == 8)
                {
                    result= false;
                }
                else
                {
                    MessageBox.Show("Solo se permiten numeros enteros o decimales", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    result = true;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
