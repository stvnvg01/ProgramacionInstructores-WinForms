using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramacionInstructores
{
    public partial class FrmMenu : Form
    {
        public FrmMenu()
        {
            InitializeComponent();
        }
        Conexion Conn = new Conexion(); //Conectar la base de datos
        DataRow fila;                   //Poder utilizar la tabla empresa


        private void FrmMenu_Load(object sender, EventArgs e)
        {
            NomUser.Text = "Usuario: " + Conexion.VarGlobal.xNomU;
            if (Conexion.VarGlobal.xEstIni == 2)  //2= inicio por el modulo de Acceso. 1 = Invitado
            {
                try
                {
                    pictureBox1.Image = Bitmap.FromStream(new System.IO.MemoryStream(Conexion.VarGlobal.xFoto));
               
                }
                catch { }
            }
            this.MaximumSize = SystemInformation.PrimaryMonitorMaximizedWindowSize;
            this.WindowState = FormWindowState.Maximized;
            Conn.ConsultaDatos("Select * From Empresa ", "Empresa");
            fila = Conn.Ds.Tables[0].Rows[0];//Guarda el Registro (pos) en la variable "fila"
            try
            {
                Conexion.VarGlobal.xFondo = (byte[])fila["Fondo"];
                Conexion.VarGlobal.xLogo = (byte[])fila["Logo"];
                pictureBox2.BackgroundImage = Bitmap.FromStream(new System.IO.MemoryStream(Conexion.VarGlobal.xLogo)); // Image.FromStream(CargarFoto);
                this.BackgroundImage = Bitmap.FromStream(new System.IO.MemoryStream(Conexion.VarGlobal.xFondo));
                
            }
            catch (Exception m) { }
            timer1.Enabled = true;

        }



        private void FrmMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult Rta = MessageBox.Show("Está seguro de salir?", "Salir del sistema", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
            if (Rta == DialogResult.Yes) { Application.ExitThread(); }
            else
            {
                e.Cancel = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Conexion.VarGlobal.xImaFon == 1)
            {
              //  this.IsMdiContainer = false;
                pictureBox2.BackgroundImage = Bitmap.FromStream(new System.IO.MemoryStream(Conexion.VarGlobal.xLogo));
                this.BackgroundImage = Bitmap.FromStream(new System.IO.MemoryStream(Conexion.VarGlobal.xFondo));
                Conexion.VarGlobal.xImaFon = 0;

            }
            label1.Text = DateTime.Now.ToString("hh:mm:ss t") + "m";
            label2.Text = DateTime.Now.ToString("dd/MM/yyyy");

        }
        private void empresaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmEmpresa FrmEmp = new FrmEmpresa { Owner = this }; //(ref instance);
            //this. IsMdiContainer =  true;
            //FrmUs.MdiParent = this;
            FrmEmp.Show();
        }
        private void administraciónDeUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmUsuarios FrmUs = new FrmUsuarios { Owner = this }; //(ref instance);
            //this. IsMdiContainer =  true;
            //FrmUs.MdiParent = this;
            FrmUs.Show();
        }

        private void estadoDelUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmEstadoUser FrmEUs = new FrmEstadoUser { Owner = this }; //(ref instance);
            //this. IsMdiContainer =  true;
            //FrmUs.MdiParent = this;
            FrmEUs.Show();
        }

        private void perfilesYRolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmPerfiles FrmPer = new FrmPerfiles { Owner = this }; //(ref instance);
            //this. IsMdiContainer =  true;
            //FrmUs.MdiParent = this;
            FrmPer.Show();
        }



        private void departamentosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmDpto FrmDpto = new FrmDpto { Owner = this }; //(ref instance);
            //this. IsMdiContainer =  true;
            //FrmUs.MdiParent = this;
            FrmDpto.Show();
        }

        private void municipiosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmMpio FrmMpio = new FrmMpio { Owner = this }; //(ref instance);
            //this. IsMdiContainer =  true;
            //FrmUs.MdiParent = this;
            FrmMpio.Show();
        }

        private void salirToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DialogResult Rta = MessageBox.Show("Está seguro de salir?", "Salir del sistema", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
            if (Rta == DialogResult.Yes) { Application.ExitThread(); }
        
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult Rta = MessageBox.Show("Está seguro de salir?", "Salir del sistema", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
            if (Rta == DialogResult.Yes) { Application.ExitThread(); }

        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmHAcercaDe FrmHAcercaDe = new FrmHAcercaDe { Owner = this }; //(ref instance);
            //this. IsMdiContainer =  true;
            //FrmUs.MdiParent = this;
            FrmHAcercaDe.Show();
        }

        private void copiasDeSeguridadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmHCopiaS FrmHCopiaS = new FrmHCopiaS { Owner = this }; //(ref instance);
            //this. IsMdiContainer =  true;
            //FrmUs.MdiParent = this;
            FrmHCopiaS.Show();
        }


        private void restaurarCopiasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmRestaurarCS FrmRestaurarCS = new FrmRestaurarCS { Owner = this }; //(ref instance);
            //this. IsMdiContainer =  true;
            //FrmUs.MdiParent = this;
            FrmRestaurarCS.Show();
        }


        private void rompecabezasImagenesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FrmRompeImage  FrmRompeImage = new FrmRompeImage { Owner = this }; //(ref instance);
            ////this. IsMdiContainer =  true;
            ////FrmUs.MdiParent = this;
            //FrmRompeImage.Show();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            FrmCronograma FrmCronograma = new FrmCronograma { Owner = this }; //(ref instance);
            //this. IsMdiContainer =  true;
            //FrmUs.MdiParent = this;
            FrmCronograma.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmGraficos FrmGraficos = new FrmGraficos { Owner = this }; //(ref instance);
            //this. IsMdiContainer =  true;
            //FrmUs.MdiParent = this;
            FrmGraficos.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FrmFichas FrmFichas = new FrmFichas { Owner = this }; //(ref instance);
            //this. IsMdiContainer =  true;
            //FrmUs.MdiParent = this;
            FrmFichas.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FrmProgramas FrmProgramas = new FrmProgramas { Owner = this }; //(ref instance);
            //this. IsMdiContainer =  true;
            //FrmUs.MdiParent = this;
            FrmProgramas.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FrmCompetencias FrmCompetencias = new FrmCompetencias { Owner = this }; //(ref instance);
            //this. IsMdiContainer =  true;
            //FrmUs.MdiParent = this;
            FrmCompetencias.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FrmCompetenciasIncremental FrmCompetenciasIncremental = new FrmCompetenciasIncremental { Owner = this }; //(ref instance);
            //this. IsMdiContainer =  true;
            //FrmUs.MdiParent = this;
            FrmCompetenciasIncremental.Show();
        }

        private void PersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmPersonas FrmPersonas = new FrmPersonas { Owner = this }; //(ref instance);
            //this. IsMdiContainer =  true;
            //FrmUs.MdiParent = this;
            FrmPersonas.Show();
        }

        private void atualizarPersonasIncrementalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmPnaIncremental FrmPnaIncremental = new FrmPnaIncremental { Owner = this }; //(ref instance);
            //this. IsMdiContainer =  true;
            //FrmUs.MdiParent = this;
            FrmPnaIncremental.Show();
        }

        //private void instructoresToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    //FrmInstructores FrmInstructores = new FrmInstructores { Owner = this }; //(ref instance);
        //    ////this. IsMdiContainer =  true;
        //    ////FrmUs.MdiParent = this;
        //    //FrmInstructores.Show();
        //}

        private void VarEntornoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmVarEntorno FrmVarEntorno = new FrmVarEntorno { Owner = this }; //(ref instance);
            //this. IsMdiContainer =  true;
            //FrmUs.MdiParent = this;
            FrmVarEntorno.Show();
        }

        private void ambientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAmbientes FrmAmbientes = new FrmAmbientes { Owner = this }; //(ref instance);
            //this. IsMdiContainer =  true;
            //FrmUs.MdiParent = this;
            FrmAmbientes.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FrmInstructores FrmInstructores = new FrmInstructores { Owner = this }; //(ref instance);
            //this. IsMdiContainer =  true;
            //FrmUs.MdiParent = this;
            FrmInstructores.Show();
        }

        //private void importarDatosToolStripMenuItem_Click(object sender, EventArgs e)
        //{

        //}
    }
}
