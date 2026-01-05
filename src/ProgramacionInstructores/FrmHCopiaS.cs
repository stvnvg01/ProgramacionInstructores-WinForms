using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; //Desactiva para el funcionamiento con SQLserver
using System.IO;

namespace ProgramacionInstructores
{
    public partial class FrmHCopiaS : Form
    {
        public FrmHCopiaS()
        {
            InitializeComponent();
        }

        private void FrmHCopiaS_Load(object sender, EventArgs e)
        {
            foreach (Control Obj in Controls) //Recorre todos los Objetos
            {
                if (Obj is TextBox || Obj is ComboBox) //Tipo TextBox y ComboBox
                {
                    Obj.KeyDown += new KeyEventHandler(Complementos.PCEnter); //Para pasar con Enter cada Objeto
                    Obj.TextChanged += new EventHandler(Complementos.ValChar); //Quita la comilla simple del objeto
                    Obj.GotFocus += new EventHandler(Complementos.RecibeFoco); //Cambiar el Color del objeto
                    Obj.LostFocus += new EventHandler(Complementos.PerderFoco); //Volver al color inicial del Objeto
                }
                //if (Obj is Label) { Obj.ForeColor = Conexion.VarGlobal.xColorFont; }
            }
            //this.BackColor = Conexion.VarGlobal.xColorFondo;
            //this.panel1.BackColor = Conexion.VarGlobal.xColorPanel;
            //this.label1.BackColor = Conexion.VarGlobal.xColorTitle;
            textBox1.Text = Conexion.VarGlobal.xNomDB; textBox1.Enabled = false;
            if (!Directory.Exists(Application.StartupPath + "\\Backup\\"))
            {
                Directory.CreateDirectory(Application.StartupPath + "\\Backup\\");
            }
            textBox2.Text = Application.StartupPath + "\\Backup\\"; textBox2.Enabled = false;
            textBox3.Text = Conexion.VarGlobal.xNomDB + "-" + DateTime.Now.ToString("dd-MMM-yyyy") + ".bak";


        }

        private void BtoDestino_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog SelCarpeta = new FolderBrowserDialog();
            if (SelCarpeta.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = SelCarpeta.SelectedPath + "\\";
            }
        }

        private void BtoAceptar_Click(object sender, EventArgs e)
        {

            if (textBox3.Text.ToUpper().Substring(textBox3.Text.Count() - 4, 4) != ".BAK") //Verifica si el nombre a guardar tiene la extension .bak
            {
                textBox3.Text = textBox3.Text.Trim() + ".bak"; //Si no existe se la agrega
            }

            string sBackup = "BACKUP DATABASE " + textBox1.Text.Trim() + " TO DISK = N'"
                   + textBox2.Text.Trim() + textBox3.Text.Trim() + "' WITH NOFORMAT, INIT, SKIP, STATS = 10"; // Consulta para crear el backup

            try //Controlar Errores de Ejecución
            {
                Conexion.ConDB.Open(); //Abre Base de Datos
                SqlCommand cmdBackUp = new SqlCommand(sBackup, Conexion.ConDB);
                cmdBackUp.ExecuteNonQuery();

                MessageBox.Show("Se ha Realizado un Backup de la DB...! <<" + textBox1.Text.Trim() + ">>",
                                "Copia de seguridad DB SQLServer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Conexion.ConDB.Close(); //Cierra la Base de Datos
                BtoCancelar.Text = " &Salir/Cerrar";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al copiar la base de datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtoCancelar_Click(object sender, EventArgs e)
        {
            this.Dispose(); 
        }
    }
}
