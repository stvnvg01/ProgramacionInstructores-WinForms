using ProgramacionInstructores;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ProgramacionInstructores
{
    public partial class FrmPnaIncremental : Form
    {
        public FrmPnaIncremental()
        {
            this.TopLevel = true;
            InitializeComponent();
        }

        // --- Dependencias propias de tu proyecto ---
        Conexion Conn = new Conexion();
        Conexion ConnAux = new Conexion();
        Complementos OnOff = new Complementos();
        OpenFileDialog BuscarArchivo = new OpenFileDialog();

        string Delimitador = ";"; // Por defecto “Punto y coma”
        string Ayuda = string.Empty;

        // ============================================================
        // EVENTOS
        // ============================================================
        private void FrmPnaIncremental_Load(object sender, EventArgs e)
        {
            textBox1.Text = Conexion.VarGlobal.xNomDB;
            textBox2.Text = "Personas";

            string carpetaImport = Path.Combine(Application.StartupPath, "DatosImport");
            if (!Directory.Exists(carpetaImport)) Directory.CreateDirectory(carpetaImport);
            textBox3.Text = carpetaImport + "\\";

            // Limpia tabla TMP
            try
            {
                using (var cn = new SqlConnection(Conexion.ConDB.ConnectionString))
                {
                    cn.Open();
                    using (SqlCommand cmdDel = new SqlCommand(
                        "IF OBJECT_ID('dbo.PersonasTMP') IS NOT NULL DELETE FROM PersonasTMP;", cn))
                    {
                        cmdDel.ExecuteNonQuery();
                    }
                }
            }
            catch { /* si no existe, ignora */ }

            // Carga grid
            Conn.ConsultaDatos(
                "IF OBJECT_ID('dbo.PersonasTMP') IS NULL SELECT 1 AS Dummy ELSE SELECT * FROM PersonasTMP ORDER BY Apellido;",
                "PersonasTMP"
            );
            dataGridView1.DataSource = Conn.Ds.Tables["PersonasTMP"];
            OrganizaGrid(dataGridView1);
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;

            TotDat.Text = "Total Registro a Importar: " + (Conn.Ds.Tables["PersonasTMP"]?.Rows.Count ?? 0);

            Ayuda =
                "INSTRUCCIONES PARA LA IMPORTACIÓN (CSV):\n" +
                "• IdPersona   = Identificador de la persona (CC/NIT u otro código)\n" +
                "• Documento   = Documento (CC/NIT)\n" +
                "• Nombre      = Nombres (máx 30)\n" +
                "• Apellido    = Apellidos (máx 30)\n" +
                "• Telefono    = Teléfono (máx 15)\n" +
                "• Correo      = Email (máx 50)\n" +
                "• Direccion   = Dirección (máx 50)\n" +
                "• IdLugar     = Código DANE ciudad (máx 5)\n" +
                "• Estado      = 1 Activo, 0 o NULL Inactivo\n" +
                "• Observacion = Texto libre (máx 300)\n\n" +
                "Primera fila = encabezados. Guarda el CSV con el delimitador seleccionado en el formulario.";
            MessageBox.Show(Ayuda, "Instrucciones para la Importación de Datos");
        }



        private void RbPuntoYComa_CheckedChanged_1(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked) Delimitador = ";";
        }

        private void RbComa_CheckedChanged_1(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked) Delimitador = ",";
        }

        private void BtoBusArc_Click(object sender, EventArgs e)
        {
            try
            {
                BuscarArchivo.FileName = "";
                BuscarArchivo.InitialDirectory = Directory.Exists(textBox3.Text) ? textBox3.Text : Application.StartupPath;
                BuscarArchivo.Title = "Buscar archivo CSV para importar Personas";
                BuscarArchivo.Filter = "Archivos CSV (*.csv)|*.csv|Todos los archivos (*.*)|*.*";

                if (BuscarArchivo.ShowDialog() != DialogResult.OK) return;

                textBox3.Text = BuscarArchivo.FileName;

                // Delimitador según radio
                char delimChar = Delimitador[0];

                // Importar con SqlBulkCopy
                ImportarConSqlBulkCopy_UTF8(textBox3.Text, delimChar);

                // Refresca el grid
                Conn.ConsultaDatos("SELECT * FROM PersonasTMP ORDER BY Apellido;", "PersonasTMP");
                dataGridView1.DataSource = Conn.Ds.Tables["PersonasTMP"];
                OrganizaGrid(dataGridView1);
                TotDat.Text = "Total Registro a Importar: " + Conn.Ds.Tables["PersonasTMP"].Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al importar los datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtoUpDate_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Ayuda, "Recordatorio de Formato CSV");

            if (string.IsNullOrWhiteSpace(textBox3.Text) || !File.Exists(textBox3.Text))
            {
                MessageBox.Show("Sin archivo válido para importar.", "Actualizando...!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Conn.Ds.Tables["PersonasTMP"] == null || Conn.Ds.Tables["PersonasTMP"].Rows.Count == 0)
            {
                MessageBox.Show("No hay datos en PersonasTMP para actualizar.", "Actualizando...!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string xMerge = @"
MERGE Personas AS destino
USING (
    SELECT
        RTRIM(LTRIM(ISNULL(IdPersona,'')))   AS IdPersona,
        RTRIM(LTRIM(ISNULL(Documento,'')))   AS Documento,
        RTRIM(LTRIM(ISNULL(Nombre,'')))      AS Nombre,
        RTRIM(LTRIM(ISNULL(Apellido,'')))    AS Apellido,
        RTRIM(LTRIM(ISNULL(Telefono,'')))    AS Telefono,
        RTRIM(LTRIM(ISNULL(Correo,'')))      AS Correo,
        RTRIM(LTRIM(ISNULL(Direccion,'')))   AS Direccion,
        RTRIM(LTRIM(ISNULL(IdLugar,'')))     AS IdLugar,
        CAST(ISNULL(Estado, 0) AS INT)       AS Estado,
        RTRIM(LTRIM(ISNULL(Observacion,''))) AS Observacion
    FROM PersonasTMP
) AS src
ON (destino.IdPersona = src.IdPersona)
WHEN MATCHED THEN
    UPDATE SET
        destino.Documento   = src.Documento,
        destino.Nombre      = src.Nombre,
        destino.Apellido    = src.Apellido,
        destino.Telefono    = src.Telefono,
        destino.Correo      = src.Correo,
        destino.Direccion   = src.Direccion,
        destino.IdLugar     = src.IdLugar,
        destino.Estado      = src.Estado,
        destino.Observacion = src.Observacion
WHEN NOT MATCHED BY TARGET THEN
    INSERT (IdPersona, Documento, Nombre, Apellido, Telefono, Correo, Direccion, IdLugar, Estado, Observacion)
    VALUES (src.IdPersona, src.Documento, src.Nombre, src.Apellido, src.Telefono, src.Correo, src.Direccion, src.IdLugar, src.Estado, src.Observacion)
OUTPUT $action AS Accion;";

            try
            {
                if (Conexion.ConDB.State != ConnectionState.Open) Conexion.ConDB.Open();

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

                Conn.ConsultaDatos("SELECT * FROM Personas ORDER BY Apellido;", "Personas");
                dataGridView1.DataSource = Conn.Ds.Tables["Personas"];
                OrganizaGrid(dataGridView1);
                TotReg.Text = "Total Registros Personas: " + Conn.Ds.Tables["Personas"].Rows.Count.ToString();

                MessageBox.Show($"Actualización completada.\nInsertados: {ins}\nActualizados: {upd}",
                    "Actualizando Datos", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

        // ============================================================
        // UTILIDADES
        // ============================================================
        private void OrganizaGrid(DataGridView Grid)
        {
            if (Grid == null || Grid.Columns.Count == 0) return;

            foreach (DataGridViewColumn col in Grid.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            AjustarCol(Grid, "IdPersona", 110, true);
            AjustarCol(Grid, "Documento", 100, true);
            AjustarCol(Grid, "Nombre", 140, true);
            AjustarCol(Grid, "Apellido", 140, true);
            AjustarCol(Grid, "Telefono", 95, true);
            AjustarCol(Grid, "Correo", 160, true);
            AjustarCol(Grid, "Direccion", 160, true);
            AjustarCol(Grid, "IdLugar", 70, true);
            AjustarCol(Grid, "Estado", 65, true);
            AjustarCol(Grid, "Observacion", 220, true);

            Grid.AllowUserToAddRows = false;
            Grid.AllowUserToDeleteRows = false;
            Grid.AllowUserToOrderColumns = false;
            Grid.ReadOnly = true;
            Grid.MultiSelect = false;
            Grid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            Grid.RowsDefaultCellStyle.BackColor = Color.LightBlue;
            Grid.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
        }

        private void AjustarCol(DataGridView grid, string nombre, int width, bool visible)
        {
            if (!grid.Columns.Contains(nombre)) return;
            var c = grid.Columns[nombre];
            c.Visible = visible;
            c.Width = width;
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Estado" && e.Value != null && e.Value != DBNull.Value)
            {
                int est;
                if (int.TryParse(e.Value.ToString(), out est))
                    e.Value = est == 1 ? "Activo" : "Inactivo";
                else
                    e.Value = "Inactivo";
                e.FormattingApplied = true;
            }
        }

        // ============================================================
        // IMPORTACIÓN UTF-8 CON SqlBulkCopy (conversión de tipos)
        // ============================================================
        private void ImportarConSqlBulkCopy_UTF8(string rutaCsv, char delim)
        {
            using (var cn = new SqlConnection(Conexion.ConDB.ConnectionString))
            {
                cn.Open();
                using (SqlCommand cmdDel = new SqlCommand("DELETE FROM PersonasTMP;", cn))
                    cmdDel.ExecuteNonQuery();

                // crea DataTable con el esquema real de PersonasTMP
                var dt = new DataTable();
                using (var da = new SqlDataAdapter("SELECT TOP 0 * FROM PersonasTMP", cn))
                    da.FillSchema(dt, SchemaType.Source);

                // leer encabezado
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

                // leer datos
                using (var sr = new StreamReader(rutaCsv, Encoding.UTF8, true))
                {
                    sr.ReadLine(); // saltar encabezado

                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        var parts = line.Split(new[] { delim }, StringSplitOptions.None);
                        var row = dt.NewRow();

                        foreach (DataColumn col in dt.Columns)
                        {
                            int idx;
                            if (!colIndex.TryGetValue(col.ColumnName, out idx) || idx >= parts.Length)
                            {
                                row[col.ColumnName] = DBNull.Value;
                                continue;
                            }

                            string raw = (parts[idx] ?? string.Empty).Trim();
                            if (raw.Length == 0)
                            {
                                row[col.ColumnName] = DBNull.Value;
                                continue;
                            }

                            if (col.ColumnName.Equals("Estado", StringComparison.OrdinalIgnoreCase))
                            {
                                int v;
                                row[col.ColumnName] = int.TryParse(raw, out v) ? (object)v : DBNull.Value;
                            }
                            else
                            {
                                row[col.ColumnName] = raw; // las demás son nchar
                            }
                        }

                        dt.Rows.Add(row);
                    }
                }

                using (var bulk = new SqlBulkCopy(cn))
                {
                    bulk.DestinationTableName = "PersonasTMP";
                    foreach (DataColumn c in dt.Columns)
                        bulk.ColumnMappings.Add(c.ColumnName, c.ColumnName);
                    bulk.WriteToServer(dt);
                }
            }
        }


    }
}

