using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramacionInstructores
{
    public partial class FrmEmpresa : Form
    {
        public FrmEmpresa()
        {
            InitializeComponent();
        }

        //Reccuerde el orden de tabulacion - El id es Tabindex Cero(0)
        Conexion Conn = new Conexion(); //Define la variable "Conn" para la conexion con la base de datos
        Conexion ConnAux = new Conexion();
        string Opc = "0"; // Define Variable tipo texto llamada Opc e inicializada en "0" ---> Tipo Texto
        

        Complementos ONTEnter = new Complementos(); // Carga la clase Complementos con el nombre ONTEnter
        DataRow fila;
        int swTimer = 0;
        ColorDialog colorDialog = new ColorDialog();


        private void FrmEmpresas_Load(object sender, EventArgs e)
        {
 

            SqlDataReader LenC;
            foreach (var Obj in this.Controls.OfType<TextBox>())
            {
                LenC = Conn.LenCampos("Empresa");
                while (LenC.Read())
                {
                    if (Obj.Tag != null && LenC.GetValue(0).ToString() == Obj.Tag.ToString())
                    {
                        Obj.Text = LenC["LenCampo"].ToString(); Obj.MaxLength = Convert.ToInt32(LenC["LenCampo"]);
                        break;
                    }
                }
            }



            foreach (Control Obj in this.Controls)
            {
                if (Obj is TextBox || Obj is ComboBox)
                {
                    Obj.KeyDown += new KeyEventHandler(Complementos.PCEnter);
                    Obj.TextChanged += new EventHandler(Complementos.ValChar);
                    Obj.GotFocus += new EventHandler(Complementos.RecibeFoco);
                    Obj.LostFocus += new EventHandler(Complementos.PerderFoco);
                }
                //if (Obj is Label) { Obj.ForeColor = Conexion.VarGlobal.xColorFont; }

            }
            //this.BackColor = Conexion.VarGlobal.xColorFondo;
            //this.panel1.BackColor = Conexion.VarGlobal.xColorPanel;
            //this.label1.BackColor = Conexion.VarGlobal.xColorTitle;
            Conn.Conectar();
            Conn.CargarEstilosDesdeBD();
            EstilosGlobales.AplicarEstiloBoton(BtoEdi, "Editar");

            EstilosGlobales.AplicarEstiloBoton(BtoGua, "Guardar");

            EstilosGlobales.AplicarEstiloBoton(BtoSal, "Salir");
            MostrarDatos();
            ActDesObjetos(false);
            BtoGua.Enabled = false;
        }

        private void ActDesObjetos(Boolean ActDes)
        {
            foreach (Control Obj in this.Controls.Cast<Control>().Union(panel1.Controls.Cast<Control>())) //Recorre todos los Objetos de tipo control           
            {
                    if (Obj is TextBox || Obj is ComboBox) { Obj.Enabled = ActDes; }
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

            Conn.ConsultaDatos("Select * From Empresa", "Empresa");

            if (Opc == "0")  //Ejecuta el procedimiento por primera y solo una vez -- Compara si es igual a Cero para ejecutarlo...
            { CargarDatos(0); } // Procedimiento que Carga en los TextBox los Datos segun el registro Pos
        }

        private void CargarDatos(int Pos)
        {
            fila = Conn.Ds.Tables[0].Rows[Pos];//Guarda el Registro (pos) en la variable "fila"
            textBox1.Text = Convert.ToString(fila["IdEmpresa"]).Trim(); // Carga los datos del Campo al textBox 
            textBox2.Text = Convert.ToString(fila["NombreE"]).Trim();
            textBox3.Text = Convert.ToString(fila["NombreP"]).Trim();
            textBox4.Text = Convert.ToString(fila["Telefono"]).Trim();
            textBox5.Text = Convert.ToString(fila["Correo"]).Trim();
            txtPinCor.Text = Convert.ToString(fila["PinCor"]).Trim();
            textBox6.Text = Convert.ToString(fila["UbicaDir"]).Trim();
            textBox7.Text = Convert.ToString(fila["Lema"]).Trim();
            //textBox8.Text = Convert.ToInt32(fila["NFactura"]).ToString("N0");

            try
            {
                comboBox1.SelectedIndex = comboBox1.FindString(((string)fila["IdLugar"]).Substring(0, 2));
                comboBox2.SelectedIndex = comboBox2.FindString(((string)fila["IdLugar"]).Substring(0, 5));
            }
            catch { }
            pictureBox1.Image = null; pictureBox2.Image = null;

            try
            {
                byte[] PicF = (byte[])fila["Logo"];
                MemoryStream CargarLog = new MemoryStream(PicF);
                pictureBox1.Image = Image.FromStream(CargarLog);
                pictureBox3.Image = pictureBox1.Image;
            }
            catch { }
            try
            {
                byte[] PicF = (byte[])fila["Fondo"];
                MemoryStream CargarFon = new MemoryStream(PicF);
                pictureBox2.Image = Image.FromStream(CargarFon);
            }
            catch { }
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

        private void BtoEdi_Click(object sender, EventArgs e)
        {
            if (Conexion.VarGlobal.xTipoU == "01" && Conexion.VarGlobal.xEstIni == 02)
            {
                EstadoBton(false);
                BtoGua.Enabled = true;
                ActDesObjetos(true);
                textBox1.Focus();
                BtoVerLogo.Enabled = true;
                BtoVerFondo.Enabled = true;

                
            }
            else { MessageBox.Show("Solo Administrador y usuarios registrados accenden a esta funcion..!"); }
        }

        static byte[] ImageToByteArray(Image imagen)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                imagen.Save(ms, imagen.RawFormat); return ms.ToArray();
            }
        }

        private void BtoGua_Click(object sender, EventArgs e)
        {
            byte[] imagenBytes1 = null; byte[] imagenBytes2 = null;
            if (pictureBox1.Image == null) { pictureBox1.Load(Conexion.VarGlobal.xRuta + "Vacio.Png"); }
            if (pictureBox2.Image == null) { pictureBox2.Load(Conexion.VarGlobal.xRuta + "Vacio.Png"); }
            imagenBytes1 = ImageToByteArray(pictureBox1.Image);
            imagenBytes2 = ImageToByteArray(pictureBox2.Image);

            string Actualizar = "Update Empresa Set IdEmpresa = @IdE, NombreE = @NEm, NombreP = @NPr, Telefono =  @Tel, " +
                                               "Correo = @Cor, PinCor = @PCo, UbicaDir = @Ubi, IdLugar = @IdL, Lema = @Lem, Logo = @Log, Fondo = @Fon ";
                                               
            Conexion.ConDB.Open();
            using (SqlCommand cmd = new SqlCommand(Actualizar, Conexion.ConDB))
            {
                cmd.Parameters.AddWithValue("@IdE", textBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@NEm", textBox2.Text.Trim());
                cmd.Parameters.AddWithValue("@NPr", textBox3.Text.Trim());
                cmd.Parameters.AddWithValue("@Tel", textBox4.Text.Trim());
                cmd.Parameters.AddWithValue("@Cor", textBox5.Text.Trim());
                cmd.Parameters.AddWithValue("@PCo", txtPinCor.Text.Trim());
                cmd.Parameters.AddWithValue("@Ubi", textBox6.Text.Trim());
                cmd.Parameters.AddWithValue("@IdL", comboBox2.Text.Substring(0, 5));
                cmd.Parameters.AddWithValue("@Lem", textBox7.Text.Trim());
                cmd.Parameters.AddWithValue("@Log", imagenBytes1);
                cmd.Parameters.AddWithValue("@Fon", imagenBytes2);
                cmd.ExecuteNonQuery();
            }
            Conexion.ConDB.Close();
            Conexion.VarGlobal.xLogo = imagenBytes1; Conexion.VarGlobal.xFondo = imagenBytes2;
            //BtoVerLogo.Enabled = false;
            //BtoVerFondo.Enabled = false;
            this.BtoSal_Click(null, null);
        }

        private void BtoSal_Click(object sender, EventArgs e)
        {
            Opc = string.Empty;

            if (BtoSal.Text.Trim() == "&Cerrar")
            {
                EstadoBton(true);
                MostrarDatos(); CargarDatos(0); ActDesObjetos(false); BtoGua.Enabled = false;
                BtoVerLogo.Enabled = false;
                BtoVerFondo.Enabled = false;

            }

            else { Conexion.VarGlobal.xImaFon = 1; this.Dispose(); }
        }

        private void BtoVerLogo_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog AbrirImagen = new OpenFileDialog();
                AbrirImagen.FileName = "";
                AbrirImagen.Title = "Buscando Archivos de Imagenes..!";
                string Filtro = "Todas las Imagenes (*.*) |*.bmp; *.jpg; *.jpeg; *.png; *.gif; *.tif; *.ico; " +
                                "Imagenes BMP (*bmp) |*.bmp|" +
                                "Imagenes JPG (*jpg) |*.jpg|" +
                                "Imagenes JPG (*jpeg) |*.jpeg|" +
                                "Imagenes PNG (*png) |*.png|" +
                                "Imagenes GIF (*gif) |*.gif|" +
                                "Imagenes TIF (*tif) |*.tif|" +
                                "Imagenes ICO (*ico) |*.ico|";
                AbrirImagen.Filter = Filtro;
                if (AbrirImagen.ShowDialog() == DialogResult.OK)
                { pictureBox1.Image = Image.FromFile(AbrirImagen.FileName); pictureBox3.Image = pictureBox1.Image; }
            }
            catch (Exception es) { MessageBox.Show(es.ToString()); }
        }

        private void BtoVerFondo_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog AbrirImagen = new OpenFileDialog();
                AbrirImagen.FileName = "";
                AbrirImagen.Title = "Buscando Archivos de Imagenes..!";
                string Filtro = "Todas las Imagenes (*.*) |*.bmp; *.jpg; *.jpeg; *.png; *.gif; *.tif; *.ico; " +
                                "Imagenes BMP (*bmp) |*.bmp|" +
                                "Imagenes JPG (*jpg) |*.jpg|" +
                                "Imagenes JPG (*jpeg) |*.jpeg|" +
                                "Imagenes PNG (*png) |*.png|" +
                                "Imagenes GIF (*gif) |*.gif|" +
                                "Imagenes TIF (*tif) |*.tif|" +
                                "Imagenes ICO (*ico) |*.ico|";
                AbrirImagen.Filter = Filtro;
                if (AbrirImagen.ShowDialog() == DialogResult.OK) { pictureBox2.Image = Image.FromFile(AbrirImagen.FileName); }
            }
            catch (Exception es) { MessageBox.Show(es.ToString()); }
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e) { swTimer = 1; }

        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e) { swTimer = 2; }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (swTimer == 1)
            {
                swTimer = 0;
                ConnAux.ConsultaDatos("Select (IdLugar+' | '+LTrim(RTrim(Nombre))) As Nombre From Municipios " +
                    "Where IdDpto = '" + comboBox1.Text.Substring(0, 2) + "' Order By Nombre", "Municipios");
                comboBox2.DataSource = ConnAux.Ds.Tables["Municipios"];
                comboBox2.DisplayMember = "Nombre";
                comboBox2.Refresh();
            }
            if (swTimer == 2) { swTimer = 0; }
        }

        //        {
        //    if (swTimer == 1)
        //    {
        //        swTimer = 0;
        //        ConnAux.ConsultaDatos("Select (IdLugar+' | '+LTrim(RTrim(Nombre))) As Nombre From Municipios " +
        //            "Where IdDpto = '" + comboBox2.Text.Substring(0, 2) + "' Order By Nombre", "Municipios");
        //        comboBox3.DataSource = ConnAux.Ds.Tables["Municipios"];
        //        comboBox3.DisplayMember = "Nombre";
        //        comboBox3.Refresh();
        //    }
        //    if (swTimer == 2) { swTimer = 0; }
        //}
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e) { Complementos.SoloNumE(e); }

        private void textBox7_Enter(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(textBox7.Text))
            { textBox7.SelectionStart = 0; }
        }

        private void FrmEmpresas_FormClosing(object sender, FormClosingEventArgs e)
        {
            Conexion.VarGlobal.xImaFon = 1; e.Cancel = false;
        }

        private void BtoColorF_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                Conexion.VarGlobal.xColorFondo = colorDialog.Color; this.BackColor = colorDialog.Color;
            }
        }

        private void BtoColorP_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                Conexion.VarGlobal.xColorPanel = colorDialog.Color; this.panel1.BackColor = colorDialog.Color;
            }
        }

        private void BtoColorT_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                Conexion.VarGlobal.xColorTitle = colorDialog.Color; this.label1.BackColor = colorDialog.Color;
            }
        }

        private void BtoColorL_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                Conexion.VarGlobal.xColorFont = colorDialog.Color;
                foreach(Control Obj in Controls) { if (Obj is Label) { Obj.ForeColor = Conexion.VarGlobal.xColorFont; } }
            }
        }

        //private void textBox7_TextChanged(object sender, EventArgs e)
        //{

        //}
    } 
}
