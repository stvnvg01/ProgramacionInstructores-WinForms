namespace ProgramacionInstructores
{
    partial class FrmRestaurarCS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRestaurarCS));
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.BtoCancelar = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.BtoRestaurar = new System.Windows.Forms.Button();
            this.BtoBuscar = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(392, 141);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(338, 160);
            this.textBox2.TabIndex = 18;
            // 
            // BtoCancelar
            // 
            this.BtoCancelar.BackgroundImage = global::ProgramacionInstructores.Properties.Resources._0_Cancelar;
            this.BtoCancelar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtoCancelar.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtoCancelar.Location = new System.Drawing.Point(585, 335);
            this.BtoCancelar.Name = "BtoCancelar";
            this.BtoCancelar.Size = new System.Drawing.Size(145, 50);
            this.BtoCancelar.TabIndex = 17;
            this.BtoCancelar.Text = "&Cancelar";
            this.BtoCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtoCancelar.UseVisualStyleBackColor = true;
            this.BtoCancelar.Click += new System.EventHandler(this.BtoCancelar_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(585, 18);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(145, 30);
            this.textBox1.TabIndex = 16;
            // 
            // BtoRestaurar
            // 
            this.BtoRestaurar.BackgroundImage = global::ProgramacionInstructores.Properties.Resources._0_Aceptar;
            this.BtoRestaurar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtoRestaurar.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtoRestaurar.Location = new System.Drawing.Point(388, 335);
            this.BtoRestaurar.Name = "BtoRestaurar";
            this.BtoRestaurar.Size = new System.Drawing.Size(145, 50);
            this.BtoRestaurar.TabIndex = 15;
            this.BtoRestaurar.Text = "&Restaurar";
            this.BtoRestaurar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtoRestaurar.UseVisualStyleBackColor = true;
            this.BtoRestaurar.Click += new System.EventHandler(this.BtoRestaurar_Click);
            // 
            // BtoBuscar
            // 
            this.BtoBuscar.BackgroundImage = global::ProgramacionInstructores.Properties.Resources._4___Buscar;
            this.BtoBuscar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtoBuscar.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtoBuscar.Location = new System.Drawing.Point(585, 65);
            this.BtoBuscar.Name = "BtoBuscar";
            this.BtoBuscar.Size = new System.Drawing.Size(145, 47);
            this.BtoBuscar.TabIndex = 14;
            this.BtoBuscar.Text = "&Buscar";
            this.BtoBuscar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtoBuscar.UseVisualStyleBackColor = true;
            this.BtoBuscar.Click += new System.EventHandler(this.BtoBuscar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(388, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 24);
            this.label2.TabIndex = 12;
            this.label2.Text = "Seleccione Archivo:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(388, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 24);
            this.label1.TabIndex = 11;
            this.label1.Text = "Base de Datos:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(21, 11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(361, 371);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // FrmRestaurarCS
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(94)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(751, 393);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.BtoCancelar);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.BtoRestaurar);
            this.Controls.Add(this.BtoBuscar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmRestaurarCS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Restaurar Copias de Seguridad..!";
            this.Load += new System.EventHandler(this.FrmRestaurarCS_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button BtoCancelar;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button BtoRestaurar;
        private System.Windows.Forms.Button BtoBuscar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}