﻿using System;
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
using System.IO;
namespace Presentacion
{
    public partial class frmInventario : Form
    {
        InventarioE objE = new InventarioE();
        InventarioN objN = new InventarioN();
        DataTable dt = new DataTable();
        Image img = null;
        int tipoBusqueda = 0;
        public frmInventario()
        {
            InitializeComponent();
        }
        public void CrearGrid()
        {
            dgvInventario.Columns.Add("Código", "Codproducto");
            dgvInventario.Columns.Add("Descripción", "Descripción");
            dgvInventario.Columns.Add("Marca", "Marca");
            dgvInventario.Columns.Add("PrecioCompra", "Precio");
            dgvInventario.Columns.Add("PrecioVenta", "PrecioVenta");
           
            dgvInventario.Columns.Add("Stock", "Stock");

            DataGridViewImageColumn imagen = new DataGridViewImageColumn();
            imagen.HeaderText = "Estado Stock";
            imagen.HeaderText = "Estado Stock";
            dgvInventario.Columns.Add(imagen);

            dgvInventario.Columns[0].Visible = true;
            dgvInventario.Columns[1].Width = 170;
            dgvInventario.Columns[2].Width = 150;
            dgvInventario.Columns[3].Width = 70;
            dgvInventario.Columns[4].Width = 100;
            
            dgvInventario.Columns[5].Width = 60;
            dgvInventario.Columns[6].Width = 90;

            DataGridViewCellStyle cssCabecera = new DataGridViewCellStyle();
            cssCabecera.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvInventario.ColumnHeadersDefaultCellStyle = cssCabecera;

            dgvInventario.AllowUserToAddRows = false;
            dgvInventario.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInventario.MultiSelect = false;

        }

        //void listarInventario()
        //{
        //    dt = objN.ListarInventario();
        //    dgvInventario.DataSource = dt;

        //}

        private void Inventario_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            CrearGrid();
            cargarInventario();
            


        }

        private void Button4_Click(object sender, EventArgs e)
        {
            //dgvInventario.Rows.Clear();
            cargarInventario();
        }
        void buscarProducto(string filtro)
        {
            try
            {
                if (filtro != string.Empty)
                {
                    int num = 0;
                    List<InventarioE> lista = InventarioN.Instancia.MostrarInventario(tipoBusqueda, filtro);
                    dgvInventario.Rows.Clear();
                    for (int i = 0; i < lista.Count; i++)
                    {
                        if (lista[i].Stock >= 10)
                        { img = Properties.Resources.circulo_verde24x24; }
                        if (lista[i].Stock < 10 && lista[i].Stock >= 1)
                        { img = Properties.Resources.CirculoNaranja24x24; }
                        if (lista[i].Stock == 0)
                        { img = Properties.Resources.circulorojo_24x24; }
                        num++;
                        string[] fila = new string[] {
                            lista[i].Codproducto,
                            lista[i].Descripción,
                            lista[i].Marca,
                            lista[i].Precio.ToString("#.00"),
                            lista[i].PrecioVenta.ToString("#.00"),
                           
                            lista[i].Stock.ToString()

                        };
                        dgvInventario.Rows.Add(fila);
                        dgvInventario.Rows[i].Cells[6].Value = img;
                    }
                }
                else if (filtro == string.Empty)
                { cargarInventario(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        void cargarInventario()
        {
            try
            {

                {
                   
                    int num = 0;
                    List<InventarioE> lista = InventarioN.Instancia.ListarInventarioGeneric();
                    dgvInventario.Rows.Clear();
                    for (int i = 0; i < lista.Count; i++)
                    {
                        if (lista[i].Stock >= 10)
                        { img = Properties.Resources.circulo_verde24x24; }
                        if (lista[i].Stock < 10 && lista[i].Stock >= 1)
                        { img = Properties.Resources.CirculoNaranja24x24; }
                        if (lista[i].Stock == 0)
                        { img = Properties.Resources.circulorojo_24x24; }
                        num++;
                        string[] fila = new string[] {
                            lista[i].Codproducto,
                            lista[i].Descripción,
                            lista[i].Marca,
                            lista[i].Precio.ToString("0.00"),
                            lista[i].PrecioVenta.ToString("0.00"),
                            
                            lista[i].Stock.ToString()
                        };
                        dgvInventario.Rows.Add(fila);
                        dgvInventario.Rows[i].Cells[6].Value = img;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        void eliminarPrenda()
        {
            try
            {
                string codProd = Convert.ToString(dgvInventario.CurrentRow.Cells[0].Value);
                objE.Codproducto = codProd;
                objN.EliminarPrenda(objE);
                MessageBox.Show("Prenda eliminada");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            dgvInventario.Rows.Clear();
            tipoBusqueda = 2;
        }

        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            dgvInventario.Rows.Clear();
            tipoBusqueda = 1;
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            dgvInventario.Rows.Clear();
            tipoBusqueda = 3;
        }

        private void Txtfiltro_TextChanged(object sender, EventArgs e)
        {
            buscarProducto(txtfiltro.Text);
        }

        private void DgvInventario_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //Agregar_Prenda Det = new Agregar_Prenda();
            //Det.ShowDialog();
          
                frmAgregar_Prenda Prendas = new frmAgregar_Prenda();

                //add handler to catch when child form is closed    
            Prendas.FormClosed += new FormClosedEventHandler(Prendas_Closed);
            Prendas.ShowDialog();
            

        }


        private void Prendas_Closed(object sender, FormClosedEventArgs e)
        {
            cargarInventario();

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            string Id = Convert.ToString(dgvInventario.CurrentRow.Cells[0].Value);
            frmEditar_Prenda editarPrenda = new frmEditar_Prenda(Id);
            editarPrenda.ShowDialog();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            frmBuscar_Prenda bprenda = new frmBuscar_Prenda(0);
            bprenda.ShowDialog();
        }

        private void DgvInventario_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                var row = (sender as DataGridView).CurrentRow;
                lblcod.Text = row.Cells[0].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button5_Click_1(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Si este producto no tiene ventas asociadas se eliminará\n\n ¿Quiere Proceder?", "Importante", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                eliminarPrenda();
                cargarInventario();
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
