using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace ProgramacionInstructores
{
    public partial class FrmAcceso : Form
    {
        public FrmAcceso()
        {
            InitializeComponent();
        }

        Conexion Conn = new Conexion();                 // Conexión a la BD (tu clase)
        Complementos OnOff = new Complementos();        // Complementos (enter, foco, etc.)

        string Cla = string.Empty;                      // Clave en BD (encriptada)
        string Nom = string.Empty;                      // Usuario (NomUser)
        string[] ArrN;                                  // Usuarios intentados
        int[] ArrI;                                     // Intentos por usuario
        int Pos = 0;

        private void FrmAcceso_Load(object sender, EventArgs e)
        {
            ArrN = new string[100];
            ArrI = new int[100];

            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;

            if (Conn.Conectar() == true)
            {
                foreach (Control Obj in this.Controls)
                {
                    if (Obj is TextBox || Obj is ComboBox)
                    {
                        Obj.KeyDown += new KeyEventHandler(Complementos.PCEnter);
                        Obj.TextChanged += new EventHandler(Complementos.ValChar);
                        Obj.GotFocus += new EventHandler(Complementos.RecibeFoco);
                        Obj.LostFocus += new EventHandler(Complementos.PerderFoco);
                    }
                }
            }
        }

        // Validación del usuario (NomUser) — MISMA SQL que tenías
        private void textBox1_Validated(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == string.Empty) return;

            // Limpieza visual
            IdUser.Text = "IdUsuario: ";
            NomUser.Text = "Nombre: ";
            pictureBoxCir1.Load(Conexion.VarGlobal.xRuta + "SinFoto.png");

            Conn.ConsultaDatos(
                "Select * From Usuarios Where NomUser = '" + textBox1.Text.Trim() + "' Order By NomUser ",
                "Usuarios");

            string reg = "NomUser='" + textBox1.Text.Trim() + "'";
            if (!Conn.ConsultaItem("Usuarios", reg))
            {
                MessageBox.Show("Usuario no encontrado en la base de datos..!");
                textBox2.Text = string.Empty;
                textBox1.Focus();
                return;
            }

            Conn.Ds.Tables[0].PrimaryKey = new DataColumn[] { Conn.Ds.Tables[0].Columns["NomUser"] };
            int PosRow = Conn.Ds.Tables[0].Rows.IndexOf(Conn.Ds.Tables[0].Rows.Find(textBox1.Text.Trim()));
            DataRow fila = Conn.Ds.Tables[0].Rows[PosRow];

            // Estado: 0 = activo, 1 = inactivo/bloqueado
            string estado = (fila["Estado"] == DBNull.Value) ? "0" : fila["Estado"].ToString().Trim();
            if (estado != "0")
            {
                MessageBox.Show("Usuario inactivo o bloqueado. Comuníquese con el administrador.");
                textBox1.Focus();
                return;
            }

            // Captura datos del nuevo esquema
            Cla = fila["Clave"]?.ToString()?.Trim() ?? string.Empty;      // Clave encriptada
            Nom = fila["NomUser"]?.ToString()?.Trim() ?? string.Empty;    // Usuario

            // Muestra en pantalla (Id desde la nueva columna Id; nombre = NomUser)
            IdUser.Text = "IdUsuario: " + (fila["IdPersona"]?.ToString()?.Trim() ?? "");
            NomUser.Text = "Nombre: " + (fila["NomUser"]?.ToString()?.Trim() ?? "");

            // Foto (image, puede ser nula)
            if (fila.Table.Columns.Contains("Foto") && fila["Foto"] != DBNull.Value)
            {
                try
                {
                    byte[] bytes = (byte[])fila["Foto"];
                    if (bytes.Length > 0)
                    {
                        pictureBoxCir1.Image = Bitmap.FromStream(new MemoryStream(bytes));
                        Conexion.VarGlobal.xFoto = bytes;
                    }
                }
                catch
                {
                    pictureBoxCir1.Load(Conexion.VarGlobal.xRuta + "SinFoto.png");
                }
            }

            // Variables globales
            Conexion.VarGlobal.xNomU = fila["NomUser"]?.ToString()?.Trim() ?? "";
            Conexion.VarGlobal.xTipoU = fila["IdPerfil"]?.ToString()?.Trim() ?? "";

            // Nombre legible del perfil (consulta adicional – no modifica tu SELECT principal)
            Conn.ConsultaDatos("Select Nombre From Perfil_Rol Where IdPerfil = '" + Conexion.VarGlobal.xTipoU + "'", "Perfil_Rol");
            if (Conn.Ds.Tables.Count > 0 && Conn.Ds.Tables[0].Rows.Count > 0)
            {
                DataRow xNomTU = Conn.Ds.Tables[0].Rows[0];
                Conexion.VarGlobal.xTipUN = xNomTU["Nombre"].ToString().Trim();
            }

            textBox2.Focus();
        }

        // Ver / Ocultar contraseña
        private void Ver_Click(object sender, EventArgs e)
        {
            if (Ver.Text == "Ocultar")
            {
                Ver.BackgroundImage = imageList1.Images[1];
                Ver.Text = "ver";
                toolTip1.SetToolTip(Ver, "Visualizar contraseña");
                textBox2.UseSystemPasswordChar = true;
            }
            else
            {
                Ver.BackgroundImage = imageList1.Images[0];
                Ver.Text = "Ocultar";
                toolTip1.SetToolTip(Ver, "Ocultar contraseña");
                textBox2.UseSystemPasswordChar = false;
            }
        }

        // Botón Ingresar
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == string.Empty)
            {
                textBox1.Focus();
                MessageBox.Show("Debe digitar el usuario.");
                return;
            }

            int sw = 0; int PosAux = 0;
            for (int k = 0; k < ArrN.Length; k++)
            {
                if (ArrN[k] == textBox1.Text.Trim()) { ArrI[k] = ArrI[k] + 1; PosAux = k; sw = 0; break; }
                else { sw = 1; }
            }
            if (sw == 1) { ArrN[Pos] = textBox1.Text.Trim(); ArrI[Pos] = ArrI[Pos] + 1; PosAux = Pos; Pos++; }

            // Bloqueo por intentos
            if (ArrI[PosAux] >= 4)
            {
                string Actualizar = "Estado = 1"; // 1 = bloqueado
                Conn.Actualizar("Usuarios", Actualizar, "NomUser= '" + (Nom ?? "").Replace("'", "") + "'");
                textBox1.Enabled = false; textBox2.Enabled = false; button1.Enabled = false;
                MessageBox.Show("Superó el número de intentos permitidos. Consulte al administrador.");
                return;
            }

            // Desencripta y valida
            string DecryptText = EncryptionHelper.Decrypt(Cla.Trim());
            Cla = DecryptText;

            if (Nom.Trim() == textBox1.Text.Trim() && Cla.Trim() == textBox2.Text.Trim())
            {
                Conexion.VarGlobal.xEstIni = 2; // Usuario autenticado
                MessageBox.Show("Bienvenido al Sistema. Usuario: " + textBox1.Text.Trim(), "MENÚ PRINCIPAL");
                this.Hide();
                FrmMenu Menu = new FrmMenu();
                Menu.ShowDialog();
            }
            else
            {
                // Re-enciende Cla para no dejarla en claro
                string EncryptText = EncryptionHelper.Encrypt(Cla);
                Cla = EncryptText;

                textBox2.Focus();
                MessageBox.Show("Nombre de usuario o contraseña incorrecta - Tiene " +
                                (3 - ArrI[PosAux]).ToString() + " intentos.", "Número de intentos");
            }
        }

        // Botón Registrarse / Invitado (como lo tenías)
        private void button2_Click(object sender, EventArgs e)
        {
            Conexion.VarGlobal.xEstIni = 1; // Invitado
            Conexion.VarGlobal.xNomU = "Invitado";
            FrmUsuarios FrmUser = new FrmUsuarios();
            FrmUser.Show();
        }

        private void FrmAcceso_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult Rta = MessageBox.Show("¿Está seguro de salir?", "Salir del sistema", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
            if (Rta == DialogResult.Yes) { Application.ExitThread(); }
            else if (Rta == DialogResult.No) { e.Cancel = true; }
        }

 


    }
}
