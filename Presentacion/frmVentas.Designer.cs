namespace Presentacion
{
    partial class frmVentas
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvVentas = new System.Windows.Forms.DataGridView();
            this.pickerFecha = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.pickerFecha1 = new System.Windows.Forms.DateTimePicker();
            this.pickerFecha2 = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblItems = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblMonto = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentas)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvVentas
            // 
            this.dgvVentas.AllowUserToAddRows = false;
            this.dgvVentas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVentas.Location = new System.Drawing.Point(51, 75);
            this.dgvVentas.Name = "dgvVentas";
            this.dgvVentas.Size = new System.Drawing.Size(602, 284);
            this.dgvVentas.TabIndex = 0;
            // 
            // pickerFecha
            // 
            this.pickerFecha.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.pickerFecha.Location = new System.Drawing.Point(34, 19);
            this.pickerFecha.Name = "pickerFecha";
            this.pickerFecha.Size = new System.Drawing.Size(129, 20);
            this.pickerFecha.TabIndex = 1;
            this.pickerFecha.ValueChanged += new System.EventHandler(this.PickerFecha_ValueChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(729, 333);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(79, 30);
            this.button1.TabIndex = 2;
            this.button1.Text = "Buscar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // pickerFecha1
            // 
            this.pickerFecha1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.pickerFecha1.Location = new System.Drawing.Point(36, 19);
            this.pickerFecha1.Name = "pickerFecha1";
            this.pickerFecha1.Size = new System.Drawing.Size(129, 20);
            this.pickerFecha1.TabIndex = 3;
            // 
            // pickerFecha2
            // 
            this.pickerFecha2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.pickerFecha2.Location = new System.Drawing.Point(36, 19);
            this.pickerFecha2.Name = "pickerFecha2";
            this.pickerFecha2.Size = new System.Drawing.Size(129, 20);
            this.pickerFecha2.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pickerFecha1);
            this.groupBox1.Location = new System.Drawing.Point(671, 179);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 48);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Fecha Inicio";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pickerFecha2);
            this.groupBox2.Location = new System.Drawing.Point(671, 256);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 48);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Fecha Fin";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblItems);
            this.groupBox3.Location = new System.Drawing.Point(51, 381);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(129, 45);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "N° de Ventas";
            // 
            // lblItems
            // 
            this.lblItems.AutoSize = true;
            this.lblItems.Location = new System.Drawing.Point(18, 20);
            this.lblItems.Name = "lblItems";
            this.lblItems.Size = new System.Drawing.Size(42, 13);
            this.lblItems.TabIndex = 0;
            this.lblItems.Text = "lblItems";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblMonto);
            this.groupBox4.Location = new System.Drawing.Point(207, 381);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(129, 45);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Monto de Ventas";
            // 
            // lblMonto
            // 
            this.lblMonto.AutoSize = true;
            this.lblMonto.Location = new System.Drawing.Point(21, 19);
            this.lblMonto.Name = "lblMonto";
            this.lblMonto.Size = new System.Drawing.Size(35, 13);
            this.lblMonto.TabIndex = 0;
            this.lblMonto.Text = "label1";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txtBuscar);
            this.groupBox5.Location = new System.Drawing.Point(51, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(219, 49);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Buscar por número de comprobante";
            // 
            // txtBuscar
            // 
            this.txtBuscar.Location = new System.Drawing.Point(21, 18);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(171, 20);
            this.txtBuscar.TabIndex = 0;
            this.txtBuscar.TextChanged += new System.EventHandler(this.TxtBuscar_TextChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(537, 381);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(116, 32);
            this.button2.TabIndex = 6;
            this.button2.Text = "Detalle de Venta";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.pickerFecha);
            this.groupBox6.Location = new System.Drawing.Point(671, 75);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(200, 51);
            this.groupBox6.TabIndex = 7;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Fecha única";
            // 
            // frmVentas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 450);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dgvVentas);
            this.Name = "frmVentas";
            this.Text = "Ventas";
            this.Load += new System.EventHandler(this.Ventas_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentas)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvVentas;
        private System.Windows.Forms.DateTimePicker pickerFecha;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DateTimePicker pickerFecha1;
        private System.Windows.Forms.DateTimePicker pickerFecha2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblItems;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label lblMonto;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox6;
    }
}