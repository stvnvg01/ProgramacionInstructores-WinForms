using System;
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
    public partial class FrmCompetencias : Form
    {
        public FrmCompetencias()
        {
            InitializeComponent();
        }

        // ===== Campos / estado =====
        Conexion Conn = new Conexion();        // principal
        Conexion ConnAux = new Conexion();     // auxiliar
        int Pos = 0;
        string Opc = "0";                      // 1 = Nuevo, 2 = Editar, 4 = Consultar
        int swTimer = 0;
        DataRow fila;

        // Para evitar reentradas del grid
        private bool _suspendGridEvents = false;

        // ========= LOAD =========
        private void FrmCompetencias_Load(object sender, EventArgs e)
        {
            Conn.CargarEstilosDesdeBD();

            EstilosGlobales.AplicarEstiloBoton(BtoNue, "Nuevo");
            EstilosGlobales.AplicarEstiloBoton(BtoEdi, "Editar");
            EstilosGlobales.AplicarEstiloBoton(BtoEli, "Eliminar");
            EstilosGlobales.AplicarEstiloBoton(BtoGua, "Guardar");
            EstilosGlobales.AplicarEstiloBoton(BtoCon, "Consultar");
            EstilosGlobales.AplicarEstiloBoton(BtoImp, "Imprimir");
            EstilosGlobales.AplicarEstiloBoton(BtoSal, "Salir");

            // Longitudes máximas desde BD (Tag de cada TextBox = nombre de campo)
            SqlDataReader LenC;
            foreach (var Obj in this.Controls.OfType<TextBox>())
            {
                LenC = Conn.LenCampos("Competencias");
                while (LenC.Read())
                {
                    if (Obj.Tag != null && LenC.GetValue(0).ToString() == Obj.Tag.ToString())
                    {
                        Obj.MaxLength = Convert.ToInt32(LenC["LenCampo"]);
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

            try
            {
                if (!ConnAux.ConsultaItem("Perfil_Rol", "IdPerfil = 'Administrador'"))
                {
                    string AddPR = "Insert Into Perfil_Rol (IdPerfil,Nombre) Values ('01','Administrador')";
                    ConnAux.Insertar(AddPR);
                }
            }
            catch { }

            Conn.Conectar();

            // Suscribir evento estable al cambiar la selección
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;

            // Carga catálogo general inicialmente
            MostrarDatos();          // catálogo general (sin programa)

            ActDesObjetos(false);
            BtoGua.Enabled = false;

            // Campos bloqueados por defecto
            textBox2.Enabled = false; // IdCompetencia
            textBox3.Enabled = false; // Nombre
            textBox4.Enabled = false; // Duración plan
        }

        private void EnsureOpen()
        {
            if (Conexion.ConDB.State != ConnectionState.Open)
                Conexion.ConDB.Open();
        }

        private void ActDesObjetos(bool ActDes)
        {
            foreach (Control Obj in this.Controls.Cast<Control>().Union(panel1.Controls.Cast<Control>()))
            {
                if (Obj is TextBox || Obj is ComboBox)
                {
                    Obj.Enabled = ActDes;
                    if (Opc == "1" && Obj.TabIndex > 0) { Obj.Text = string.Empty; }
                }
            }
        }

        // ================== GRID / CONSULTA ==================
        // idPrograma null => catálogo general; si viene => competencias de ese programa
        private void MostrarDatos(string idPrograma = null)
        {
            string sql;

            if (!string.IsNullOrWhiteSpace(idPrograma))
            {
                string p = idPrograma.Replace("'", "''");
                sql = @"
SELECT 
    pc.IdPrograma,
    c.IdCompetencia,
    c.Nombre,
    pc.DuracionPlan
FROM ProgramasCompetencias pc
INNER JOIN Competencias c ON c.IdCompetencia = pc.IdCompetencia
WHERE pc.IdPrograma = '" + p + @"'
ORDER BY c.IdCompetencia";
            }
            else
            {
                // Mostrar TODO (sin filtrar): cada fila con su IdPrograma real
                sql = @"
SELECT 
    pc.IdPrograma,
    c.IdCompetencia,
    c.Nombre,
    pc.DuracionPlan
FROM ProgramasCompetencias pc
INNER JOIN Competencias c ON c.IdCompetencia = pc.IdCompetencia
ORDER BY pc.IdPrograma, c.IdCompetencia";
            }

            Conn.ConsultaDatos(sql, "Competencias");
            dataGridView1.DataSource = Conn.Ds.Tables["Competencias"];

            // Configuración columnas
            if (dataGridView1.Columns.Contains("IdPrograma"))
            {
                dataGridView1.Columns["IdPrograma"].HeaderText = "IdPrograma";
                dataGridView1.Columns["IdPrograma"].Width = 120;
            }
            dataGridView1.Columns["IdCompetencia"].HeaderText = "ID Competencia";
            dataGridView1.Columns["Nombre"].HeaderText = "Nombre";
            dataGridView1.Columns["DuracionPlan"].HeaderText = "Duración (plan)";

            dataGridView1.Columns["IdCompetencia"].Width = 150;
            dataGridView1.Columns["Nombre"].Width = 520;
            dataGridView1.Columns["DuracionPlan"].Width = 120;

            foreach (DataGridViewColumn col in dataGridView1.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.MultiSelect = false;

            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView1.RowsDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.White;

            if (Opc == "0")
            {
                Pos = Math.Min(Pos, Math.Max(Conn.Ds.Tables[0].Rows.Count - 1, 0));
                CargarDatos(Pos);
            }
        }

        // Llena SOLO los campos (no toca selección del grid)
        private void CargarCamposDesdeFila(int pos)
        {
            if (Conn.Ds.Tables[0].Rows.Count == 0) return;
            if (pos < 0 || pos >= Conn.Ds.Tables[0].Rows.Count) return;

            var f = Conn.Ds.Tables[0].Rows[pos];

            if (Conn.Ds.Tables[0].Columns.Contains("IdPrograma") && f["IdPrograma"] != DBNull.Value)
                textBox1.Text = Convert.ToString(f["IdPrograma"]).Trim();

            textBox2.Text = Convert.ToString(f["IdCompetencia"]).Trim();
            textBox3.Text = Convert.ToString(f["Nombre"]).Trim();
            textBox4.Text = Convert.ToString(f["DuracionPlan"]).Trim();

            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
        }

        // Selecciona fila y programa el enfoque de celda con BeginInvoke (sin reentrar)
        private void CargarDatos(int pos)
        {
            if (Conn.Ds.Tables[0].Rows.Count < 1)
            {
                EstadoBton(false);
                BtoNue.Enabled = true;
                BtoSal.Image = imageList1.Images[7];
                BtoSal.Text = " &Salir";
                Opc = "1"; ActDesObjetos(false); Opc = "0";
                return;
            }

            CargarCamposDesdeFila(pos);

            if (pos >= 0 && pos < dataGridView1.Rows.Count)
            {
                _suspendGridEvents = true;
                try
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[pos].Selected = true;

                    this.BeginInvoke(new Action(() =>
                    {
                        if (pos < dataGridView1.Rows.Count && dataGridView1.Columns.Count > 0)
                        {
                            _suspendGridEvents = true;
                            try
                            {
                                dataGridView1.CurrentCell = dataGridView1.Rows[pos].Cells[0];
                            }
                            finally { _suspendGridEvents = false; }
                        }
                    }));
                }
                finally { _suspendGridEvents = false; }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (_suspendGridEvents) return;
            if (dataGridView1.CurrentRow == null) return;

            Pos = dataGridView1.CurrentRow.Index;
            CargarCamposDesdeFila(Pos);
        }

        private void EstadoBton(bool ActDes)
        {
            foreach (Control Obj in this.Controls.Cast<Control>().Union(panel1.Controls.Cast<Control>()))
                if (Obj is Button) { Obj.Enabled = ActDes; }

            foreach (Control Obj in this.Controls)
                if (Obj is Button) { Obj.Enabled = ActDes; }

            BtoSal.Enabled = true;
            BtoCon.Enabled = true;   // consultar siempre habilitado

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

        // Aplica la consulta según IdPrograma (vacío = listar todo)
        private void AplicarConsulta()
        {
            string p = textBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(p))
                MostrarDatos();          // sin filtro
            else
                MostrarDatos(p);         // con filtro

            // Restablecer estado de la UI
            EstadoBton(true);
            ActDesObjetos(false);
            BtoGua.Enabled = false;
            dataGridView1.Enabled = true;
            Opc = "0";

            Pos = 0;
            if (Conn.Ds.Tables[0].Rows.Count > 0) CargarDatos(Pos);
        }

        // ================== NAVEGACIÓN ==================
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
            if (Pos >= (Conn.Ds.Tables[0].Rows.Count - 1)) { Ult_Click(null, null); }
            else { CargarDatos(Pos); Pri.Enabled = true; Ant.Enabled = true; }
        }

        private void Ult_Click(object sender, EventArgs e)
        {
            Pos = Conn.Ds.Tables[0].Rows.Count - 1;
            CargarDatos(Pos);
            Pri.Enabled = true; Ant.Enabled = true; Sig.Enabled = false; Ult.Enabled = false;
        }

        // ================== CRUD ==================
        // NUEVO
        private void BtoNue_Click(object sender, EventArgs e)
        {
            if (Conexion.VarGlobal.xTipoU == "01" && Conexion.VarGlobal.xEstIni == 2 || Conexion.VarGlobal.xNomU == "Invitado")
            {
                Opc = "1";
                EstadoBton(true);
                BtoGua.Enabled = true;
                ActDesObjetos(true);

                textBox1.Enabled = true;   // IdPrograma
                textBox2.Enabled = true;   // IdCompetencia
                textBox3.Enabled = true;   // Nombre
                textBox4.Enabled = true;   // DuracionPlan

                // Limpio menos IdPrograma si ya estaba en contexto
                // textBox1.Text = string.Empty;
                textBox2.Text = string.Empty;
                textBox3.Text = string.Empty;
                textBox4.Text = string.Empty;

                textBox1.Focus();
                dataGridView1.Enabled = false;
                if (Conexion.VarGlobal.xNomU == "Invitado") { Conexion.VarGlobal.xNomU = ""; }
            }
            else
            {
                MessageBox.Show("Solo Administrador y usuarios registrados acceden a esta función..!");
            }
        }

        // EDITAR (solo nombre y duración plan)
        private void BtoEdi_Click(object sender, EventArgs e)
        {
            if (Conexion.VarGlobal.xTipoU == "01" && Conexion.VarGlobal.xEstIni == 2)
            {
                Opc = "2";
                EstadoBton(false);
                BtoGua.Enabled = true;

                ActDesObjetos(true);
                textBox1.Enabled = false;  // IdPrograma fijo
                textBox2.Enabled = false;  // IdCompetencia fijo
                textBox3.Enabled = true;   // Nombre
                textBox4.Enabled = true;   // DuracionPlan

                dataGridView1.Enabled = false;
            }
            else { MessageBox.Show("Solo Administradores y usuarios registrados acceden a esta función..!"); }
        }

        private void BtoGua_Click(object sender, EventArgs e)
        {
            // 1=IdPrograma, 2=IdCompetencia, 3=Nombre, 4=DuracionPlan
            string idPrograma = textBox1.Text.Trim();
            string idCompetencia = textBox2.Text.Trim();
            string nombre = textBox3.Text.Trim();

            if (string.IsNullOrWhiteSpace(idPrograma))
            { MessageBox.Show("Digite el IdPrograma (contexto)."); textBox1.Focus(); return; }

            if (string.IsNullOrWhiteSpace(idCompetencia))
            { MessageBox.Show("Digite el IdCompetencia."); textBox2.Focus(); return; }

            if (string.IsNullOrWhiteSpace(nombre))
            { MessageBox.Show("Digite el Nombre de la competencia."); textBox3.Focus(); return; }

            if (!int.TryParse(textBox4.Text.Trim(), out int duracionPlan) || duracionPlan < 0)
            { MessageBox.Show("Duración plan inválida."); textBox4.Focus(); return; }

            try
            {
                EnsureOpen();

                // 0) Verificar programa
                using (var chk = new SqlCommand("SELECT 1 FROM Programas WHERE IdPrograma=@p", Conexion.ConDB))
                {
                    chk.Parameters.AddWithValue("@p", idPrograma);
                    var ok = chk.ExecuteScalar();
                    if (ok == null)
                    {
                        MessageBox.Show("El IdPrograma no existe en la tabla Programas.");
                        textBox1.Focus(); return;
                    }
                }

                // 1) UPSERT catálogo Competencias
                if (Opc == "1") // NUEVO → evita duplicado
                {
                    using (var ex = new SqlCommand("SELECT 1 FROM Competencias WHERE IdCompetencia=@c", Conexion.ConDB))
                    {
                        ex.Parameters.AddWithValue("@c", idCompetencia);
                        if (ex.ExecuteScalar() != null)
                        {
                            MessageBox.Show("Ese IdCompetencia ya existe en el catálogo.");
                            textBox2.Focus(); return;
                        }
                    }
                }

                using (var upsertC = new SqlCommand(@"
IF EXISTS (SELECT 1 FROM Competencias WHERE IdCompetencia=@c)
   UPDATE Competencias SET Nombre=@n WHERE IdCompetencia=@c;
ELSE
   INSERT INTO Competencias (IdCompetencia, Nombre, Duracion) VALUES (@c, @n, @d0);
", Conexion.ConDB))
                {
                    upsertC.Parameters.AddWithValue("@c", idCompetencia);
                    upsertC.Parameters.AddWithValue("@n", nombre);
                    upsertC.Parameters.AddWithValue("@d0", duracionPlan); // opcional
                    upsertC.ExecuteNonQuery();
                }

                // 2) UPSERT relación Programa-Competencia
                using (var upsertPC = new SqlCommand(@"
IF EXISTS (SELECT 1 FROM ProgramasCompetencias WHERE IdPrograma=@p AND IdCompetencia=@c)
   UPDATE ProgramasCompetencias SET DuracionPlan=@d WHERE IdPrograma=@p AND IdCompetencia=@c;
ELSE
   INSERT INTO ProgramasCompetencias (IdPrograma, IdCompetencia, DuracionPlan) VALUES (@p, @c, @d);
", Conexion.ConDB))
                {
                    upsertPC.Parameters.AddWithValue("@p", idPrograma);
                    upsertPC.Parameters.AddWithValue("@c", idCompetencia);
                    upsertPC.Parameters.AddWithValue("@d", duracionPlan);
                    upsertPC.ExecuteNonQuery();
                }

                // Refrescar en contexto del programa
                string xId = idCompetencia;
                MostrarDatos(idPrograma);

                Conn.Ds.Tables[0].PrimaryKey = new DataColumn[] { Conn.Ds.Tables[0].Columns["IdCompetencia"] };
                Pos = Conn.Ds.Tables[0].Rows.IndexOf(Conn.Ds.Tables[0].Rows.Find(xId));
                if (Pos < 0) Pos = 0;
                CargarDatos(Pos);

                dataGridView1.Enabled = true;
                BtoSal.Text = "&Cerrar";
                this.BtoSal_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (Conexion.ConDB.State == ConnectionState.Open) Conexion.ConDB.Close();
            }
        }

        // ======= CONSULTAR =======
        private void BtoCon_Click(object sender, EventArgs e)
        {
            Opc = "4";
            EstadoBton(false);

            // Solo se edita el IdPrograma para consultar
            textBox1.Enabled = true;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;

            textBox1.Text = string.Empty;
            textBox1.Focus();

            // Tip: muestra todo si el usuario no digita nada y presiona Enter o Tab
            toolTip1.SetToolTip(textBox1, "Digite un IdPrograma y presione Enter. Déjelo vacío para ver todo.");
        }

        private void BtoEli_Click(object sender, EventArgs e)
        {
            if (Conexion.VarGlobal.xTipoU == "01" && Conexion.VarGlobal.xEstIni == 2)
            {
                DialogResult resu = MessageBox.Show("¿Está seguro de eliminar <" + textBox1.Text.Trim() + " | " + textBox2.Text.Trim() +
                                                    "> de la base de datos?", "A D V E R T E N C I A", MessageBoxButtons.YesNo);
                if (resu == DialogResult.Yes)
                {
                    try
                    {
                        EnsureOpen();

                        // 1) Quitar relación del programa actual (si hay)
                        using (var delPC = new SqlCommand("DELETE FROM ProgramasCompetencias WHERE IdPrograma=@p AND IdCompetencia=@c", Conexion.ConDB))
                        {
                            delPC.Parameters.AddWithValue("@p", textBox1.Text.Trim());
                            delPC.Parameters.AddWithValue("@c", textBox2.Text.Trim());
                            delPC.ExecuteNonQuery();
                        }

                        if (Conexion.ConDB.State == ConnectionState.Open) Conexion.ConDB.Close();

                        Pos -= 1;
                        int PosN = Pos < 0 ? 0 : Pos;

                        var p = textBox1.Text.Trim();
                        if (!string.IsNullOrWhiteSpace(p))
                            MostrarDatos(p);
                        else
                            MostrarDatos();

                        CargarDatos(PosN);
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
            string xOp = Opc; Opc = string.Empty;

            if (BtoSal.Text.Trim() == "&Cerrar")
            {
                EstadoBton(true);
                int xPos = Pos;
                if (xOp != "4")
                {
                    var p = textBox1.Text.Trim();
                    if (!string.IsNullOrWhiteSpace(p)) MostrarDatos(p);
                    else MostrarDatos();
                }
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

        // ================== VALIDACIONES CAMPO A CAMPO ==================
        private void textBox1_Validated(object sender, EventArgs e)
        {
            // textBox1 = IdPrograma
            var p = textBox1.Text.Trim();
            if (p.Length == 0) { return; }

            EnsureOpen();
            using (var cmd = new SqlCommand("SELECT 1 FROM Programas WHERE IdPrograma=@p", Conexion.ConDB))
            {
                cmd.Parameters.AddWithValue("@p", p);
                var ok = cmd.ExecuteScalar();
                if (ok == null)
                {
                    MessageBox.Show("El IdPrograma no existe.");
                    textBox1.Focus();
                    if (Conexion.ConDB.State == ConnectionState.Open) Conexion.ConDB.Close();
                    return;
                }
            }
            if (Conexion.ConDB.State == ConnectionState.Open) Conexion.ConDB.Close();

            // Cargar en contexto
            MostrarDatos(p);
        }

        private void textBox2_Validated(object sender, EventArgs e)
        {
            // En modo NUEVO, si no existe ese IdCompetencia, habilitamos nombre/duración
            if (Opc == "1" && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                if (Conn.ConsultaItem("Competencias", "IdCompetencia='" + textBox2.Text.Trim() + "'"))
                {
                    // Existe: precarga su nombre
                    EnsureOpen();
                    using (var cmd = new SqlCommand("SELECT Nombre FROM Competencias WHERE IdCompetencia=@c", Conexion.ConDB))
                    {
                        cmd.Parameters.AddWithValue("@c", textBox2.Text.Trim());
                        var n = cmd.ExecuteScalar()?.ToString();
                        if (!string.IsNullOrEmpty(n)) textBox3.Text = n.Trim();
                    }
                    if (Conexion.ConDB.State == ConnectionState.Open) Conexion.ConDB.Close();
                }
                textBox3.Enabled = true;
                textBox4.Enabled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Opc == "4" && e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;    // no beep
                AplicarConsulta();
                return;
            }

            // (comportamiento previo)
            if (e.KeyChar == (char)Keys.Enter && textBox1.Text.Trim() == string.Empty && Opc != "4")
            {
                textBox1.Focus();
                MessageBox.Show("Digite el IdPrograma");
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Si quieres restringir a numérico, usa: Complementos.SoloNumE(e);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Nombre: no restringir a numérico
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Duración plan: solo numérico
            Complementos.SoloNumE(e);
        }

        // ================== IMPRESIÓN ==================
        string IdN, NoE, NoP, Tel, Cor, Dir, Lem;
        byte[] ImaLog;
        Image Log;
        int Pag;
        private int currentRow = 0;

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
            PrintPreviewDialog ImpDoc = new PrintPreviewDialog
            {
                ClientSize = new System.Drawing.Size(1000, 800),
                StartPosition = FormStartPosition.CenterScreen,
                Location = new System.Drawing.Point(1, 1),
                Document = DocPrin
            };

            DocPrin.DefaultPageSettings.PaperSize = new PaperSize("Tamaño Carta", 850, 1100);
            DocPrin.PrintPage += new PrintPageEventHandler(this.DocPrin);
            DocPrin.EndPrint += new PrintEventHandler(DocPrinEndP);
            ImpDoc.ShowDialog();
        }

        //private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{

        //}

        private void DocPrinEndP(object sender, PrintEventArgs e) { Pag = 0; }

        private void DocPrin(object sender, PrintPageEventArgs e)
        {
            Font txtNeg = new Font("Arial Narrow", 12, FontStyle.Bold, GraphicsUnit.Point);
            Font txtSim = new Font("Arial Narrow", 11, FontStyle.Regular, GraphicsUnit.Point);

            SizeF txtSize1 = e.Graphics.MeasureString(NoE, txtNeg);
            SizeF txtSize2 = e.Graphics.MeasureString(IdN, txtNeg);
            SizeF txtSize3 = e.Graphics.MeasureString("<< C O M P E T E N C I A S >>", txtNeg);
            SizeF txtSize4 = e.Graphics.MeasureString(Tel + " - " + Cor + " - " + Dir, txtNeg);
            SizeF txtSize5 = e.Graphics.MeasureString(Lem, txtNeg);

            int Fil = 50;

            e.Graphics.DrawString("Pág. " + (Pag += 1), txtNeg, Brushes.Black, e.PageBounds.Width - 100, 30);
            if (Log != null) e.Graphics.DrawImage(Log, new Rectangle(20, 20, 90, 80));
            e.Graphics.DrawString(NoE, txtNeg, Brushes.Black, (e.PageBounds.Width - txtSize1.Width) / 2, 20);
            e.Graphics.DrawString(IdN, txtNeg, Brushes.Black, (e.PageBounds.Width - txtSize2.Width) / 2, 40);
            e.Graphics.DrawString("<< C O M P E T E N C I A S >>", txtNeg, Brushes.Black, (e.PageBounds.Width - txtSize3.Width) / 2, Fil += 25);

            e.Graphics.DrawString("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬", txtNeg, Brushes.Black, new RectangleF(70, Fil += 15, 800, 20));
            e.Graphics.DrawString("   IdCompetencia     |               Nombre               |  Duración (h) ",
                txtNeg, Brushes.Black, new RectangleF(70, Fil += 15, 900, 20));
            e.Graphics.DrawString("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬", txtNeg, Brushes.Black, new RectangleF(70, Fil += 10, 800, 20));

            int itemPerPage = 1;
            int maxItemsPerPage = (e.MarginBounds.Height - 30) / 20;

            for (int i = currentRow; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow xFil = dataGridView1.Rows[i];

                string idC = Convert.ToString(xFil.Cells["IdCompetencia"].Value);
                string nom = Convert.ToString(xFil.Cells["Nombre"].Value);
                string dur = Convert.ToString(xFil.Cells["DuracionPlan"].Value);

                e.Graphics.DrawString(idC + " ", txtSim, Brushes.Black, new Rectangle(80, Fil += 20, 150, 20));
                e.Graphics.DrawString(nom + " ", txtSim, Brushes.Black, new Rectangle(240, Fil, 420, 20));
                e.Graphics.DrawString(dur + " ", txtSim, Brushes.Black, new Rectangle(670, Fil, 120, 20));

                itemPerPage++;
                if (itemPerPage >= maxItemsPerPage)
                {
                    Fil = (e.MarginBounds.Height + 100);
                    e.Graphics.DrawString("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬", txtNeg, Brushes.Black, new RectangleF(70, Fil += 15, 800, 20));
                    e.Graphics.DrawString("☏ " + Tel + " - ☒" + Cor + " - ⚑" + Dir, txtNeg, Brushes.Black, (e.PageBounds.Width - txtSize4.Width) / 2, Fil += 15);
                    e.Graphics.DrawString(Lem, txtNeg, Brushes.Black, (e.PageBounds.Width - txtSize5.Width) / 2, Fil += 20);

                    currentRow = i + 1;
                    e.HasMorePages = true;
                    return;
                }
            }

            e.Graphics.DrawString("Total Registro: " + Convert.ToString(dataGridView1.Rows.Count) + " ", txtNeg, Brushes.Black, new Rectangle(400, Fil += 30, 200, 20));

            Fil = (e.MarginBounds.Height + 75);
            e.Graphics.DrawString("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬", txtNeg, Brushes.Black, new RectangleF(70, Fil += 15, 800, 20));
            e.Graphics.DrawString("☏ " + Tel + " - ☒" + Cor + " - ⚑" + Dir, txtNeg, Brushes.Black, (e.PageBounds.Width - txtSize4.Width) / 2, Fil += 15);
            e.Graphics.DrawString(Lem, txtNeg, Brushes.Black, (e.PageBounds.Width - txtSize5.Width) / 2, Fil += 20);

            currentRow = 0;
            e.HasMorePages = false;
        }

        // ======= (opcionales del timer/combos) =======
        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e) { swTimer = 1; }
        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e) { swTimer = 2; }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (swTimer == 1) { swTimer = 0; }
            if (swTimer == 2) { swTimer = 0; }
        }

        // Validación adicional en modo consultar (Enter en IdPrograma)
        private void textBox1_Validated_1(object sender, EventArgs e)
        {
            if (Opc == "4")
            {
                // En modo consultar, aplicar filtro (vacío = todo)
                AplicarConsulta();
                return;
            }
        }
    }
}
