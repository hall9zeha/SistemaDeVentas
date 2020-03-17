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
                if (txtIdCliente.Text != "0") { _tipoAccion = 2; objE.idCliente = Convert.ToInt32(txtIdCliente.Text); }
                objE.tipoDocumento = Convert.ToInt32(cboTipDoc.SelectedValue);
                objE.nroDocumento = txtNumDoc.Text;
                objE.nombreCliente = txtNombre.Text;
                objE.apellidoCliente = txtApellido.Text;
                if (rbMasculino.Checked == true) objE.sexoCliente = "M"; else objE.sexoCliente = "F";
                objE.direccionCliente = txtDireccion.Text;
                objE.telefonoCliente = txtCelular.Text;
                objE.correoCliente = txtCorreo.Text;
                objE.fechaRegistro = dtpFechaNac.Value.ToString("yy/MM/dd");
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
                    cboTipDoc.Text = c.descTipDocumento;
                    txtNumDoc.Text = c.nroDocumento;
                    txtNombre.Text = c.nombreCliente;
                    txtApellido.Text = c.apellidoCliente;
                    txtCelular.Text = c.telefonoCliente;
                    txtCorreo.Text = c.correoCliente;
                    txtDireccion.Text = c.direccionCliente;
                    dtpFechaNac.Value = Convert.ToDateTime(c.fechaRegistro);
                    if (c.sexoCliente == "M") rbMasculino.Checked = true; else rbFemenino.Checked = true;
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
