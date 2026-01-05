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
    public partial class FrmPersonas : Form
    {
        public FrmPersonas()
        {
            InitializeComponent();
        }

        //Recuerde el Orden de Tabulación - El id es TabIndex Cero (0)
        Conexion Conn = new Conexion(); // Define la variable "Conn" para la conexión con la base de datos
        Conexion ConnAux = new Conexion(); // Define la variable "Conn" para la conexión con la base de datos
        int Pos = 0; //Define variable tipo entero llamada pos e inicializada en cero tipo publico
        string Opc = "0"; //Define variable tipo texto llamada Opc e inicializada en "0" --> tipo texto
        string ValDocUser = string.Empty; //Valida el Documento del Usuario al Editar

        Complementos ONTEnter = new Complementos(); //Carga la clase complementos con el nombre ONTEnter
        DataRow fila;
        int swTimer = 0; //Cuando Hay ComboBox de Municipios
        private void FrmClientes_Load(object sender, EventArgs e)
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

            //Cada objeto textBox en la propiedad "Tag" debe contener el nombre del campo SQL
            SqlDataReader LenC;
            foreach (var Obj in this.Controls.OfType<TextBox>())
            {
                LenC = Conn.LenCampos("Personas");
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
                if (Obj is TextBox || Obj is ComboBox) //Tipo TextBox y ComboBox
                {
                    Obj.KeyDown += new KeyEventHandler(Complementos.PCEnter); //Para pasar con Enter cada Objeto
                    Obj.TextChanged += new EventHandler(Complementos.ValChar); //Quita la comilla simple del objeto
                    Obj.GotFocus += new EventHandler(Complementos.RecibeFoco); //Cambiar el Color del objeto
                    Obj.LostFocus += new EventHandler(Complementos.PerderFoco); //Volver al color inicial del Objeto
                }
                if (Obj is Label) { Obj.ForeColor = Conexion.VarGlobal.xColorFont; }
            }
            //this.BackColor = Conexion.VarGlobal.xColorFondo;
            //this.panel1.BackColor = Conexion.VarGlobal.xColorPanel;
            //this.label1.BackColor = Conexion.VarGlobal.xColorTitle;

            Conn.Conectar(); // Actualizar el estado de las bases de datos (Activada o Desactivada)
            MostrarDatos(); // Muestra los datos de la Tabla
            ActDesObjetos(false); // Activa o Deshabilita los Botones y TextBox
            BtoGua.Enabled = false;
            //dateTimePicker1.Enabled = false;
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
            ConnAux.ConsultaDatos("Select (IdDpto+' | '+LTrim(RTrim(Nombre))) As Nombre From Departamentos Order By Nombre", "Departamentos");
            comboBox1.DataSource = ConnAux.Ds.Tables["Departamentos"];
            comboBox1.DisplayMember = "Nombre";

            ConnAux.ConsultaDatos("Select (IdLugar+' | '+LTrim(RTrim(Nombre))) As Nombre From Municipios Order By Nombre", "Municipios");
            comboBox2.DataSource = ConnAux.Ds.Tables["Municipios"];
            comboBox2.DisplayMember = "Nombre";

            Conn.ConsultaDatos("Select * From Personas Order By Nombre", "Personas");
            dataGridView1.DataSource = Conn.Ds.Tables["Personas"];
            //DataGridView1.Enabled = true;

            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[0].Width = 70;
            dataGridView1.Columns[1].Width = 100;
            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[4].Width = 100;
            dataGridView1.Columns[5].Width = 60;
            dataGridView1.Columns[6].Width = 60;
            foreach (DataGridViewColumn Row in dataGridView1.Columns)
            {
                Row.SortMode = DataGridViewColumnSortMode.NotSortable; //No permite ordenar columnas
            }
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.MultiSelect = false;

            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView1.RowsDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.White;

            if (Opc == "0") //Ejecuta el procedimiento por primera y solo una vez -- Compara si es igual a Cero para ejecutarlo...
            { CargarDatos(Pos); } // Procedimiento que Carga en los TextBox los Datos segun el registro Pos
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
            textBox1.Text = Convert.ToString(fila["IdPersona"]).Trim(); // Carga los datos del Campo al textBox 
            textBox2.Text = Convert.ToString(fila["Documento"]).Trim();
            textBox3.Text = Convert.ToString(fila["Nombre"]).Trim();
            textBox4.Text = Convert.ToString(fila["Apellido"]).Trim();
            textBox5.Text = Convert.ToString(fila["Direccion"]).Trim();
            textBox6.Text = Convert.ToString(fila["Correo"]).Trim();
            textBox7.Text = Convert.ToString(fila["Telefono"]).Trim();
            textBox8.Text = Convert.ToString(fila["Observacion"]).Trim();
            comboBox3.Text = Convert.ToString(fila["Estado"]).Trim();

           

            try
            {/* dateTimePicker1.Value = Convert.ToDateTime(fila["FecNac"]);*/
                comboBox1.SelectedIndex = comboBox1.FindString(((string)fila["IdLugar"]).Substring(0, 2));
                comboBox2.SelectedIndex = comboBox2.FindString(((string)fila["IdLugar"]).Substring(0, 5));
            } catch { }

            dataGridView1.Rows[Pos].Selected = true;//El Grid en la Fila con posición X "Seleccionela" (true/false)   
            dataGridView1.CurrentCell = dataGridView1.Rows[Pos].Cells[dataGridView1.CurrentCell.ColumnIndex];
        }

        private void EstadoBton(Boolean ActDes) //Procedimiento para Activar o Desactivar los Botones
        {
            foreach (Control Obj in this.Controls.Cast<Control>().Union(panel1.Controls.Cast<Control>())) //Recorre todos los Objetos de tipo control           
            {
                if (Obj is Button) { Obj.Enabled = ActDes; }
            }
            foreach (Control Obj in this.Controls) { if (Obj is Button) { Obj.Enabled = ActDes; } }
            BtoSal.Enabled = true;  
            BtoSal.Image = imageList1.Images[7]; //Cambia la Imagen si ActDes es Igual a falso  - Imagen 7 = Salir
            BtoSal.Text = " &Salir";  //Pone texto Salir en el boton
            toolTip1.SetToolTip(BtoSal, "Salir del Formulario");
            if (ActDes == false)
            { //Compara si la variable ActDes es igual a false
                BtoSal.Image = imageList1.Images[6]; //Cambia la Imagen si ActDes es Igual a falso  - Imagen 6 = Cerrar
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
            // Cuando se navegue por el grid con las direccionales, puedes presionar ENTER para cargar datos
            Pos = dataGridView1.CurrentCell.RowIndex; //Identifica el indice o pos del registro
            Pri.Enabled = true; Ant.Enabled = true; Sig.Enabled = true; Ult.Enabled = true;
            if (Pos == 0) { Pri_Click(null, null); }
            if (Pos >= Conn.Ds.Tables[0].Rows.Count - 1) { Ult_Click(null, null); }
            CargarDatos(Pos);
        }
        private void BtoNue_Click(object sender, EventArgs e)
        {
            Opc = "1"; //Identifica el Tipo de Operación Opc="1" --> Nuevo
            EstadoBton(false); //Ejecuta el procedimiento EstadoBton (Activar o Deshabilitar botones).. aca los activa por ser true.
            textBox1.Enabled = true; //Habilita el textBox
            textBox1.Text = string.Empty; //Limpia el TextBox con Vacio tipo texto
            textBox1.Focus(); //Ubica el Cursor en el TextBox Seleccionado
            dataGridView1.Enabled = false;
            //dateTimePicker1.Enabled = true;
            ValDocUser = textBox2.Text.Trim();
        }
        private void BtoEdi_Click(object sender, EventArgs e)
        {
            Opc = "2";  //Identifica el Tipo de Operación Opc="2" --> Editar
            EstadoBton(false); // Habilitar los Objetos Botones
            BtoGua.Enabled = true; //habilitar Boton Guardar
            ActDesObjetos(true); // Habilitar los Objetos Cuadros de Textos
            textBox1.Enabled = false; //Deshabilita el cuadro de texto Principal
            textBox2.Focus(); //Ubica el curso en el Objeto
            dataGridView1.Enabled = false;
            //dateTimePicker1.Enabled = true;
            ValDocUser = textBox2.Text.Trim();
        }

        private void BtoGua_Click(object sender, EventArgs e)
        {
            //dateTimePicker1.Enabled = false;
            if (Opc == "1") // Nuevo
            {
                string Agregar = "Insert Into Personas SELECT '" +  //(IdUsuario,Nombres,Apellidos,Direccion,Correo,Telefono,IdLugar,IdPerfil,NomUser,Clave,Foto,Huella)
                textBox1.Text.Trim() + "','" + //IdClientes
                textBox2.Text.Trim() + "','" + //Documento
                textBox3.Text.Trim() + "','" + //Nombre
                textBox4.Text.Trim() + "','" + //Apellido
                textBox5.Text.Trim() + "','" + //Direccion
                textBox6.Text.Trim() + "','" + //Correo
                textBox7.Text.Trim() + "','" + //Telefono
                textBox8.Text.Trim() + "','" + //Observacion
                //dateTimePicker1.Value.ToString("dd/MM/yyyy") + "','" + //FechaNac
                comboBox2.Text.Substring(0, 5) + "','" +               //IdLugar
                comboBox3.Text + "'";                                  //ListaP

                if (Conn.Insertar(Agregar))
                {
                    string xId = textBox1.Text.Trim(); MostrarDatos();
                    Conn.Ds.Tables[0].PrimaryKey = new DataColumn[] { Conn.Ds.Tables[0].Columns["IdPersona"] }; //Crea Indice el Tabla temporal dt
                    Pos = Conn.Ds.Tables[0].Rows.IndexOf(Conn.Ds.Tables[0].Rows.Find(xId));
                    CargarDatos(Pos);
                }
                else { MessageBox.Show("Error al Agregar Registro"); }
            }
            if (Opc == "2") // Editar
            {
                string Actualizar = "Documento  = '" + textBox2.Text.Trim() + "'," +
                                    "Nombre     = '" + textBox3.Text.Trim() + "'," +
                                    "Apellido   = '" + textBox4.Text.Trim() + "'," +
                                    "Direccion  = '" + textBox5.Text.Trim() + "'," +
                                    "Correo     = '" + textBox6.Text.Trim() + "'," +
                                    "Telefono   = '" + textBox7.Text.Trim() + "'," +
                                    "Observacion= '" + textBox8.Text.Trim() + "'," +
                                    "IdLugar    = '" + comboBox2.Text.Substring(0, 5) + "'," +
                                    "Estado     = '" + comboBox3.Text.Trim() + "'"; // "')";

                if (Conn.Actualizar("Personas", Actualizar, "IdPersona='" + textBox1.Text.Trim() + "'"))
                { int NPos = Pos; MostrarDatos(); CargarDatos(NPos); MessageBox.Show("Datos Actualizados"); } 
                else { MessageBox.Show("Error al Actualizar"); }
            }
            dataGridView1.Enabled = true; this.BtoSal_Click(null, null);
        }

        private void BtoCon_Click(object sender, EventArgs e)
        {
            Opc = "4"; // Identifica el Proceso CONSULTA
            EstadoBton(false); //Ejecuta el procedimiento EstadoBton (Activar o Deshabilitar botones).. aca los activa por ser true.
            textBox1.Enabled = true; //Habilita el Cuadro de texto
            textBox1.Text = string.Empty; //Limpia el cuadro de texto
            textBox1.Focus(); //Ubica el curso en el textbox
        }

        private void BtoEli_Click(object sender, EventArgs e)
        {
            DialogResult resu = MessageBox.Show("Esta seguro de eliminar <" + textBox1.Text.Trim() +
                                                   "> de la Base de Datos?", "A D V E R T E N C I A", MessageBoxButtons.YesNo);
            if (resu == DialogResult.Yes)
                try
                {
                    string reg = "IdPersona='" + textBox1.Text.Trim() + "'";
                    Conn.Eliminar("Personas", reg);
                    Pos += -1; int PosN = Pos;
                    if (PosN < 1) { PosN = 0; }
                    MostrarDatos();
                    CargarDatos(PosN);
                }
                catch (Exception m) { Conn.Conectar(); MessageBox.Show(m.Message); }
        }

        private void BtoSal_Click(object sender, EventArgs e)
        {
            dataGridView1.Enabled = true;
            string xOp = Opc;
            Opc = string.Empty; //Inicializa variable en el procedimiento.
            if (BtoSal.Text.Trim() == "&Cerrar") //.Image == CleanEnergy.Properties.Resources._8_0_Cerrar)
            {
                EstadoBton(true); //Ejecuta el procedimiento EstadoBton (Activar o Deshabilitar botones).. aca los activa por ser true.      
                int xPos = Pos; if (xOp != "4") { MostrarDatos(); }
                CargarDatos(xPos); ActDesObjetos(false); BtoGua.Enabled = false; //Deshabilita el boton
            }
            else { Conexion.VarGlobal.xImaFon = 1; this.Dispose(); } //Libera Recursos y Cierra el Formulario
         }


        private void textBox1_Validated(object sender, EventArgs e)
        {
            int BakPos = Pos;
            string reg = "IdPersona='" + textBox1.Text.Trim() + "'";
            if (Conn.ConsultaItem("Personas", reg))
            {
                Conn.Ds.Tables[0].PrimaryKey = new DataColumn[] { Conn.Ds.Tables[0].Columns["IdPersona"] };
                Pos = Conn.Ds.Tables[0].Rows.IndexOf(Conn.Ds.Tables[0].Rows.Find(textBox1.Text.Trim()));
                this.BtoSal_Click(null, null); CargarDatos(Pos); return;
            }
            else
            {
                if (Opc == "4")
                {
                    MessageBox.Show("Resgistro NO ENCONTRADO"); textBox1.Focus(); Pos = BakPos;
                    this.BtoSal_Click(null, null);
                }
                else if (Opc == "1") // NUEVO
                {
                    ActDesObjetos(true);
                    comboBox1.SelectedIndex = 0; comboBox2.SelectedIndex = 0;
                    textBox1.Enabled = false; textBox2.Focus();
                    BtoGua.Enabled = true;                    // <-- HABILITA GUARDAR AQUÍ
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Complementos.SoloNumE(e);
            if (e.KeyChar == (char)Keys.Enter && textBox1.Text == string.Empty)
            {
                textBox1.Focus(); MessageBox.Show("Digite el Id");
            }
        }

        private void textBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && textBox1.Text == string.Empty)
            {
                MessageBox.Show("Digite el Id"); e.IsInputKey = true;
            }
        }

        private void textBox2_Validated(object sender, EventArgs e)
        {
            if (textBox2.Text == string.Empty) { textBox2.Focus(); MessageBox.Show("No puede ser vacio"); }
            else
            {
                //string reg = "Documento='" + textBox2.Text.Trim() + "'";
                //if (Conn.ConsultaItem("Personas", reg) && textBox2.Text.Trim() == ValDocUser.Trim())
                //{
                //    MessageBox.Show("El dato ya existe, Digite Otro"); textBox2.Focus();
                //}
                //else { BtoGua.Enabled = true; }
                ////Habilita el boton
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e) { Complementos.SoloNumE(e); }
        private void textBox7_KeyPress(object sender, KeyPressEventArgs e) { Complementos.SoloNumE(e); }

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

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e) { swTimer = 1; }
        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e) { swTimer = 2; }



        string IdN, NoE, NoP, Tel, Cor, Dir, Lem; //Variables para almacenar información de la empresa
        byte[] ImaLog; //Variable para almacenar el logo de la empresa en formato de bytes

        private void label13_Click(object sender, EventArgs e)
        {

        }

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
            //Dir = Convert.ToString(fila["UbicaDir"]).Trim();
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
            SizeF txtSize3 = e.Graphics.MeasureString("<< C L I E N T E S >>", txtNeg);
            SizeF txtSize4 = e.Graphics.MeasureString(Tel + " - " + Cor + " - " + Dir, txtNeg);
            SizeF txtSize5 = e.Graphics.MeasureString(Lem, txtNeg);

            int Fil = 50; //Cordenada inicial vertical para la impresión

            //(e.PageBounds.Width - textSize.Width) / 2

            //imprime encabezado de la pagina
            e.Graphics.DrawString("Pág. " + (Pag += 1), txtNeg, Brushes.Black, e.PageBounds.Width - 100, 030);
            e.Graphics.DrawImage(Log, new Rectangle(20, 20, 90, 80));
            e.Graphics.DrawString(NoE, txtNeg, Brushes.Black, (e.PageBounds.Width - txtSize1.Width) / 2, 020);
            e.Graphics.DrawString(IdN, txtNeg, Brushes.Black, (e.PageBounds.Width - txtSize2.Width) / 2, 040);
            e.Graphics.DrawString("<< C L I E N T E S >>", txtNeg, Brushes.Black, (e.PageBounds.Width - txtSize3.Width) / 2, Fil += 25);
            //Linea divisoria y encabezados de la tabla
            e.Graphics.DrawString("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬", txtNeg, Brushes.Black, new RectangleF(70, Fil += 15, 800, 20));
            e.Graphics.DrawString(" IdCliente   │    Documento   |   Nombre   |   Apellido   |   IdLugar                     ", 
                txtNeg, Brushes.Black, new RectangleF(70, Fil += 15, 900, 20));
            e.Graphics.DrawString("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬", txtNeg, Brushes.Black, new RectangleF(70, Fil += 10, 800, 20));

            int itemPerPage = 1;  // Contador de ítems por página
            int maxItemsPerPage = (e.MarginBounds.Height - 30) / 20;  // Número máximo de ítems por página basado en la altura de la página y el espacio entre ítems

            //Recorre sobre las filas del DataGridView para imprimirlas
            for (int i = currentRow; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow xFil = dataGridView1.Rows[i];
                //columnas que se imprimen
                e.Graphics.DrawString(xFil.Cells[0].Value.ToString() + " ", txtSim, Brushes.Black, new Rectangle(80, Fil += 20, 050, 20));
                e.Graphics.DrawString(xFil.Cells[1].Value.ToString() + " ", txtSim, Brushes.Black, new Rectangle(160, Fil, 450, 20));
                e.Graphics.DrawString(xFil.Cells[2].Value.ToString() + " ", txtSim, Brushes.Black, new Rectangle(280, Fil, 350, 20));
                e.Graphics.DrawString(xFil.Cells[3].Value.ToString() + " ", txtSim, Brushes.Black, new Rectangle(360, Fil, 350, 20));
                e.Graphics.DrawString(xFil.Cells[8].Value.ToString() + " ", txtSim, Brushes.Black, new Rectangle(440, Fil, 350, 20));
                itemPerPage++;
                if (itemPerPage >= maxItemsPerPage)// Verificar si se alcanzó el máximo de ítems por página
                {
                    //Imprime pie de pagina y prepara la siguiente
                    Fil = (e.MarginBounds.Height + 75);
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
