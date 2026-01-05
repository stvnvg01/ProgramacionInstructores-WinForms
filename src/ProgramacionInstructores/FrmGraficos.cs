using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Net;
using System.Net.Mail;
using Microsoft.VisualBasic; // <- necesario para Interaction.InputBox


using System.IO; // <— para rutas y archivos
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ProgramacionInstructores
{
    public partial class FrmGraficos : Form
    {

        // ---- Sugerencias & Print ----
        private Panel pnlSugerencias;
        private DataGridView dgvSugerencias;
        private bool _seleccionandoSugerencia;

        private readonly System.Drawing.Printing.PrintDocument _printDoc = new System.Drawing.Printing.PrintDocument();
        private Bitmap _capturaPanel;

        public FrmGraficos()
        {
            InitializeComponent();

            // Importante para flechas / ESC globales
            this.KeyPreview = true;

            // Botones
            btnAceptar.Click += btnAceptar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnImprimir.Click += btnImprimir_Click;

            // Enter = Aceptar
            txtIdFicha.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; btnAceptar.PerformClick(); }
            };

            // Solo dígitos (si Id es numérico)
            txtIdFicha.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
            };

            // Buscador en vivo
            CrearUIBuscador();
            txtIdFicha.TextChanged += (s, e) =>
            {
                if (_seleccionandoSugerencia) return;
                CargarSugerencias(txtIdFicha.Text.Trim());
            };
            // Un solo clic: seleccionar y cargar de una
            dgvSugerencias.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0) AceptarSugerencia();
            };


            // Impresión: horizontal
            _printDoc.DefaultPageSettings.Landscape = true;
            _printDoc.PrintPage += PrintDoc_PrintPage;

            // Atajos
            this.KeyDown += (s, e) =>
            {
                if (e.Control && e.KeyCode == Keys.P)
                {
                    e.SuppressKeyPress = true;
                    btnImprimir.PerformClick();
                }
            };
        }

        // =======================
        //  UTILIDADES
        // =======================
        private bool ConexionAbierta()
        {
            if (Conexion.ConDB.State != ConnectionState.Open)
                Conexion.ConDB.Open();
            return true;
        }

        private bool ExisteFicha(string idFicha)
        {
            const string sql = "SELECT 1 FROM Fichas WHERE RTRIM(IdFicha) = @IdFicha";
            ConexionAbierta();
            using (var cmd = new SqlCommand(sql, Conexion.ConDB))
            {
                cmd.Parameters.Add("@IdFicha", SqlDbType.NVarChar, 50).Value = idFicha.Trim();
                return cmd.ExecuteScalar() != null;
            }
        }



        private string Truncar(string s, int n)
        {
            s = (s ?? string.Empty).Trim();
            return (s.Length <= n) ? s : s.Substring(0, n) + "…";
        }

        private int RedondearArriba(int valor, int paso)
        {
            if (valor <= 0) return paso;
            return ((valor + paso - 1) / paso) * paso;
        }

        // =======================
        //  DATOS (SQL)
        // =======================
        private DataTable ObtenerProgresoCompetencias(string idFicha)
        {
            string query = @"
            SELECT
                c.IdCompetencia,
                c.Nombre,
                pc.DuracionPlan,
                ISNULL(SUM(cd.DuracionBloque), 0) AS HorasCursadas
            FROM Fichas f
            JOIN ProgramasCompetencias pc
                ON pc.IdPrograma = f.IdPrograma
            JOIN Competencias c
                ON c.IdCompetencia = pc.IdCompetencia
            LEFT JOIN CronogramaDetalle cd
                ON cd.IdFicha = f.IdFicha
               AND cd.IdCompetencia = c.IdCompetencia
            WHERE f.IdFicha = @IdFicha
            GROUP BY c.IdCompetencia, c.Nombre, pc.DuracionPlan
            ORDER BY c.Nombre;";

            ConexionAbierta();
            using (var cmd = new SqlCommand(query, Conexion.ConDB))
            {
                cmd.Parameters.Add("@IdFicha", SqlDbType.NVarChar, 15).Value = idFicha.Trim();
                var da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // =======================
        //  UI SUGERENCIAS
        // =======================
        private void CrearUIBuscador()
        {
            pnlSugerencias = new Panel
            {
                Visible = false,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Width = txtIdFicha.Width + 300,
                Height = 200
            };

            dgvSugerencias = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
                MultiSelect = false
            };

            pnlSugerencias.Controls.Add(dgvSugerencias);

            // Ponerlo sobre el FORM para que flote por encima de paneles dock-fill
            this.Controls.Add(pnlSugerencias);
            pnlSugerencias.BringToFront();

            // Reposicionar bajo el textbox siempre que cambie layout/tamaños
            this.Layout += (s, e) => PosicionarPanelSugerencias();
            this.SizeChanged += (s, e) => PosicionarPanelSugerencias();
            txtIdFicha.Parent.SizeChanged += (s, e) => PosicionarPanelSugerencias();
            txtIdFicha.GotFocus += (s, e) => PosicionarPanelSugerencias();

            // Aceptar/Cancelar desde la grilla
            dgvSugerencias.CellDoubleClick += (s, e) => AceptarSugerencia();
            dgvSugerencias.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    AceptarSugerencia();
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    pnlSugerencias.Visible = false;
                    txtIdFicha.Focus();
                }
            };

            // Ocultar con click fuera
            this.MouseDown += (s, e) =>
            {
                if (pnlSugerencias.Visible && !pnlSugerencias.Bounds.Contains(e.Location))
                    pnlSugerencias.Visible = false;
            };
            panelGrafico.MouseDown += (s, e) => pnlSugerencias.Visible = false;

            // Desde textbox, flecha abajo para entrar a la lista
            txtIdFicha.KeyDown += (s, e) =>
            {
                if (pnlSugerencias.Visible && e.KeyCode == Keys.Down && dgvSugerencias.Rows.Count > 0)
                {
                    dgvSugerencias.Focus();
                    if (dgvSugerencias.CurrentCell == null)
                        dgvSugerencias.CurrentCell = dgvSugerencias[0, 0];
                    e.Handled = true;
                }
            };
        }

        private void PosicionarPanelSugerencias()
        {
            var p = this.PointToClient(txtIdFicha.Parent.PointToScreen(txtIdFicha.Location));
            pnlSugerencias.Location = new Point(p.X, p.Y + txtIdFicha.Height + 2);
            pnlSugerencias.Width = Math.Max(txtIdFicha.Width + 100, 260);
        }


        private void CargarSugerencias(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                pnlSugerencias.Visible = false;
                return;
            }

            const string sql = @"
        SELECT TOP 20 RTRIM(f.IdFicha) AS IdFicha
        FROM Fichas f
        WHERE RTRIM(f.IdFicha) LIKE @q
        ORDER BY f.IdFicha;";

            try
            {
                ConexionAbierta();
                using (var cmd = new SqlCommand(sql, Conexion.ConDB))
                {
                    cmd.Parameters.Add("@q", SqlDbType.NVarChar, 50).Value = "%" + term.Trim() + "%";
                    var da = new SqlDataAdapter(cmd);
                    var dt = new DataTable();
                    da.Fill(dt);
                    dgvSugerencias.DataSource = dt;
                }

                // Ajuste visual
                dgvSugerencias.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvSugerencias.RowHeadersVisible = false;

                if (dgvSugerencias.Rows.Count > 0)
                {
                    pnlSugerencias.Visible = true;
                    pnlSugerencias.BringToFront();
                    dgvSugerencias.CurrentCell = dgvSugerencias[0, 0];
                }
                else
                {
                    pnlSugerencias.Visible = false;
                }
            }
            catch (Exception ex)
            {
                pnlSugerencias.Visible = false;
                MessageBox.Show("Error buscando fichas: " + ex.Message, "Búsqueda", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AceptarSugerencia()
        {
            if (dgvSugerencias.CurrentRow == null) return;
            var id = dgvSugerencias.CurrentRow.Cells["IdFicha"].Value?.ToString();
            if (string.IsNullOrWhiteSpace(id)) return;

            _seleccionandoSugerencia = true;
            txtIdFicha.Text = id;
            txtIdFicha.SelectionStart = txtIdFicha.TextLength;
            _seleccionandoSugerencia = false;

            pnlSugerencias.Visible = false;
            btnAceptar.PerformClick();
        }

        // =======================
        //  GRÁFICO + TABLA
        // =======================
        private void MostrarGrafico(string idFicha)
        {
            panelGrafico.Controls.Clear();

            DataTable dt = ObtenerProgresoCompetencias(idFicha);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para esa ficha.");
                return;
            }

            int maxPlan = dt.AsEnumerable().Select(r => Convert.ToInt32(r["DuracionPlan"])).DefaultIfEmpty(0).Max();
            int maxProg = dt.AsEnumerable().Select(r => Convert.ToInt32(r["HorasCursadas"])).DefaultIfEmpty(0).Max();
            int totalPlan = dt.AsEnumerable().Sum(r => Convert.ToInt32(r["DuracionPlan"]));
            int totalProg = dt.AsEnumerable().Sum(r => Convert.ToInt32(r["HorasCursadas"]));
            double pctGlobal = totalPlan == 0 ? 0 : (100.0 * totalProg / totalPlan);

            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                Panel1MinSize = 0,
                Panel2MinSize = 0,
                SplitterWidth = 6
            };
            panelGrafico.Controls.Add(split);

            void AjustarSplit()
            {
                int width = split.ClientSize.Width;
                if (width <= 0) return;

                int p1MinDeseado = 300;
                int p2MinDeseado = 170;
                if (p1MinDeseado + p2MinDeseado + split.SplitterWidth >= width)
                {
                    int half = Math.Max(0, (width - split.SplitterWidth) / 2);
                    p1MinDeseado = half;
                    p2MinDeseado = half;
                }

                split.Panel1MinSize = p1MinDeseado;
                split.Panel2MinSize = p2MinDeseado;

                int desired = (int)(width * 0.60);
                int min = split.Panel1MinSize;
                int max = Math.Max(min, width - split.Panel2MinSize - split.SplitterWidth - 1);
                int clamped = Math.Max(min, Math.Min(desired, max));
                if (clamped < 0) clamped = 0;

                split.SplitterDistance = clamped;
            }

            this.BeginInvoke(new Action(AjustarSplit));
            split.SizeChanged += (s, e) => AjustarSplit();

            // Chart
            var chart = new Chart { Dock = DockStyle.Fill };
            var area = new ChartArea("MainArea");
            chart.ChartAreas.Add(area);
            split.Panel1.Controls.Add(chart);

            area.AxisX.Interval = 1;
            area.AxisX.LabelStyle.Angle = 0;
            area.AxisX.Title = "Competencias";
            area.AxisY.Title = "Horas";

            int maxY = Math.Max(maxPlan, maxProg);
            maxY = Math.Min(3000, RedondearArriba((int)(maxY * 1.10), 100));
            area.AxisY.Maximum = Math.Max(maxY, 1000);
            area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;

            var seriePlan = new Series("Plan")
            {
                ChartArea = "MainArea",
                ChartType = SeriesChartType.Bar,
                Color = Color.Gainsboro,
                IsVisibleInLegend = false
            };
            seriePlan["PointWidth"] = "0.6";
            seriePlan["DrawSideBySide"] = "False";

            var serieProg = new Series("Programadas")
            {
                ChartArea = "MainArea",
                ChartType = SeriesChartType.Bar,
                IsVisibleInLegend = false
            };
            serieProg["PointWidth"] = "0.6";
            serieProg["DrawSideBySide"] = "False";

            Color cCompleta = Color.LimeGreen;
            Color cParcial = Color.SteelBlue;
            Color cCero = Color.IndianRed;

            // Derecha: resumen + grid
            var rightHost = new Panel { Dock = DockStyle.Fill, Padding = new Padding(8) };
            split.Panel2.Controls.Add(rightHost);

            var headerPanel = new Panel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink };
            rightHost.Controls.Add(headerPanel);

            var lblTotales = new Label
            {
                AutoSize = true,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 0, 0, 6),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Text = $"Totales → Plan: {totalPlan} h | Programadas: {totalProg} h | Avance: {pctGlobal:0.0}%"
            };
            headerPanel.Controls.Add(lblTotales);

            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            };
            rightHost.Controls.Add(dgv);

            var tbl = new DataTable();
            tbl.Columns.Add("IdCompetencia");
            tbl.Columns.Add("Nombre");
            tbl.Columns.Add("Plan", typeof(int));
            tbl.Columns.Add("Programadas", typeof(int));
            tbl.Columns.Add("%", typeof(double));

            foreach (DataRow r in dt.Rows)
            {
                string idComp = r["IdCompetencia"].ToString().Trim();
                string nombre = r["Nombre"].ToString();
                int plan = Convert.ToInt32(r["DuracionPlan"]);
                int prog = Convert.ToInt32(r["HorasCursadas"]);
                double pct = plan == 0 ? 0 : (100.0 * prog / plan);

                string etiqueta = $"{idComp} - {Truncar(nombre, 22)}";

                seriePlan.Points.AddXY(etiqueta, plan);

                Color colorBarra = (prog == 0) ? cCero : (prog >= plan ? cCompleta : cParcial);

                var p = new DataPoint
                {
                    AxisLabel = etiqueta,
                    YValues = new[] { (double)prog },
                    Color = colorBarra,
                    Label = $"{prog} h",
                    LabelForeColor = Color.Black,
                    Font = new Font("Arial", 9, FontStyle.Bold)
                };
                serieProg.Points.Add(p);

                var rowTbl = tbl.NewRow();
                rowTbl["IdCompetencia"] = idComp;
                rowTbl["Nombre"] = Truncar(nombre, 60);
                rowTbl["Plan"] = plan;
                rowTbl["Programadas"] = prog;
                rowTbl["%"] = Math.Round(pct, 1);
                tbl.Rows.Add(rowTbl);
            }

            chart.Series.Add(seriePlan);
            chart.Series.Add(serieProg);

            dgv.DataSource = tbl;
            dgv.Columns["%"].DefaultCellStyle.Format = "0.0";
            dgv.AutoResizeColumns();
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dgv.Columns["Nombre"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ((DataGridViewTextBoxColumn)dgv.Columns["Nombre"]).MinimumWidth = 140;
        }

        // =======================
        //  EVENTOS
        // =======================
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            pnlSugerencias.Visible = false; // <— ocultar la ventana de previsualización

            string idFicha = txtIdFicha.Text.Trim();
            if (string.IsNullOrWhiteSpace(idFicha))
            {
                MessageBox.Show("Digite el IdFicha.");
                txtIdFicha.Focus();
                return;
            }

            if (!ExisteFicha(idFicha))
            {
                MessageBox.Show("La ficha no existe.");
                txtIdFicha.SelectAll();
                txtIdFicha.Focus();
                return;
            }

            MostrarGrafico(idFicha);
        }


        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtIdFicha.Clear();
            panelGrafico.Controls.Clear();
            pnlSugerencias.Visible = false;
            txtIdFicha.Focus();
        }
        // --- SIN CAMBIOS DE FIRMA ---
        // Genera un nombre de archivo estándar para el PDF
        private string GetDefaultPdfName()
        {
            string id = txtIdFicha.Text.Trim();
            string fecha = DateTime.Now.ToString("yyyy-MM-dd");
            return $"Avance de Ficha {id} - {fecha}.pdf";
        }

        // Ruta por defecto (no usada por el envío, pero la conservamos)
        private string GetDefaultPdfFullPath()
        {
            string carpeta = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return Path.Combine(carpeta, GetDefaultPdfName());
        }

        // Busca una impresora PDF instalada
        private bool TryGetPdfPrinter(out string printerName)
        {
            printerName = null;
            foreach (string p in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                if (p.Equals("Microsoft Print to PDF", StringComparison.OrdinalIgnoreCase))
                { printerName = p; return true; }

            foreach (string p in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                if (p.IndexOf("PDF", StringComparison.OrdinalIgnoreCase) >= 0)
                { printerName = p; return true; }

            return false;
        }

        // Guarda el PDF silenciosamente (sin diálogos) en la ruta indicada
        private void GuardarPdfAuto(string rutaPdf)
        {
            if (!TryGetPdfPrinter(out string impresora))
                throw new InvalidOperationException("No se encontró una impresora PDF instalada.");

            _printDoc.PrintController = new StandardPrintController();
            _printDoc.PrinterSettings = new PrinterSettings
            {
                PrinterName = impresora,
                PrintToFile = true,
                PrintFileName = rutaPdf
            };

            _printDoc.DefaultPageSettings.Landscape = true;
            _printDoc.Print();
        }




        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (panelGrafico.Controls.Count == 0)
            {
                MessageBox.Show("Primero genera un gráfico.");
                return;
            }

            // Captura del panel que vamos a imprimir
            _capturaPanel = CapturarControl(panelGrafico);

            // Diálogo para guardar
            using (var sfd = new SaveFileDialog())
            {
                sfd.Title = "Guardar reporte en PDF";
                sfd.Filter = "PDF (*.pdf)|*.pdf";
                sfd.FileName = GetDefaultPdfName();
                sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                sfd.AddExtension = true;
                sfd.DefaultExt = "pdf";
                sfd.OverwritePrompt = true;

                var result = sfd.ShowDialog();  // <- sin 'this'

                if (result != DialogResult.OK) return;

                string rutaElegida = sfd.FileName;

                try
                {
                    GuardarPdfAuto(rutaElegida);   // usa tu impresora PDF (“Microsoft Print to PDF”)
                    MessageBox.Show($"PDF guardado:\n{rutaElegida}", "Impresión", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // Si no hay impresora PDF, caemos a vista previa como respaldo
                    using (var ppd = new PrintPreviewDialog())
                    {
                        ppd.Document = _printDoc;
                        ppd.WindowState = FormWindowState.Maximized;
                        ppd.ShowDialog(this);
                    }
                    // Si quieres ver el motivo del fallo, descomenta:
                    // MessageBox.Show("No se pudo imprimir a PDF automáticamente: " + ex.Message);
                }
            }
        }



        private void PrintDoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            if (_capturaPanel == null)
            {
                e.HasMorePages = false;
                return;
            }

            Rectangle area = e.MarginBounds;

            // Mantener proporción dentro de márgenes (en horizontal)
            float ratioCtl = (float)_capturaPanel.Width / _capturaPanel.Height;
            int targetW = area.Width;
            int targetH = (int)(targetW / ratioCtl);
            if (targetH > area.Height)
            {
                targetH = area.Height;
                targetW = (int)(targetH * ratioCtl);
            }

            using (var titleFont = new Font("Segoe UI", 12, FontStyle.Bold))
            {
                string titulo = $"Avance de Ficha {txtIdFicha.Text.Trim()} - {DateTime.Now:yyyy-MM-dd}";
                SizeF ts = e.Graphics.MeasureString(titulo, titleFont, area.Width);
                // Pequeño margen arriba
                e.Graphics.DrawString(titulo, titleFont, Brushes.Black, area.Left, area.Top - ts.Height - 6);
            }

            var destino = new Rectangle(area.Left, area.Top, targetW, targetH);
            e.Graphics.DrawImage(_capturaPanel, destino);

            e.HasMorePages = false;
        }

        private Bitmap CapturarControl(Control ctl)
        {
            var bmp = new Bitmap(ctl.ClientSize.Width, ctl.ClientSize.Height);
            ctl.DrawToBitmap(bmp, new Rectangle(Point.Empty, ctl.ClientSize));
            return bmp;
        }

        private void FrmGraficos_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
            if (Conexion.ConDB.State == ConnectionState.Open)
                Conexion.ConDB.Close();
        }

        // Anti-parpadeo opcional
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }
        // CORREOOOOOOOOOOOOOOOOO
        private sealed class CredencialesEmpresa
        {
            public string FromEmail { get; set; }
            public string AppPass { get; set; }
            public string DisplayName { get; set; }
        }


        // Devuelve: correo remitente, contraseña de aplicación (PinCor), nombre para mostrar
        private CredencialesEmpresa ObtenerCredencialesEmpresa()
        {
            const string sql = "SELECT TOP 1 Correo, PinCor, NombreE FROM Empresa";
            if (Conexion.ConDB.State != ConnectionState.Open) Conexion.ConDB.Open();

            using (var cmd = new SqlCommand(sql, Conexion.ConDB))
            using (var rd = cmd.ExecuteReader())
            {
                if (!rd.Read())
                    throw new Exception("No hay registro en Empresa (Correo/PinCor).");

                string fromEmail = ((rd["Correo"] as string) ?? "").Trim();
                string appPass = ((rd["PinCor"] as string) ?? "").Replace(" ", "").Trim(); // quita espacios
                string displayName = ((rd["NombreE"] as string) ?? "").Trim();

                if (string.IsNullOrWhiteSpace(fromEmail) || string.IsNullOrWhiteSpace(appPass))
                    throw new Exception("Empresa.Correo o Empresa.PinCor están vacíos.");

                if (string.IsNullOrWhiteSpace(displayName)) displayName = fromEmail;

                return new CredencialesEmpresa
                {
                    FromEmail = fromEmail,
                    AppPass = appPass,
                    DisplayName = displayName
                };
            }
        }


        public static class Prompt
        {
            public static string Show(string title, string label, string @default = "")
            {
                using (var f = new Form())
                using (var lbl = new Label() { Left = 12, Top = 12, AutoSize = true, Text = label })
                using (var tb = new TextBox() { Left = 12, Top = 36, Width = 360, Text = @default })
                using (var ok = new Button() { Text = "Aceptar", Left = 212, Width = 80, Top = 72, DialogResult = DialogResult.OK })
                using (var cancel = new Button() { Text = "Cancelar", Left = 292, Width = 80, Top = 72, DialogResult = DialogResult.Cancel })
                {
                    f.Text = title;
                    f.FormBorderStyle = FormBorderStyle.FixedDialog;
                    f.StartPosition = FormStartPosition.CenterParent;
                    f.ClientSize = new Size(384, 110);
                    f.MinimizeBox = false; f.MaximizeBox = false;
                    f.Controls.AddRange(new Control[] { lbl, tb, ok, cancel });
                    f.AcceptButton = ok; f.CancelButton = cancel;

                    return f.ShowDialog() == DialogResult.OK ? tb.Text : string.Empty;
                }
            }
        }

        private void btnEnviarCorreo_Click(object sender, EventArgs e)
        {
            try
            {
                // A) Asegurar que hay algo que enviar: si no hay gráfico, lo generamos si hay ficha
                if (panelGrafico.Controls.Count == 0)
                {
                    string id = txtIdFicha.Text.Trim();
                    if (string.IsNullOrWhiteSpace(id) || !ExisteFicha(id))
                    {
                        MessageBox.Show("Primero digita/valida la ficha para generar el reporte.");
                        return;
                    }
                    MostrarGrafico(id); // genera el panel y totales
                }

                // B) Datos de envío
                string to = Prompt.Show("Enviar reporte", "Correo destinatario:");
                if (string.IsNullOrWhiteSpace(to)) { MessageBox.Show("No se ingresó ningún correo."); return; }
                try { _ = new MailAddress(to); } catch { MessageBox.Show("Correo no válido."); return; }

                string asunto = Prompt.Show("Enviar reporte", "Asunto:",
                    $"Avance de Ficha {txtIdFicha.Text.Trim()} - {DateTime.Now:yyyy-MM-dd}");
                if (string.IsNullOrWhiteSpace(asunto))
                    asunto = $"Avance de Ficha {txtIdFicha.Text.Trim()} - {DateTime.Now:yyyy-MM-dd}";

                // C) Cuerpo en HTML y texto plano (multipart/alternative)
                string cuerpoHtml =
                    $"Buen día,<br/><br/>Adjunto el reporte de avance de la ficha <b>{txtIdFicha.Text.Trim()}</b>.<br/><br/>Saludos.";
                string cuerpoText =
                    $"Buen día,\r\n\r\nAdjunto el reporte de avance de la ficha {txtIdFicha.Text.Trim()}.\r\n\r\nSaludos.";

                // D) Generar PDF silencioso siempre (temporal)
                string rutaTemp = Path.Combine(Path.GetTempPath(), GetDefaultPdfName());
                _capturaPanel = CapturarControl(panelGrafico);  // vuelve a capturar por si cambió el tamaño
                GuardarPdfAuto(rutaTemp);                       // usa tu impresora PDF instalada

                // E) Credenciales
                var cred = ObtenerCredencialesEmpresa();

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (var msg = new MailMessage())
                {
                    msg.From = new MailAddress(cred.FromEmail, cred.DisplayName);
                    msg.Sender = new MailAddress(cred.FromEmail, cred.DisplayName);
                    msg.ReplyToList.Clear();
                    msg.ReplyToList.Add(new MailAddress(cred.FromEmail));
                    msg.To.Add(to);
                    msg.Bcc.Add(cred.FromEmail); // verificación de entrega

                    msg.Subject = asunto;

                    // Multipart: texto + HTML
                    var altViewText = AlternateView.CreateAlternateViewFromString(cuerpoText, null, "text/plain");
                    var altViewHtml = AlternateView.CreateAlternateViewFromString(cuerpoHtml, null, "text/html");
                    msg.AlternateViews.Add(altViewText);
                    msg.AlternateViews.Add(altViewHtml);
                    msg.IsBodyHtml = true; // para clientes que usen Body directamente

                    msg.Attachments.Add(new Attachment(rutaTemp));

                    // DSN / acuse de lectura (si el receptor lo permite)
                    msg.DeliveryNotificationOptions =
                        DeliveryNotificationOptions.OnFailure | DeliveryNotificationOptions.Delay | DeliveryNotificationOptions.OnSuccess;
                    msg.Headers.Add("Disposition-Notification-To", cred.FromEmail);

                    using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.EnableSsl = true; // STARTTLS
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(cred.FromEmail, cred.AppPass);
                        smtp.Send(msg);
                    }
                }

                MessageBox.Show($"Correo enviado a:\n{to}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (InvalidOperationException ex) // p.ej. no hay impresora PDF
            {
                MessageBox.Show(
                    "No se pudo generar el PDF en modo automático.\n" +
                    "Instala una impresora PDF (e.g. Microsoft Print to PDF) o agrega una librería PDF.\n\n" +
                    ex.Message, "PDF", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (SmtpException ex)
            {
                MessageBox.Show("Error SMTP:\n" + ex.StatusCode + "\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar correo:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        //private void btnAceptar_Click_1(object sender, EventArgs e)
        //{

        //}
    }
}
