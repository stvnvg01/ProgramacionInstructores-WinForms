namespace ProgramacionInstructores
{
    partial class FrmHCopiaS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmHCopiaS));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.BtoDestino = new System.Windows.Forms.Button();
            this.BtoAceptar = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.BtoCancelar = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(20, 29);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(361, 371);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(387, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Base de Datos:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(387, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "Ruta Destino:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(387, 308);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 24);
            this.label3.TabIndex = 3;
            this.label3.Text = "Nombre BackUp:";
            // 
            // BtoDestino
            // 
            this.BtoDestino.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtoDestino.BackgroundImage")));
            this.BtoDestino.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtoDestino.Location = new System.Drawing.Point(584, 83);
            this.BtoDestino.Name = "BtoDestino";
            this.BtoDestino.Size = new System.Drawing.Size(145, 47);
            this.BtoDestino.TabIndex = 4;
            this.BtoDestino.Text = "&Carpeta";
            this.BtoDestino.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtoDestino.UseVisualStyleBackColor = true;
            this.BtoDestino.Click += new System.EventHandler(this.BtoDestino_Click);
            // 
            // BtoAceptar
            // 
            this.BtoAceptar.BackgroundImage = global::ProgramacionInstructores.Properties.Resources._0_Aceptar;
            this.BtoAceptar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtoAceptar.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtoAceptar.Location = new System.Drawing.Point(387, 353);
            this.BtoAceptar.Name = "BtoAceptar";
            this.BtoAceptar.Size = new System.Drawing.Size(145, 47);
            this.BtoAceptar.TabIndex = 5;
            this.BtoAceptar.Text = "&Aceptar";
            this.BtoAceptar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtoAceptar.UseVisualStyleBackColor = true;
            this.BtoAceptar.Click += new System.EventHandler(this.BtoAceptar_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(584, 36);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(145, 30);
            this.textBox1.TabIndex = 6;
            // 
            // BtoCancelar
            // 
            this.BtoCancelar.BackgroundImage = global::ProgramacionInstructores.Properties.Resources._0_Cancelar;
            this.BtoCancelar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtoCancelar.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtoCancelar.Location = new System.Drawing.Point(584, 353);
            this.BtoCancelar.Name = "BtoCancelar";
            this.BtoCancelar.Size = new System.Drawing.Size(145, 47);
            this.BtoCancelar.TabIndex = 7;
            this.BtoCancelar.Text = "&Cancelar";
            this.BtoCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtoCancelar.UseVisualStyleBackColor = true;
            this.BtoCancelar.Click += new System.EventHandler(this.BtoCancelar_Click);
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(391, 136);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(338, 160);
            this.textBox2.TabIndex = 8;
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(584, 302);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(145, 30);
            this.textBox3.TabIndex = 9;
            // 
            // FrmHCopiaS
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(94)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(753, 423);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.BtoCancelar);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.BtoAceptar);
            this.Controls.Add(this.BtoDestino);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmHCopiaS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Administración de Copias de Seguridad";
            this.Load += new System.EventHandler(this.FrmHCopiaS_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button BtoDestino;
        private System.Windows.Forms.Button BtoAceptar;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button BtoCancelar;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
    }
}