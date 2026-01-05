using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Linq; // necesario para AsEnumerable

namespace ProgramacionInstructores
{
    public partial class FrmCompetenciasIncremental : Form
    {
        public FrmCompetenciasIncremental()
        {
            this.TopLevel = true;
            InitializeComponent();
        }

        Conexion Conn = new Conexion();                 // Tu clase de conexión
        Complementos OnOff = new Complementos();        // Tu helper de UI
        OpenFileDialog BuscarArchivo = new OpenFileDialog();

        string Delimitador = ";";
        string Ayuda = string.Empty;

        // ==== Autocompletado Programas ====
        private DataTable dtProgramas;         // cache de Programas (IdPrograma, Nombre)
        private ListBox lstSugProgramas;       // lista emergente
        private ToolTip ttPrograma = new ToolTip();

        // ----------------------------------------------------------------------
        // Helpers
        private void EnsureOpen()
        {
            if (Conexion.ConDB.State != ConnectionState.Open)
                Conexion.ConDB.Open();
        }
        // ----------------------------------------------------------------------

        private void FrmCompetenciasIncremental_Load(object sender, EventArgs e)
        {
            // textBox1 = Base de datos, textBox2 = Tabla fija "Competencias",
            // textBox3 = ruta del CSV, TxtBoxPrograma = IdPrograma a relacionar
            textBox1.Text = Conexion.VarGlobal.xNomDB;
            textBox2.Text = "Competencias";

            string carpetaImport = Path.Combine(Application.StartupPath, "DatosImport");
            if (!Directory.Exists(carpetaImport)) Directory.CreateDirectory(carpetaImport);
            textBox3.Text = carpetaImport + "\\";

            // Limpia tabla TMP (si existe)
            try
            {
                using (var cn = new SqlConnection(Conexion.ConDB.ConnectionString))
                {
                    cn.Open();
                    using (SqlCommand cmdDel = new SqlCommand(
                        "IF OBJECT_ID('dbo.CompetenciasTMP') IS NOT NULL DELETE FROM dbo.CompetenciasTMP;", cn))
                    {
                        cmdDel.ExecuteNonQuery();
                    }
                }
            }
            catch { /* ignora si no existe */ }

            // Cargar grid desde TMP (si no existe, devuelve dummy)
            Conn.ConsultaDatos(
                "IF OBJECT_ID('dbo.CompetenciasTMP') IS NULL SELECT 1 AS Dummy ELSE SELECT * FROM dbo.CompetenciasTMP ORDER BY IdCompetencia;",
                "CompetenciasTMP"
            );
            dataGridView1.DataSource = Conn.Ds.Tables["CompetenciasTMP"];
            OrganizaGrid(dataGridView1);

            TotDat.Text = "Total Registros a Importar: " + (Conn.Ds.Tables["CompetenciasTMP"]?.Rows.Count ?? 0);

            // Ayuda
            Ayuda =
                "INSTRUCCIONES PARA LA IMPORTACIÓN (CSV):\n" +
                "• IdCompetencia = Código/número de la competencia (máx 50)\n" +
                "• Nombre        = Nombre de la competencia (máx 3000)\n" +
                "• Duracion      = Duración en horas (entero)\n\n" +
                "Primera fila = encabezados. Guarda el CSV con el delimitador seleccionado. Codificación UTF-8.\n\n" +
                "IMPORTANTE: antes de ACTUALIZAR escriba el IdPrograma en el campo correspondiente.";
            MessageBox.Show(Ayuda, "Instrucciones para la Importación de Datos");

            // ==== Autocompletado / previsualización de Programas ====
            CargarProgramasCache();

            lstSugProgramas = new ListBox
            {
                Visible = false,
                IntegralHeight = true,
                Height = 120,
                Width = TxtBoxPrograma.Width + 220,
                Font = TxtBoxPrograma.Font,
                TabStop = false,
                CausesValidation = false // evita disparar Validated del textbox
            };

            // Agregar al mismo contenedor del TextBox y traer al frente
            TxtBoxPrograma.Parent.Controls.Add(lstSugProgramas);
            lstSugProgramas.BringToFront();

            PositionarListaSugerencias();

            // Eventos para la lista
            lstSugProgramas.KeyDown += LstSugProgramas_KeyDown;
            lstSugProgramas.MouseDown += LstSugProgramas_MouseDown; // selección por clic temprano (MouseDown)

            // Eventos para el TextBox  
            TxtBoxPrograma.TextChanged += TxtBoxPrograma_TextChanged;
            TxtBoxPrograma.KeyDown += TxtBoxPrograma_KeyDown;
            TxtBoxPrograma.LostFocus += TxtBoxPrograma_LostFocus;
            TxtBoxPrograma.Validated += TxtBoxPrograma_Validated;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            PositionarListaSugerencias();
        }

        private void PositionarListaSugerencias()
        {
            if (lstSugProgramas == null || TxtBoxPrograma == null) return;
            var pt = this.PointToClient(TxtBoxPrograma.Parent.PointToScreen(TxtBoxPrograma.Location));
            lstSugProgramas.Left = pt.X;
            lstSugProgramas.Top = pt.Y + TxtBoxPrograma.Height + 2;
            lstSugProgramas.Width = TxtBoxPrograma.Width + 220; // acompaña el tamaño del textbox
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked) Delimitador = ";";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked) Delimitador = ",";
        }

        // --- Botón: Cargar Datos (CSV → CompetenciasTMP) ---
        private void BtoBusArc_Click(object sender, EventArgs e)
        {
            try
            {
                BuscarArchivo.FileName = "";
                BuscarArchivo.InitialDirectory = Directory.Exists(textBox3.Text) ? textBox3.Text : Application.StartupPath;
                BuscarArchivo.Title = "Buscar archivo CSV para importar Competencias";
                BuscarArchivo.Filter = "Archivos CSV (*.csv)|*.csv|Todos los archivos (*.*)|*.*";

                if (BuscarArchivo.ShowDialog() != DialogResult.OK) return;

                textBox3.Text = BuscarArchivo.FileName;
                char delimChar = Delimitador[0];

                ImportarConSqlBulkCopy_UTF8(textBox3.Text, delimChar);

                Conn.ConsultaDatos("SELECT * FROM dbo.CompetenciasTMP ORDER BY IdCompetencia;", "CompetenciasTMP");
                dataGridView1.DataSource = Conn.Ds.Tables["CompetenciasTMP"];
                OrganizaGrid(dataGridView1);
                TotDat.Text = "Total Registros a Importar: " + Conn.Ds.Tables["CompetenciasTMP"].Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al importar los datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- Botón: Actualizar (MERGE CompetenciasTMP → Competencias + ProgramasCompetencias) ---
        private void BtoUpDate_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Ayuda, "Recordatorio de Formato CSV");

            // Validaciones básicas de archivo / TMP
            if (string.IsNullOrWhiteSpace(textBox3.Text) || !File.Exists(textBox3.Text))
            {
                MessageBox.Show("Sin archivo válido para importar.", "Actualizando...!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Conn.Ds.Tables["CompetenciasTMP"] == null || Conn.Ds.Tables["CompetenciasTMP"].Rows.Count == 0)
            {
                MessageBox.Show("No hay datos en CompetenciasTMP para actualizar.", "Actualizando...!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validación dura del IdPrograma (aquí sí bloquea si no existe)
            var idPrograma = TxtBoxPrograma.Text.Trim();
            if (string.IsNullOrWhiteSpace(idPrograma))
            {
                MessageBox.Show("Digite el IdPrograma en el campo correspondiente.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TxtBoxPrograma.Focus();
                return;
            }
            try
            {
                EnsureOpen();
                using (var chk = new SqlCommand("SELECT 1 FROM dbo.Programas WHERE IdPrograma=@p", Conexion.ConDB))
                {
                    chk.Parameters.AddWithValue("@p", idPrograma);
                    var ok = chk.ExecuteScalar();
                    if (ok == null)
                    {
                        MessageBox.Show("El IdPrograma no existe en la tabla Programas.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtBoxPrograma.Focus();
                        return;
                    }
                }
            }
            catch (Exception exVal)
            {
                MessageBox.Show("Error validando IdPrograma: " + exVal.Message, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                if (Conexion.ConDB.State == ConnectionState.Open) Conexion.ConDB.Close();
            }

            // MERGE al catálogo de Competencias
            string xMerge = @"
MERGE dbo.Competencias AS destino
USING (
    SELECT
        RTRIM(LTRIM(ISNULL(IdCompetencia,'')))   AS IdCompetencia,
        RTRIM(LTRIM(ISNULL(Nombre,'')))          AS Nombre,
        CAST(NULLIF(RTRIM(LTRIM(ISNULL(CAST(Duracion AS NVARCHAR(50)),''))), '') AS INT) AS Duracion
    FROM dbo.CompetenciasTMP
) AS src
ON (destino.IdCompetencia = src.IdCompetencia)
WHEN MATCHED THEN
    UPDATE SET
        destino.Nombre   = src.Nombre,
        destino.Duracion = src.Duracion
WHEN NOT MATCHED BY TARGET THEN
    INSERT (IdCompetencia, Nombre, Duracion)
    VALUES (src.IdCompetencia, src.Nombre, src.Duracion)
OUTPUT $action AS Accion;";

            try
            {
                EnsureOpen();

                int ins = 0, upd = 0;
                using (SqlCommand cmd = new SqlCommand(xMerge, Conexion.ConDB))
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        string accion = rd.GetString(0);
                        if (accion.Equals("INSERT", StringComparison.OrdinalIgnoreCase)) ins++;
                        else if (accion.Equals("UPDATE", StringComparison.OrdinalIgnoreCase)) upd++;
                    }
                }

                // MERGE ProgramasCompetencias: relaciona TODO lo cargado en TMP con el IdPrograma
                string mergePC = @"
MERGE dbo.ProgramasCompetencias AS dst
USING (
    SELECT
        @IdPrograma                              AS IdPrograma,
        RTRIM(LTRIM(ISNULL(t.IdCompetencia,''))) AS IdCompetencia,
        CAST(NULLIF(RTRIM(LTRIM(ISNULL(CAST(t.Duracion AS NVARCHAR(50)),''))), '') AS INT) AS DuracionPlan
    FROM dbo.CompetenciasTMP t
) AS src
ON (dst.IdPrograma = src.IdPrograma AND dst.IdCompetencia = src.IdCompetencia)
WHEN MATCHED THEN
    UPDATE SET dst.DuracionPlan = src.DuracionPlan
WHEN NOT MATCHED BY TARGET THEN
    INSERT (IdPrograma, IdCompetencia, DuracionPlan)
    VALUES (src.IdPrograma, src.IdCompetencia, src.DuracionPlan)
OUTPUT $action AS Accion;";

                int insPC = 0, updPC = 0;
                using (var cmdPC = new SqlCommand(mergePC, Conexion.ConDB))
                {
                    cmdPC.Parameters.AddWithValue("@IdPrograma", idPrograma);
                    using (var rdPC = cmdPC.ExecuteReader())
                    {
                        while (rdPC.Read())
                        {
                            var acc = rdPC.GetString(0);
                            if (acc.Equals("INSERT", StringComparison.OrdinalIgnoreCase)) insPC++;
                            else if (acc.Equals("UPDATE", StringComparison.OrdinalIgnoreCase)) updPC++;
                        }
                    }
                }

                // Refrescos UI
                Conn.ConsultaDatos("SELECT * FROM dbo.Competencias ORDER BY IdCompetencia;", "Competencias");
                dataGridView1.DataSource = Conn.Ds.Tables["Competencias"];
                OrganizaGrid(dataGridView1);
                TotReg.Text = "Total Registros Competencias: " + Conn.Ds.Tables["Competencias"].Rows.Count.ToString();

                Conn.ConsultaDatos(@"
SELECT pc.IdPrograma, pc.IdCompetencia, c.Nombre, pc.DuracionPlan
FROM dbo.ProgramasCompetencias pc
JOIN dbo.Competencias c ON c.IdCompetencia = pc.IdCompetencia
WHERE pc.IdPrograma = '" + idPrograma.Replace("'", "''") + @"'
ORDER BY pc.IdCompetencia;", "PC");
                // Si quieres ver el resultado en otro grid, asígnalo aquí:
                // dataGridView2.DataSource = Conn.Ds.Tables["PC"];

                MessageBox.Show(
                    $"Actualización completada.\n" +
                    $"Competencias → Insertados: {ins} | Actualizados: {upd}\n" +
                    $"Relaciones (Programa = {idPrograma}) → Insertadas: {insPC} | Actualizadas: {updPC}",
                    "Actualizando Datos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                BuscarArchivo.FileName = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al Actualizar los datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (Conexion.ConDB.State == ConnectionState.Open) Conexion.ConDB.Close();
            }
        }

        private void BtoCanSal_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        // ===================== UTILIDADES GRID ==========================
        private void OrganizaGrid(DataGridView grid)
        {
            if (grid == null || grid.Columns.Count == 0) return;

            foreach (DataGridViewColumn col in grid.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            AjustarCol(grid, "IdCompetencia", 100, true);
            AjustarCol(grid, "Nombre", 270, true);
            AjustarCol(grid, "Duracion", 90, true);

            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToOrderColumns = false;
            grid.ReadOnly = true;
            grid.MultiSelect = false;
            grid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            grid.RowsDefaultCellStyle.BackColor = Color.LightBlue;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
        }

        private void AjustarCol(DataGridView grid, string nombre, int width, bool visible)
        {
            if (!grid.Columns.Contains(nombre)) return;
            var c = grid.Columns[nombre];
            c.Visible = visible;
            c.Width = width;
        }

        // ======= IMPORTACIÓN UTF-8 CON SqlBulkCopy (tipos seguros) =======
        private void ImportarConSqlBulkCopy_UTF8(string rutaCsv, char delim)
        {
            using (var cn = new SqlConnection(Conexion.ConDB.ConnectionString))
            {
                cn.Open();

                // Limpia TMP
                using (var cmdDel = new SqlCommand("DELETE FROM dbo.CompetenciasTMP;", cn))
                    cmdDel.ExecuteNonQuery();

                // Obtiene esquema real de TMP
                var dt = new DataTable();
                using (var da = new SqlDataAdapter("SELECT TOP 0 * FROM dbo.CompetenciasTMP", cn))
                    da.FillSchema(dt, SchemaType.Source);

                // Leer encabezados
                string[] headers;
                using (var srHeader = new StreamReader(rutaCsv, Encoding.UTF8, true))
                {
                    var headerLine = srHeader.ReadLine();
                    if (string.IsNullOrWhiteSpace(headerLine))
                        throw new InvalidOperationException("El archivo no tiene encabezado.");

                    headers = headerLine.Split(new[] { delim }, StringSplitOptions.None);
                    for (int i = 0; i < headers.Length; i++)
                        headers[i] = headers[i].Trim();
                }

                var colIndex = new System.Collections.Generic.Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < headers.Length; i++) colIndex[headers[i]] = i;

                // Leer filas
                using (var sr = new StreamReader(rutaCsv, Encoding.UTF8, true))
                {
                    sr.ReadLine(); // salta encabezado

                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        var parts = line.Split(new[] { delim }, StringSplitOptions.None);
                        var row = dt.NewRow();

                        // IdCompetencia (nchar)
                        row["IdCompetencia"] = GetString(parts, colIndex, "IdCompetencia");

                        // Nombre (nchar)
                        row["Nombre"] = GetString(parts, colIndex, "Nombre");

                        // Duracion (int)
                        string rawDur = GetString(parts, colIndex, "Duracion");
                        if (int.TryParse(rawDur, out int dur))
                            row["Duracion"] = dur;
                        else
                            row["Duracion"] = DBNull.Value;

                        dt.Rows.Add(row);
                    }
                }

                // BulkCopy → TMP
                using (var bulk = new SqlBulkCopy(cn))
                {
                    bulk.DestinationTableName = "dbo.CompetenciasTMP";
                    foreach (DataColumn c in dt.Columns)
                        bulk.ColumnMappings.Add(c.ColumnName, c.ColumnName);
                    bulk.WriteToServer(dt);
                }
            }
        }

        private string GetString(string[] parts, System.Collections.Generic.Dictionary<string, int> colIndex, string col)
        {
            if (!colIndex.TryGetValue(col, out int idx) || idx >= parts.Length) return null;
            var raw = parts[idx] ?? string.Empty;
            raw = raw.Trim();
            return raw.Length == 0 ? null : raw;
        }

        private void BtoIA_Click(object sender, EventArgs e)
        {
            try
            {
                var r = MessageBox.Show(
                    "Serás redirigido a ChatGPT.\n\n" +
                    "👉 Allí solo debes subir:\n" +
                    "  1) El PDF del diseño curricular\n" +
                    "  2) La plantilla 'CompetenciasPlantilla.csv'\n\n" +
                    "Se abrirá también una carpeta con la plantilla lista para arrastrar al chat.",
                    "Enviar a IA", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (r != DialogResult.OK) return;

                // 1) Asegurar plantilla en el output (DatosImport/CompetenciasPlantilla.csv)
                string plantillaOrigen = Path.Combine(Application.StartupPath, "DatosImport", "CompetenciasPlantilla.csv");
                if (!File.Exists(plantillaOrigen))
                {
                    MessageBox.Show(
                        "No se encontró 'DatosImport/CompetenciasPlantilla.csv'.\n" +
                        "Inclúyela en el proyecto (Build Action: Content, Copy to Output Directory: Copy always).",
                        "Plantilla no encontrada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2) Carpeta temporal y copia de plantilla
                string tempDir = Path.Combine(Path.GetTempPath(), "SENA_Competencias_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"));
                Directory.CreateDirectory(tempDir);
                string dstCsv = Path.Combine(tempDir, Path.GetFileName(plantillaOrigen));
                File.Copy(plantillaOrigen, dstCsv, true);

                // 3) Abrir ChatGPT con prompt listo
                string prompt =
@"Tengo un diseño curricular en PDF de un programa de formación del SENA y una plantilla CSV de competencias. 
Necesito que analices el documento y extraigas ABSOLUTAMENTE todas y cada una de las competencias del programa, con los siguientes datos: Id competencia, nombre de la competencia, horas.
completa la plantilla CSV de competencias con el mismo formato y estructura que se usó en el archivo CompetenciasPlantilla.csv.

Asegúrate de:

Mantener la codificación en UTF-8 y usar punto y coma (;) como delimitador.

Incluir TODAS Y CADA UNA de las competencias en orden, sin omitir ninguna.

Respetar exactamente los nombres de las columnas de la plantilla.

Guardar el resultado como un nuevo archivo CSV listo para importar en el sistema.";

                string url = "https://chatgpt.com/?q=" + Uri.EscapeDataString(prompt);
                try { Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }); } catch { }

                // 4) Abrir la carpeta temporal con la plantilla
                try { Process.Start("explorer.exe", tempDir); } catch { }

                // 5) (Opcional) portapapeles
                try
                {
                    Clipboard.SetText(prompt);
                    var files = new StringCollection();
                    files.Add(dstCsv);
                    Clipboard.SetFileDropList(files);
                }
                catch { /* si falla no es crítico */ }

                MessageBox.Show(
                    "Listo:\n\n1) Se abrió ChatGPT con el prompt (ya está en el portapapeles).\n" +
                    "2) Se abrió una carpeta con la plantilla CSV lista para arrastrar al chat.\n\n" +
                    "Ahora sube al chat el PDF del diseño curricular y la plantilla.",
                    "Preparado para IA", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error:\n" + ex.Message, "BtoIA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===================== AUTOCOMPLETADO PROGRAMAS (métodos) ==========================
        private void CargarProgramasCache()
        {
            try
            {
                EnsureOpen();
                using (var da = new SqlDataAdapter(
                    "SELECT RTRIM(IdPrograma) AS IdPrograma, RTRIM(Nombre) AS Nombre FROM dbo.Programas ORDER BY IdPrograma",
                    Conexion.ConDB))
                {
                    dtProgramas = new DataTable();
                    da.Fill(dtProgramas);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron cargar los Programas: " + ex.Message, "Programas",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtProgramas = new DataTable();
                dtProgramas.Columns.Add("IdPrograma");
                dtProgramas.Columns.Add("Nombre");
            }
            finally
            {
                if (Conexion.ConDB.State == ConnectionState.Open) Conexion.ConDB.Close();
            }
        }

        private void MostrarSugerencias(string query)
        {
            if (dtProgramas == null || dtProgramas.Rows.Count == 0) { lstSugProgramas.Visible = false; return; }

            query = (query ?? string.Empty).Trim();

            var rows = dtProgramas.AsEnumerable()
                .Where(r =>
                {
                    var id = (r.Field<string>("IdPrograma") ?? string.Empty);
                    var no = (r.Field<string>("Nombre") ?? string.Empty);
                    return id.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0
                        || no.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0;
                })
                .Take(50)
                .Select(r => new
                {
                    Id = r.Field<string>("IdPrograma") ?? "",
                    Nom = r.Field<string>("Nombre") ?? ""
                })
                .ToList();

            if (rows.Count == 0) { lstSugProgramas.Visible = false; return; }

            lstSugProgramas.BeginUpdate();
            lstSugProgramas.Items.Clear();
            foreach (var it in rows)
                lstSugProgramas.Items.Add($"{it.Id}  —  {it.Nom}");
            lstSugProgramas.EndUpdate();

            PositionarListaSugerencias();
            lstSugProgramas.BringToFront();
            lstSugProgramas.Visible = true;
        }

        private void AceptarSugerenciaSeleccionada()
        {
            if (!lstSugProgramas.Visible || lstSugProgramas.SelectedIndex < 0) return;

            string sel = lstSugProgramas.SelectedItem.ToString();
            int sep = sel.IndexOf('—');
            string id = sep > 0 ? sel.Substring(0, sep).Trim() : sel.Trim();

            TxtBoxPrograma.Text = id;
            lstSugProgramas.Visible = false;

            var row = dtProgramas.AsEnumerable()
                .FirstOrDefault(r => (r.Field<string>("IdPrograma") ?? "") == id);
            string nom = row == null ? "" : (row.Field<string>("Nombre") ?? "");
            ttPrograma.SetToolTip(TxtBoxPrograma, $"{id} — {nom}");
        }

        private void TxtBoxPrograma_TextChanged(object sender, EventArgs e)
        {
            var q = TxtBoxPrograma.Text;
            if (string.IsNullOrWhiteSpace(q))
            {
                lstSugProgramas.Visible = false;
                ttPrograma.SetToolTip(TxtBoxPrograma, "");
                return;
            }
            MostrarSugerencias(q);
        }

        private void TxtBoxPrograma_KeyDown(object sender, KeyEventArgs e)
        {
            if (!lstSugProgramas.Visible) return;

            if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                if (lstSugProgramas.Items.Count > 0)
                {
                    if (lstSugProgramas.SelectedIndex < 0) lstSugProgramas.SelectedIndex = 0;
                    else if (lstSugProgramas.SelectedIndex < lstSugProgramas.Items.Count - 1)
                        lstSugProgramas.SelectedIndex++;
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                e.Handled = true;
                if (lstSugProgramas.SelectedIndex > 0) lstSugProgramas.SelectedIndex--;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                AceptarSugerenciaSeleccionada();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                lstSugProgramas.Visible = false;
            }
        }

        private void TxtBoxPrograma_LostFocus(object sender, EventArgs e)
        {
            if (lstSugProgramas == null) return;

            // ¿El mouse está encima de la lista? -> no ocultes (está a punto de hacer clic)
            var rectScreen = lstSugProgramas.RectangleToScreen(lstSugProgramas.ClientRectangle);
            bool mouseSobreLista = rectScreen.Contains(Cursor.Position);

            bool listaConFoco = lstSugProgramas.ContainsFocus;

            if (lstSugProgramas.Visible && (mouseSobreLista || listaConFoco))
                return;

            lstSugProgramas.Visible = false;
        }

        private void LstSugProgramas_MouseDown(object sender, MouseEventArgs e)
        {
            int idx = lstSugProgramas.IndexFromPoint(e.Location);
            if (idx >= 0)
            {
                lstSugProgramas.SelectedIndex = idx;
                AceptarSugerenciaSeleccionada();
            }
        }

        private void LstSugProgramas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                AceptarSugerenciaSeleccionada();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                lstSugProgramas.Visible = false;
                TxtBoxPrograma.Focus();
            }
        }

        private void TxtBoxPrograma_Validated(object sender, EventArgs e)
        {
            var p = TxtBoxPrograma.Text.Trim();
            if (p.Length == 0) { ttPrograma.SetToolTip(TxtBoxPrograma, ""); return; }

            // Validación suave: solo tooltip si hay coincidencia exacta en caché (sin golpear BD)
            if (dtProgramas != null)
            {
                var row = dtProgramas.AsEnumerable()
                    .FirstOrDefault(r => string.Equals(
                        (r.Field<string>("IdPrograma") ?? string.Empty).Trim(),
                        p,
                        StringComparison.OrdinalIgnoreCase));

                if (row != null)
                {
                    var nom = row.Field<string>("Nombre") ?? "";
                    ttPrograma.SetToolTip(TxtBoxPrograma, $"{p} — {nom}");
                }
                else
                {
                    ttPrograma.SetToolTip(TxtBoxPrograma, "");
                }
            }
        }
    }
}
