namespace ProgramacionInstructores
{
    partial class FrmUsuarios
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUsuarios));
            this.BtoFoto = new System.Windows.Forms.Button();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.BtoSal = new System.Windows.Forms.Button();
            this.BtoEdi = new System.Windows.Forms.Button();
            this.BtoCon = new System.Windows.Forms.Button();
            this.BtoEli = new System.Windows.Forms.Button();
            this.BtoNue = new System.Windows.Forms.Button();
            this.BtoGua = new System.Windows.Forms.Button();
            this.BtoImp = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Ant = new System.Windows.Forms.Button();
            this.Pri = new System.Windows.Forms.Button();
            this.Sig = new System.Windows.Forms.Button();
            this.Ult = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label14 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // BtoFoto
            // 
            this.BtoFoto.Enabled = false;
            this.BtoFoto.ForeColor = System.Drawing.Color.Black;
            this.BtoFoto.Location = new System.Drawing.Point(785, 148);
            this.BtoFoto.Margin = new System.Windows.Forms.Padding(4);
            this.BtoFoto.Name = "BtoFoto";
            this.BtoFoto.Size = new System.Drawing.Size(104, 26);
            this.BtoFoto.TabIndex = 12;
            this.BtoFoto.Text = "&Buscar Foto";
            this.BtoFoto.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BtoFoto.UseVisualStyleBackColor = true;
            this.BtoFoto.Click += new System.EventHandler(this.BtoFoto_Click);
            // 
            // textBox9
            // 
            this.textBox9.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox9.Location = new System.Drawing.Point(461, 234);
            this.textBox9.Margin = new System.Windows.Forms.Padding(4);
            this.textBox9.MaxLength = 15;
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(243, 35);
            this.textBox9.TabIndex = 11;
            this.textBox9.Tag = "Clave";
            this.textBox9.Text = "9";
            this.textBox9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox9.UseSystemPasswordChar = true;
            this.textBox9.Validated += new System.EventHandler(this.textBox9_Validated);
            // 
            // textBox8
            // 
            this.textBox8.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox8.Location = new System.Drawing.Point(461, 180);
            this.textBox8.Margin = new System.Windows.Forms.Padding(4);
            this.textBox8.MaxLength = 15;
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(243, 30);
            this.textBox8.TabIndex = 10;
            this.textBox8.Tag = "NomUser";
            this.textBox8.Text = "8";
            this.textBox8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox8.Validated += new System.EventHandler(this.textBox8_Validated);
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(354, 185);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(85, 25);
            this.label13.TabIndex = 86;
            this.label13.Text = "Usuario:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.SystemColors.Window;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(712, 60);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(247, 28);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.Validated += new System.EventHandler(this.comboBox1_Validated_1);
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(575, 59);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(129, 25);
            this.label12.TabIndex = 83;
            this.label12.Text = "Tipo de Usuario:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(214)))), ((int)(((byte)(167)))));
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(193, 290);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(783, 185);
            this.dataGridView1.TabIndex = 89;
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(362, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(528, 36);
            this.label1.TabIndex = 81;
            this.label1.Text = "Administración de Usuarios";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(712, 92);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 25);
            this.label5.TabIndex = 84;
            this.label5.Text = "Apellidos:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(457, 92);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 25);
            this.label4.TabIndex = 82;
            this.label4.Text = "Nombres:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(193, 117);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 25);
            this.label3.TabIndex = 75;
            this.label3.Text = "Documento:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(193, 60);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 25);
            this.label2.TabIndex = 74;
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
            // BtoSal
            // 
            this.BtoSal.BackColor = System.Drawing.Color.Transparent;
            this.BtoSal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtoSal.FlatAppearance.BorderSize = 0;
            this.BtoSal.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtoSal.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtoSal.ForeColor = System.Drawing.Color.Black;
            this.BtoSal.Image = global::ProgramacionInstructores.Properties.Resources._8___Salir;
            this.BtoSal.Location = new System.Drawing.Point(20, 407);
            this.BtoSal.Margin = new System.Windows.Forms.Padding(4);
            this.BtoSal.Name = "BtoSal";
            this.BtoSal.Size = new System.Drawing.Size(150, 50);
            this.BtoSal.TabIndex = 6;
            this.BtoSal.Text = "  &Salir";
            this.BtoSal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.BtoSal, "Salir del Formulario");
            this.BtoSal.UseVisualStyleBackColor = false;
            this.BtoSal.Click += new System.EventHandler(this.BtoSal_Click);
            // 
            // BtoEdi
            // 
            this.BtoEdi.BackColor = System.Drawing.Color.Transparent;
            this.BtoEdi.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtoEdi.FlatAppearance.BorderSize = 0;
            this.BtoEdi.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtoEdi.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtoEdi.ForeColor = System.Drawing.Color.Black;
            this.BtoEdi.Image = global::ProgramacionInstructores.Properties.Resources._2___Editar;
            this.BtoEdi.Location = new System.Drawing.Point(20, 118);
            this.BtoEdi.Margin = new System.Windows.Forms.Padding(4);
            this.BtoEdi.Name = "BtoEdi";
            this.BtoEdi.Size = new System.Drawing.Size(150, 50);
            this.BtoEdi.TabIndex = 2;
            this.BtoEdi.Text = "  &Editar";
            this.BtoEdi.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.BtoEdi, "Editar Registro");
            this.BtoEdi.UseVisualStyleBackColor = false;
            this.BtoEdi.Click += new System.EventHandler(this.BtoEdi_Click);
            // 
            // BtoCon
            // 
            this.BtoCon.BackColor = System.Drawing.Color.Transparent;
            this.BtoCon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtoCon.FlatAppearance.BorderSize = 0;
            this.BtoCon.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtoCon.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtoCon.ForeColor = System.Drawing.Color.Black;
            this.BtoCon.Image = global::ProgramacionInstructores.Properties.Resources._4___Buscar;
            this.BtoCon.Location = new System.Drawing.Point(20, 234);
            this.BtoCon.Margin = new System.Windows.Forms.Padding(4);
            this.BtoCon.Name = "BtoCon";
            this.BtoCon.Size = new System.Drawing.Size(150, 50);
            this.BtoCon.TabIndex = 3;
            this.BtoCon.Text = "  &Consultar";
            this.BtoCon.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.BtoCon, "Buscar Registro");
            this.BtoCon.UseVisualStyleBackColor = false;
            this.BtoCon.Click += new System.EventHandler(this.BtoCon_Click);
            // 
            // BtoEli
            // 
            this.BtoEli.BackColor = System.Drawing.Color.Transparent;
            this.BtoEli.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtoEli.FlatAppearance.BorderSize = 0;
            this.BtoEli.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtoEli.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtoEli.ForeColor = System.Drawing.Color.Black;
            this.BtoEli.Image = global::ProgramacionInstructores.Properties.Resources._5___Eliminar;
            this.BtoEli.Location = new System.Drawing.Point(20, 292);
            this.BtoEli.Margin = new System.Windows.Forms.Padding(4);
            this.BtoEli.Name = "BtoEli";
            this.BtoEli.Size = new System.Drawing.Size(150, 50);
            this.BtoEli.TabIndex = 4;
            this.BtoEli.Text = "  &Eliminar";
            this.BtoEli.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.BtoEli, "Eliminiar Registro");
            this.BtoEli.UseVisualStyleBackColor = false;
            this.BtoEli.Click += new System.EventHandler(this.BtoEli_Click);
            // 
            // BtoNue
            // 
            this.BtoNue.BackColor = System.Drawing.Color.Transparent;
            this.BtoNue.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtoNue.FlatAppearance.BorderSize = 0;
            this.BtoNue.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtoNue.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtoNue.ForeColor = System.Drawing.Color.Black;
            this.BtoNue.Image = global::ProgramacionInstructores.Properties.Resources._1___Nuevo;
            this.BtoNue.Location = new System.Drawing.Point(20, 60);
            this.BtoNue.Margin = new System.Windows.Forms.Padding(4);
            this.BtoNue.Name = "BtoNue";
            this.BtoNue.Size = new System.Drawing.Size(150, 50);
            this.BtoNue.TabIndex = 1;
            this.BtoNue.Text = "  &Nuevo";
            this.BtoNue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.BtoNue, "Nuevo Registro");
            this.BtoNue.UseVisualStyleBackColor = false;
            this.BtoNue.Click += new System.EventHandler(this.BtoNue_Click);
            // 
            // BtoGua
            // 
            this.BtoGua.BackColor = System.Drawing.Color.Transparent;
            this.BtoGua.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtoGua.FlatAppearance.BorderSize = 0;
            this.BtoGua.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtoGua.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtoGua.ForeColor = System.Drawing.Color.Black;
            this.BtoGua.Image = global::ProgramacionInstructores.Properties.Resources._3___Guardar;
            this.BtoGua.Location = new System.Drawing.Point(20, 176);
            this.BtoGua.Margin = new System.Windows.Forms.Padding(4);
            this.BtoGua.Name = "BtoGua";
            this.BtoGua.Size = new System.Drawing.Size(150, 50);
            this.BtoGua.TabIndex = 0;
            this.BtoGua.Text = "  &Guardar";
            this.BtoGua.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.BtoGua, "Guardar Registro");
            this.BtoGua.UseVisualStyleBackColor = false;
            this.BtoGua.Click += new System.EventHandler(this.BtoGua_Click);
            // 
            // BtoImp
            // 
            this.BtoImp.BackColor = System.Drawing.Color.Transparent;
            this.BtoImp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtoImp.FlatAppearance.BorderSize = 0;
            this.BtoImp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtoImp.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtoImp.ForeColor = System.Drawing.Color.Black;
            this.BtoImp.Image = global::ProgramacionInstructores.Properties.Resources._6___Imprimir;
            this.BtoImp.Location = new System.Drawing.Point(20, 350);
            this.BtoImp.Margin = new System.Windows.Forms.Padding(4);
            this.BtoImp.Name = "BtoImp";
            this.BtoImp.Size = new System.Drawing.Size(150, 50);
            this.BtoImp.TabIndex = 5;
            this.BtoImp.Text = "  &Imprimir";
            this.BtoImp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.BtoImp, "Imprimir Registros");
            this.BtoImp.UseVisualStyleBackColor = false;
            this.BtoImp.Click += new System.EventHandler(this.BtoImp_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(214)))), ((int)(((byte)(167)))));
            this.panel1.Controls.Add(this.BtoSal);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.BtoEdi);
            this.panel1.Controls.Add(this.BtoCon);
            this.panel1.Controls.Add(this.BtoEli);
            this.panel1.Controls.Add(this.BtoNue);
            this.panel1.Controls.Add(this.BtoGua);
            this.panel1.Controls.Add(this.BtoImp);
            this.panel1.Location = new System.Drawing.Point(-1, -1);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(186, 482);
            this.panel1.TabIndex = 13;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(4, 10);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(178, 37);
            this.label9.TabIndex = 18;
            this.label9.Text = "Menu Principal";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox4
            // 
            this.textBox4.Enabled = false;
            this.textBox4.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold);
            this.textBox4.Location = new System.Drawing.Point(712, 117);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(247, 26);
            this.textBox4.TabIndex = 4;
            this.textBox4.Tag = "Apellidos";
            this.textBox4.Text = "4";
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold);
            this.textBox3.Location = new System.Drawing.Point(457, 118);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(247, 26);
            this.textBox3.TabIndex = 3;
            this.textBox3.Tag = "Nombres";
            this.textBox3.Text = "3";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.Window;
            this.textBox2.Enabled = false;
            this.textBox2.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(306, 117);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4);
            this.textBox2.MaxLength = 15;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(145, 26);
            this.textBox2.TabIndex = 2;
            this.textBox2.Tag = "Documento";
            this.textBox2.Text = "2";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox2_KeyPress);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(306, 59);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.MaxLength = 15;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(145, 26);
            this.textBox1.TabIndex = 0;
            this.textBox1.Tag = "IdUsuario";
            this.textBox1.Text = "1";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            this.textBox1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.textBox1_PreviewKeyDown);
            this.textBox1.Validated += new System.EventHandler(this.textBox1_Validated);
            // 
            // Ant
            // 
            this.Ant.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Ant.Image = ((System.Drawing.Image)(resources.GetObject("Ant.Image")));
            this.Ant.Location = new System.Drawing.Point(984, 338);
            this.Ant.Margin = new System.Windows.Forms.Padding(4);
            this.Ant.Name = "Ant";
            this.Ant.Size = new System.Drawing.Size(44, 41);
            this.Ant.TabIndex = 15;
            this.Ant.UseVisualStyleBackColor = true;
            this.Ant.Click += new System.EventHandler(this.Ant_Click);
            // 
            // Pri
            // 
            this.Pri.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Pri.Image = global::ProgramacionInstructores.Properties.Resources.A_1___Pri;
            this.Pri.Location = new System.Drawing.Point(984, 290);
            this.Pri.Margin = new System.Windows.Forms.Padding(4);
            this.Pri.Name = "Pri";
            this.Pri.Size = new System.Drawing.Size(44, 41);
            this.Pri.TabIndex = 14;
            this.Pri.UseVisualStyleBackColor = true;
            this.Pri.Click += new System.EventHandler(this.Pri_Click);
            // 
            // Sig
            // 
            this.Sig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Sig.Image = ((System.Drawing.Image)(resources.GetObject("Sig.Image")));
            this.Sig.Location = new System.Drawing.Point(984, 386);
            this.Sig.Margin = new System.Windows.Forms.Padding(4);
            this.Sig.Name = "Sig";
            this.Sig.Size = new System.Drawing.Size(44, 41);
            this.Sig.TabIndex = 16;
            this.Sig.UseVisualStyleBackColor = true;
            this.Sig.Click += new System.EventHandler(this.Sig_Click);
            // 
            // Ult
            // 
            this.Ult.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Ult.Image = ((System.Drawing.Image)(resources.GetObject("Ult.Image")));
            this.Ult.Location = new System.Drawing.Point(984, 434);
            this.Ult.Margin = new System.Windows.Forms.Padding(4);
            this.Ult.Name = "Ult";
            this.Ult.Size = new System.Drawing.Size(44, 41);
            this.Ult.TabIndex = 17;
            this.Ult.UseVisualStyleBackColor = true;
            this.Ult.Click += new System.EventHandler(this.Ult_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ProgramacionInstructores.Properties.Resources.SinFoto;
            this.pictureBox1.Location = new System.Drawing.Point(787, 175);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(103, 103);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 80;
            this.pictureBox1.TabStop = false;
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(327, 242);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(112, 25);
            this.label14.TabIndex = 87;
            this.label14.Text = "Contraseña:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmUsuarios
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(94)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(1039, 480);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Ant);
            this.Controls.Add(this.Pri);
            this.Controls.Add(this.Sig);
            this.Controls.Add(this.Ult);
            this.Controls.Add(this.BtoFoto);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmUsuarios";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Administración de Usuarios";
            this.Load += new System.EventHandler(this.FrmUsuarios_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Ant;
        private System.Windows.Forms.Button Pri;
        private System.Windows.Forms.Button Sig;
        private System.Windows.Forms.Button Ult;
        private System.Windows.Forms.Button BtoFoto;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BtoSal;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button BtoEdi;
        private System.Windows.Forms.Button BtoCon;
        private System.Windows.Forms.Button BtoEli;
        private System.Windows.Forms.Button BtoNue;
        private System.Windows.Forms.Button BtoGua;
        private System.Windows.Forms.Button BtoImp;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label14;
    }
}