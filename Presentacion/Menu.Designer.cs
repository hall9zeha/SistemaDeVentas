namespace Presentacion
{
    partial class Menu
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.inventarioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mantenimientoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.movimientosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.boletaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.boletaToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.consultarVentasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.AutoSize = false;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inventarioToolStripMenuItem,
            this.mantenimientoToolStripMenuItem,
            this.movimientosToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(948, 58);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // inventarioToolStripMenuItem
            // 
            this.inventarioToolStripMenuItem.Name = "inventarioToolStripMenuItem";
            this.inventarioToolStripMenuItem.Size = new System.Drawing.Size(72, 54);
            this.inventarioToolStripMenuItem.Text = "Inventario";
            this.inventarioToolStripMenuItem.Click += new System.EventHandler(this.InventarioToolStripMenuItem_Click);
            // 
            // mantenimientoToolStripMenuItem
            // 
            this.mantenimientoToolStripMenuItem.Name = "mantenimientoToolStripMenuItem";
            this.mantenimientoToolStripMenuItem.Size = new System.Drawing.Size(101, 54);
            this.mantenimientoToolStripMenuItem.Text = "Mantenimiento";
            this.mantenimientoToolStripMenuItem.Click += new System.EventHandler(this.MantenimientoToolStripMenuItem_Click);
            // 
            // movimientosToolStripMenuItem
            // 
            this.movimientosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.boletaToolStripMenuItem,
            this.consultarVentasToolStripMenuItem});
            this.movimientosToolStripMenuItem.Name = "movimientosToolStripMenuItem";
            this.movimientosToolStripMenuItem.Size = new System.Drawing.Size(89, 54);
            this.movimientosToolStripMenuItem.Text = "Movimientos";
            // 
            // boletaToolStripMenuItem
            // 
            this.boletaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.boletaToolStripMenuItem1});
            this.boletaToolStripMenuItem.Name = "boletaToolStripMenuItem";
            this.boletaToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.boletaToolStripMenuItem.Text = "Ventas";
            // 
            // boletaToolStripMenuItem1
            // 
            this.boletaToolStripMenuItem1.Name = "boletaToolStripMenuItem1";
            this.boletaToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.boletaToolStripMenuItem1.Text = "Boleta";
            this.boletaToolStripMenuItem1.Click += new System.EventHandler(this.BoletaToolStripMenuItem1_Click);
            // 
            // consultarVentasToolStripMenuItem
            // 
            this.consultarVentasToolStripMenuItem.Name = "consultarVentasToolStripMenuItem";
            this.consultarVentasToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.consultarVentasToolStripMenuItem.Text = "Consultar Ventas";
            this.consultarVentasToolStripMenuItem.Click += new System.EventHandler(this.ConsultarVentasToolStripMenuItem_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 553);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Menu";
            this.Text = "Menu";
            this.Load += new System.EventHandler(this.Menu_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem inventarioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mantenimientoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem movimientosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem boletaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem boletaToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem consultarVentasToolStripMenuItem;
    }
}