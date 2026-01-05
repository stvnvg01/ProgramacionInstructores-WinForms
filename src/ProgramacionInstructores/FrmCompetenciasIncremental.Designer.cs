namespace ProgramacionInstructores
{
    partial class FrmCompetenciasIncremental
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.BtoBusArc = new System.Windows.Forms.Button();
            this.BtoUpDate = new System.Windows.Forms.Button();
            this.BtoCanSal = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TotDat = new System.Windows.Forms.Label();
            this.TotReg = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.BtoIA = new System.Windows.Forms.Button();
            this.TxtBoxPrograma = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(206, 79);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(147, 22);
            this.textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(206, 123);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(147, 22);
            this.textBox2.TabIndex = 1;
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Location = new System.Drawing.Point(52, 354);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(301, 69);
            this.textBox3.TabIndex = 2;
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(214)))), ((int)(((byte)(167)))));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(398, 79);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(508, 397);
            this.dataGridView1.TabIndex = 3;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton1.ForeColor = System.Drawing.Color.White;
            this.radioButton1.Location = new System.Drawing.Point(208, 207);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(141, 28);
            this.radioButton1.TabIndex = 4;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Punto y Coma";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton2.ForeColor = System.Drawing.Color.White;
            this.radioButton2.Location = new System.Drawing.Point(208, 247);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(76, 28);
            this.radioButton2.TabIndex = 5;
            this.radioButton2.Text = "Coma";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // BtoBusArc
            // 
            this.BtoBusArc.BackgroundImage = global::ProgramacionInstructores.Properties.Resources._1___Nuevo;
            this.BtoBusArc.Font = new System.Drawing.Font("Arial Narrow", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtoBusArc.Location = new System.Drawing.Point(208, 301);
            this.BtoBusArc.Name = "BtoBusArc";
            this.BtoBusArc.Size = new System.Drawing.Size(145, 47);
            this.BtoBusArc.TabIndex = 6;
            this.BtoBusArc.Text = "&Cargar Datos";
            this.BtoBusArc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtoBusArc.UseVisualStyleBackColor = true;
            this.BtoBusArc.Click += new System.EventHandler(this.BtoBusArc_Click);
            // 
            // BtoUpDate
            // 
            this.BtoUpDate.BackgroundImage = global::ProgramacionInstructores.Properties.Resources._0_Aceptar;
            this.BtoUpDate.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtoUpDate.Location = new System.Drawing.Point(52, 429);
            this.BtoUpDate.Name = "BtoUpDate";
            this.BtoUpDate.Size = new System.Drawing.Size(145, 47);
            this.BtoUpDate.TabIndex = 7;
            this.BtoUpDate.Text = "&Actualizar";
            this.BtoUpDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtoUpDate.UseVisualStyleBackColor = true;
            this.BtoUpDate.Click += new System.EventHandler(this.BtoUpDate_Click);
            // 
            // BtoCanSal
            // 
            this.BtoCanSal.BackgroundImage = global::ProgramacionInstructores.Properties.Resources._0_Cancelar;
            this.BtoCanSal.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtoCanSal.Location = new System.Drawing.Point(206, 429);
            this.BtoCanSal.Name = "BtoCanSal";
            this.BtoCanSal.Size = new System.Drawing.Size(145, 47);
            this.BtoCanSal.TabIndex = 8;
            this.BtoCanSal.Text = "&Cancelar";
            this.BtoCanSal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtoCanSal.UseVisualStyleBackColor = true;
            this.BtoCanSal.Click += new System.EventHandler(this.BtoCanSal_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(80, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 22);
            this.label1.TabIndex = 9;
            this.label1.Text = "Base de Datos";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(64, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 24);
            this.label2.TabIndex = 10;
            this.label2.Text = "Tabla a Importar";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(102, 207);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 24);
            this.label3.TabIndex = 11;
            this.label3.Text = "Delimitador";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(80, 310);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 24);
            this.label4.TabIndex = 12;
            this.label4.Text = "Origen / Datos";
            // 
            // TotDat
            // 
            this.TotDat.AutoSize = true;
            this.TotDat.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotDat.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.TotDat.Location = new System.Drawing.Point(403, 49);
            this.TotDat.Name = "TotDat";
            this.TotDat.Size = new System.Drawing.Size(84, 24);
            this.TotDat.TabIndex = 13;
            this.TotDat.Text = "Registros";
            // 
            // TotReg
            // 
            this.TotReg.AutoSize = true;
            this.TotReg.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotReg.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.TotReg.Location = new System.Drawing.Point(750, 49);
            this.TotReg.Name = "TotReg";
            this.TotReg.Size = new System.Drawing.Size(156, 24);
            this.TotReg.TabIndex = 14;
            this.TotReg.Text = "Total de Registros:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.Font = new System.Drawing.Font("Arial Narrow", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(51, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(346, 42);
            this.label5.TabIndex = 15;
            this.label5.Text = "Actualizar Competencias";
            // 
            // BtoIA
            // 
            this.BtoIA.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(214)))), ((int)(((byte)(167)))));
            this.BtoIA.Font = new System.Drawing.Font("Arial Narrow", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtoIA.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.BtoIA.Location = new System.Drawing.Point(917, 429);
            this.BtoIA.Name = "BtoIA";
            this.BtoIA.Size = new System.Drawing.Size(100, 47);
            this.BtoIA.TabIndex = 16;
            this.BtoIA.Text = "Botón IA";
            this.BtoIA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtoIA.UseVisualStyleBackColor = false;
            this.BtoIA.Click += new System.EventHandler(this.BtoIA_Click);
            // 
            // TxtBoxPrograma
            // 
            this.TxtBoxPrograma.Location = new System.Drawing.Point(206, 166);
            this.TxtBoxPrograma.Name = "TxtBoxPrograma";
            this.TxtBoxPrograma.Size = new System.Drawing.Size(147, 22);
            this.TxtBoxPrograma.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(94, 163);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 24);
            this.label6.TabIndex = 18;
            this.label6.Text = "Id Programa";
            // 
            // FrmCompetenciasIncremental
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(94)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(1029, 499);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.TxtBoxPrograma);
            this.Controls.Add(this.BtoIA);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.TotReg);
            this.Controls.Add(this.TotDat);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtoCanSal);
            this.Controls.Add(this.BtoUpDate);
            this.Controls.Add(this.BtoBusArc);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCompetenciasIncremental";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Actualizar Clientes Incremental";
            this.Load += new System.EventHandler(this.FrmCompetenciasIncremental_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Button BtoBusArc;
        private System.Windows.Forms.Button BtoUpDate;
        private System.Windows.Forms.Button BtoCanSal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label TotDat;
        private System.Windows.Forms.Label TotReg;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button BtoIA;
        private System.Windows.Forms.TextBox TxtBoxPrograma;
        private System.Windows.Forms.Label label6;
    }
}