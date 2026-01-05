using BibliotecaExcel2;
using ProgramacionInstructores;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Excel = Microsoft.Office.Interop.Excel;

namespace ProgramacionInstructores
{
    public partial class FrmCronograma : Form
    {
        // Conexión
        private SqlConnection ConDB => Conexion.ConDB;

        public FrmCronograma()
        {
            InitializeComponent();
        }

        // Estados y auxiliares
        Dictionary<string, bool[,]> cronogramasGuardados = new Dictionary<string, bool[,]>();
        private List<DataGridViewCell> celdasSeleccionadas = new List<DataGridViewCell>();
        private bool seleccionando = false;
        private Point celdaInicio;

        private Dictionary<string, Infoceldas> infoCeldas = new Dictionary<string, Infoceldas>();
        private Infoceldas datosArrastre = null;

        private GeneradorCronograma generadorCronograma = new GeneradorCronograma();

        private AutoCompleteStringCollection autoCompleteCollection;
        private Dictionary<string, string> instructoresMap; // Nombre -> IdPersona

        // === MAPA DE COLORES COMPARTIDO (GRID <-> EXCEL) ===
        private readonly Dictionary<string, Color> coloresAsignados = new Dictionary<string, Color>();
        public Dictionary<string, Color> GetColorMap() => coloresAsignados;
        public static string MakeKey(string ficha, string competencia)
            => $"{(ficha ?? "").Trim()}_{(competencia ?? "").Trim()}";

        private readonly List<Color> paletaColores = new List<Color>
        {
            Color.LightBlue, Color.LightGreen, Color.LightCoral, Color.LightGoldenrodYellow,
            Color.LightPink, Color.LightSalmon, Color.LightSeaGreen, Color.LightSkyBlue,
            Color.LightSteelBlue, Color.LightCyan, Color.Plum, Color.Khaki
        };

        private void Form1cronograma_Load(object sender, EventArgs e)
        {
            dgvCronograma.ReadOnly = false;
            dgvCronograma.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvCronograma.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvCronograma.AllowUserToAddRows = false;
            dgvCronograma.AllowUserToDeleteRows = false;
            dgvCronograma.MultiSelect = true;

            generadorCronograma.CheckBox1CheckedChanged += checkBox1_CheckedChanged;
            generadorCronograma.CheckBox2CheckedChanged += checkBox2_CheckedChanged;

            IdPersona.TabStop = true; IdPersona.TabIndex = 0;
            btnGenerar.TabStop = true; btnGenerar.TabIndex = 1;

            IdPersona.KeyDown += (s, ev) =>
            {
                if (ev.KeyCode == Keys.Enter)
                {
                    btnGenerar.Focus();
                    ev.Handled = true;
                }
            };

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy";
            dateTimePicker1.ShowUpDown = true;

            checkBox1.Enabled = false; checkBox2.Enabled = false;
            checkBox1.Checked = false; checkBox2.Checked = false;

            // Solo 1 hora
            chk1Hora.Visible = true;
            chk2Horas.Visible = false;
            chk4Horas.Visible = false;

            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            checkBox2.CheckedChanged += checkBox2_CheckedChanged;
            chk1Hora.CheckedChanged += chkHoras_CheckedChanged;
            chk1Hora.Checked = true;

            comboBox3.Items.AddRange(new string[] {
                "Enero","Febrero","Marzo","Abril","Mayo","Junio",
                "Julio","Agosto","Septiembre","Octubre","Noviembre","Diciembre"
            });
            comboBox3.SelectedIndex = DateTime.Now.Month - 1;

            comboBox3.SelectedIndexChanged += (s, ev) =>
            {
                int año = dateTimePicker1.Value.Year;
                int mes = comboBox3.SelectedIndex + 1;
                dateTimePicker1.Value = new DateTime(año, mes, 1);
            };
            dateTimePicker1.ValueChanged += (s, ev) =>
            {
                comboBox3.SelectedIndex = dateTimePicker1.Value.Month - 1;
            };

            dgvCronograma.MouseDown += dgvCronograma_MouseDown;
            dgvCronograma.MouseMove += dgvCronograma_MouseMove;
            dgvCronograma.MouseUp += dgvCronograma_MouseUp;
            dgvCronograma.KeyDown += dgvCronograma_KeyDown;
            dgvCronograma.KeyPress += dgvCronograma_KeyPress;

            Departamentos();
            Departamento.SelectedIndexChanged += Departamento_SelectedIndexChanged;

            ConfigurarAutocompletado();
        }

        private void CargarDepartamentoMunicipioPorInstructor(string idPersona)
        {
            try
            {
                string idLugar = null, idDpto = null;
                const string sql = @"
                    SELECT p.IdLugar, m.IdDpto
                    FROM Personas p
                    INNER JOIN Municipios m ON p.IdLugar = m.IdLugar
                    WHERE p.IdPersona = @id";

                using (var cmd = new SqlCommand(sql, ConDB))
                {
                    cmd.Parameters.AddWithValue("@id", idPersona);
                    if (ConDB.State != ConnectionState.Open) ConDB.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            idLugar = rdr["IdLugar"].ToString();
                            idDpto = rdr["IdDpto"].ToString();
                        }
                    }
                }

                if (idLugar != null && idDpto != null)
                {
                    Departamento.SelectedValue = idDpto;

                    var dtM = new DataTable();
                    using (var cmdM = new SqlCommand(
                        "SELECT IdLugar, Nombre FROM Municipios WHERE IdDpto = @dpto ORDER BY Nombre", ConDB))
                    {
                        cmdM.Parameters.AddWithValue("@dpto", idDpto);
                        using (var da = new SqlDataAdapter(cmdM)) da.Fill(dtM);
                    }
                    Municipio.DisplayMember = "Nombre";
                    Municipio.ValueMember = "IdLugar";
                    Municipio.DataSource = dtM;

                    Municipio.SelectedValue = idLugar;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar Departamento/Municipio: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarAutocompletado()
        {
            try
            {
                autoCompleteCollection = new AutoCompleteStringCollection();
                instructoresMap = new Dictionary<string, string>();
                CargarInstructoresParaAutocompletado();

                IdPersona.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                IdPersona.AutoCompleteSource = AutoCompleteSource.CustomSource;
                IdPersona.AutoCompleteCustomSource = autoCompleteCollection;

                IdPersona.TextChanged += IdPersona_TextChanged;
                IdPersona.Leave += IdPersona_Leave;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al configurar autocompletado: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CargarInstructoresParaAutocompletado()
        {
            const string sqlConPersonas = @"
               SELECT DISTINCT 
               i.IdPersona,
               i.IdInstructor,
               i.ObservacionPerfil,
               CONCAT(p.Nombres, ' ', p.Apellidos) as NombreCompleto,
               p.NumeroDocumento
               FROM Instructores i
               LEFT JOIN Personas p ON i.IdPersona = p.IdPersona
               WHERE i.IdPersona IS NOT NULL 
               AND i.IdInstructor IS NOT NULL
               ORDER BY COALESCE(CONCAT(p.Nombres, ' ', p.Apellidos), i.IdInstructor)";

            const string sqlSoloInstructores = @"
                SELECT DISTINCT 
                IdPersona,
                IdInstructor,
                ObservacionPerfil
                FROM Instructores
                WHERE IdPersona IS NOT NULL 
                AND IdInstructor IS NOT NULL
                ORDER BY IdInstructor";

            try
            {
                using (var cmd = new SqlCommand(sqlConPersonas, ConDB))
                {
                    if (ConDB.State != ConnectionState.Open) ConDB.Open();

                    try
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            autoCompleteCollection.Clear();
                            instructoresMap.Clear();

                            while (reader.Read())
                            {
                                string idPersona = reader["IdPersona"].ToString();
                                string idInstructor = reader["IdInstructor"].ToString();
                                string observacionPerfil = reader["ObservacionPerfil"]?.ToString() ?? "";
                                string nombreCompleto = reader["NombreCompleto"]?.ToString() ?? "";
                                string numeroDocumento = reader["NumeroDocumento"]?.ToString() ?? "";

                                string displayText;
                                if (!string.IsNullOrEmpty(nombreCompleto))
                                {
                                    displayText = string.IsNullOrEmpty(numeroDocumento)
                                        ? $"{nombreCompleto} ({idInstructor})"
                                        : $"{nombreCompleto} - {numeroDocumento} ({idInstructor})";
                                }
                                else
                                {
                                    displayText = string.IsNullOrEmpty(observacionPerfil)
                                        ? $"{idInstructor} (ID: {idPersona})"
                                        : $"{idInstructor} - {observacionPerfil}";
                                }

                                autoCompleteCollection.Add(displayText);
                                autoCompleteCollection.Add(idInstructor);
                                autoCompleteCollection.Add(idPersona);
                                if (!string.IsNullOrEmpty(nombreCompleto)) autoCompleteCollection.Add(nombreCompleto);
                                if (!string.IsNullOrEmpty(numeroDocumento)) autoCompleteCollection.Add(numeroDocumento);
                                if (!string.IsNullOrEmpty(observacionPerfil)) autoCompleteCollection.Add(observacionPerfil);

                                instructoresMap[displayText] = idPersona;
                                instructoresMap[idInstructor] = idPersona;
                                instructoresMap[idPersona] = idPersona;
                                if (!string.IsNullOrEmpty(nombreCompleto)) instructoresMap[nombreCompleto] = idPersona;
                                if (!string.IsNullOrEmpty(numeroDocumento)) instructoresMap[numeroDocumento] = idPersona;
                                if (!string.IsNullOrEmpty(observacionPerfil)) instructoresMap[observacionPerfil] = idPersona;
                            }
                        }
                    }
                    catch (SqlException)
                    {
                        using (var cmdSimple = new SqlCommand(sqlSoloInstructores, ConDB))
                        using (var reader = cmdSimple.ExecuteReader())
                        {
                            autoCompleteCollection.Clear();
                            instructoresMap.Clear();

                            while (reader.Read())
                            {
                                string idPersona = reader["IdPersona"].ToString();
                                string idInstructor = reader["IdInstructor"].ToString();
                                string observacionPerfil = reader["ObservacionPerfil"]?.ToString() ?? "";

                                string displayText = string.IsNullOrEmpty(observacionPerfil)
                                    ? $"{idInstructor}"
                                    : $"{idInstructor} - {observacionPerfil}";

                                autoCompleteCollection.Add(displayText);
                                autoCompleteCollection.Add(idInstructor);
                                autoCompleteCollection.Add(idPersona);
                                if (!string.IsNullOrEmpty(observacionPerfil)) autoCompleteCollection.Add(observacionPerfil);

                                instructoresMap[displayText] = idPersona;
                                instructoresMap[idInstructor] = idPersona;
                                instructoresMap[idPersona] = idPersona;
                                if (!string.IsNullOrEmpty(observacionPerfil)) instructoresMap[observacionPerfil] = idPersona;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar instructores: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnsureInstructorExists(string idPersona)
        {
            const string sql = @"
               IF NOT EXISTS (
               SELECT 1 
               FROM Instructores 
               WHERE IdPersona = @id)
               BEGIN
               INSERT INTO Instructores (IdPersona, IdInstructor)
               VALUES (@id, @id);
               END";
            using (var cmd = new SqlCommand(sql, ConDB))
            {
                cmd.Parameters.AddWithValue("@id", idPersona);
                if (ConDB.State != ConnectionState.Open) ConDB.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void Departamentos()
        {
            var dt = new DataTable();
            using (var cmd = new SqlCommand("SELECT IdDpto, Nombre FROM Departamentos ORDER BY Nombre", ConDB))
            using (var da = new SqlDataAdapter(cmd)) da.Fill(dt);

            Departamento.DisplayMember = "Nombre";
            Departamento.ValueMember = "IdDpto";
            Departamento.DataSource = dt;
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            // Reset selección temporal
            infoCeldas.Clear();
            celdasSeleccionadas.Clear();
            seleccionando = false;

            // Generar estructura
            generadorCronograma.GenerarCronograma(
                dgvCronograma, checkBox1, checkBox2, IdPersona, dateTimePicker1, comboBox3,
                null, // antes se usaba comboBox2
                chk1Hora, chk2Horas, chk4Horas, cronogramasGuardados, AplicarColorSabadoODomingo,
                AdjustDataGridViewHeight
            );

            // Cargar desde DB
            LoadCronogramaFromDb();

            // Colorear fuera de mes
            int año = dateTimePicker1.Value.Year;
            int mes = comboBox3.SelectedIndex + 1;
            DateTime primerDiaMes = new DateTime(año, mes, 1);
            int offset = ((int)primerDiaMes.DayOfWeek + 6) % 7;
            DateTime diaInicio = primerDiaMes.AddDays(-offset);

            for (int col = 1; col < dgvCronograma.Columns.Count; col++)
            {
                DateTime fechaCol = diaInicio.AddDays(col - 1);
                if (fechaCol.Month != mes)
                {
                    for (int fila = 0; fila < dgvCronograma.Rows.Count; fila++)
                    {
                        var celda = dgvCronograma.Rows[fila].Cells[col];
                        string celdaKey = $"{fila}_{col}";
                        if (!infoCeldas.ContainsKey(celdaKey) && celda.Value?.ToString() != "X")
                        {
                            celda.Style.BackColor = Color.LightGray;
                            celda.ReadOnly = true;
                        }
                    }
                }
            }

            // Fines de semana
            AplicarColorSabadoODomingo(DayOfWeek.Saturday, checkBox1.Checked);
            AplicarColorSabadoODomingo(DayOfWeek.Sunday, checkBox2.Checked);

            if (IdPersona.Tag != null)
            {
                string idPersonaReal = IdPersona.Tag.ToString();
                CargarDepartamentoMunicipioPorInstructor(idPersonaReal);
            }
        }

        private void AdjustDataGridViewHeight()
        {
            int rowHeight = dgvCronograma.Rows.Count > 0 ? dgvCronograma.Rows[0].Height : 22;
            int headerHeight = dgvCronograma.ColumnHeadersHeight;
            int totalHeight = headerHeight + (dgvCronograma.RowCount * rowHeight);
            dgvCronograma.Height = totalHeight + 4;
        }

        private void dgvCronograma_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var handler = new dgvCronograma_Cellclick.dgvCronograma_Cellclic();
            handler.CellClick(
                dgvCronograma: dgvCronograma,
                fechaSeleccionada: dateTimePicker1.Value,
                mesSeleccionado: comboBox3.SelectedIndex + 1,
                incluirSabados: checkBox1.Checked,
                incluirDomingos: checkBox2.Checked,
                idTexto: IdPersona.Text,
                e: e
            );
        }

        private Color ObtenerColorPorHora(int fila)
        {
            Color[] coloresPorHora = Enumerable.Repeat(Color.LightGreen, 16).ToArray();
            return coloresPorHora[fila % coloresPorHora.Length];
        }

        private void dgvCronograma_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true; // bloquear escritura
        }

        private void btnGuardarExcel_Click(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex < 0)
            {
                MessageBox.Show("Selecciona un mes antes de guardar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(IdPersona.Text))
            {
                MessageBox.Show("Selecciona un instructor antes de guardar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string mes = comboBox3.SelectedItem?.ToString() ?? DateTime.Now.ToString("MMMM");
            int anio = dateTimePicker1.Value.Year;
            int mesNum = comboBox3.SelectedIndex + 1;

            string nombreInstructor = !string.IsNullOrWhiteSpace(txtInstructorDisplay.Text)
                ? txtInstructorDisplay.Text
                : IdPersona.Text;

            string suggestedName = $"Cronograma_{SafeFilePart(mes)}_{anio}_{SafeFilePart(nombreInstructor)}.xlsx";

            using (var sfd = new SaveFileDialog())
            {
                sfd.Title = "Guardar Cronograma";
                sfd.FileName = suggestedName;
                sfd.Filter = "Excel (*.xlsx)|*.xlsx";
                sfd.AddExtension = true;
                sfd.DefaultExt = "xlsx";
                sfd.OverwritePrompt = true;

                if (sfd.ShowDialog(this) != DialogResult.OK) return;

                string outputPath = sfd.FileName;
                if (!outputPath.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                    outputPath += ".xlsx";

                try
                {
                    MessageBox.Show("Guardando archivo, por favor espera…", "Procesando",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    var excel = new BibliotecaExcel2.Class1();
                    excel.GenerarExelAPath(
                        dgvCronograma,
                        nombreInstructor,
                        "1 HORA",
                        mes,
                        ObtenerIdInstructorReal(),
                        mesNum,
                        anio,
                        outputPath,
                        GetColorMap() // <-- mismo mapa de colores
                    );

                    MessageBox.Show($"Cronograma guardado en:\n{outputPath}", "Listo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo guardar el Excel:\n" + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) { AplicarColorSabadoODomingo(DayOfWeek.Saturday, checkBox1.Checked); }
        private void checkBox2_CheckedChanged(object sender, EventArgs e) { AplicarColorSabadoODomingo(DayOfWeek.Sunday, checkBox2.Checked); }

        private void AplicarColorSabadoODomingo(DayOfWeek diaSemana, bool habilitar)
        {
            int año = dateTimePicker1.Value.Year;
            int mes = comboBox3.SelectedIndex + 1;

            DateTime primerDiaMes = new DateTime(año, mes, 1);
            int offset = ((int)primerDiaMes.DayOfWeek + 6) % 7;
            DateTime diaInicio = primerDiaMes.AddDays(-offset);

            for (int col = 1; col < dgvCronograma.Columns.Count; col++)
            {
                DateTime fechaCol = diaInicio.AddDays(col - 1);

                if (fechaCol.DayOfWeek == diaSemana)
                {
                    for (int fila = 0; fila < dgvCronograma.Rows.Count; fila++)
                    {
                        var celda = dgvCronograma.Rows[fila].Cells[col];
                        string celdaKey = $"{fila}_{col}";
                        bool tieneDatosGuardados = infoCeldas.ContainsKey(celdaKey) || celda.Value?.ToString() == "X";

                        if (!habilitar)
                        {
                            if (!tieneDatosGuardados)
                            {
                                celda.Style.BackColor = Color.LightGray;
                                celda.ReadOnly = true;
                                celda.Value = ""; celda.ToolTipText = "";
                            }
                        }
                        else
                        {
                            if (fechaCol.Month == mes && celda.Style.BackColor == Color.LightGray && !tieneDatosGuardados)
                            {
                                celda.ReadOnly = false; celda.Style.BackColor = Color.White;
                            }
                        }
                    }
                }
            }
        }

        private bool EsCeldaEditable(int fila, int columna)
        {
            if (columna == 0) return false;
            var celda = dgvCronograma.Rows[fila].Cells[columna];
            if (celda.Style.BackColor == Color.LightGray) return false;

            int año = dateTimePicker1.Value.Year;
            int mes = comboBox3.SelectedIndex + 1;
            DateTime primerDiaMes = new DateTime(año, mes, 1);
            int offset = ((int)primerDiaMes.DayOfWeek + 6) % 7;
            DateTime diaInicio = primerDiaMes.AddDays(-offset);
            DateTime fechaCol = diaInicio.AddDays(columna - 1);

            if (fechaCol.DayOfWeek == DayOfWeek.Saturday && !checkBox1.Checked) return false;
            if (fechaCol.DayOfWeek == DayOfWeek.Sunday && !checkBox2.Checked) return false;

            return true;
        }

        private void chkHoras_CheckedChanged(object sender, EventArgs e)
        {
            if (!(sender as CheckBox).Checked) return;

            if (sender == chk1Hora) { chk2Horas.Checked = false; chk4Horas.Checked = false; }
            else if (sender == chk2Horas) { chk1Hora.Checked = false; chk4Horas.Checked = false; }
            else if (sender == chk4Horas) { chk1Hora.Checked = false; chk2Horas.Checked = false; }

            if (!string.IsNullOrWhiteSpace(IdPersona.Text))
                btnGenerar.PerformClick();
        }

        private void dgvCronograma_MouseDown(object sender, MouseEventArgs e)
        {
            var hit = dgvCronograma.HitTest(e.X, e.Y);
            if (hit.RowIndex >= 0 && hit.ColumnIndex > 0)
            {
                if (!EsCeldaEditable(hit.RowIndex, hit.ColumnIndex)) return;

                seleccionando = true;
                celdaInicio = new Point(hit.ColumnIndex, hit.RowIndex);
                celdasSeleccionadas.Clear();
                var celda = dgvCronograma.Rows[hit.RowIndex].Cells[hit.ColumnIndex];
                celdasSeleccionadas.Add(celda);
            }
        }

        private void dgvCronograma_MouseMove(object sender, MouseEventArgs e)
        {
            if (!seleccionando) return;

            var hit = dgvCronograma.HitTest(e.X, e.Y);
            if (hit.RowIndex >= 0 && hit.ColumnIndex > 0)
            {
                int colIni = Math.Min(celdaInicio.X, hit.ColumnIndex);
                int colFin = Math.Max(celdaInicio.X, hit.ColumnIndex);
                int rowIni = Math.Min(celdaInicio.Y, hit.RowIndex);
                int rowFin = Math.Max(celdaInicio.Y, hit.RowIndex);

                celdasSeleccionadas.Clear();

                for (int fila = rowIni; fila <= rowFin; fila++)
                {
                    for (int col = colIni; col <= colFin; col++)
                    {
                        if (EsCeldaEditable(fila, col))
                        {
                            var celda = dgvCronograma.Rows[fila].Cells[col];
                            if (!infoCeldas.ContainsKey(fila + "_" + col))
                                celda.Style.BackColor = Color.White;
                            celdasSeleccionadas.Add(celda);
                        }
                    }
                }
            }
        }

        private void dgvCronograma_MouseUp(object sender, MouseEventArgs e)
        {
            if (celdasSeleccionadas.Count == 0) return;

            if (datosArrastre == null)
            {
                FrmInfoCelda frm = new FrmInfoCelda();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    datosArrastre = new Infoceldas
                    {
                        Ficha = frm.Ficha,
                        Programa = frm.Programa,
                        Competencia = frm.Competencia,
                    };
                }
                else
                {
                    seleccionando = false;
                    celdasSeleccionadas.Clear();
                    return;
                }
            }

            foreach (var celda in celdasSeleccionadas)
            {
                int fila = celda.RowIndex;
                int col = celda.ColumnIndex;
                string celdaKey = $"{fila}_{col}";

                Color colorAsignado = ObtenerColorPorFichaCompetencia(datosArrastre.Ficha, datosArrastre.Competencia);
                celda.Style.BackColor = colorAsignado;
                celda.Value = "X";
                celda.ToolTipText = $"Ficha: {datosArrastre.Ficha}\n" +
                                   $"Programa: {datosArrastre.Programa}\n" +
                                   $"Competencia: {datosArrastre.Competencia}";

                infoCeldas[celdaKey] = datosArrastre;

                int diaMes = CalcularDiaIndex(col);
                SaveCellToDb(diaMes, fila, datosArrastre);
            }
            seleccionando = false;
            celdasSeleccionadas.Clear();
        }

        private void SaveCellToDb(int diaMes, int horaIndex, Infoceldas info)
        {
            string idInstructorReal = ObtenerIdInstructorReal();
            EnsureInstructorExists(idInstructorReal);

            const string sql = @"
               INSERT INTO CronogramaDetalle
               (IdInstructor, IdLugar, Anio, Mes, DiaMes, HoraInicio, DuracionBloque,
                IdFicha, IdPrograma, IdCompetencia)
               VALUES
               (@instr, @lugar, @anio, @mes, @dia, @hora, @dur,
                @ficha, @prog, @comp);";

            using (var cmd = new SqlCommand(sql, ConDB))
            {
                cmd.Parameters.AddWithValue("@instr", idInstructorReal);
                cmd.Parameters.AddWithValue("@lugar", Municipio.SelectedValue ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@anio", dateTimePicker1.Value.Year);
                cmd.Parameters.AddWithValue("@mes", comboBox3.SelectedIndex + 1);
                cmd.Parameters.AddWithValue("@dia", diaMes);

                TimeSpan hora = TimeSpan.FromHours(6 + horaIndex);
                cmd.Parameters.AddWithValue("@hora", hora);

                int dur = chk1Hora.Checked ? 1 : chk2Horas.Checked ? 2 : 4;
                cmd.Parameters.AddWithValue("@dur", dur);

                cmd.Parameters.AddWithValue("@ficha", info.Ficha ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@prog", info.Programa ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@comp", info.Competencia ?? (object)DBNull.Value);

                if (ConDB.State != ConnectionState.Open) ConDB.Open();
                cmd.ExecuteNonQuery();
            }
            CalcularHorasDesdeGrid();
        }

        private void DeleteCellFromDb(int diaMes, int horaIndex)
        {
            string idInstructorReal = ObtenerIdInstructorReal();

            using (var cmd = new SqlCommand(@"
               DELETE FROM CronogramaDetalle
               WHERE IdInstructor=@instr
                 AND Anio   =@anio
                 AND Mes    =@mes
                 AND DiaMes =@dia
                 AND HoraInicio=@hidx", ConDB))
            {
                cmd.Parameters.AddWithValue("@instr", idInstructorReal);
                cmd.Parameters.AddWithValue("@anio", dateTimePicker1.Value.Year);
                cmd.Parameters.AddWithValue("@mes", comboBox3.SelectedIndex + 1);
                TimeSpan hora = TimeSpan.FromHours(6 + horaIndex);
                cmd.Parameters.AddWithValue("@hidx", hora);

                if (ConDB.State != ConnectionState.Open) ConDB.Open();
                cmd.ExecuteNonQuery();
            }
            CalcularHorasDesdeGrid();
        }

        private void LoadCronogramaFromDb()
        {
            infoCeldas.Clear();
            ResetGridAppearance();

            string idInstructorReal = ObtenerIdInstructorReal();
            DateTime fechaBase = new DateTime(dateTimePicker1.Value.Year, comboBox3.SelectedIndex + 1, 1);
            int offset = ((int)fechaBase.DayOfWeek + 6) % 7;
            DateTime primerDia = fechaBase.AddDays(-offset);

            using (var cmd = new SqlCommand(@"
                SELECT DiaMes, HoraInicio, DuracionBloque, IdFicha, IdPrograma, IdCompetencia, IdLugar
                FROM CronogramaDetalle
                WHERE IdInstructor = @instr
                  AND Anio = @anio
                  AND Mes  = @mes
                ORDER BY DiaMes, HoraInicio, IdFicha, IdCompetencia", ConDB))
            {
                cmd.Parameters.AddWithValue("@instr", idInstructorReal);
                cmd.Parameters.AddWithValue("@anio", dateTimePicker1.Value.Year);
                cmd.Parameters.AddWithValue("@mes", comboBox3.SelectedIndex + 1);

                if (ConDB.State != ConnectionState.Open) ConDB.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        int dia = rdr.GetByte(0);
                        TimeSpan h0 = rdr.GetTimeSpan(1);
                        int dur = rdr.GetByte(2);

                        var info = new Infoceldas
                        {
                            Ficha = rdr.IsDBNull(3) ? "" : rdr.GetString(3),
                            Programa = rdr.IsDBNull(4) ? "" : rdr.GetString(4),
                            Competencia = rdr.IsDBNull(5) ? "" : rdr.GetString(5)
                        };

                        DateTime fechaObjetivo = new DateTime(dateTimePicker1.Value.Year, comboBox3.SelectedIndex + 1, dia);
                        int colIndex = (fechaObjetivo - primerDia).Days + 1;

                        if (colIndex >= 1 && colIndex < dgvCronograma.Columns.Count)
                        {
                            int fila = h0.Hours - 6;
                            if (fila >= 0 && fila < dgvCronograma.Rows.Count)
                            {
                                var celda = dgvCronograma.Rows[fila].Cells[colIndex];
                                celda.Value = "X";

                                // Color único por ficha + competencia
                                Color colorAsignado = ObtenerColorPorFichaCompetencia(info.Ficha, info.Competencia);
                                celda.Style.BackColor = colorAsignado;

                                celda.ToolTipText = $"Ficha: {info.Ficha}\n" +
                                                   $"Programa: {info.Programa}\n" +
                                                   $"Competencia: {info.Competencia}";

                                infoCeldas[$"{fila}_{colIndex}"] = info;
                            }
                        }
                    }
                }
            }

            // Reaplicar fuera de mes
            for (int col = 1; col < dgvCronograma.Columns.Count; col++)
            {
                DateTime fechaCol = primerDia.AddDays(col - 1);
                if (fechaCol.Month != comboBox3.SelectedIndex + 1)
                {
                    for (int fila = 0; fila < dgvCronograma.Rows.Count; fila++)
                    {
                        var celda = dgvCronograma.Rows[fila].Cells[col];
                        string celdaKey = $"{fila}_{col}";
                        if (!infoCeldas.ContainsKey(celdaKey) && celda.Value?.ToString() != "X")
                        {
                            celda.Style.BackColor = Color.LightGray;
                            celda.ReadOnly = true;
                        }
                    }
                }
            }

            // Fines de semana
            AplicarColorSabadoODomingo(DayOfWeek.Saturday, checkBox1.Checked);
            AplicarColorSabadoODomingo(DayOfWeek.Sunday, checkBox2.Checked);

            // 🔧 Sembrar el mapa según el orden visual (izq→der, arriba→abajo)
            RebuildColorMapFromGrid();

            CalcularHorasDesdeGrid();
        }

        // Siembra/normaliza colores según el orden visual del grid
        private void RebuildColorMapFromGrid()
        {
            var seen = new HashSet<string>();
            int idx = coloresAsignados.Count; // si ya hay colores previos, continúa

            for (int col = 1; col < dgvCronograma.Columns.Count; col++)
            {
                for (int row = 0; row < dgvCronograma.Rows.Count; row++)
                {
                    string keyCell = $"{row}_{col}";
                    if (!infoCeldas.TryGetValue(keyCell, out var info)) continue;

                    string k = MakeKey(info.Ficha, info.Competencia);
                    if (!seen.Contains(k) && !coloresAsignados.ContainsKey(k))
                    {
                        coloresAsignados[k] = paletaColores[idx % paletaColores.Count];
                        idx++;
                        seen.Add(k);
                    }
                }
            }
        }

        private int CalcularDiaIndex(int colIndex)
        {
            DateTime fechaBase = new DateTime(dateTimePicker1.Value.Year, comboBox3.SelectedIndex + 1, 1);
            int offset = ((int)fechaBase.DayOfWeek + 6) % 7;
            DateTime primerDia = fechaBase.AddDays(-offset);

            DateTime fechaColumna = primerDia.AddDays(colIndex - 1);
            if (fechaColumna.Month == comboBox3.SelectedIndex + 1) return fechaColumna.Day;

            return -1;
        }

        private int CalcularDiaParaColumna(int colIndex)
        {
            DateTime fechaBase = new DateTime(dateTimePicker1.Value.Year, comboBox3.SelectedIndex + 1, 1);
            DateTime primerDia = fechaBase.AddDays(-(((int)fechaBase.DayOfWeek + 6) % 7));
            int diaIndex = -1;
            for (int col = 1; col < dgvCronograma.Columns.Count; col++)
            {
                var partes = dgvCronograma.Columns[col].HeaderText.Split('\n');
                if (partes.Length < 2 || !int.TryParse(partes[1], out int dia)) continue;

                DateTime f = primerDia.AddDays(col - 1);
                if (f.Month == comboBox3.SelectedIndex + 1)
                {
                    if (col == colIndex) return dia;
                }
            }
            return diaIndex;
        }

        private void dgvCronograma_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete) return;

            foreach (DataGridViewCell celda in dgvCronograma.SelectedCells)
            {
                if (!EsCeldaEditable(celda.RowIndex, celda.ColumnIndex)) continue;

                int diaMes = CalcularDiaParaColumna(celda.ColumnIndex);
                int horaIndex = celda.RowIndex;

                celda.Value = "";
                celda.Style.BackColor = Color.White;

                DeleteCellFromDb(diaMes, horaIndex);
            }
            e.Handled = true;
        }

        private void btnLimpiarCursor_Click(object sender, EventArgs e)
        {
            datosArrastre = null;
            MessageBox.Show("Cursor de datos limpiado. Puedes ingresar nuevos datos.", "Limpieza exitosa",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Departamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Departamento.SelectedValue == null) return;

            var dt = new DataTable();
            using (var cmd = new SqlCommand(
                "SELECT IdLugar, Nombre FROM Municipios WHERE IdDpto = @dpto ORDER BY Nombre", ConDB))
            {
                cmd.Parameters.AddWithValue("@dpto", Departamento.SelectedValue);
                using (var da = new SqlDataAdapter(cmd)) da.Fill(dt);
            }

            Municipio.DisplayMember = "Nombre";
            Municipio.ValueMember = "IdLugar";
            Municipio.DataSource = dt;
        }

        private void IdPersona_TextChanged(object sender, EventArgs e)
        {
            bool tieneTexto = !string.IsNullOrWhiteSpace(IdPersona.Text);
            checkBox1.Enabled = tieneTexto;
            checkBox2.Enabled = tieneTexto;
            if (!tieneTexto) { checkBox1.Checked = false; checkBox2.Checked = false; }
        }

        private void IdPersona_Leave(object sender, EventArgs e)
        {
            string textoIngresado = IdPersona.Text.Trim();
            if (string.IsNullOrEmpty(textoIngresado))
            {
                txtInstructorDisplay.Text = "";
                return;
            }

            if (instructoresMap.ContainsKey(textoIngresado))
            {
                string idPersonaReal = instructoresMap[textoIngresado];
                IdPersona.Tag = idPersonaReal;
                CargarNombreApellido(idPersonaReal);
                return;
            }

            var coincidencias = instructoresMap.Keys
                .Where(k => k.IndexOf(textoIngresado, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            if (coincidencias.Count == 1)
            {
                IdPersona.Text = coincidencias[0];
                IdPersona.Tag = instructoresMap[coincidencias[0]];
                CargarNombreApellido(instructoresMap[coincidencias[0]]);
            }
            else if (coincidencias.Count > 1)
            {
                MostrarDialogoSeleccion(coincidencias, textoIngresado);
            }
            else
            {
                var r = MessageBox.Show(
                    $"No se encontró un instructor con el texto '{textoIngresado}'.\n¿Desea crear un nuevo instructor?",
                    "Instructor no encontrado", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (r == DialogResult.Yes)
                {
                    IdPersona.Text = ""; IdPersona.Tag = null; txtInstructorDisplay.Text = "";
                }
            }
        }

        private void MostrarDialogoSeleccion(List<string> coincidencias, string textoBuscado)
        {
            using (var form = new Form())
            {
                form.Text = "Seleccionar Instructor";
                form.Size = new Size(500, 300);
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MaximizeBox = false;
                form.MinimizeBox = false;

                var label = new Label()
                {
                    Text = $"Se encontraron múltiples coincidencias para '{textoBuscado}':",
                    Location = new Point(10, 10),
                    Size = new Size(460, 20)
                };

                var listBox = new ListBox()
                {
                    Location = new Point(10, 35),
                    Size = new Size(460, 180),
                    DataSource = coincidencias
                };

                var btnOK = new System.Windows.Forms.Button()
                {
                    Text = "Seleccionar",
                    Location = new Point(315, 225),
                    Size = new Size(80, 30),
                    DialogResult = DialogResult.OK
                };

                var btnCancel = new System.Windows.Forms.Button()
                {
                    Text = "Cancelar",
                    Location = new Point(400, 225),
                    Size = new Size(80, 30),
                    DialogResult = DialogResult.Cancel
                };

                form.Controls.AddRange(new Control[] { label, listBox, btnOK, btnCancel });
                form.AcceptButton = btnOK;
                form.CancelButton = btnCancel;

                if (form.ShowDialog() == DialogResult.OK && listBox.SelectedItem != null)
                {
                    string seleccionado = listBox.SelectedItem.ToString();
                    IdPersona.Text = seleccionado;
                    IdPersona.Tag = instructoresMap[seleccionado];
                }
            }
        }

        private string ObtenerIdInstructorReal()
        {
            if (IdPersona.Tag != null) return IdPersona.Tag.ToString();

            string textoActual = IdPersona.Text.Trim();
            if (instructoresMap.ContainsKey(textoActual)) return instructoresMap[textoActual];

            return textoActual;
        }

        private void ResetGridAppearance()
        {
            foreach (DataGridViewRow row in dgvCronograma.Rows)
            {
                for (int c = 1; c < dgvCronograma.Columns.Count; c++)
                {
                    var celda = row.Cells[c];
                    celda.Style.BackColor = Color.White;
                    celda.Value = null;
                    celda.ReadOnly = false;
                    celda.ToolTipText = "";
                }
            }
            infoCeldas.Clear();
        }

        public void RefrescarListaInstructores() => CargarInstructoresParaAutocompletado();

        private void CargarNombreApellido(string idPersona)
        {
            try
            {
                const string sql = @"SELECT Nombre, Apellido FROM Personas WHERE IdPersona = @id";
                using (var cmd = new SqlCommand(sql, ConDB))
                {
                    cmd.Parameters.AddWithValue("@id", idPersona);
                    if (ConDB.State != ConnectionState.Open) ConDB.Open();

                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            string nombres = rdr["Nombre"].ToString();
                            string apellidos = rdr["Apellido"].ToString();
                            txtInstructorDisplay.Text = $"{nombres} {apellidos}";
                        }
                        else
                        {
                            txtInstructorDisplay.Text = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar nombre y apellido: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Color ObtenerColorPorFichaCompetencia(string ficha, string competencia)
        {
            string clave = MakeKey(ficha, competencia);

            if (!coloresAsignados.ContainsKey(clave))
            {
                int index = coloresAsignados.Count % paletaColores.Count;
                coloresAsignados[clave] = paletaColores[index];
            }
            return coloresAsignados[clave];
        }

        private void CalcularHorasDesdeGrid()
        {
            int totalHoras = 0;
            foreach (DataGridViewRow fila in dgvCronograma.Rows)
            {
                foreach (DataGridViewCell celda in fila.Cells)
                    if (celda.Value != null && celda.Value.ToString() == "X") totalHoras++;
            }
            HorasPRO.Text = $"Horas trabajadas: {totalHoras}";
        }

        private void btnEnviarCorreo_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Enviando correo, Por favor Espere..!");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            try
            {
                btnEnviarCorreo.Enabled = false;

                string rutaAdjunto = ExportarCronogramaAExcel();
                if (string.IsNullOrWhiteSpace(rutaAdjunto) || !File.Exists(rutaAdjunto))
                    throw new Exception("No se pudo generar el archivo de cronograma para adjuntar.");

                string from = null, appPass = null, fromName = null;
                using (var cmd = new SqlCommand("SELECT TOP 1 Correo, PinCor, NombreE FROM Empresa", ConDB))
                {
                    if (ConDB.State != System.Data.ConnectionState.Open) ConDB.Open();
                    using (var rd = cmd.ExecuteReader())
                    {
                        if (!rd.Read()) throw new Exception("Falta registro en Empresa (Correo/PinCor).");
                        from = rd["Correo"] as string;
                        appPass = (rd["PinCor"] as string)?.Replace(" ", "");
                        fromName = rd["NombreE"] as string;
                    }
                }
                if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(appPass))
                    throw new Exception("Empresa.Correo o Empresa.PinCor están vacíos.");

                string idPersona = ObtenerIdInstructorReal();
                string to = ObtenerCorreoInstructorActual(idPersona);
                if (string.IsNullOrWhiteSpace(to))
                    throw new Exception("El instructor no tiene correo en Personas.Correo.");

                int anio = dateTimePicker1.Value.Year;
                string mesTxt = comboBox3.SelectedItem.ToString();
                string nombreMostrar = string.IsNullOrWhiteSpace(txtInstructorDisplay.Text)
                    ? idPersona : txtInstructorDisplay.Text;

                string asunto = $"Cronograma {mesTxt} {anio} - {nombreMostrar}";
                string cuerpoHtml =
                    "<p>Hola " + nombreMostrar + ",</p>" +
                    "<p>Adjunto tu cronograma de <b>" + mesTxt + " " + anio + "</b>.</p>" +
                    "<p>Saludos,<br/>" + (string.IsNullOrWhiteSpace(fromName) ? from : fromName) + "</p>";

                using (var msg = new MailMessage())
                {
                    msg.From = new MailAddress(from, string.IsNullOrWhiteSpace(fromName) ? from : fromName);
                    msg.To.Add(to);
                    msg.Subject = asunto;
                    msg.Body = cuerpoHtml;
                    msg.IsBodyHtml = true;
                    msg.Attachments.Add(new Attachment(rutaAdjunto));

                    using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(from, appPass);
                        smtp.Send(msg);
                    }
                }

                MessageBox.Show("Correo enviado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar correo:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnEnviarCorreo.Enabled = true;
            }
        }

        // ==== Helpers de exportación ====
        private static string SafeFilePart(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";
            var invalid = Path.GetInvalidFileNameChars();
            var sb = new StringBuilder(s.Length);
            foreach (var ch in s) if (!invalid.Contains(ch)) sb.Append(ch);
            var cleaned = Regex.Replace(sb.ToString().Trim(), @"\s+", "_");
            return cleaned;
        }

        private string ExportarCronogramaAExcel()
        {
            string mes = comboBox3.SelectedItem.ToString();
            int anio = dateTimePicker1.Value.Year;
            int mesNum = comboBox3.SelectedIndex + 1;

            string nombreInstructor = string.IsNullOrWhiteSpace(txtInstructorDisplay.Text)
                ? ObtenerIdInstructorReal()
                : txtInstructorDisplay.Text;

            string fileName = $"Cronograma_{SafeFilePart(mes)}_{anio}_{SafeFilePart(nombreInstructor)}.xlsx";
            string outputPath = Path.Combine(Path.GetTempPath(), fileName);

            var excel = new BibliotecaExcel2.Class1();
            return excel.GenerarExelAPath(
                dgvCronograma,
                nombreInstructor,
                "1 HORA",
                mes,
                ObtenerIdInstructorReal(),
                mesNum,
                anio,
                outputPath,
                GetColorMap()               // <<<<<<<<<<<<<<<<<< AÑADIR
            );

        }

        private string ObtenerCorreoInstructorActual(string idPersona)
        {
            const string sql = "SELECT TOP 1 Correo FROM Personas WHERE IdPersona = @id";
            using (var cmd = new SqlCommand(sql, ConDB))
            {
                cmd.Parameters.AddWithValue("@id", idPersona);
                if (ConDB.State != System.Data.ConnectionState.Open) ConDB.Open();
                object r = cmd.ExecuteScalar();
                return r == null ? null : r.ToString();
            }
        }

        private void FrmCronograma_FormClosing(object sender, FormClosingEventArgs e)
        {
            Conexion.ConDB.Close();
        }
    }
}
