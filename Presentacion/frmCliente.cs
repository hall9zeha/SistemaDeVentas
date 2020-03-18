using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Negocio;
using Entidades;
namespace Presentacion
{
    public partial class frmCliente : Form
    {
        ClienteN objN = new ClienteN();
        ClienteE objE = new ClienteE();
        DataTable dt = new DataTable();
        int _tipoAccion = 0, _idCliente = 0;
        
        public frmCliente(int? idCliente, int? tipoAccion)
        {
            InitializeComponent();
            this._tipoAccion = (int)tipoAccion;
            this._idCliente = (int)idCliente;
        }

        private void Cliente_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            cargarTipoDoc();
            rbMasculino.Checked = true;
            txtIdCliente.Text = _idCliente.ToString();
            traerCliente();
        }
        void cargarTipoDoc()
        {
            dt = objN.CargarTipoDoc();
            cboTipDoc.DataSource = dt;
            cboTipDoc.DisplayMember = "AbreviaturaNombre";
            cboTipDoc.ValueMember = "idTipoDoc";
        }
        void mantenimientoCliente()
        {
            try
            {
                _tipoAccion = 1;
                if (txtIdCliente.Text != "0") { _tipoAccion = 2; objE.IdCliente = Convert.ToInt32(txtIdCliente.Text); }
                objE.TipoDocumento = Convert.ToInt32(cboTipDoc.SelectedValue);
                objE.NroDocumento = txtNumDoc.Text;
                objE.NombreCliente = txtNombre.Text;
                objE.ApellidoCliente = txtApellido.Text;
                if (rbMasculino.Checked == true) objE.SexoCliente = "M"; else objE.SexoCliente = "F";
                objE.DireccionCliente = txtDireccion.Text;
                objE.TelefonoCliente = txtCelular.Text;
                objE.CorreoCliente = txtCorreo.Text;
                objE.FechaRegistro = dtpFechaNac.Value.ToString("yy/MM/dd");
                objN.MantenimientoCliente(objE, _tipoAccion);
                if (_tipoAccion == 1) { MessageBox.Show("Cliente registrado"); }
                else { MessageBox.Show("Registro Modificado"); }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        void traerCliente()
        {
            try
            {
                if (_idCliente != 0)
                {
                    ClienteE c = objN.TraerCliente(_idCliente, 0.ToString());
                    cboTipDoc.Text = c.DescTipDocumento;
                    txtNumDoc.Text = c.NroDocumento;
                    txtNombre.Text = c.NombreCliente;
                    txtApellido.Text = c.ApellidoCliente;
                    txtCelular.Text = c.TelefonoCliente;
                    txtCorreo.Text = c.CorreoCliente;
                    txtDireccion.Text = c.DireccionCliente;
                    dtpFechaNac.Value = Convert.ToDateTime(c.FechaRegistro);
                    if (c.SexoCliente == "M") rbMasculino.Checked = true; else rbFemenino.Checked = true;
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

        }
        private void BtnRegresar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            //tipoAccion = 1;
            mantenimientoCliente();
            this.Dispose();
        }
    }
}
