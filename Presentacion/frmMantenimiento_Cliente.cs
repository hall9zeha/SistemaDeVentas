using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entidades;
using Negocio;

namespace Presentacion
{
    public partial class frmMantenimiento_Cliente : Form
    {
        ClienteN objCliN = new ClienteN();
        ClienteE objE = new ClienteE();
        int tipoBusqueda = 0, tipoAccion = 0;

        
        public frmMantenimiento_Cliente()
        {
            InitializeComponent();
        }
        void crearGrid()
        {
            dgvClientes.Columns.Add("Id", "Id");
            dgvClientes.Columns.Add("Tip/Doc", "Tip/Doc");
            dgvClientes.Columns.Add("Nro/Doc", "Nro/Doc");
            dgvClientes.Columns.Add("Nombres", "Nombres");
            dgvClientes.Columns.Add("Apellidos", "Apellidos");
            dgvClientes.Columns.Add("Sexo", "Sexo");
            dgvClientes.Columns.Add("Dirección", "dirección");
            dgvClientes.Columns.Add("Teléfono", "Teléfono");
            dgvClientes.Columns.Add("Email", "Email");
            dgvClientes.Columns.Add("FechaRegistro", "FechaRegistro");

            DataGridViewCellStyle css = new DataGridViewCellStyle();
            css.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvClientes.DefaultCellStyle = css;

            dgvClientes.AllowUserToAddRows = false;
            dgvClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.AllowUserToResizeColumns = false;

        }
        void habilitarBotones(bool nuevo, bool editar, bool eliminar)
        {
            btnNuevo.Enabled = nuevo;
            btnEditar.Enabled = editar;
            btnEliminar.Enabled = eliminar;
        }

        private void Mantenimiento_Cliente_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            crearGrid();
            habilitarBotones(true, false, false);
        }
        void buscarCliente(string filtro)
        {
            try
            {
                if (filtro != "")
                {
                    int num = 0;
                    List<ClienteE> lista = objCliN.BuscarCliente(tipoBusqueda, filtro);
                    dgvClientes.Rows.Clear();
                    for (int i = 0; i < lista.Count; i++)
                    {
                        num++;
                        String[] fila = new string[] {
                        lista[i].idCliente.ToString(),
                        lista[i].descTipDocumento,
                        lista[i].nroDocumento,
                        lista[i].nombreCliente,
                        lista[i].apellidoCliente,
                        lista[i].sexoCliente,
                        lista[i].direccionCliente,
                        lista[i].telefonoCliente,
                        lista[i].correoCliente,
                        lista[i].fechaRegistro,

                    };
                        dgvClientes.Rows.Add(fila);
                    }
                }
                else
                {
                    dgvClientes.Rows.Clear();
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        void listarCliente()
        {
            try
            {
                
                    int num = 0;
                    List<ClienteE> lista = objCliN.ListarCliente();
                    dgvClientes.Rows.Clear();
                    for (int i = 0; i < lista.Count; i++)
                    {
                        num++;
                    String[] fila = new string[] {
                        lista[i].idCliente.ToString(),
                        num.ToString(),
                        lista[i].descTipDocumento,
                        lista[i].nroDocumento,
                        lista[i].nombreCliente,
                        lista[i].apellidoCliente,
                        lista[i].sexoCliente,
                        lista[i].direccionCliente,
                        lista[i].telefonoCliente,
                        lista[i].correoCliente,
                        lista[i].fechaRegistro,

                    };
                        dgvClientes.Rows.Add(fila);
                    }
                
             
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        private void TxtBuscar1_TextChanged(object sender, EventArgs e)
        {
            tipoBusqueda = 1;
            buscarCliente(txtBuscar1.Text);
        }

        private void TxtBuscar2_TextChanged(object sender, EventArgs e)
        {
            tipoBusqueda = 2;
            buscarCliente(txtBuscar2.Text);
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            try
            {
               int  idCliente = Convert.ToInt32(dgvClientes.CurrentRow.Cells[0].Value);
                if (idCliente.ToString() =="" || idCliente.ToString() == null)
                {
                   MessageBox.Show("Debe Seleccionar un Cliente", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                frmCliente objCliente = new frmCliente(idCliente, tipoAccion);
                objCliente.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            int idCliente = Convert.ToInt32(dgvClientes.CurrentRow.Cells[0].Value);
            DialogResult dr = MessageBox.Show("Realmente quiere eliminar el cliente", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                objE.idCliente = idCliente;
                objCliN.MantenimientoCliente(objE,3);
                MessageBox.Show("Cliente eliminado");
                
            }

        }

        private void BtnListar_Click(object sender, EventArgs e)
        {

            try
            {
                if (dgvClientes.RowCount > 0)
                {
                    int idCli = Convert.ToInt32(dgvClientes.CurrentRow.Cells[0].Value);
                    listarCliente();
                    foreach (DataGridViewRow row in dgvClientes.Rows)
                    {
                        if (Convert.ToInt32(row.Cells[0].Value) == idCli)
                        {
                            row.Selected = true;
                            return;
                        }
                    }
                }
                else listarCliente();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void DgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            habilitarBotones(true, true, true);
        }

        private void BtnVender_Click(object sender, EventArgs e)
        {
            try
            {
                int idCli = Convert.ToInt32(dgvClientes.CurrentRow.Cells[0].Value);
                
                int frmInvocador = LocalBD.Instancia.Invocador(0,0);
                if (frmInvocador == 1)
                {
                    LocalBD.Instancia.ReturnIdCliente(1, idCli);
                }
                else if (frmInvocador == 2)
                {
                    LocalBD.Instancia.ReturnIdClienteFact(1, idCli);
                }
                else
                {
                    LocalBD.Instancia.Invocador(1, 0);
                }

                this.Close();
                //El invocador para saber en que form si boleta o factura listar el cliente lo haremos desupes
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            tipoAccion = 1;
            frmCliente objCliente = new frmCliente(0,tipoAccion);
            objCliente.ShowDialog();
        }

    }
}
