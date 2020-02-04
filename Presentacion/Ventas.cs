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

namespace Presentacion
{
    public partial class Ventas : Form

    {

        VentasN objV = new VentasN();
        public Ventas()
        {
            InitializeComponent();
        }

        private void Ventas_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            cargarVentas();   
        }
        void cargarVentas()
        {
            DataTable dt = new DataTable();
            dt= objV.MostrarVentasSimple(pickerFecha.Text);
            dgvVentas.DataSource = dt;
        }
    }
}
