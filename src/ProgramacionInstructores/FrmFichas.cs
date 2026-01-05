using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ProgramacionInstructores
{
    public partial class FrmFichas : Form
    {
        public FrmFichas()
        {
            InitializeComponent();
        }

        //Reccuerde el orden de tabulacion - El id es Tabindex Cero(0)
        Conexion Conn = new Conexion(); //Define la variable "Conn" para la conexion con la base de datos
        Conexion ConnAux = new Conexion();
        int Pos = 0; //Define variables tipo netero llamada pos e inicializada en cero tipo publico 
        string Opc = "0"; // Define Variable tipo texto llamada Opc e inicializada en "0" ---> Tipo Texto
        string ValNomUser = string.Empty;
        string ValDocUser = string.Empty;
        Complementos ONTEnter = new Complementos(); //Carga la clase complementos con el nombre ONTEnter
        DataRow fila;
        int swTimer = 0;

        OpenFileDialog BuscarFile = new OpenFileDialog();

        private void FrmUsuarios_Load(object sender, EventArgs e)
        {
            Conn.CargarEstilosDesdeBD(); // 🔁 Carga desde la base de datos a VarGlobal
            // Aplica automáticamente los estilos desde VarGlobal
            EstilosGlobales.AplicarEstiloBoton(BtoNue, "Nuevo");
            EstilosGlobales.AplicarEstiloBoton(BtoEdi, "Editar");
            EstilosGlobales.AplicarEstiloBoton(BtoEli, "Eliminar");
            EstilosGlobales.AplicarEstiloBoton(BtoGua, "Guardar");
            EstilosGlobales.AplicarEstiloBoton(BtoCon, "Consultar");
            EstilosGlobales.AplicarEstiloBoton(BtoImp, "Imprimir");
            EstilosGlobales.AplicarEstiloBoton(BtoSal, "Salir");

            //Cada Objeto textbox en la propiedad "Tag" debe contener el nombre del campo SQL
            SqlDataReader LenC;
            foreach (var Obj in this.Controls.OfType<System.Windows.Forms.TextBox>())
            {
                LenC = Conn.LenCampos("Fichas");
                while (LenC.Read())
                {
                    if (Obj.Tag != null && LenC.GetValue(0).ToString() == Obj.Tag.ToString())
                    {
                        Obj.Text = LenC["LenCampo"].ToString(); Obj.MaxLength = Convert.ToInt32(LenC["LenCampo"]); break;
                    }
                }
            }

            foreach (Control Obj in this.Controls) //Recorre todos los Objetos de tipo control
            {
                if (Obj is System.Windows.Forms.TextBox || Obj is System.Windows.Forms.ComboBox)   //Tipo Textbox y Combobox
                {
                    Obj.KeyDown += new KeyEventHandler(Complementos.PCEnter); // Para pasar con Enter cada Objeto
                    Obj.TextChanged += new EventHandler(Complementos.ValChar); //Quita la comilla simple del objeto
                    Obj.GotFocus += new EventHandler(Complementos.RecibeFoco); // Cambia el color del Objeto
                    Obj.LostFocus += new EventHandler(Complementos.PerderFoco); //Volcer al color inicial del Objeto
                }
                if (Obj is Label) { Obj.ForeColor = Conexion.VarGlobal.xColorFont; }
            }
            //this.BackColor = Conexion.VarGlobal.xColorFondo;
            //this.panel1.BackColor = Conexion.VarGlobal.xColorPanel;
            //this.label1.BackColor = Conexion.VarGlobal.xColorTitle;
            try
            {
                if (!ConnAux.ConsultaItem("Perfil_Rol", "IdPerfil = 'Administrador'"))
                {
                    string AddPR = "Insert Into Perfil_Rol (IdPerfil,Nombre) Values ('01','Administrador')";
                    ConnAux.Insertar(AddPR);
                }
            }
            catch { }


            Conn.Conectar();
            MostrarDatos();
            ActDesObjetos(false);
            BtoGua.Enabled = false;
            //BtoFoto.Enabled = false;
            textBox2.Enabled = false; textBox3.Enabled = false; textBox4.Enabled = false;
            //if (pictureBox1.Image == null) { pictureBox1.Load(Conexion.VarGlobal.xRuta + "SinFoto.Png"); }
        }

        private void ActDesObjetos(Boolean ActDes)
        {
            foreach (Control Obj in this.Controls.Cast<Control>().Union(panel1.Controls.Cast<Control>())) //Recorre todos los Objetos de tipo control           
            {
                if (Obj is System.Windows.Forms.TextBox || Obj is System.Windows.Forms.ComboBox) //Tipo TextBox y ComboBox
                {
                    Obj.Enabled = ActDes; //Deshabilita todos los Objetos TextBox y ComboBox
                    if (Opc == "1" && Obj.TabIndex > 0) { Obj.Text = string.Empty; }
                }
            }
        }

        private void MostrarDatos()
        {
            ConnAux.ConsultaDatos("Select (IdDpto+' | '+LTrim(RTrim(Nombre))) As Nombre From Departamentos Order By Nombre", "Departamentos");
            comboBox1.DataSource = ConnAux.Ds.Tables["Departamentos"];
            comboBox1.DisplayMember = "Nombre";

            ConnAux.ConsultaDatos("Select (IdLugar+' | '+LTrim(RTrim(Nombre))) As Nombre From Municipios Order By Nombre", "Municipios");
            comboBox2.DataSource = ConnAux.Ds.Tables["Municipios"];
            comboBox2.DisplayMember = "Nombre";

            Conn.ConsultaDatos("Select * From Fichas Order By IdFicha", "Fichas");
            //dataGridView1.DataSource = Conn.Ds.Tables["Personas"];

            dataGridView1.DataSource = Conn.Ds.Tables["Fichas"];

            dataGridView1.Columns[0].HeaderText = "ID Ficha";
            dataGridView1.Columns[1].HeaderText = "ID Programa";
            dataGridView1.Columns[2].HeaderText = "Version";
            dataGridView1.Columns[3].HeaderText = "FecIni";
            dataGridView1.Columns[4].HeaderText = "FecFin";
            dataGridView1.Columns[5].HeaderText = "IdLugar";
            dataGridView1.Columns[6].HeaderText = "IdAmbiente";

            dataGridView1.Columns[0].Width = 115;
            dataGridView1.Columns[1].Width = 115;
            dataGridView1.Columns[2].Width = 115;
            dataGridView1.Columns[3].Width = 115;
            dataGridView1.Columns[4].Width = 115;
            dataGridView1.Columns[5].Width = 80;

            foreach (DataGridViewColumn Row in dataGridView1.Columns)
            {
                Row.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.MultiSelect = false;

            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView1.RowsDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.White;

            if (Opc == "0") { CargarDatos(Pos); }
        }


        private void CargarDatos(int Pos)
        {
            dataGridView1.ClearSelection();
            if ((Conn.Ds.Tables[0].Rows.Count) < 1) // dt.Rows.Count < 1) //Controlar si hay registros en la TABLA
            {
                EstadoBton(false); //Procedimientos que desactiva todos los Botones porque ActDes = False
                BtoNue.Enabled = true; // Activa el Boton 1 Nuevo
                BtoSal.Image = imageList1.Images[7];  //Cambia la Imagen del Boton 7 Salir - Imagen 7 = Salir
                BtoSal.Text = " &Salir"; //
                Opc = "1"; ActDesObjetos(false); Opc = "0";
                return; //Sale del procedimiento y no ejecuta el resto de los pasos.
            }
            fila = Conn.Ds.Tables[0].Rows[Pos];//Guarda el Registro (pos) en la variable "fila"
            textBox1.Text = Convert.ToString(fila["IdFicha"]).Trim(); // Carga los datos del Campo al textBox 
            textBox2.Text = Convert.ToString(fila["IdPrograma"]).Trim();
            textBox3.Text = Convert.ToString(fila["Version"]).Trim();
            textBox4.Text = Convert.ToString(fila["IdAmbiente"]).Trim();
            dateTimePicker1.Value = Convert.ToDateTime(fila["FecIni"]);
            dateTimePicker2.Value = Convert.ToDateTime(fila["FecFin"]);




            try
            {/* dateTimePicker1.Value = Convert.ToDateTime(fila["FecNac"]);*/
                comboBox1.SelectedIndex = comboBox1.FindString(((string)fila["IdLugar"]).Substring(0, 2));
                comboBox2.SelectedIndex = comboBox2.FindString(((string)fila["IdLugar"]).Substring(0, 5));
            }
            catch { }

            dataGridView1.Rows[Pos].Selected = true;//El Grid en la Fila con posición X "Seleccionela" (true/false)   
            dataGridView1.CurrentCell = dataGridView1.Rows[Pos].Cells[dataGridView1.CurrentCell.ColumnIndex];
        }


        private void EstadoBton(Boolean ActDes) //Procedimiento para Activar o Desactivar los Botones
        {
            foreach (Control Obj in this.Controls.Cast<Control>().Union(panel1.Controls.Cast<Control>())) //Recorre todos los Objetos de tipo control           
            {
                if (Obj is System.Windows.Forms.Button) { Obj.Enabled = ActDes; }
            }
            foreach (Control Obj in this.Controls) { if (Obj is System.Windows.Forms.Button) { Obj.Enabled = ActDes; } }
            BtoSal.Enabled = true;
            BtoSal.Image = imageList1.Images[6]; //Cambia la Imagen si ActDes es Igual a falso  - Imagen 7 = Salir
            BtoSal.Text = " &Salir";  //Pone texto Salir en el boton
            toolTip1.SetToolTip(BtoSal, "Salir del Formulario");
            if (ActDes == false)
            { //Compara si la variable ActDes es igual a false
                BtoSal.Image = imageList1.Images[7]; //Cambia la Imagen si ActDes es Igual a falso  - Imagen 6 = Cerrar
                BtoSal.Text = " &Cerrar"; //Pone texto Cerrar en el boton
                toolTip1.SetToolTip(BtoSal, "Cerrar procedimiento");
            }
        }

        private void Pri_Click(object sender, EventArgs e)
        {
            Pos = 0; //Inicializa Pos en Cero "este es el primer registro de la tabla
            CargarDatos(Pos); //Carga los datos Ubicados en la Posición Cero
            Pri.Enabled = false; Ant.Enabled = false; Sig.Enabled = true; Ult.Enabled = true;
        }

        private void Ant_Click(object sender, EventArgs e)
        {
            Pos += -1; //Retrocede un registro --- Si es el primer registro ejecuta el procedimiento Pri_Click
            if (Pos < 1) { Pos = 0; Pri_Click(null, null); }
            else { CargarDatos(Pos); Sig.Enabled = true; Ult.Enabled = true; }
        }

        private void Sig_Click(object sender, EventArgs e)
        {
            Pos += 1;
            if (Pos >= (Conn.Ds.Tables[0].Rows.Count - 1)) { Ult_Click(null, null); }
            else { CargarDatos(Pos); Pri.Enabled = true; Ant.Enabled = true; }
        }

        private void Ult_Click(object sender, EventArgs e)
        {
            Pos = Conn.Ds.Tables[0].Rows.Count - 1; CargarDatos(Pos);
            Pri.Enabled = true; Ant.Enabled = true; Sig.Enabled = false; Ult.Enabled = false;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            Pos = dataGridView1.CurrentCell.RowIndex;
            Pri.Enabled = true; Ant.Enabled = true; Sig.Enabled = true; Ult.Enabled = true;
            if (Pos == 0) { Pri_Click(null, null); }
            if (Pos >= Conn.Ds.Tables[0].Rows.Count - 1) { Ult_Click(null, null); }
            CargarDatos(Pos);
        }
        private void BtoNue_Click(object sender, EventArgs e)
        {
            if (Conexion.VarGlobal.xTipoU == "01" && Conexion.VarGlobal.xEstIni == 2 || Conexion.VarGlobal.xNomU == "Invitado")
            {
                Opc = "1";
                EstadoBton(false);
                BtoGua.Enabled = true;
                ActDesObjetos(true);

                textBox1.Enabled = true; // PK
                textBox1.Text = string.Empty;
                textBox2.Text = string.Empty;
                textBox3.Text = string.Empty;
                textBox4.Text = string.Empty;
                dateTimePicker1.Value = DateTime.Today;
                dateTimePicker2.Value = DateTime.Today;
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;

                dataGridView1.Enabled = false;
                textBox1.Focus();
            }
            else { MessageBox.Show("Solo Administrador y usuarios registrados acceden a esta función..!"); }
        }

        private void BtoEdi_Click(object sender, EventArgs e)
        {
            if (Conexion.VarGlobal.xTipoU == "01" && Conexion.VarGlobal.xEstIni == 2)
            {
                Opc = "2";
                EstadoBton(false);
                BtoGua.Enabled = true;
                ActDesObjetos(true);

                textBox1.Enabled = false; // no editar PK
                dataGridView1.Enabled = false;
            }
            else { MessageBox.Show("Solo Administradores y usuarios registrados acceden a esta función..!"); }
        }

        private void BtoGua_Click(object sender, EventArgs e)
        {
            // Opc: "1" = Nuevo, "2" = Editar
            if (Opc == "1") // Nuevo
            {
                string Agregar = "Insert Into Fichas SELECT '" +
                    textBox1.Text.Trim() + "','" +                                 // IdFicha
                    textBox2.Text.Trim() + "','" +                                 // IdPrograma
                    textBox3.Text.Trim() + "','" +                                 // Version
                    dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +         // FecIni (date)
                    dateTimePicker2.Value.ToString("yyyy-MM-dd") + "','" +         // FecFin (date)
                    comboBox2.Text.Substring(0, 5) + "','" +                       // IdLugar (DDMMM)
                    textBox4.Text.Trim() + "'";                                    // IdAmbiente

                if (Conn.Insertar(Agregar))
                {
                    string xId = textBox1.Text.Trim();
                    MostrarDatos();
                    Conn.Ds.Tables[0].PrimaryKey = new DataColumn[] { Conn.Ds.Tables[0].Columns["IdFicha"] };
                    Pos = Conn.Ds.Tables[0].Rows.IndexOf(Conn.Ds.Tables[0].Rows.Find(xId));
                    CargarDatos(Pos);
                }
                else { MessageBox.Show("Error al Agregar Registro"); }
            }

            if (Opc == "2") // Editar
            {
                string Actualizar =
                    "IdPrograma = '" + textBox2.Text.Trim() + "'," +
                    "Version    = '" + textBox3.Text.Trim() + "'," +
                    "FecIni     = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'," +
                    "FecFin     = '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "'," +
                    "IdLugar    = '" + comboBox2.Text.Substring(0, 5) + "'," +
                    "IdAmbiente = '" + textBox4.Text.Trim() + "'";

                if (Conn.Actualizar("Fichas", Actualizar, "IdFicha='" + textBox1.Text.Trim() + "'"))
                {
                    int NPos = Pos;
                    MostrarDatos();
                    CargarDatos(NPos);
                    MessageBox.Show("Datos Actualizados");
                }
                else { MessageBox.Show("Error al Actualizar"); }
            }

            dataGridView1.Enabled = true;
            this.BtoSal_Click(null, null);
        }
        private void BtoCon_Click(object sender, EventArgs e)
        {
            Opc = "4"; //Identifica el proceso CONSULTA
            EstadoBton(false); // Ejecuta el procedimiento EstadoBton (Activar o Deshabilitar botones)...
            textBox1.Enabled = true; // Habilita el cuadro de texto
            textBox1.Text = String.Empty; //Limpia el cuadro de Texto
            textBox1.Focus(); //Ubica el Cursor en el Textbo
        }

        private void BtoEli_Click(object sender, EventArgs e)
        {
            if (Conexion.VarGlobal.xTipoU == "01" && Conexion.VarGlobal.xEstIni == 2)
            {
                DialogResult resu = MessageBox.Show("¿Está seguro de eliminar <" + textBox1.Text.Trim() + " | " + textBox3.Text.Trim() +
                                                    "> de la base de datos?", "A D V E R T E N C I A", MessageBoxButtons.YesNo);
                if (resu == DialogResult.Yes)
                {
                    try
                    {
                        string reg = "IdFicha='" + textBox1.Text.Trim() + "'";
                        Conn.Eliminar("Fichas", reg); // ✅ Cambiado a la tabla correcta
                        Pos -= 1;
                        int PosN = Pos < 0 ? 0 : Pos;
                        MostrarDatos();
                        CargarDatos(PosN);
                    }
                    catch (Exception m)
                    {
                        MessageBox.Show(m.Message);
                        Conn.Conectar();
                    }
                }
            }
            else
            {
                MessageBox.Show("¡Solo Administradores y usuarios registrados acceden a esta función!");
            }
        }

        private void BtoSal_Click(object sender, EventArgs e)
        {
            dataGridView1.Enabled = true;
            string xOp = Opc;
            Opc = string.Empty;
            if (BtoSal.Text.Trim() == "&Cerrar")
            {
                EstadoBton(true);
                int xPos = Pos;
                if (xOp != "4") { MostrarDatos(); }
                CargarDatos(xPos);
                ActDesObjetos(false);
                BtoGua.Enabled = false;
            }
            else
            {
                Conexion.VarGlobal.xImaFon = 1;
                this.Dispose();
            }
        }



        private void textBox1_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
                return;

            int BackPos = Pos;
            string reg = "IdFicha='" + textBox1.Text.Trim() + "'";

            if (Conn.ConsultaItem("Fichas", reg))
            {
                // ✅ Si existe, cargamos desde BD para edición
                ConnAux.ConsultaDatos("SELECT * FROM Fichas WHERE IdFicha='" + textBox1.Text.Trim() + "'", "Fichas");
                if (ConnAux.Ds.Tables["Fichas"].Rows.Count > 0)
                {
                    DataRow filaFicha = ConnAux.Ds.Tables["Fichas"].Rows[0];

                    textBox2.Text = filaFicha["IdPrograma"].ToString().Trim();
                    textBox3.Text = filaFicha["Version"].ToString().Trim();
                    textBox4.Text = filaFicha["IdAmbiente"].ToString().Trim();
                    dateTimePicker1.Value = Convert.ToDateTime(filaFicha["FecIni"]);
                    dateTimePicker2.Value = Convert.ToDateTime(filaFicha["FecFin"]);

                    // 🔹 Asignar combos
                    try
                    {
                        comboBox1.SelectedIndex = comboBox1.FindString(filaFicha["IdLugar"].ToString().Substring(0, 2));
                        comboBox2.SelectedIndex = comboBox2.FindString(filaFicha["IdLugar"].ToString().Substring(0, 5));
                    }
                    catch { }

                    // ⚠️ Modo edición
                    Opc = "2";
                    textBox1.Enabled = false;
                    BtoGua.Enabled = true;
                }
            }
            else
            {
                // ❌ No existe, será un registro nuevo
                MessageBox.Show("No existe una ficha con este Id. Se creará como nueva.", "Nuevo registro");
                Opc = "1";
                ActDesObjetos(true);
                textBox1.Enabled = true;
                textBox2.Focus();
            }
        }



        private void textBox8_Validated(object sender, EventArgs e)
        {
            if (textBox2.Text == string.Empty) { textBox2.Focus(); MessageBox.Show("No puede ser vacio"); }
            else
            {
                string reg = "NomUser='" + textBox2.Text.Trim() + "'";
                if (Conn.ConsultaItem("Usuarios", reg) && textBox2.Text.Trim() != ValDocUser.Trim())
                {
                    MessageBox.Show("El dato ya existe, digite otro"); textBox2.Focus();
                }
            }
            BtoGua.Enabled = true;
        }




       
        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e) { swTimer = 1; }

        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e) { swTimer = 2; }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (swTimer == 1) //Si es "1" es porque la consulta esta ACTIVADA
            {
                swTimer = 0; //Cero "0" la consulta NO SE EJECUTA
                ConnAux.ConsultaDatos("Select (IdLugar+' | '+LTrim(RTrim(Nombre))) As Nombre From Municipios Where IdDpto = '" +
                comboBox1.Text.Substring(0, 2) + "' Order By Nombre", "Municipios");
                comboBox2.DataSource = ConnAux.Ds.Tables["Municipios"];
                comboBox2.DisplayMember = "Nombre";
                comboBox2.Refresh();
            }
            if (swTimer == 2) { swTimer = 0; } //Es "2", pase por municipio y la iguale a "0" = Apagar la Consulta
        }


        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Complementos.SoloNumE(e);
            if (e.KeyChar == (char)Keys.Enter && textBox1.Text.Trim() == string.Empty)
            {
                textBox1.Focus(); MessageBox.Show("Digite Su ID");
            }
        }

        private void textBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && textBox1.Text == string.Empty)
            {
                MessageBox.Show("Digite Su ID"); e.IsInputKey = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            Complementos.SoloNumE(e);
            if (e.KeyChar == (char)Keys.Enter && textBox2.Text.Trim() == string.Empty)
            { textBox2.Focus(); MessageBox.Show("Digite Su Documento"); }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            Complementos.SoloNumE(e);
            //if (e.KeyChar == (char)Keys.Enter && textBox6.Text.Trim() == string.Empty)
            { }
        }

        private void comboBox3_Validated(object sender, EventArgs e) { }

        string IdN, NoE, NoP, Tel, Cor, Dir, Lem; //Variables para almacenar información de la empresa
        byte[] ImaLog; //Variable para almacenar el logo de la empresa en formato de bytes

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { swTimer = 1; }

        private void comboBox2_SelectionChangeCommitted_1(object sender, EventArgs e) { swTimer = 2; }

        Image Log; //Variable para almacenar el lofo como una imagen
        int Pag;

        private void BtoImp_Click(object sender, EventArgs e)
        {
            Pag = 0; //Inicializa el contaador de paginas

            //----------------------------DESCOMENTAR LUEGO DE CREAR EMPRESA----------------------------------

            Conexion ConE = new Conexion(); // Deine la variable Conn para la conexión con la base de datos
            ConE.ConsultaDatos("Select * From Empresa", "Empresa"); //Consulta datos de la tabla Empresa
            fila = ConE.Ds.Tables[0].Rows[0];//Obtiene la primera fila del resultado de la consulta

            //Asigna los valores de los campos de la tabla empresa a las variables corrrespondientes
            IdN = Convert.ToString(fila["IdEmpresa"]).Trim(); // Carga los datos del Campo al textBox 
            NoE = Convert.ToString(fila["NombreE"]).Trim();
            NoP = Convert.ToString(fila["NombreP"]).Trim();
            Tel = Convert.ToString(fila["Telefono"]).Trim();
            Cor = Convert.ToString(fila["Correo"]).Trim();
            Dir = Convert.ToString(fila["UbicaDir"]).Trim();
            Lem = Convert.ToString(fila["Lema"]).Trim();

            ImaLog = (byte[])fila["Logo"];
            if (ImaLog != null && ImaLog.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(ImaLog)) { Log = Image.FromStream(ms); }
            }
            //Configura el Documento para la impresión y su vista previa
            PrintDocument DocPrin = new PrintDocument();
            PrintPreviewDialog ImpDoc = new PrintPreviewDialog();

            ImpDoc.ClientSize = new System.Drawing.Size(1000, 800);  //establece el tamaño de la ventana de vista de impresion en pixeles
            ImpDoc.StartPosition = FormStartPosition.CenterScreen; //Define que la ventana de vistaprecia estará centrada
            ImpDoc.Location = new System.Drawing.Point(1, 1); //Especifica la posición de la ventana en las coordenadas (1,1) desde la esquina superior izquierda de la pantalla

            //Configura el tamaño del papel y los eventos de impresión
            DocPrin.DefaultPageSettings.PaperSize = new PaperSize("Tamaño Carta", 850, 1100);
            DocPrin.PrintPage += new PrintPageEventHandler(this.DocPrin); //Asocia el evento PrintPage al metodo DocPrint, Que define el contenido que se imprimirá en cada página.
            DocPrin.EndPrint += new PrintEventHandler(DocPrinEndP);  //Asocia el evento EndPrint al metodo DocPrinEndP.
            ImpDoc.Document = DocPrin; ImpDoc.ShowDialog(); //Muestra la vista previa del documento
        }

        //Evento que se ejecuta al finalizar la impresion
        private void DocPrinEndP(object sender, PrintEventArgs e) { Pag = 0; } //Reinicia el contador de paginas 

        private int currentRow = 0; //ariable para rastrear la fila actual en la impresión 
        private void DocPrin(object sender, PrintPageEventArgs e)
        {
            //StringFormat TxtDer = new StringFormat(); TxtDer.Alignment = StringAlignment.Far;
            //StringFormat TxtIzq = new StringFormat(); TxtIzq.Alignment = StringAlignment.Near;

            //Define estilos y fientes para los textos
            Font txtNeg = new Font("Arial Narrow", 12, FontStyle.Bold, GraphicsUnit.Point);
            Font txtSim = new Font("Arial Narrow", 11, FontStyle.Regular, GraphicsUnit.Point);

            //Calcula tamaños de texto para centrado y ajuste
            SizeF txtSize1 = e.Graphics.MeasureString(NoE, txtNeg);
            SizeF txtSize2 = e.Graphics.MeasureString(IdN, txtNeg);
            SizeF txtSize3 = e.Graphics.MeasureString("<< U S U A R I O S >>", txtNeg);
            SizeF txtSize4 = e.Graphics.MeasureString(Tel + " - " + Cor + " - " + Dir, txtNeg);
            SizeF txtSize5 = e.Graphics.MeasureString(Lem, txtNeg);

            int Fil = 50; //Cordenada inicial vertical para la impresión

            //(e.PageBounds.Width - textSize.Width) / 2

            //imprime encabezado de la pagina
            e.Graphics.DrawString("Pág. " + (Pag += 1), txtNeg, Brushes.Black, e.PageBounds.Width - 100, 030);
            e.Graphics.DrawImage(Log, new Rectangle(20, 20, 90, 80));
            e.Graphics.DrawString(NoE, txtNeg, Brushes.Black, (e.PageBounds.Width - txtSize1.Width) / 2, 020);
            e.Graphics.DrawString(IdN, txtNeg, Brushes.Black, (e.PageBounds.Width - txtSize2.Width) / 2, 040);
            e.Graphics.DrawString("<< U S U A R I O S >>", txtNeg, Brushes.Black, (e.PageBounds.Width - txtSize3.Width) / 2, Fil += 25);
            //Linea divisoria y encabezados de la tabla
            e.Graphics.DrawString("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬", txtNeg, Brushes.Black, new RectangleF(70, Fil += 15, 800, 20));
            e.Graphics.DrawString(" IdCliente   │   Documento  |        Nombre       |         Apellido      |     Telefono     |    IdLugar                     ",
                txtNeg, Brushes.Black, new RectangleF(70, Fil += 15, 900, 20));
            e.Graphics.DrawString("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬", txtNeg, Brushes.Black, new RectangleF(70, Fil += 10, 800, 20));

            int itemPerPage = 1;  // Contador de ítems por página
            int maxItemsPerPage = (e.MarginBounds.Height - 30) / 20;  // Número máximo de ítems por página basado en la altura de la página y el espacio entre ítems

            //Recorre sobre las filas del DataGridView para imprimirlas
            for (int i = currentRow; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow xFil = dataGridView1.Rows[i];
                //columnas que se imprimen
                e.Graphics.DrawString(xFil.Cells[1].Value.ToString() + " ", txtSim, Brushes.Black, new Rectangle(80, Fil += 20, 050, 20));
                e.Graphics.DrawString(xFil.Cells[3].Value.ToString() + " ", txtSim, Brushes.Black, new Rectangle(160, Fil, 450, 20));
                //e.Graphics.DrawString(xFil.Cells[2].Value.ToString() + " ", txtSim, Brushes.Black, new Rectangle(220, Fil, 350, 20));
                e.Graphics.DrawString(xFil.Cells[4].Value.ToString() + " ", txtSim, Brushes.Black, new Rectangle(270, Fil, 350, 20));
                e.Graphics.DrawString(xFil.Cells[5].Value.ToString() + " ", txtSim, Brushes.Black, new Rectangle(380, Fil, 350, 20));
                //e.Graphics.DrawString(xFil.Cells[6].Value.ToString() + " ", txtSim, Brushes.Black, new Rectangle(580, Fil, 350, 20));
                e.Graphics.DrawString(xFil.Cells[7].Value.ToString() + " ", txtSim, Brushes.Black, new Rectangle(500, Fil, 350, 20));
                //e.Graphics.DrawString(xFil.Cells[9].Value.ToString() + " ", txtSim, Brushes.Black, new Rectangle(620, Fil, 350, 20));
                itemPerPage++;
                if (itemPerPage >= maxItemsPerPage)// Verificar si se alcanzó el máximo de ítems por página
                {
                    //Imprime pie de pagina y prepara la siguiente
                    Fil = (e.MarginBounds.Height + 100);
                    e.Graphics.DrawString("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬", txtNeg, Brushes.Black, new RectangleF(070, Fil += 15, 800, 20));
                    e.Graphics.DrawString("☏ " + Tel + " - ☒" + Cor + " - ⚑" + Dir, txtNeg, Brushes.Black, (e.PageBounds.Width - txtSize4.Width) / 2, Fil += 15);
                    e.Graphics.DrawString(Lem, txtNeg, Brushes.Black, (e.PageBounds.Width - txtSize5.Width) / 2, Fil += 20);

                    currentRow = i + 1;  // Guardar la posición actual para continuar en la siguiente página
                    e.HasMorePages = true;  /* Indicar que hay más páginas */
                    return;
                }
            }
            //Imprime pie final en la ultma página
            e.Graphics.DrawString("Total Registro: " + Convert.ToString(dataGridView1.Rows.Count) + " ", txtNeg, Brushes.Black, new Rectangle(400, Fil += 30, 200, 20));

            Fil = (e.MarginBounds.Height + 75);
            e.Graphics.DrawString("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬", txtNeg, Brushes.Black, new RectangleF(070, Fil += 15, 800, 20));
            e.Graphics.DrawString("☏ " + Tel + " - ☒" + Cor + " - ⚑" + Dir, txtNeg, Brushes.Black, (e.PageBounds.Width - txtSize4.Width) / 2, Fil += 15);
            e.Graphics.DrawString(Lem, txtNeg, Brushes.Black, (e.PageBounds.Width - txtSize5.Width) / 2, Fil += 20);

            currentRow = 0;  // Reiniciar el contador de filas si no hay más páginas
            e.HasMorePages = false;  // Indicar que ya no hay más páginas         }
        }
    }
}

