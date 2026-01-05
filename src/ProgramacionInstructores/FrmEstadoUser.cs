using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ProgramacionInstructores
{
    public partial class FrmEstadoUser : Form
    {
        public FrmEstadoUser()
        {
            InitializeComponent();
        }
        Conexion Conn = new Conexion(); //Define la variable "Conn" para la conexion con la base de datos
        int Pos = 0; //Define variables tipo netero llamada pos e inicializada en cero tipo publico 
        string Opc = "0"; // Define Variable tipo texto llamada Opc e inicializada en "0" ---> Tipo Texto

        Complementos ONTEnter = new Complementos(); // Carga la clase Complementos con el nombre ONTEnter
        DataRow fila;  // controla cada registro de la tabla

        private void FrmEstadoUser_Load(object sender, EventArgs e)
        {
            Conn.CargarEstilosDesdeBD(); // 🔁 Carga desde la base de datos a VarGlobal
            // Aplica automáticamente los estilos desde VarGlobal

            EstilosGlobales.AplicarEstiloBoton(BtoEdi, "Editar");
            EstilosGlobales.AplicarEstiloBoton(BtoGua, "Guardar");
            EstilosGlobales.AplicarEstiloBoton(BtoCon, "Consultar");
            EstilosGlobales.AplicarEstiloBoton(BtoSal, "Salir");

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

            EstadoBton(true); Conn.Conectar(); MostrarDatos(); //Muestra los datos de la tabla
            BtoGua.Enabled = false;
        }
        private void MostrarDatos()
        {
            Conn.ConsultaDatos(
            "SELECT U.IdPersona, P.Documento, (P.Nombre + P.Apellido) AS Nombres, U.Estado " +
            "FROM Usuarios U " +
            "INNER JOIN Personas P ON U.IdPersona = P.IdPersona " +
            "ORDER BY U.IdPersona", "Usuarios");

            dataGridView1.DataSource = Conn.Ds.Tables["Usuarios"];

            //Conn.ConsultaDatos("Select IdPersona, Estado From Usuarios Order By IdPersona", "Usuarios");
            //comboBox1.DataSource = Conn.Ds.Tables["Usuarios"];
            //dataGridView1.DataSource = Conn.Ds.Tables["Usuarios"];
            comboBox1.Enabled = false;


            //DataGridView1.Enabled = true;

            dataGridView1.Columns[0].Width = 100;
            dataGridView1.Columns[1].Width = 100;
            dataGridView1.Columns[2].Width = 190;
            dataGridView1.Columns[3].Width = 76;

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

            if (Opc == "0") ;  //Ejecuta el procedimiento por primera y solo una vez -- Compara si es igual a Cero para ejecutarlo...
            { CargarDatos(Pos); } // Procedimiento que Carga en los TextBox los Datos segun el registro Pos
        }
        private void CargarDatos(int Pos)
        {
            dataGridView1.ClearSelection();
            if ((Conn.Ds.Tables[0].Rows.Count) < 1) // dt.Rows.Count < 1) //Controlar si hay registros en la TABLA
            {
                EstadoBton(false); //Procedimientos que desactiva todos los Botones porque ActDes = False
                BtoSal.Image = imageList1.Images[7];  //Cambia la Imagen del Boton 7 Salir - Imagen 7 = Salir
                BtoSal.Text = " &Salir"; //
                return; //Sale del procedimiento y no ejecuta el resto de los pasos.
            }
            fila = Conn.Ds.Tables[0].Rows[Pos];//Guarda el Registro (pos) en la variable "fila"
            textBox1.Text = Convert.ToString(fila["IdPersona"]).Trim();
            textBox2.Text = Convert.ToString(fila["Documento"]).Trim();
            textBox3.Text = Convert.ToString(fila["Nombres"]).Trim(); 

            if (fila["Estado"].ToString() == "0") { comboBox1.SelectedIndex = 0; }
            else { comboBox1.SelectedIndex = 1; }

            dataGridView1.Rows[Pos].Selected = true; //El Grid en la Fila con posición X "Seleccionela" (true/false)
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
            Pos = dataGridView1.CurrentCell.RowIndex;
            Pri.Enabled = true; Ant.Enabled = true; Sig.Enabled = true; Ult.Enabled = true;
            if (Pos == 0) { Pri_Click(null, null); }
            if (Pos >= Conn.Ds.Tables[0].Rows.Count - 1) { Ult_Click(null, null); }
            CargarDatos(Pos);
        }

        private void BtoEdi_Click(object sender, EventArgs e)
        {
            if (Conexion.VarGlobal.xTipoU == "01" && Conexion.VarGlobal.xEstIni == 2)
            {
                Opc = "2";  //Identifica el Tipo de Operación Opc="2" --> Editar
                EstadoBton(false); // Habilitar los Objetos Botones
                BtoGua.Enabled = true; //habilitar Boton Guardar
                textBox1.Enabled = false;
                comboBox1.SelectedIndex = 0;
                comboBox1.Enabled = true; //Deshabilita el cuadro de texto Principal     
                comboBox1.Focus(); 

            }

            else { MessageBox.Show("Solo Administradores y usuarios registrados acceden a esta función..!"); }
        }
        private void BtoGua_Click(object sender, EventArgs e)
        {
            if (Opc == "2")
            {
                string Actualizar = "Estado = '" + comboBox1.SelectedIndex + "'";

                if (Conn.Actualizar("Usuarios", Actualizar, "IdPersona='" + textBox1.Text.Trim() + "'"))
                { int NPos = Pos; MostrarDatos(); CargarDatos(NPos); }
                else { MessageBox.Show("Error al Actualizar"); }

            }
            dataGridView1.Enabled = true; this.BtoSal_Click(null, null);
        }


        private void BtoCon_Click(object sender, EventArgs e)
        {
            Opc = "4"; //Identifica el proceso CONSULTA
            EstadoBton(false); // Ejecuta el procedimiento EstadoBton (Activar o Deshabilitar botones)...
            textBox1.Enabled = true; // Habilita el cuadro de texto
            textBox1.Text = String.Empty; //Limpia el cuadro de Texto
            textBox1.Focus(); //Ubica el Cursor en el Textbox
        }

        private void BtoSal_Click(object sender, EventArgs e)
        {
            dataGridView1.Enabled = true;
            Opc = string.Empty; //Inicializa variable en el procedimiento
            if (BtoSal.Text.Trim() == "&Cerrar")

            {
                EstadoBton(true); //Ejecuta el procedimiento EstadoBton
                CargarDatos(Pos); BtoGua.Enabled = false;// Deshabilita el botón
                textBox1.Enabled = false; comboBox1.Enabled = false;
            }

            else { Conexion.VarGlobal.xImaFon = 1; this.Dispose(); } //Libera recursos y cierra el formuklario
        }

        private void textBox1_Validated(object sender, EventArgs e)
        {
            int BakPos = Pos; //Captura la posición Actual --- Cerrar sin guardar...
            string reg = "IdUsuario='" + textBox1.Text.Trim() + "'";
            if (Conn.ConsultaItem("Usuarios", reg))
            {
                Conn.Ds.Tables[0].PrimaryKey = new DataColumn[] { Conn.Ds.Tables[0].Columns["IdUsuario"] }; //Crea
                Pos = Conn.Ds.Tables[0].Rows.IndexOf(Conn.Ds.Tables[0].Rows.Find(textBox1.Text.Trim()));
                CargarDatos(Pos); //MessageBox.Show("Registro Existente en la base de datos")
            }

            else { MessageBox.Show("Registro NO ENCONTRADO"); textBox1.Focus(); Pos = BakPos; }
            this.BtoSal_Click(null, null);

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && textBox1.Text.Trim() == string.Empty)
            {
                textBox1.Focus(); MessageBox.Show("Digite el Id");
            }
        }

        private void textBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && textBox1.Text.Trim() == string.Empty)
            {
                textBox1.Focus(); MessageBox.Show("Digite el Id");
            }
        }


    }
}
