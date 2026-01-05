using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ProgramacionInstructores
{
    public partial class FrmAmbientes : Form
    {
        public FrmAmbientes()
        {
            InitializeComponent();
        }

        // ================== VARIABLES GLOBALES ==================
        Conexion Conn = new Conexion();      // Conexión principal
        Conexion ConnAux = new Conexion();   // Conexión auxiliar
        DataRow fila;

        int Pos = 0;               // Posición actual en el DataSet
        string Opc = "0";          // 1=Nuevo, 2=Editar, 4=Consultar
        int swTimer = 0;

        Complementos ONTEnter = new Complementos();

        // ================ FORM LOAD =============================
        private void FrmAmbientes_Load(object sender, EventArgs e)
        {
            // Estilos (si usas tu gestor de estilos)
            Conn.CargarEstilosDesdeBD();
            EstilosGlobales.AplicarEstiloBoton(BtoNue, "Nuevo");
            EstilosGlobales.AplicarEstiloBoton(BtoEdi, "Editar");
            EstilosGlobales.AplicarEstiloBoton(BtoEli, "Eliminar");
            EstilosGlobales.AplicarEstiloBoton(BtoGua, "Guardar");
            EstilosGlobales.AplicarEstiloBoton(BtoCon, "Consultar");
            EstilosGlobales.AplicarEstiloBoton(BtoImp, "Imprimir");
            EstilosGlobales.AplicarEstiloBoton(BtoSal, "Salir");

            // Longitudes desde la tabla Ambientes (usa Tag de cada TextBox)
            SqlDataReader LenC;
            foreach (var Obj in this.Controls.OfType<TextBox>())
            {
                LenC = Conn.LenCampos("Ambientes");
                while (LenC.Read())
                {
                    if (Obj.Tag != null && LenC.GetValue(0).ToString() == Obj.Tag.ToString())
                    {
                        Obj.Text = LenC["LenCampo"].ToString();
                        Obj.MaxLength = Convert.ToInt32(LenC["LenCampo"]);
                        break;
                    }
                }
            }

            // Eventos comunes de UX
            foreach (Control Obj in this.Controls.Cast<Control>().Union(panel1.Controls.Cast<Control>()))
            {
                if (Obj is TextBox || Obj is ComboBox)
                {
                    Obj.KeyDown += new KeyEventHandler(Complementos.PCEnter);
                    Obj.TextChanged += new EventHandler(Complementos.ValChar);
                    Obj.GotFocus += new EventHandler(Complementos.RecibeFoco);
                    Obj.LostFocus += new EventHandler(Complementos.PerderFoco);
                }
                if (Obj is Label) { Obj.ForeColor = Conexion.VarGlobal.xColorFont; }
            }

            CargarComboEstado();     // Combo Estado
            Conn.Conectar();
            MostrarDatos();
            ActDesObjetos(false);
            BtoGua.Enabled = false;

            textBox2.Enabled = true; // Nombre sí se edita
        }

        // ================== COMBO ESTADO ========================
        private void CargarComboEstado()
        {
            var dt = new DataTable();
            dt.Columns.Add("Valor", typeof(int));
            dt.Columns.Add("Texto", typeof(string));
            dt.Rows.Add(1, "Activo");
            dt.Rows.Add(0, "Inactivo");

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "Texto";
            comboBox1.ValueMember = "Valor";
        }

        // ============== HABILITAR / DESHABILITAR CONTROLES ======
        private void ActDesObjetos(bool ActDes)
        {
            foreach (Control Obj in this.Controls.Cast<Control>().Union(panel1.Controls.Cast<Control>()))
            {
                if (Obj is TextBox || Obj is ComboBox)
                {
                    Obj.Enabled = ActDes;
                    if (Opc == "1" && Obj.TabIndex > 0) Obj.Text = string.Empty;
                }
            }
        }

        private void EstadoBton(bool ActDes)
        {
            foreach (Control Obj in this.Controls.Cast<Control>().Union(panel1.Controls.Cast<Control>()))
                if (Obj is Button) Obj.Enabled = ActDes;

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

        // ================== CONSULTA / GRID =====================
        private void MostrarDatos()
        {
            Conn.ConsultaDatos(
                "SELECT " +
                "  IdAmbiente, " +
                "  RTRIM(LTRIM(Nombre)) AS Nombre, " +
                "  Estado, " +
                "  CASE WHEN Estado = 1 THEN 'Activo' ELSE 'Inactivo' END AS EstadoTxt " +
                "FROM Ambientes " +
                "ORDER BY IdAmbiente", "Ambientes");

            dataGridView1.DataSource = Conn.Ds.Tables["Ambientes"];

            // Encabezados
            dataGridView1.Columns["IdAmbiente"].HeaderText = "ID Ambiente";
            dataGridView1.Columns["Nombre"].HeaderText = "Nombre";
            dataGridView1.Columns["Estado"].HeaderText = "Estado (Num)";
            dataGridView1.Columns["EstadoTxt"].HeaderText = "Estado";

            // Mostrar sólo el texto de estado al usuario
            dataGridView1.Columns["Estado"].Visible = false;
            dataGridView1.Columns["EstadoTxt"].DisplayIndex = 2;

            dataGridView1.Columns["IdAmbiente"].Width = 120;
            dataGridView1.Columns["Nombre"].Width = 250;
            dataGridView1.Columns["EstadoTxt"].Width = 120;

            foreach (DataGridViewColumn c in dataGridView1.Columns)
                c.SortMode = DataGridViewColumnSortMode.NotSortable;

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.MultiSelect = false;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView1.RowsDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.White;

            if (Opc == "0") CargarDatos(Pos);
        }

        private void CargarDatos(int pos)
        {
            dataGridView1.ClearSelection();

            if (Conn.Ds.Tables["Ambientes"].Rows.Count < 1)
            {
                EstadoBton(false);
                BtoNue.Enabled = true;
                BtoSal.Image = imageList1.Images[7];
                BtoSal.Text = " &Salir";
                Opc = "1"; ActDesObjetos(false); Opc = "0";
                return;
            }

            fila = Conn.Ds.Tables["Ambientes"].Rows[pos];

            textBox1.Text = Convert.ToString(fila["IdAmbiente"]).Trim();
            textBox2.Text = Convert.ToString(fila["Nombre"]).Trim();

            int est = (fila["Estado"] == DBNull.Value) ? 0 : Convert.ToInt32(fila["Estado"]);
            comboBox1.SelectedValue = est;

            dataGridView1.Rows[pos].Selected = true;
            dataGridView1.CurrentCell = dataGridView1.Rows[pos].Cells[dataGridView1.Columns["IdAmbiente"].Index];
        }

        // ============== NAVEGACIÓN GRID =========================
        private void Pri_Click(object sender, EventArgs e)
        {
            Pos = 0;
            CargarDatos(Pos);
            Pri.Enabled = false; Ant.Enabled = false; Sig.Enabled = true; Ult.Enabled = true;
        }

        private void Ant_Click(object sender, EventArgs e)
        {
            Pos -= 1;
            if (Pos < 1) { Pos = 0; Pri_Click(null, null); }
            else { CargarDatos(Pos); Sig.Enabled = true; Ult.Enabled = true; }
        }

        private void Sig_Click(object sender, EventArgs e)
        {
            Pos += 1;
            if (Pos >= (Conn.Ds.Tables["Ambientes"].Rows.Count - 1)) { Ult_Click(null, null); }
            else { CargarDatos(Pos); Pri.Enabled = true; Ant.Enabled = true; }
        }

        private void Ult_Click(object sender, EventArgs e)
        {
            Pos = Conn.Ds.Tables["Ambientes"].Rows.Count - 1; CargarDatos(Pos);
            Pri.Enabled = true; Ant.Enabled = true; Sig.Enabled = false; Ult.Enabled = false;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            Pos = dataGridView1.CurrentCell.RowIndex;
            Pri.Enabled = true; Ant.Enabled = true; Sig.Enabled = true; Ult.Enabled = true;
            if (Pos == 0) Pri_Click(null, null);
            if (Pos >= Conn.Ds.Tables["Ambientes"].Rows.Count - 1) Ult_Click(null, null);
            CargarDatos(Pos);
        }

        // ================== NUEVO / EDITAR / GUARDAR / ELIMINAR ==
        private void BtoNue_Click(object sender, EventArgs e)
        {
            if (Conexion.VarGlobal.xTipoU == "01" && Conexion.VarGlobal.xEstIni == 2 || Conexion.VarGlobal.xNomU == "Invitado")
            {
                Opc = "1";
                EstadoBton(true);
                ActDesObjetos(true);

                textBox1.Enabled = true;
                textBox1.Text = string.Empty;
                textBox2.Text = string.Empty;
                comboBox1.SelectedValue = 1; // por defecto Activo

                textBox1.Focus();
                dataGridView1.Enabled = false;
                if (Conexion.VarGlobal.xNomU == "Invitado") Conexion.VarGlobal.xNomU = "";
            }
            else MessageBox.Show("Solo Administrador y usuarios registrados acceden a esta función..!");
        }

        private void BtoEdi_Click(object sender, EventArgs e)
        {
            if (Conexion.VarGlobal.xTipoU == "01" && Conexion.VarGlobal.xEstIni == 2)
            {
                Opc = "2";
                EstadoBton(false);
                BtoGua.Enabled = true;
                ActDesObjetos(true);

                textBox1.Enabled = false; // llave no editable
                dataGridView1.Enabled = false;
            }
            else MessageBox.Show("Solo Administradores y usuarios registrados acceden a esta función..!");
        }

        private void BtoGua_Click(object sender, EventArgs e)
        {
            string id = textBox1.Text.Trim();
            string nom = textBox2.Text.Trim();
            int est = (comboBox1.SelectedValue == null) ? 0 : Convert.ToInt32(comboBox1.SelectedValue);

            if (string.IsNullOrWhiteSpace(id))
            {
                MessageBox.Show("El IdAmbiente no puede estar vacío.");
                textBox1.Focus(); return;
            }

            if (Opc == "1") // INSERT
            {
                string sql = "INSERT INTO Ambientes (IdAmbiente, Nombre, Estado) VALUES (@IdAmbiente, @Nombre, @Estado)";
                Conexion.ConDB.Open();
                using (SqlCommand cmd = new SqlCommand(sql, Conexion.ConDB))
                {
                    cmd.Parameters.AddWithValue("@IdAmbiente", id);
                    cmd.Parameters.AddWithValue("@Nombre", nom);
                    cmd.Parameters.AddWithValue("@Estado", est);
                    cmd.ExecuteNonQuery();
                }
                Conexion.ConDB.Close();

                MostrarDatos();
                Conn.Ds.Tables["Ambientes"].PrimaryKey = new DataColumn[] { Conn.Ds.Tables["Ambientes"].Columns["IdAmbiente"] };
                Pos = Conn.Ds.Tables["Ambientes"].Rows.IndexOf(Conn.Ds.Tables["Ambientes"].Rows.Find(id));
                CargarDatos(Pos);
            }
            else if (Opc == "2") // UPDATE
            {
                string sql = "UPDATE Ambientes SET Nombre=@Nombre, Estado=@Estado WHERE IdAmbiente=@IdAmbiente";
                Conexion.ConDB.Open();
                using (SqlCommand cmd = new SqlCommand(sql, Conexion.ConDB))
                {
                    cmd.Parameters.AddWithValue("@IdAmbiente", id);
                    cmd.Parameters.AddWithValue("@Nombre", nom);
                    cmd.Parameters.AddWithValue("@Estado", est);
                    cmd.ExecuteNonQuery();
                }
                Conexion.ConDB.Close();

                int npos = Pos; MostrarDatos(); CargarDatos(npos);
            }

            dataGridView1.Enabled = true;
            BtoSal.Text = "&Cerrar";
            this.BtoSal_Click(null, null);
        }

        private void BtoEli_Click(object sender, EventArgs e)
        {
            if (Conexion.VarGlobal.xTipoU == "01" && Conexion.VarGlobal.xEstIni == 2)
            {
                DialogResult resu = MessageBox.Show(
                    "¿Está seguro de eliminar el ambiente <" + textBox1.Text.Trim() + " | " + textBox2.Text.Trim() + ">?",
                    "A D V E R T E N C I A", MessageBoxButtons.YesNo);

                if (resu == DialogResult.Yes)
                {
                    try
                    {
                        string sql = "DELETE FROM Ambientes WHERE IdAmbiente=@IdAmbiente";
                        Conexion.ConDB.Open();
                        using (SqlCommand cmd = new SqlCommand(sql, Conexion.ConDB))
                        {
                            cmd.Parameters.AddWithValue("@IdAmbiente", textBox1.Text.Trim());
                            cmd.ExecuteNonQuery();
                        }
                        Conexion.ConDB.Close();

                        Pos -= 1; if (Pos < 0) Pos = 0;
                        MostrarDatos(); CargarDatos(Pos);
                    }
                    catch (Exception m)
                    {
                        MessageBox.Show(m.Message);
                        Conn.Conectar();
                    }
                }
            }
            else MessageBox.Show("¡Solo Administradores y usuarios registrados acceden a esta función!");
        }

        private void BtoCon_Click(object sender, EventArgs e)
        {
            Opc = "4";
            EstadoBton(false);
            textBox1.Enabled = true;
            textBox1.Text = string.Empty;
            textBox1.Focus();
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
                if (xOp != "4") MostrarDatos();
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

        // ============== VALIDACIONES BÁSICAS ====================
        private void textBox1_Validated(object sender, EventArgs e)
        {
            if (Opc == "1")
            {
                string reg = "IdAmbiente='" + textBox1.Text.Trim() + "'";
                if (Conn.ConsultaItem("Ambientes", reg))
                {
                    MessageBox.Show("Ese IdAmbiente ya existe. Digite otro.");
                    textBox1.Focus();
                    return;
                }
            }
            BtoGua.Enabled = true;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Complementos.SoloNumE(e); // si tu Id es numérico; elimínalo si usas alfanumérico
            if (e.KeyChar == (char)Keys.Enter && textBox1.Text.Trim() == string.Empty)
            { textBox1.Focus(); MessageBox.Show("Digite el IdAmbiente"); }
        }

        private void textBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && textBox1.Text == string.Empty)
            { MessageBox.Show("Digite el IdAmbiente"); e.IsInputKey = true; }
        }

        // ============== TIMER (si llegas a usarlo) ==============
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e) { swTimer = 1; }
        private void timer1_Tick(object sender, EventArgs e) { if (swTimer == 1) swTimer = 0; }

        // ================== IMPRESIÓN ===========================
        Image Log;
        byte[] ImaLog;
        int Pag;
        int currentRow = 0;

        private void BtoImp_Click(object sender, EventArgs e)
        {
            Pag = 0;

            // Datos empresa (opcional, si ya tienes tabla Empresa)
            try
            {
                Conexion ConE = new Conexion();
                ConE.ConsultaDatos("SELECT TOP 1 * FROM Empresa", "Empresa");
                if (ConE.Ds.Tables["Empresa"].Rows.Count > 0)
                {
                    var f = ConE.Ds.Tables["Empresa"].Rows[0];
                    ImaLog = f["Logo"] as byte[];
                    if (ImaLog != null && ImaLog.Length > 0)
                        using (MemoryStream ms = new MemoryStream(ImaLog)) { Log = Image.FromStream(ms); }
                }
            }
            catch { /* opcional: ignorar si no existe Empresa */ }

            PrintDocument DocPrin = new PrintDocument();
            PrintPreviewDialog ImpDoc = new PrintPreviewDialog
            {
                ClientSize = new Size(1000, 800),
                StartPosition = FormStartPosition.CenterScreen,
                Location = new Point(1, 1)
            };

            DocPrin.DefaultPageSettings.PaperSize = new PaperSize("Tamaño Carta", 850, 1100);
            DocPrin.PrintPage += new PrintPageEventHandler(this.DocPrin);
            DocPrin.EndPrint += new PrintEventHandler(DocPrinEndP);
            ImpDoc.Document = DocPrin;
            ImpDoc.ShowDialog();
        }

        private void DocPrinEndP(object sender, PrintEventArgs e) { Pag = 0; }

        private void DocPrin(object sender, PrintPageEventArgs e)
        {
            Font txtNeg = new Font("Arial Narrow", 12, FontStyle.Bold, GraphicsUnit.Point);
            Font txtSim = new Font("Arial Narrow", 11, FontStyle.Regular, GraphicsUnit.Point);

            int Fil = 50;

            e.Graphics.DrawString("Pág. " + (Pag += 1), txtNeg, Brushes.Black, e.PageBounds.Width - 100, 30);
            if (Log != null) e.Graphics.DrawImage(Log, new Rectangle(20, 20, 90, 80));
            e.Graphics.DrawString("<< A M B I E N T E S >>", txtNeg, Brushes.Black, (e.PageBounds.Width - 200) / 2, Fil += 25);

            e.Graphics.DrawString("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬", txtNeg, Brushes.Black, new RectangleF(70, Fil += 15, 800, 20));
            e.Graphics.DrawString(" IdAmbiente │          Nombre           │   Estado  ",
                txtNeg, Brushes.Black, new RectangleF(70, Fil += 15, 900, 20));
            e.Graphics.DrawString("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬", txtNeg, Brushes.Black, new RectangleF(70, Fil += 10, 800, 20));

            int itemPerPage = 1;
            int maxItemsPerPage = (e.MarginBounds.Height - 30) / 20;

            for (int i = currentRow; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow xFil = dataGridView1.Rows[i];

                string id = Convert.ToString(xFil.Cells["IdAmbiente"].Value);
                string nom = Convert.ToString(xFil.Cells["Nombre"].Value);
                string est = Convert.ToString(xFil.Cells["EstadoTxt"].Value);

                e.Graphics.DrawString(id, txtSim, Brushes.Black, new Rectangle(80, Fil += 20, 100, 20));
                e.Graphics.DrawString(nom, txtSim, Brushes.Black, new Rectangle(180, Fil, 350, 20));
                e.Graphics.DrawString(est, txtSim, Brushes.Black, new Rectangle(540, Fil, 120, 20));

                itemPerPage++;
                if (itemPerPage >= maxItemsPerPage)
                {
                    Fil = (e.MarginBounds.Height + 100);
                    e.HasMorePages = true;
                    currentRow = i + 1;
                    return;
                }
            }

            e.Graphics.DrawString("Total Registros: " + Convert.ToString(dataGridView1.Rows.Count),
                txtNeg, Brushes.Black, new Rectangle(400, Fil += 30, 200, 20));

            currentRow = 0;
            e.HasMorePages = false;
        }
    }
}
