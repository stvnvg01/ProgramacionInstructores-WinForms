namespace ProgramacionInstructores
{
    partial class FrmVarEntorno
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmVarEntorno));
            this.btnCambiarColor = new System.Windows.Forms.Button();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.btnConsultar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCambiarIcono = new System.Windows.Forms.Button();
            this.comboBotones = new System.Windows.Forms.ComboBox();
            this.btnReestablecer = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGuardarCambios = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCambiarColor
            // 
            this.btnCambiarColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(214)))), ((int)(((byte)(167)))));
            this.btnCambiarColor.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCambiarColor.Image = ((System.Drawing.Image)(resources.GetObject("btnCambiarColor.Image")));
            this.btnCambiarColor.Location = new System.Drawing.Point(62, 93);
            this.btnCambiarColor.Name = "btnCambiarColor";
            this.btnCambiarColor.Size = new System.Drawing.Size(194, 94);
            this.btnCambiarColor.TabIndex = 0;
            this.btnCambiarColor.Text = "Cambiar\r\nColor";
            this.btnCambiarColor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCambiarColor.UseVisualStyleBackColor = false;
            this.btnCambiarColor.Click += new System.EventHandler(this.btnCambiarColor_Click);
            // 
            // btnNuevo
            // 
            this.btnNuevo.Image = global::ProgramacionInstructores.Properties.Resources._1___Nuevo;
            this.btnNuevo.Location = new System.Drawing.Point(39, 50);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(194, 94);
            this.btnNuevo.TabIndex = 1;
            this.btnNuevo.Text = "&Nuevo";
            this.btnNuevo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNuevo.UseVisualStyleBackColor = true;
            // 
            // btnEditar
            // 
            this.btnEditar.Image = global::ProgramacionInstructores.Properties.Resources._2___Editar;
            this.btnEditar.Location = new System.Drawing.Point(39, 150);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(194, 94);
            this.btnEditar.TabIndex = 2;
            this.btnEditar.Text = "&Editar";
            this.btnEditar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEditar.UseVisualStyleBackColor = true;
            // 
            // btnGuardar
            // 
            this.btnGuardar.Image = global::ProgramacionInstructores.Properties.Resources._3___Guardar;
            this.btnGuardar.Location = new System.Drawing.Point(39, 351);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(194, 94);
            this.btnGuardar.TabIndex = 3;
            this.btnGuardar.Text = "&Guardar";
            this.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGuardar.UseVisualStyleBackColor = true;
            // 
            // btnEliminar
            // 
            this.btnEliminar.Image = global::ProgramacionInstructores.Properties.Resources._5___Eliminar;
            this.btnEliminar.Location = new System.Drawing.Point(249, 150);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(194, 94);
            this.btnEliminar.TabIndex = 4;
            this.btnEliminar.Text = "&Eliminar";
            this.btnEliminar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEliminar.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(214)))), ((int)(((byte)(167)))));
            this.panel1.Controls.Add(this.btnImprimir);
            this.panel1.Controls.Add(this.btnSalir);
            this.panel1.Controls.Add(this.btnConsultar);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnEditar);
            this.panel1.Controls.Add(this.btnEliminar);
            this.panel1.Controls.Add(this.btnNuevo);
            this.panel1.Controls.Add(this.btnGuardar);
            this.panel1.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(329, 52);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(649, 459);
            this.panel1.TabIndex = 5;
            // 
            // btnImprimir
            // 
            this.btnImprimir.Image = global::ProgramacionInstructores.Properties.Resources._6___Imprimir;
            this.btnImprimir.Location = new System.Drawing.Point(249, 50);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(194, 94);
            this.btnImprimir.TabIndex = 8;
            this.btnImprimir.Text = "&Imprimir";
            this.btnImprimir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImprimir.UseVisualStyleBackColor = true;
            // 
            // btnSalir
            // 
            this.btnSalir.Image = global::ProgramacionInstructores.Properties.Resources._8___Salir;
            this.btnSalir.Location = new System.Drawing.Point(249, 249);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(194, 94);
            this.btnSalir.TabIndex = 7;
            this.btnSalir.Text = "&Salir";
            this.btnSalir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalir.UseVisualStyleBackColor = true;
            // 
            // btnConsultar
            // 
            this.btnConsultar.Image = global::ProgramacionInstructores.Properties.Resources._4___Buscar;
            this.btnConsultar.Location = new System.Drawing.Point(39, 249);
            this.btnConsultar.Name = "btnConsultar";
            this.btnConsultar.Size = new System.Drawing.Size(194, 94);
            this.btnConsultar.TabIndex = 6;
            this.btnConsultar.Text = "&Consultar";
            this.btnConsultar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConsultar.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(76, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 35);
            this.label1.TabIndex = 5;
            this.label1.Text = "Botones";
            // 
            // btnCambiarIcono
            // 
            this.btnCambiarIcono.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(214)))), ((int)(((byte)(167)))));
            this.btnCambiarIcono.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCambiarIcono.Image = ((System.Drawing.Image)(resources.GetObject("btnCambiarIcono.Image")));
            this.btnCambiarIcono.Location = new System.Drawing.Point(62, 193);
            this.btnCambiarIcono.Name = "btnCambiarIcono";
            this.btnCambiarIcono.Size = new System.Drawing.Size(194, 94);
            this.btnCambiarIcono.TabIndex = 6;
            this.btnCambiarIcono.Text = "Cambiar \r\nFondo";
            this.btnCambiarIcono.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCambiarIcono.UseVisualStyleBackColor = false;
            this.btnCambiarIcono.Click += new System.EventHandler(this.btnCambiarIcono_Click);
            // 
            // comboBotones
            // 
            this.comboBotones.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(214)))), ((int)(((byte)(167)))));
            this.comboBotones.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBotones.FormattingEnabled = true;
            this.comboBotones.Location = new System.Drawing.Point(62, 293);
            this.comboBotones.Name = "comboBotones";
            this.comboBotones.Size = new System.Drawing.Size(193, 32);
            this.comboBotones.TabIndex = 7;
            // 
            // btnReestablecer
            // 
            this.btnReestablecer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(214)))), ((int)(((byte)(167)))));
            this.btnReestablecer.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReestablecer.Location = new System.Drawing.Point(63, 331);
            this.btnReestablecer.Name = "btnReestablecer";
            this.btnReestablecer.Size = new System.Drawing.Size(193, 79);
            this.btnReestablecer.TabIndex = 8;
            this.btnReestablecer.Text = "Reestablecer \r\nCambios";
            this.btnReestablecer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReestablecer.UseVisualStyleBackColor = false;
            this.btnReestablecer.Click += new System.EventHandler(this.btnReestablecer_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(295, 40);
            this.label2.TabIndex = 9;
            this.label2.Text = "Personalizar Entorno";
            // 
            // btnGuardarCambios
            // 
            this.btnGuardarCambios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(214)))), ((int)(((byte)(167)))));
            this.btnGuardarCambios.Location = new System.Drawing.Point(63, 416);
            this.btnGuardarCambios.Name = "btnGuardarCambios";
            this.btnGuardarCambios.Size = new System.Drawing.Size(194, 94);
            this.btnGuardarCambios.TabIndex = 9;
            this.btnGuardarCambios.Text = "&Guardar Cambios";
            this.btnGuardarCambios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGuardarCambios.UseVisualStyleBackColor = false;
            this.btnGuardarCambios.Click += new System.EventHandler(this.btnGuardarCambios_Click);
            // 
            // FrmVarEntorno
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(94)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(990, 544);
            this.Controls.Add(this.btnGuardarCambios);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnReestablecer);
            this.Controls.Add(this.comboBotones);
            this.Controls.Add(this.btnCambiarIcono);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnCambiarColor);
            this.Font = new System.Drawing.Font("Arial Narrow", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmVarEntorno";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FrmVarEntorno_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCambiarColor;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCambiarIcono;
        private System.Windows.Forms.ComboBox comboBotones;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnReestablecer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnImprimir;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Button btnConsultar;
        private System.Windows.Forms.Button btnGuardarCambios;
    }
}

