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
    public partial class FrmProgramas : Form
    {
        public FrmProgramas()
        {
            InitializeComponent();
        }

        Conexion Conn = new Conexion();
        Conexion ConnAux = new Conexion();
        int Pos = 0;
        string Opc = "0";
        string ValNomUser = string.Empty;
        string ValDocUser = string.Empty;

        Complementos ONTEnter = new Complementos();
        DataRow fila;
        int swTimer = 0;

        OpenFileDialog BuscarFile = new OpenFileDialog();

        // --------- utilitario: cargar niveles en comboBox1 ----------
        private void CargarNiveles()
        {
            try
            {
                ConnAux.ConsultaDatos(
                    "SELECT RTRIM(LTRIM(IdNivel)) AS IdNivel, RTRIM(LTRIM(Nombre)) AS Nombre " +
                    "FROM Niveles ORDER BY Nombre",
                    "Niveles");

                comboBox1.DataSource = ConnAux.Ds.Tables["Niveles"];
                comboBox1.DisplayMember = "Nombre";
                comboBox1.ValueMember = "IdNivel";
                comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox1.SelectedIndex = -1;
                comboBox1.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No pude cargar Niveles: " + ex.Message);
                comboBox1.DataSource = null;
                comboBox1.Items.Clear();
                comboBox1.Enabled = false;
            }
        }
        // ------------------------------------------------------------

        // *** NO CAMBIO EL NOMBRE DEL LOAD ***
        private void FrmUsuarios_Load(object sender, EventArgs e)
        {
            Conn.CargarEstilosDesdeBD();
            EstilosGlobales.AplicarEstiloBoton(BtoNue, "Nuevo");
            EstilosGlobales.AplicarEstiloBoton(BtoEdi, "Editar");
            EstilosGlobales.AplicarEstiloBoton(BtoEli, "Eliminar");
            EstilosGlobales.AplicarEstiloBoton(BtoGua, "Guardar");
            EstilosGlobales.AplicarEstiloBoton(BtoCon, "Consultar");
            EstilosGlobales.AplicarEstiloBoton(BtoImp, "Imprimir");
            EstilosGlobales.AplicarEstiloBoton(BtoSal, "Salir");

            // Longitudes: usa Programas (cerrando correctamente el DataReader)
            foreach (var Obj in this.Controls.OfType<System.Windows.Forms.TextBox>())
            {
                using (SqlDataReader LenC = Conn.LenCampos("Programas"))
                {
                    while (LenC.Read())
                    {
                        if (Obj.Tag != null && LenC.GetValue(0).ToString() == Obj.Tag.ToString())
                        { Obj.MaxLength = Convert.ToInt32(LenC["LenCampo"]); break; }
                    }
                }
            }

            foreach (Control Obj in this.Controls)
            {
                if (Obj is System.Windows.Forms.TextBox || Obj is System.Windows.Forms.ComboBox)
                {
                    Obj.KeyDown += new KeyEventHandler(Complementos.PCEnter);
                    Obj.TextChanged += new EventHandler(Complementos.ValChar);
                    Obj.GotFocus += new EventHandler(Complementos.RecibeFoco);
                    Obj.LostFocus += new EventHandler(Complementos.PerderFoco);
                }
                if (Obj is Label) { Obj.ForeColor = Conexion.VarGlobal.xColorFont; }
            }

            CargarNiveles();

            Conn.Conectar();
            MostrarDatos();
            ActDesObjetos(false);
            BtoGua.Enabled = false;

            textBox2.Enabled = true;   // Versión
            textBox3.Enabled = true;   // Nombre
            textBox4.Enabled = true;   // Duración
        }

        private void ActDesObjetos(Boolean ActDes)
        {
            foreach (Control Obj in this.Controls.Cast<Control>().Union(panel1.Controls.Cast<Control>()))
            {
                if (Obj is System.Windows.Forms.TextBox tb)
                {
                    tb.Enabled = ActDes;
                    if (Opc == "1" && tb.TabIndex > 0) tb.Text = string.Empty;
                }
                else if (Obj is System.Windows.Forms.ComboBox cb)
                {
                    cb.Enabled = ActDes;
                    if (Opc == "1" && cb.TabIndex > 0)
                    {
                        if (cb.DataSource != null) cb.SelectedIndex = -1; // NO borrar Items de combos enlazados
                        else { cb.Items.Clear(); cb.Text = string.Empty; }
                    }
                }
            }
        }

        private void MostrarDatos()
        {
            Conn.ConsultaDatos(
                "SELECT P.IdPrograma, P.Version, P.Nombre, P.IdNivel, P.Duracion, " +
                "       ISNULL(N.Nombre,'') AS NombreNivel " +
                "FROM Programas P LEFT JOIN Niveles N ON N.IdNivel = P.IdNivel " +
                "ORDER BY P.Nombre", "Programas");

            dataGridView1.DataSource = Conn.Ds.Tables["Programas"];

            dataGridView1.Columns["IdPrograma"].HeaderText = "IdPrograma";
            dataGridView1.Columns["Version"].HeaderText = "Versión";
            dataGridView1.Columns["Nombre"].HeaderText = "Nombre";
            dataGridView1.Columns["IdNivel"].HeaderText = "IdNivel";
            dataGridView1.Columns["Duracion"].HeaderText = "Duración";
            if (dataGridView1.Columns.Contains("NombreNivel"))
                dataGridView1.Columns["NombreNivel"].HeaderText = "Nivel";

            dataGridView1.Columns["IdPrograma"].Width = 110;
            dataGridView1.Columns["Version"].Width = 100;
            dataGridView1.Columns["Nombre"].Width = 220;
            dataGridView1.Columns["IdNivel"].Width = 90;
            dataGridView1.Columns["Duracion"].Width = 90;

            foreach (DataGridViewColumn Row in dataGridView1.Columns)
                Row.SortMode = DataGridViewColumnSortMode.NotSortable;

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
            if (Conn.Ds.Tables["Programas"].Rows.Count < 1)
            {
                EstadoBton(false);
                BtoNue.Enabled = true;
                BtoSal.Image = imageList1.Images[7];
                BtoSal.Text = " &Salir";
                Opc = "1"; ActDesObjetos(false); Opc = "0";
                return;
            }

            fila = Conn.Ds.Tables["Programas"].Rows[Pos];

            textBox1.Text = Convert.ToString(fila["IdPrograma"]).Trim();
            textBox2.Text = Convert.ToString(fila["Version"]).Trim();
            textBox3.Text = Convert.ToString(fila["Nombre"]).Trim();

            string idNivelRow = Convert.ToString(fila["IdNivel"]).Trim();
            if (comboBox1.DataSource != null && comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedValue = idNivelRow;
                if (comboBox1.SelectedIndex == -1) comboBox1.Text = idNivelRow;
            }
            else comboBox1.Text = idNivelRow;

            textBox4.Text = Convert.ToString(fila["Duracion"]).Trim();

            dataGridView1.Rows[Pos].Selected = true;
            dataGridView1.CurrentCell = dataGridView1.Rows[Pos].Cells[0];
        }

        private void EstadoBton(Boolean ActDes)
        {
            foreach (Control Obj in this.Controls.Cast<Control>().Union(panel1.Controls.Cast<Control>()))
            {
                if (Obj is System.Windows.Forms.Button) { Obj.Enabled = ActDes; }
            }
            foreach (Control Obj in this.Controls) { if (Obj is System.Windows.Forms.Button) { Obj.Enabled = ActDes; } }
            BtoSal.Enabled = true;
            BtoSal.Image = imageList1.Images[6];
            BtoSal.Text = " &Salir";
            toolTip1.SetToolTip(BtoSal, "Salir del Formulario");
            if (ActDes == false)
            {
                BtoSal.Image = imageList1.Images[7];
                BtoSal.Text = " &Cerrar";
                toolTip1.SetToolTip(BtoSal, "Cerrar procedimiento");
            }
        }

        private void Pri_Click(object sender, EventArgs e)
        {
            Pos = 0;
            CargarDatos(Pos);
            Pri.Enabled = false; Ant.Enabled = false; Sig.Enabled = true; Ult.Enabled = true;
        }

        private void Ant_Click(object sender, EventArgs e)
        {
            Pos += -1;
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

                CargarNiveles();
                ActDesObjetos(true);
                comboBox1.Enabled = true;

                textBox1.Enabled = true;
                textBox1.Text = string.Empty;
                textBox2.Text = string.Empty;
                textBox3.Text = string.Empty;
                textBox4.Text = string.Empty;
                comboBox1.SelectedIndex = -1;

                textBox1.Focus();
                dataGridView1.Enabled = false;
                if (Conexion.VarGlobal.xNomU == "Invitado") { Conexion.VarGlobal.xNomU = ""; }
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

                CargarNiveles();
                ActDesObjetos(true);
                comboBox1.Enabled = true;

                textBox1.Enabled = false;
                dataGridView1.Enabled = false;
            }
            else { MessageBox.Show("Solo Administradores y usuarios registrados acceden a esta función..!"); }
        }

        // ====== AQUI EL CAMBIO CLAVE: usar conexión LOCAL en lugar de la global ======
        private void BtoGua_Click(object sender, EventArgs e)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(textBox1.Text)) { MessageBox.Show("IdPrograma es obligatorio"); textBox1.Focus(); return; }
            if (string.IsNullOrWhiteSpace(textBox3.Text)) { MessageBox.Show("Nombre es obligatorio"); textBox3.Focus(); return; }
            if (comboBox1.SelectedValue == null && string.IsNullOrWhiteSpace(comboBox1.Text)) { MessageBox.Show("Seleccione un nivel"); comboBox1.Focus(); return; }
            if (!int.TryParse(textBox4.Text, out _)) { MessageBox.Show("Duración debe ser numérica"); textBox4.Focus(); return; }

            string idNivel = comboBox1.SelectedValue != null ? comboBox1.SelectedValue.ToString().Trim()
                                                             : comboBox1.Text.Trim();

            // Usa una conexión local para evitar conflictos con la conexión global
            string cs = Conexion.ConDB.ConnectionString;
            try
            {
                using (var cn = new SqlConnection(cs))
                {
                    cn.Open();

                    if (Opc == "1")
                    {
                        // Validar duplicado sin tocar la conexión global
                        using (var chk = new SqlCommand("SELECT COUNT(1) FROM Programas WHERE IdPrograma=@Id", cn))
                        {
                            chk.Parameters.AddWithValue("@Id", textBox1.Text.Trim());
                            int existe = Convert.ToInt32(chk.ExecuteScalar());
                            if (existe > 0) { MessageBox.Show("Ese IdPrograma ya existe."); return; }
                        }

                        using (var cmd = new SqlCommand(
                            "INSERT INTO Programas (IdPrograma, Version, Nombre, IdNivel, Duracion) " +
                            "VALUES (@IdPrograma, @Version, @Nombre, @IdNivel, @Duracion)", cn))
                        {
                            cmd.Parameters.AddWithValue("@IdPrograma", textBox1.Text.Trim());
                            cmd.Parameters.AddWithValue("@Version", textBox2.Text.Trim());
                            cmd.Parameters.AddWithValue("@Nombre", textBox3.Text.Trim());
                            cmd.Parameters.AddWithValue("@IdNivel", idNivel);
                            cmd.Parameters.AddWithValue("@Duracion", int.Parse(textBox4.Text.Trim()));
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else if (Opc == "2")
                    {
                        using (var cmd = new SqlCommand(
                            "UPDATE Programas SET Version=@Version, Nombre=@Nombre, IdNivel=@IdNivel, Duracion=@Duracion " +
                            "WHERE IdPrograma=@IdPrograma", cn))
                        {
                            cmd.Parameters.AddWithValue("@IdPrograma", textBox1.Text.Trim());
                            cmd.Parameters.AddWithValue("@Version", textBox2.Text.Trim());
                            cmd.Parameters.AddWithValue("@Nombre", textBox3.Text.Trim());
                            cmd.Parameters.AddWithValue("@IdNivel", idNivel);
                            cmd.Parameters.AddWithValue("@Duracion", int.Parse(textBox4.Text.Trim()));
                            cmd.ExecuteNonQuery();
                        }
                    }

                    cn.Close(); // se cierra aquí siempre por el using
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            // Refrescar con los métodos que usan la conexión global (ya no hay conflicto)
            if (Opc == "1")
            {
                string xId = textBox1.Text.Trim();
                MostrarDatos();
                Conn.Ds.Tables["Programas"].PrimaryKey = new DataColumn[] { Conn.Ds.Tables["Programas"].Columns["IdPrograma"] };
                Pos = Conn.Ds.Tables["Programas"].Rows.IndexOf(Conn.Ds.Tables["Programas"].Rows.Find(xId));
                CargarDatos(Pos);
            }
            else if (Opc == "2")
            {
                int NPos = Pos; MostrarDatos(); CargarDatos(NPos);
            }

            dataGridView1.Enabled = true;
            BtoSal.Text = "&Cerrar";
            this.BtoSal_Click(null, null);
        }
        // ================================================================================

        private void BtoCon_Click(object sender, EventArgs e)
        {
            Opc = "4";
            EstadoBton(false);
            textBox1.Enabled = true;
            textBox1.Text = String.Empty;
            textBox1.Focus();
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
                        string reg = "IdPrograma='" + textBox1.Text.Trim() + "'";
                        Conn.Eliminar("Programas", reg);
                        Pos = Math.Max(0, Pos - 1);
                        MostrarDatos();
                        CargarDatos(Pos);
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
            if (Opc == "4")
            {
                int BackPos = Pos;
                string id = textBox1.Text.Trim();
                if (Conn.ConsultaItem("Programas", $"IdPrograma='{id}'"))
                {
                    DataTable t = Conn.Ds.Tables["Programas"];
                    if (t.PrimaryKey.Length == 0) t.PrimaryKey = new DataColumn[] { t.Columns["IdPrograma"] };
                    DataRow r = t.Rows.Find(id);
                    if (r != null) { Pos = t.Rows.IndexOf(r); CargarDatos(Pos); }
                }
                else
                {
                    MessageBox.Show("No existe un Programa con ese Id.");
                    textBox1.Focus();
                    Pos = BackPos;
                    this.BtoSal_Click(null, null);
                }
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
            if (swTimer == 1) { swTimer = 0; }
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
        }

        private void comboBox3_Validated(object sender, EventArgs e) { }

        // -------- Impresión (sin cambios funcionales) --------
        string IdN, NoE, NoP, Tel, Cor, Dir, Lem;
        byte[] ImaLog;

        //private void label6_Click(object sender, EventArgs e)
        //{

        //}

        Image Log;
        int Pag;

        private void BtoImp_Click(object sender, EventArgs e)
        {
            Pag = 0;

            Conexion ConE = new Conexion();
            ConE.ConsultaDatos("Select * From Empresa", "Empresa");
            fila = ConE.Ds.Tables[0].Rows[0];

            IdN = Convert.ToString(fila["IdEmpresa"]).Trim();
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

            PrintDocument DocPrin = new PrintDocument();
            PrintPreviewDialog ImpDoc = new PrintPreviewDialog();

            ImpDoc.ClientSize = new System.Drawing.Size(1000, 800);
            ImpDoc.StartPosition = FormStartPosition.CenterScreen;
            ImpDoc.Location = new System.Drawing.Point(1, 1);

            DocPrin.DefaultPageSettings.PaperSize = new PaperSize("Tamaño Carta", 850, 1100);
            DocPrin.PrintPage += new PrintPageEventHandler(this.DocPrin);
            DocPrin.EndPrint += new PrintEventHandler(DocPrinEndP);
            ImpDoc.Document = DocPrin; ImpDoc.ShowDialog();
        }

        private void DocPrinEndP(object sender, PrintEventArgs e) { Pag = 0; }

        private int currentRow = 0;
        private void DocPrin(object sender, PrintPageEventArgs e)
        {
            Font txtNeg = new Font("Arial Narrow", 12, FontStyle.Bold, GraphicsUnit.Point);
            Font txtSim = new Font("Arial Narrow", 11, FontStyle.Regular, GraphicsUnit.Point);

            SizeF txtSize1 = e.Graphics.MeasureString(NoE, txtNeg);
            SizeF txtSize2 = e.Graphics.MeasureString(IdN, txtNeg);
            SizeF txtSize3 = e.Graphics.MeasureString("<< U S U A R I O S >>", txtNeg);
            SizeF txtSize4 = e.Graphics.MeasureString(Tel + " - " + Cor + " - " + Dir, txtNeg);
            SizeF txtSize5 = e.Graphics.MeasureString(Lem, txtNeg);

            int Fil = 50;

            e.Graphics.DrawString("Pág. " + (Pag += 1), txtNeg, Brushes.Black, e.PageBounds.Width - 100, 030);
            e.Graphics.DrawImage(Log, new Rectangle(20, 20, 90, 80));
            e.Graphics.DrawString(NoE, txtNeg, Brushes.Black, (e.PageBounds.Width - txtSize1.Width) / 2, 020);
            e.Graphics.DrawString(IdN, txtNeg, Brushes.Black, (e.PageBounds.Width - txtSize2.Width) / 2, 040);
            e.Graphics.DrawString("<< U S U A R I O S >>", txtNeg, Brushes.Black, (e.PageBounds.Width - txtSize3.Width) / 2, Fil += 25);
            e.Graphics.DrawString("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬", txtNeg, Brushes.Black, new RectangleF(70, Fil += 15, 800, 20));
            e.Graphics.DrawString(" IdCliente   │   Documento  |        Nombre       |         Apellido      |     Telefono     |    IdLugar                     ",
                txtNeg, Brushes.Black, new RectangleF(70, Fil += 15, 900, 20));
            e.Graphics.DrawString("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬", txtNeg, Brushes.Black, new RectangleF(70, Fil += 10, 800, 20));

            int itemPerPage = 1;
            int maxItemsPerPage = (e.MarginBounds.Height - 30) / 20;

            for (int i = currentRow; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow xFil = dataGridView1.Rows[i];
                e.Graphics.DrawString(xFil.Cells[1].Value.ToString() + " ", txtSim, Brushes.Black, new Rectangle(80, Fil += 20, 050, 20));
                e.Graphics.DrawString(xFil.Cells[3].Value.ToString() + " ", txtSim, Brushes.Black, new Rectangle(160, Fil, 450, 20));
                e.Graphics.DrawString(xFil.Cells[4].Value.ToString() + " ", txtSim, Brushes.Black, new Rectangle(270, Fil, 350, 20));
                e.Graphics.DrawString(xFil.Cells[5].Value.ToString() + " ", txtSim, Brushes.Black, new Rectangle(380, Fil, 350, 20));
                e.Graphics.DrawString(xFil.Cells[7].Value.ToString() + " ", txtSim, Brushes.Black, new Rectangle(500, Fil, 350, 20));
                itemPerPage++;
                if (itemPerPage >= maxItemsPerPage)
                {
                    Fil = (e.MarginBounds.Height + 100);
                    e.Graphics.DrawString("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬", txtNeg, Brushes.Black, new RectangleF(070, Fil += 15, 800, 20));
                    e.Graphics.DrawString("☏ " + Tel + " - ☒" + Cor + " - ⚑" + Dir, txtNeg, Brushes.Black, (e.PageBounds.Width - txtSize4.Width) / 2, Fil += 15);
                    e.Graphics.DrawString(Lem, txtNeg, Brushes.Black, (e.PageBounds.Width - txtSize5.Width) / 2, Fil += 20);

                    currentRow = i + 1;
                    e.HasMorePages = true;
                    return;
                }
            }

            e.Graphics.DrawString("Total Registro: " + Convert.ToString(dataGridView1.Rows.Count) + " ", txtNeg, Brushes.Black, new Rectangle(400, Fil += 30, 200, 20));

            Fil = (e.MarginBounds.Height + 75);
            e.Graphics.DrawString("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬", txtNeg, Brushes.Black, new RectangleF(070, Fil += 15, 800, 20));
            e.Graphics.DrawString("☏ " + Tel + " - ☒" + Cor + " - ⚑" + Dir, txtNeg, Brushes.Black, (e.PageBounds.Width - txtSize4.Width) / 2, Fil += 15);
            e.Graphics.DrawString(Lem, txtNeg, Brushes.Black, (e.PageBounds.Width - txtSize5.Width) / 2, Fil += 20);

            currentRow = 0;
            e.HasMorePages = false;
        }
    }
}
   