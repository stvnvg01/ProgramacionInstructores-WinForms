namespace ProgramacionInstructores
{
    partial class FrmCronograma
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
            this.label1 = new System.Windows.Forms.Label();
            this.IdPersona = new System.Windows.Forms.TextBox();
            this.btnGenerar = new System.Windows.Forms.Button();
            this.btnGuardarExcel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.chk1Hora = new System.Windows.Forms.CheckBox();
            this.chk2Horas = new System.Windows.Forms.CheckBox();
            this.chk4Horas = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.btnLimpiarCursor = new System.Windows.Forms.Button();
            this.Departamento = new System.Windows.Forms.ComboBox();
            this.Municipio = new System.Windows.Forms.ComboBox();
            this.txtInstructorDisplay = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dgvCronograma = new System.Windows.Forms.DataGridView();
            this.HorasPRO = new System.Windows.Forms.Label();
            this.btnEnviarCorreo = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCronograma)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(431, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(440, 40);
            this.label1.TabIndex = 1;
            this.label1.Text = "Programador Instructores";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // IdPersona
            // 
            this.IdPersona.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IdPersona.Location = new System.Drawing.Point(120, 62);
            this.IdPersona.Margin = new System.Windows.Forms.Padding(4);
            this.IdPersona.Name = "IdPersona";
            this.IdPersona.Size = new System.Drawing.Size(426, 30);
            this.IdPersona.TabIndex = 0;
            this.IdPersona.TextChanged += new System.EventHandler(this.IdPersona_TextChanged);
            this.IdPersona.Leave += new System.EventHandler(this.IdPersona_Leave);
            // 
            // btnGenerar
            // 
            this.btnGenerar.BackColor = System.Drawing.Color.Transparent;
            this.btnGenerar.BackgroundImage = global::ProgramacionInstructores.Properties.Resources._0_Aceptar;
            this.btnGenerar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGenerar.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGenerar.Location = new System.Drawing.Point(567, 142);
            this.btnGenerar.Margin = new System.Windows.Forms.Padding(4);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(120, 50);
            this.btnGenerar.TabIndex = 2;
            this.btnGenerar.Text = "Aceptar";
            this.btnGenerar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGenerar.UseVisualStyleBackColor = false;
            this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
            // 
            // btnGuardarExcel
            // 
            this.btnGuardarExcel.BackgroundImage = global::ProgramacionInstructores.Properties.Resources._3___Guardar;
            this.btnGuardarExcel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGuardarExcel.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuardarExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGuardarExcel.Location = new System.Drawing.Point(823, 143);
            this.btnGuardarExcel.Margin = new System.Windows.Forms.Padding(4);
            this.btnGuardarExcel.Name = "btnGuardarExcel";
            this.btnGuardarExcel.Size = new System.Drawing.Size(120, 50);
            this.btnGuardarExcel.TabIndex = 4;
            this.btnGuardarExcel.Text = "G. Excel";
            this.btnGuardarExcel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGuardarExcel.UseVisualStyleBackColor = true;
            this.btnGuardarExcel.Click += new System.EventHandler(this.btnGuardarExcel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(13, 65);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 24);
            this.label2.TabIndex = 7;
            this.label2.Text = "Id Instructor";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Arial Narrow", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.checkBox1.Location = new System.Drawing.Point(1077, 147);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(52, 20);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "SAB";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Font = new System.Drawing.Font("Arial Narrow", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.checkBox2.Location = new System.Drawing.Point(1077, 172);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(54, 20);
            this.checkBox2.TabIndex = 7;
            this.checkBox2.Text = "DOM";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // chk1Hora
            // 
            this.chk1Hora.AutoSize = true;
            this.chk1Hora.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chk1Hora.Location = new System.Drawing.Point(1322, 9);
            this.chk1Hora.Margin = new System.Windows.Forms.Padding(4);
            this.chk1Hora.Name = "chk1Hora";
            this.chk1Hora.Size = new System.Drawing.Size(18, 17);
            this.chk1Hora.TabIndex = 10;
            this.chk1Hora.UseVisualStyleBackColor = true;
            // 
            // chk2Horas
            // 
            this.chk2Horas.AutoSize = true;
            this.chk2Horas.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chk2Horas.Location = new System.Drawing.Point(1322, 29);
            this.chk2Horas.Margin = new System.Windows.Forms.Padding(4);
            this.chk2Horas.Name = "chk2Horas";
            this.chk2Horas.Size = new System.Drawing.Size(18, 17);
            this.chk2Horas.TabIndex = 11;
            this.chk2Horas.UseVisualStyleBackColor = true;
            // 
            // chk4Horas
            // 
            this.chk4Horas.AutoSize = true;
            this.chk4Horas.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chk4Horas.Location = new System.Drawing.Point(1322, 54);
            this.chk4Horas.Margin = new System.Windows.Forms.Padding(4);
            this.chk4Horas.Name = "chk4Horas";
            this.chk4Horas.Size = new System.Drawing.Size(18, 17);
            this.chk4Horas.TabIndex = 12;
            this.chk4Horas.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(64, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 24);
            this.label3.TabIndex = 16;
            this.label3.Text = "Dpto";
            // 
            // comboBox3
            // 
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(120, 150);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(426, 32);
            this.comboBox3.TabIndex = 1;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(1301, 9);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(15, 22);
            this.dateTimePicker1.TabIndex = 21;
            this.dateTimePicker1.Visible = false;
            // 
            // btnLimpiarCursor
            // 
            this.btnLimpiarCursor.BackgroundImage = global::ProgramacionInstructores.Properties.Resources._5___Eliminar;
            this.btnLimpiarCursor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLimpiarCursor.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimpiarCursor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLimpiarCursor.Location = new System.Drawing.Point(695, 142);
            this.btnLimpiarCursor.Margin = new System.Windows.Forms.Padding(4);
            this.btnLimpiarCursor.Name = "btnLimpiarCursor";
            this.btnLimpiarCursor.Size = new System.Drawing.Size(120, 50);
            this.btnLimpiarCursor.TabIndex = 3;
            this.btnLimpiarCursor.Text = "B. Seleccion";
            this.btnLimpiarCursor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLimpiarCursor.UseVisualStyleBackColor = true;
            this.btnLimpiarCursor.Click += new System.EventHandler(this.btnLimpiarCursor_Click);
            // 
            // Departamento
            // 
            this.Departamento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Departamento.Enabled = false;
            this.Departamento.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Departamento.FormattingEnabled = true;
            this.Departamento.Location = new System.Drawing.Point(120, 103);
            this.Departamento.Name = "Departamento";
            this.Departamento.Size = new System.Drawing.Size(426, 32);
            this.Departamento.TabIndex = 26;
            this.Departamento.SelectedIndexChanged += new System.EventHandler(this.Departamento_SelectedIndexChanged);
            // 
            // Municipio
            // 
            this.Municipio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Municipio.Enabled = false;
            this.Municipio.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Municipio.FormattingEnabled = true;
            this.Municipio.Location = new System.Drawing.Point(645, 103);
            this.Municipio.Name = "Municipio";
            this.Municipio.Size = new System.Drawing.Size(426, 32);
            this.Municipio.TabIndex = 27;
            // 
            // txtInstructorDisplay
            // 
            this.txtInstructorDisplay.Enabled = false;
            this.txtInstructorDisplay.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInstructorDisplay.Location = new System.Drawing.Point(644, 62);
            this.txtInstructorDisplay.Margin = new System.Windows.Forms.Padding(4);
            this.txtInstructorDisplay.Name = "txtInstructorDisplay";
            this.txtInstructorDisplay.ReadOnly = true;
            this.txtInstructorDisplay.Size = new System.Drawing.Size(426, 30);
            this.txtInstructorDisplay.TabIndex = 28;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Location = new System.Drawing.Point(554, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 24);
            this.label4.TabIndex = 29;
            this.label4.Text = "Municipio";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label5.Location = new System.Drawing.Point(554, 68);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 24);
            this.label5.TabIndex = 30;
            this.label5.Text = "Nombre";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label6.Location = new System.Drawing.Point(67, 156);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 24);
            this.label6.TabIndex = 31;
            this.label6.Text = "Mes";
            // 
            // dgvCronograma
            // 
            this.dgvCronograma.AllowUserToAddRows = false;
            this.dgvCronograma.AllowUserToDeleteRows = false;
            this.dgvCronograma.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(214)))), ((int)(((byte)(167)))));
            this.dgvCronograma.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCronograma.Location = new System.Drawing.Point(120, 201);
            this.dgvCronograma.Margin = new System.Windows.Forms.Padding(4);
            this.dgvCronograma.Name = "dgvCronograma";
            this.dgvCronograma.RowHeadersVisible = false;
            this.dgvCronograma.RowHeadersWidth = 51;
            this.dgvCronograma.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvCronograma.Size = new System.Drawing.Size(950, 400);
            this.dgvCronograma.TabIndex = 2;
            this.dgvCronograma.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCronograma_CellClick);
            this.dgvCronograma.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvCronograma_KeyDown);
            this.dgvCronograma.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvCronograma_KeyPress);
            this.dgvCronograma.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvCronograma_MouseDown);
            this.dgvCronograma.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvCronograma_MouseMove);
            this.dgvCronograma.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvCronograma_MouseUp);
            // 
            // HorasPRO
            // 
            this.HorasPRO.AutoSize = true;
            this.HorasPRO.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HorasPRO.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.HorasPRO.Location = new System.Drawing.Point(896, 34);
            this.HorasPRO.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.HorasPRO.Name = "HorasPRO";
            this.HorasPRO.Size = new System.Drawing.Size(174, 24);
            this.HorasPRO.TabIndex = 32;
            this.HorasPRO.Text = "Horas Programadas: ";
            // 
            // btnEnviarCorreo
            // 
            this.btnEnviarCorreo.BackgroundImage = global::ProgramacionInstructores.Properties.Resources.btogmail_removebg_preview;
            this.btnEnviarCorreo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEnviarCorreo.Font = new System.Drawing.Font("Arial Narrow", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnviarCorreo.Location = new System.Drawing.Point(950, 144);
            this.btnEnviarCorreo.Name = "btnEnviarCorreo";
            this.btnEnviarCorreo.Size = new System.Drawing.Size(120, 50);
            this.btnEnviarCorreo.TabIndex = 5;
            this.btnEnviarCorreo.Text = "E. Correo";
            this.btnEnviarCorreo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEnviarCorreo.UseVisualStyleBackColor = true;
            this.btnEnviarCorreo.Click += new System.EventHandler(this.btnEnviarCorreo_Click);
            // 
            // FrmCronograma
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(94)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(1189, 652);
            this.Controls.Add(this.btnEnviarCorreo);
            this.Controls.Add(this.HorasPRO);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtInstructorDisplay);
            this.Controls.Add(this.Municipio);
            this.Controls.Add(this.Departamento);
            this.Controls.Add(this.btnLimpiarCursor);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chk4Horas);
            this.Controls.Add(this.chk2Horas);
            this.Controls.Add(this.chk1Hora);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnGuardarExcel);
            this.Controls.Add(this.btnGenerar);
            this.Controls.Add(this.IdPersona);
            this.Controls.Add(this.dgvCronograma);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCronograma";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Programador de Instructores";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmCronograma_FormClosing);
            this.Load += new System.EventHandler(this.Form1cronograma_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCronograma)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox IdPersona;
        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.Button btnGuardarExcel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox chk1Hora;
        private System.Windows.Forms.CheckBox chk2Horas;
        private System.Windows.Forms.CheckBox chk4Horas;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button btnLimpiarCursor;
        private System.Windows.Forms.ComboBox Departamento;
        private System.Windows.Forms.ComboBox Municipio;
        private System.Windows.Forms.TextBox txtInstructorDisplay;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView dgvCronograma;
        private System.Windows.Forms.Label HorasPRO;
        private System.Windows.Forms.Button btnEnviarCorreo;
    }
}