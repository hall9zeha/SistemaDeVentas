using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void InventarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Inventario_Prendas objInventario_Prendas = new Inventario_Prendas();
                objInventario_Prendas.MdiParent = this;
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm is Inventario_Prendas)
                    {
                        frm.Show();
                        frm.Size = MinimumSize;
                        frm.WindowState = FormWindowState.Normal;
                        return;
                    }
                }
                objInventario_Prendas.Show();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void MantenimientoToolStripMenuItem_Click(object sender, EventArgs e)
        {
          

        }

        private void BoletaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                Boleta_de_Venta objBoleta = new Boleta_de_Venta();
                objBoleta.MdiParent = this;
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm is Boleta_de_Venta)
                    {
                        frm.Show();
                        frm.Size = MinimumSize;
                        frm.WindowState = FormWindowState.Normal;
                        return;
                    }
                }
                objBoleta.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConsultarVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            try
            {
                Ventas objVentas = new Ventas();
                objVentas.MdiParent = this;
                foreach (Form frm in Application.OpenForms)

                {
                    if (frm is Ventas)
                    {
                        frm.Show();
                        frm.Size = MinimumSize;
                        frm.WindowState = FormWindowState.Normal;
                            return;
                    }
                }
                objVentas.Show();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void InventarioToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                Inventario objInventario = new Inventario();
                objInventario.MdiParent = this;
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm is Inventario)
                    {
                        frm.Show();
                        frm.Size = MinimumSize;
                        frm.WindowState = FormWindowState.Normal;
                        return;
                    }
                }
                objInventario.Show();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
    }
}
