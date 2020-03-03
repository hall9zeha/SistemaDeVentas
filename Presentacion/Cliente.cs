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
    public partial class Cliente : Form
    {
        ClienteN objN = new ClienteN();
        ClienteE objE = new ClienteE();
        DataTable dt = new DataTable();
        int tipoAccion = 0;
        int IdCliente = 0;
        public Cliente()
        {
            InitializeComponent();
        }

        private void Cliente_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            cargarTipoDoc();
            txtIdCliente.Text = IdCliente.ToString();
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
                tipoAccion = 1;
                if (txtIdCliente.Text != "0") { tipoAccion = 2; objE.idCliente = Convert.ToInt32(txtIdCliente.Text); }
                objE.tipoDocumento = Convert.ToInt32(cboTipDoc.SelectedValue);
                objE.nroDocumento = txtNumDoc.Text;
                objE.nombreCliente = txtNombre.Text;
                objE.apellidoCliente = txtApellido.Text;
                if (rbMasculino.Checked == true) objE.sexoCliente = "M"; else objE.sexoCliente = "F";
                objE.direccionCliente = txtDireccion.Text;
                objE.telefonoCliente = txtCelular.Text;
                objE.correoCliente = txtCorreo.Text;
                objE.fechaRegistro = dtpFechaNac.Value.ToString("yy/MM/dd");
                objN.MantenimientoCliente(objE, tipoAccion);
                MessageBox.Show("Cliente registrado");
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        private void BtnRegresar_Click(object sender, EventArgs e)
        {

        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            //tipoAccion = 1;
            mantenimientoCliente();
        }
    }
}
