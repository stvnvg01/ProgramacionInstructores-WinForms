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
    public partial class FrmRestaurarCS : Form
    {
        public FrmRestaurarCS()
        {
            this.TopLevel = true;
            InitializeComponent();
        }
        OpenFileDialog BuscarFile = new OpenFileDialog();

        private void FrmRestaurarCS_Load(object sender, EventArgs e)
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
                if (Obj is Label) { Obj.ForeColor = Conexion.VarGlobal.xColorFont; }
            }
            //this.BackColor = Conexion.VarGlobal.xColorFondo;
            //this.panel1.BackColor = Conexion.VarGlobal.xColorPanel;
            //this.label1.BackColor = Conexion.VarGlobal.xColorTitle;

            //Asigna el nombre de la base de datos al textbox
            textBox1.Text = Conexion.VarGlobal.xNomDB;
            textBox1.Enabled = false;
            //Verifica si la carpeta de respaldo existe, si no, la crea
            if (!Directory.Exists(Application.StartupPath + "\\Backup\\"))
            {
                Directory.CreateDirectory(Application.StartupPath + "\\Backup\\");
            }
            //Asigna la ruta de la carpeta de eresplado al textbox 2 y lo deshabilita
            textBox2.Text = Application.StartupPath + "\\Backup\\";
            textBox2.Enabled = false;

        }

        //Evento para buscar un archivo de respaldo (.bak)
        private void BtoBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                BuscarFile.FileName = ""; // Limpia el nombre de archivo seleccionado
                BuscarFile.InitialDirectory = textBox2.Text; // Establece la carpeta inicial de búsqueda
                BuscarFile.Title = "Buscando Archivos de Copias de Seguridad"; // Título del cuadro de diálogo

                // Define los filtros para el cuadro de diálogo de selección de archivos
                string Filtro = "Tipos de Archivo (.bak) |.bak; *.bak|" +
                                "Todos los Archivos (.) |.";
                BuscarFile.Filter = Filtro;

                // Si el usuario cancela la selección, se sale del método
                if (BuscarFile.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
                else
                {
                    textBox2.Text = BuscarFile.FileName; // Asigna la ruta del archivo seleccionado al TextBox
                }
            }
            catch (Exception es)
            {
                MessageBox.Show(es.ToString()); // Muestra un mensaje de error en caso de excepción
            }
        }
        // Evento para restaurar la base de datos desde un archivo de respaldo
        private void BtoRestaurar_Click(object sender, EventArgs e)
        {
            // Verifica si se ha seleccionado un archivo de respaldo, si no, muestra un mensaje y enfoca el botón de búsqueda
            if (BuscarFile.FileName == "")
            {
                MessageBox.Show("Buscar el archivo del backup..!");
                BtoBuscar.Focus();
                return;
            }

            // Indica visualmente que se está ejecutando la restauración
            BtoRestaurar.Text = "Ejecutando...";

            // Comando SQL para cerrar la base de datos antes de restaurarla
            string sClosDB = "ALTER DATABASE " + this.textBox1.Text.Trim() + " SET OFFLINE WITH ROLLBACK IMMEDIATE";

            // Comando SQL para restaurar la base de datos desde el archivo seleccionado
            string sBackup = "RESTORE DATABASE " + this.textBox1.Text.Trim() +
                             " FROM DISK = N'" + textBox2.Text.Trim() + "'" +
                             " WITH File = 1, NoUnLoad, Stats = 50, REPLACE";

            try
            {
                // Abre la conexión con la base de datos
                Conexion.ConDB.Open();

                // Ejecuta el comando para cerrar la base de datos de forma inmediata
                SqlCommand cmdClosDB = new SqlCommand(sClosDB, Conexion.ConDB);
                cmdClosDB.ExecuteNonQuery();

                // Ejecuta el comando para restaurar la base de datos desde el archivo de respaldo
                SqlCommand cmdBackUp = new SqlCommand(sBackup, Conexion.ConDB);
                cmdBackUp.ExecuteNonQuery();

                // Muestra un mensaje de éxito cuando la restauración se completa
                MessageBox.Show("Se ha restaurado la copia de la base de datos.",
                                "Restaurar base de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Cierra la conexión con la base de datos
                Conexion.ConDB.Close();

                // Cambia el texto del botón para indicar que ahora se usa para salir/cerrar
                BtoCancelar.Text = " &Salir/Cerrar";
            }
            catch (Exception ex) // Captura errores y muestra un mensaje al usuario
            {
                MessageBox.Show(ex.Message,
                                "Error al restaurar la base de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Restaura el texto original del botón después de finalizar la ejecución
            BtoRestaurar.Text = " &Restaurar";
        }

        private void BtoCancelar_Click(object sender, EventArgs e)
        {
            Conexion.VarGlobal.xImaFon = 1;
            this.Dispose();
        }
    }
}
