namespace ProgramacionInstructores
{
    partial class FrmEstadoUser
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEstadoUser));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtoSal = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.BtoEdi = new System.Windows.Forms.Button();
            this.BtoCon = new System.Windows.Forms.Button();
            this.BtoGua = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.Ant = new System.Windows.Forms.Button();
            this.Pri = new System.Windows.Forms.Button();
            this.Sig = new System.Windows.Forms.Button();
            this.Ult = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(306, 55);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.MaxLength = 15;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(145, 30);
            this.textBox1.TabIndex = 0;
            this.textBox1.Tag = "IdUsuario";
            this.textBox1.Text = "1";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            this.textBox1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.textBox1_PreviewKeyDown);
            this.textBox1.Validated += new System.EventHandler(this.textBox1_Validated);
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(559, 55);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4);
            this.textBox2.MaxLength = 15;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(145, 30);
            this.textBox2.TabIndex = 1;
            this.textBox2.Tag = "Documento";
            this.textBox2.Text = "2";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(306, 92);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(398, 30);
            this.textBox3.TabIndex = 2;
            this.textBox3.Tag = "Nombres";
            this.textBox3.Text = "3";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(214)))), ((int)(((byte)(167)))));
            this.panel1.Controls.Add(this.BtoSal);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.BtoEdi);
            this.panel1.Controls.Add(this.BtoCon);
            this.panel1.Controls.Add(this.BtoGua);
            this.panel1.Location = new System.Drawing.Point(-1, -5);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(186, 329);
            this.panel1.TabIndex = 105;
            // 
            // BtoSal
            // 
            this.BtoSal.BackColor = System.Drawing.Color.Green;
            this.BtoSal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtoSal.FlatAppearance.BorderSize = 0;
            this.BtoSal.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtoSal.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtoSal.ForeColor = System.Drawing.Color.Black;
            this.BtoSal.Image = global::ProgramacionInstructores.Properties.Resources._8___Salir;
            this.BtoSal.Location = new System.Drawing.Point(20, 259);
            this.BtoSal.Margin = new System.Windows.Forms.Padding(4);
            this.BtoSal.Name = "BtoSal";
            this.BtoSal.Size = new System.Drawing.Size(145, 47);
            this.BtoSal.TabIndex = 3;
            this.BtoSal.Text = "  &Salir";
            this.BtoSal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtoSal.UseVisualStyleBackColor = false;
            this.BtoSal.Click += new System.EventHandler(this.BtoSal_Click);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(4, 24);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(178, 37);
            this.label9.TabIndex = 18;
            this.label9.Text = "Menu Principal";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtoEdi
            // 
            this.BtoEdi.BackColor = System.Drawing.Color.Green;
            this.BtoEdi.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtoEdi.FlatAppearance.BorderSize = 0;
            this.BtoEdi.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtoEdi.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtoEdi.ForeColor = System.Drawing.Color.Black;
            this.BtoEdi.Image = global::ProgramacionInstructores.Properties.Resources._2___Editar;
            this.BtoEdi.Location = new System.Drawing.Point(20, 88);
            this.BtoEdi.Margin = new System.Windows.Forms.Padding(4);
            this.BtoEdi.Name = "BtoEdi";
            this.BtoEdi.Size = new System.Drawing.Size(145, 47);
            this.BtoEdi.TabIndex = 1;
            this.BtoEdi.Text = "  &Editar";
            this.BtoEdi.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtoEdi.UseVisualStyleBackColor = false;
            this.BtoEdi.Click += new System.EventHandler(this.BtoEdi_Click);
            // 
            // BtoCon
            // 
            this.BtoCon.BackColor = System.Drawing.Color.Green;
            this.BtoCon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtoCon.FlatAppearance.BorderSize = 0;
            this.BtoCon.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtoCon.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtoCon.ForeColor = System.Drawing.Color.Black;
            this.BtoCon.Image = global::ProgramacionInstructores.Properties.Resources._4___Buscar;
            this.BtoCon.Location = new System.Drawing.Point(20, 204);
            this.BtoCon.Margin = new System.Windows.Forms.Padding(4);
            this.BtoCon.Name = "BtoCon";
            this.BtoCon.Size = new System.Drawing.Size(145, 47);
            this.BtoCon.TabIndex = 2;
            this.BtoCon.Text = "  &Consultar";
            this.BtoCon.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtoCon.UseVisualStyleBackColor = false;
            this.BtoCon.Click += new System.EventHandler(this.BtoCon_Click);
            // 
            // BtoGua
            // 
            this.BtoGua.BackColor = System.Drawing.Color.Green;
            this.BtoGua.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtoGua.FlatAppearance.BorderSize = 0;
            this.BtoGua.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtoGua.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtoGua.ForeColor = System.Drawing.Color.Black;
            this.BtoGua.Image = global::ProgramacionInstructores.Properties.Resources._3___Guardar;
            this.BtoGua.Location = new System.Drawing.Point(20, 146);
            this.BtoGua.Margin = new System.Windows.Forms.Padding(4);
            this.BtoGua.Name = "BtoGua";
            this.BtoGua.Size = new System.Drawing.Size(145, 47);
            this.BtoGua.TabIndex = 0;
            this.BtoGua.Text = "  &Guardar";
            this.BtoGua.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtoGua.UseVisualStyleBackColor = false;
            this.BtoGua.Click += new System.EventHandler(this.BtoGua_Click);
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(181, 133);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(120, 25);
            this.label11.TabIndex = 115;
            this.label11.Text = "Estado:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(214)))), ((int)(((byte)(167)))));
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(198, 178);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(510, 146);
            this.dataGridView1.TabIndex = 124;
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(193, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(515, 36);
            this.label1.TabIndex = 117;
            this.label1.Text = "Configuración de Usuarios";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(194, 97);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 25);
            this.label4.TabIndex = 118;
            this.label4.Text = "Nombres:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(451, 59);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 25);
            this.label3.TabIndex = 111;
            this.label3.Text = "Documento:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(194, 59);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 25);
            this.label2.TabIndex = 110;
            this.label2.Text = "IdUsuario:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "1 - Nuevo.jpg");
            this.imageList1.Images.SetKeyName(1, "2 - Editar.jpg");
            this.imageList1.Images.SetKeyName(2, "3 - Guardar.jpg");
            this.imageList1.Images.SetKeyName(3, "4 - Buscar.jpg");
            this.imageList1.Images.SetKeyName(4, "5 - Eliminar.jpg");
            this.imageList1.Images.SetKeyName(5, "6 - Imprimir.jpg");
            this.imageList1.Images.SetKeyName(6, "7 - Cerrar.jpg");
            this.imageList1.Images.SetKeyName(7, "8 - Salir.jpg");
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Activo",
            "Inactivo"});
            this.comboBox1.Location = new System.Drawing.Point(306, 132);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(187, 28);
            this.comboBox1.TabIndex = 126;
            // 
            // Ant
            // 
            this.Ant.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Ant.Image = ((System.Drawing.Image)(resources.GetObject("Ant.Image")));
            this.Ant.Location = new System.Drawing.Point(552, 129);
            this.Ant.Margin = new System.Windows.Forms.Padding(4);
            this.Ant.Name = "Ant";
            this.Ant.Size = new System.Drawing.Size(44, 41);
            this.Ant.TabIndex = 5;
            this.Ant.UseVisualStyleBackColor = true;
            this.Ant.Click += new System.EventHandler(this.Ant_Click);
            // 
            // Pri
            // 
            this.Pri.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Pri.Image = global::ProgramacionInstructores.Properties.Resources.A_1___Pri;
            this.Pri.Location = new System.Drawing.Point(500, 129);
            this.Pri.Margin = new System.Windows.Forms.Padding(4);
            this.Pri.Name = "Pri";
            this.Pri.Size = new System.Drawing.Size(44, 41);
            this.Pri.TabIndex = 4;
            this.Pri.UseVisualStyleBackColor = true;
            this.Pri.Click += new System.EventHandler(this.Pri_Click);
            // 
            // Sig
            // 
            this.Sig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Sig.Image = ((System.Drawing.Image)(resources.GetObject("Sig.Image")));
            this.Sig.Location = new System.Drawing.Point(604, 129);
            this.Sig.Margin = new System.Windows.Forms.Padding(4);
            this.Sig.Name = "Sig";
            this.Sig.Size = new System.Drawing.Size(44, 41);
            this.Sig.TabIndex = 6;
            this.Sig.UseVisualStyleBackColor = true;
            this.Sig.Click += new System.EventHandler(this.Sig_Click);
            // 
            // Ult
            // 
            this.Ult.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Ult.Image = ((System.Drawing.Image)(resources.GetObject("Ult.Image")));
            this.Ult.Location = new System.Drawing.Point(656, 129);
            this.Ult.Margin = new System.Windows.Forms.Padding(4);
            this.Ult.Name = "Ult";
            this.Ult.Size = new System.Drawing.Size(44, 41);
            this.Ult.TabIndex = 7;
            this.Ult.UseVisualStyleBackColor = true;
            this.Ult.Click += new System.EventHandler(this.Ult_Click);
            // 
            // FrmEstadoUser
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(94)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(710, 330);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Ant);
            this.Controls.Add(this.Pri);
            this.Controls.Add(this.Sig);
            this.Controls.Add(this.Ult);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmEstadoUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmConUsuarios";
            this.Load += new System.EventHandler(this.FrmEstadoUser_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BtoSal;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button BtoEdi;
        private System.Windows.Forms.Button BtoCon;
        private System.Windows.Forms.Button BtoGua;
        private System.Windows.Forms.Button Ant;
        private System.Windows.Forms.Button Pri;
        private System.Windows.Forms.Button Sig;
        private System.Windows.Forms.Button Ult;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}