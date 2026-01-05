using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ProgramacionInstructores
{
    public partial class FrmInstructores : Form
    {
        public FrmInstructores()
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

        Complementos ONTEnter = new Complementos(); // Carga la clase Complementos con el nombre ONTEnter
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
            foreach (var Obj in this.Controls.OfType<TextBox>())
            {
                LenC = Conn.LenCampos("Instructores");
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
                if (Obj is TextBox || Obj is ComboBox)   //Tipo Textbox y Combobox
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
                if (Obj is TextBox || Obj is ComboBox) //Tipo TextBox y ComboBox
                {
                    Obj.Enabled = ActDes; //Deshabilita todos los Objetos TextBox y ComboBox
                    if (Opc == "1" && Obj.TabIndex > 0) { Obj.Text = string.Empty; }
                }
            }
        }

        private void MostrarDatos()
        {
            Conn.ConsultaDatos(
                "SELECT I.IdPersona, P.Documento, P.Nombre, P.Apellido, I.IdInstructor, I.ObservacionPerfil " +
                "FROM Instructores I " +
                "INNER JOIN Personas P ON I.IdPersona = P.IdPersona " +
                "ORDER BY I.IdPersona", "Instructores");

            dataGridView1.DataSource = Conn.Ds.Tables["Instructores"];

            dataGridView1.Columns[0].HeaderText = "ID Persona";
            dataGridView1.Columns[1].HeaderText = "Documento";
            dataGridView1.Columns[2].HeaderText = "Nombre";
            dataGridView1.Columns[3].HeaderText = "Apellido";
            dataGridView1.Columns[4].HeaderText = "ID Instructor";
            dataGridView1.Columns[5].HeaderText = "Observación";

            dataGridView1.Columns[0].Width = 100;
            dataGridView1.Columns[1].Width = 100;
            dataGridView1.Columns[2].Width = 150;
            dataGridView1.Columns[3].Width = 150;
            dataGridView1.Columns[4].Width = 100;
            dataGridView1.Columns[5].Width = 200;

            foreach (DataGridViewColumn Row in dataGridView1.Columns)
            {
                Row.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;



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
            if ((Conn.Ds.Tables[0].Rows.Count) < 1)
            {
                EstadoBton(false);
                BtoNue.Enabled = true;
                BtoSal.Image = imageList1.Images[7];
                BtoSal.Text = " &Salir";
                Opc = "1"; ActDesObjetos(false); Opc = "0";
                return;
            }

            fila = Conn.Ds.Tables[0].Rows[Pos];
            textBox1.Text = Convert.ToString(fila["IdPersona"]).Trim();

            // Obtener datos informativos de la persona
            string idPersona = textBox1.Text.Trim();
            ConnAux.ConsultaDatos("SELECT Documento, Nombre, Apellido FROM Personas WHERE IdPersona = '" + idPersona + "'", "Personas");
            if (ConnAux.Ds.Tables["Personas"].Rows.Count > 0)
            {
                DataRow filaPer = ConnAux.Ds.Tables["Personas"].Rows[0];
                textBox2.Text = filaPer["Documento"].ToString().Trim();
                textBox3.Text = filaPer["Nombre"].ToString().Trim();
                textBox4.Text = filaPer["Apellido"].ToString().Trim();
                textBox5.Text = fila["ObservacionPerfil"].ToString().Trim();

            }
            else
            {
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
            }

            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;

            dataGridView1.Rows[Pos].Selected = true;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }


        private void EstadoBton(Boolean ActDes) //Procedimiento para Activar o Desactivar los Botones
        {
            foreach (Control Obj in this.Controls.Cast<Control>().Union(panel1.Controls.Cast<Control>())) //Recorre todos los Objetos de tipo control           
            {
                if (Obj is Button) { Obj.Enabled = ActDes; }
            }
            foreach (Control Obj in this.Controls) { if (Obj is Button) { Obj.Enabled = ActDes; } }
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
                Opc = "1"; //Identifica el Tipo de Operación Opc="1" --> Nuevo
                EstadoBton(true); //Ejecuta el procedimiento EstadoBton (Activar o Deshabilitar botones).. aca los activa por ser true.
                textBox1.Enabled = true; //Habilita el textBox
                textBox1.Text = string.Empty; //Limpia el TextBox con Vacio tipo texto
                string ruta = Conexion.VarGlobal.xRuta + "SinFoto.Png";

                textBox1.Focus(); //Ubica el Cursor en el TextBox Seleccionado
                dataGridView1.Enabled = false; dataGridView1.Enabled = false;
                textBox2.Text = string.Empty;
                textBox3.Text = string.Empty;
                textBox4.Text = string.Empty;
                ValDocUser = textBox2.Text.Trim(); // Guardar el numero de documento del usuario para evitar duplicados
                ValNomUser = string.Empty; // Guardar el Normbre del usuario para evitar duplicados
                if (Conexion.VarGlobal.xNomU == "Invitado") { Conexion.VarGlobal.xNomU = ""; } //Solo una Oport. para entrar

            }
            else { MessageBox.Show("Solo Administrador y usuarios registrados accenden a esta funcion..!"); }
        }

        private void BtoEdi_Click(object sender, EventArgs e)
        {
            if (Conexion.VarGlobal.xTipoU == "01" && Conexion.VarGlobal.xEstIni == 2)
            {
                Opc = "2";  //Identifica el Tipo de Operación Opc="2" --> Editar
                EstadoBton(false); // Habilitar los Objetos Botones
                BtoGua.Enabled = true; //habilitar Boton Guardar
                ActDesObjetos(true); // Habilitar los Objetos Cuadros de Textos
                textBox1.Enabled = false; //Deshabilita el cuadro de texto Principal
                textBox2.Enabled = false; textBox3.Enabled = false; textBox4.Enabled = false;
               
                dataGridView1.Enabled = false;

                //comboBox3.SelectedIndex = 0;

                ValDocUser = textBox2.Text.Trim(); // Guardar el numero de documento del usuario para evitar duplicados
            }
            else { MessageBox.Show("Solo Administradores y usuarios registrados acceden a esta función..!"); }
        }


        private void BtoGua_Click(object sender, EventArgs e)
        {
           
                if (Opc == "1")
                {
                string Agregar = "INSERT INTO Instructores (IdPersona, IdInstructor, ObservacionPerfil) " +
                                 "VALUES (@IdPersona, @IdInstructor, @ObservacionPerfil)";
                Conexion.ConDB.Open();
                    using (SqlCommand cmd = new SqlCommand(Agregar, Conexion.ConDB))
                    {
                        
                        cmd.Parameters.AddWithValue("@IdPersona", textBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@IdInstructor", textBox1.Text.Trim()); // o como definas este ID
                        cmd.Parameters.AddWithValue("@ObservacionPerfil", textBox5.Text.Trim());



                    //   cmd.Parameters.AddWithValue("@Est", 0);

                    //cmd.Parameters.AddWithValue("@Hue", imagenBytes2);
                    cmd.ExecuteNonQuery();

                        string xId = textBox1.Text.Trim(); MostrarDatos();
                        Conn.Ds.Tables[0].PrimaryKey = new DataColumn[] { Conn.Ds.Tables[0].Columns["IdPersona"] };
                        Pos = Conn.Ds.Tables[0].Rows.IndexOf(Conn.Ds.Tables[0].Rows.Find(xId));
                        CargarDatos(Pos);
                    }
                    Conexion.ConDB.Close();
                }
                if (Opc == "2")
                {

                string Actualizar = "UPDATE Instructores SET ObservacionPerfil = @ObservacionPerfil WHERE IdPersona = @IdPersona";
                Conexion.ConDB.Open();
                    using (SqlCommand cmd = new SqlCommand(Actualizar, Conexion.ConDB))
                    {
                    cmd.Parameters.AddWithValue("@IdPersona", textBox1.Text.Trim());
                    cmd.Parameters.AddWithValue("@ObservacionPerfil", textBox5.Text.Trim());



                    cmd.ExecuteNonQuery();
                    }

                    Conexion.ConDB.Close(); int NPos = Pos; MostrarDatos(); CargarDatos(NPos);
                }
                dataGridView1.Enabled = true;
                BtoSal.Text = "&Cerrar";
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
                        string reg = "IdPersona='" + textBox1.Text.Trim() + "'";
                        Conn.Eliminar("Instructores", reg); // ✅ Cambiado a la tabla correcta
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
            int BackPos = Pos;
            string Reg = "IdPersona='" + textBox1.Text.Trim() + "'";

            if (Conn.ConsultaItem("Personas", Reg))
            {
                // Habilitar campos de usuario
                ActDesObjetos(true);
                textBox1.Enabled = false;
                

                // ⚠️ Obtener datos de la tabla Personas
                ConnAux.ConsultaDatos("SELECT Documento, Nombre, Apellido FROM Personas WHERE IdPersona = '" + textBox1.Text.Trim() + "'", "Personas");
                DataRow filaPer = ConnAux.Ds.Tables["Personas"].Rows[0];

                // Mostrar en campos informativos
                textBox2.Text = filaPer["Documento"].ToString().Trim();
                textBox3.Text = filaPer["Nombre"].ToString().Trim();
                textBox4.Text = filaPer["Apellido"].ToString().Trim();

                // Deshabilitar edición
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
            }
            else
            {
                MessageBox.Show("La persona con ese ID no está registrada.\nPor favor, regístrela primero en la tabla Personas.",
                                "Persona no encontrada", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                textBox1.Focus();
                Pos = BackPos;
                this.BtoSal_Click(null, null);
                textBox2.Enabled = false; textBox3.Enabled = false; textBox4.Enabled = false;
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
            if (swTimer == 1)
            {
                swTimer = 0;
                //ConnAux.ConsultaDatos("Select (IdLugar+' | '+LTrim(RTrim(Nombre))) As Nombre From Municipios " +
                //    "Where IdDpto = '" + comboBox2.Text.Substring(0, 2) + "' Order By Nombre", "Municipios");
                //comboBox3.DataSource = ConnAux.Ds.Tables["Municipios"];
                //comboBox3.DisplayMember = "Nombre";
                //comboBox3.Refresh();
            }
            if (swTimer == 2) { swTimer = 0; }
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

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

        }
   

        //private void label1_Click(object sender, EventArgs e)
        //{

        //}

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
            e.Graphics.DrawString(" IdInstructor   │   Nombre  |        Apellido       |         Obs. Perfil      |     Telefono     |    IdLugar                     ",
                txtNeg, Brushes.Black, new RectangleF(70, Fil += 15, 900, 20));
            e.Graphics.DrawString("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬", txtNeg, Brushes.Black, new RectangleF(70, Fil += 10, 800, 20));

            int itemPerPage = 1;  // Contador de ítems por página
            int maxItemsPerPage = (e.MarginBounds.Height - 30) / 20;  // Número máximo de ítems por página basado en la altura de la página y el espacio entre ítems

            //Recorre sobre las filas del DataGridView para imprimirlas
            for (int i = currentRow; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow xFil = dataGridView1.Rows[i];
                //columnas que se imprimen
                e.Graphics.DrawString(xFil.Cells[4].Value.ToString() + " ", txtSim, Brushes.Black, new Rectangle(80, Fil += 20, 050, 20));
                e.Graphics.DrawString(xFil.Cells[2].Value.ToString() + " ", txtSim, Brushes.Black, new Rectangle(160, Fil, 350, 20));

                e.Graphics.DrawString(xFil.Cells[3].Value.ToString() + " ", txtSim, Brushes.Black, new Rectangle(250, Fil, 450, 20));
                //e.Graphics.DrawString(xFil.Cells[2].Value.ToString() + " ", txtSim, Brushes.Black, new Rectangle(220, Fil, 350, 20));
                e.Graphics.DrawString(xFil.Cells[4].Value.ToString() + " ", txtSim, Brushes.Black, new Rectangle(350, Fil, 350, 20));
                e.Graphics.DrawString(xFil.Cells[5].Value.ToString() + " ", txtSim, Brushes.Black, new Rectangle(450, Fil, 350, 20));
                //e.Graphics.DrawString(xFil.Cells[6].Value.ToString() + " ", txtSim, Brushes.Black, new Rectangle(580, Fil, 350, 20));
                //e.Graphics.DrawString(xFil.Cells[7].Value.ToString() + " ", txtSim, Brushes.Black, new Rectangle(500, Fil, 350, 20));
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

