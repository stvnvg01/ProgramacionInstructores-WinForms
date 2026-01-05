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
    public partial class FrmHAcercaDe : Form
    {
        public FrmHAcercaDe()
        {
            this.TopLevel = true;
            InitializeComponent();
        }

        private void FrmHAcercaDe_Load(object sender, EventArgs e)
        {
            button2_Click(null, null);
            // this.BackColor = Conexion.VarGlobal.xColorFondo;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            this.Size = new Size(this.Size.Width, 450); //Formulario
            textBox1.Size = new Size(textBox1.Size.Width, 350); //Objeto textBox
            button1.Enabled = false;
            button2.Enabled = true;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Size = new Size(this.Size.Width, 200); //Formulario
            textBox1.Size = new Size(textBox1.Size.Width, 200); //Objeto textBox
            button1.Enabled = true;
            button2.Enabled = false;
        }

    
    }
}
