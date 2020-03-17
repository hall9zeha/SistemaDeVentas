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
    public partial class frmMenu : Form
    {
        public frmMenu()
        {
            InitializeComponent();
        }

        private void InventarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmInventario_Prendas objInventario_Prendas = new frmInventario_Prendas();
                objInventario_Prendas.MdiParent = this;
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm is frmInventario_Prendas)
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
                frmVentas objVentas = new frmVentas();
                objVentas.MdiParent = this;
                foreach (Form frm in Application.OpenForms)

                {
                    if (frm is frmVentas)
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
                frmInventario objInventario = new frmInventario();
                objInventario.MdiParent = this;
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm is frmInventario)
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

        private void CambioDePrendaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmCambio_de_Prenda objCambios = new frmCambio_de_Prenda();
                objCambios.MdiParent = this;
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm is frmCambio_de_Prenda)
                    {
                        frm.Show();
                        frm.Size = MinimumSize;
                        frm.WindowState = FormWindowState.Normal;
                        return;
                    }
                }
                objCambios.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FacturaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Factura_Venta objFactura = new Factura_Venta();
                objFactura.MdiParent = this;
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm is Factura_Venta)
                    {
                        frm.Show();
                        frm.Size = MinimumSize;
                        frm.WindowState = FormWindowState.Normal;
                        return;
                    }
                }
                objFactura.Show();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void ClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
                frmMantenimiento_Cliente objCliente = new frmMantenimiento_Cliente();
                objCliente.MdiParent = this;

                foreach (Form frm in Application.OpenForms)
                {
                    if (frm is frmMantenimiento_Cliente)
                    {
                        frm.Show();
                        frm.Size = MinimumSize;
                        frm.WindowState = FormWindowState.Normal;
                        return;
                    }
                }
                objCliente.Show();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void ClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void NotaDeVentaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmNota_De_Venta objNotaVenta = new frmNota_De_Venta();
                objNotaVenta.MdiParent = this;
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm is frmNota_De_Venta)
                    {
                        frm.Show();
                        frm.Size = MinimumSize;
                        frm.WindowState = FormWindowState.Normal;
                        return;
                    }
                }
                objNotaVenta.Show();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
    }
}
