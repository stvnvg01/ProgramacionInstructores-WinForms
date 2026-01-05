using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ProgramacionInstructores
{
    public partial class FrmInfoCelda : Form
    {
        public string Ficha { get; set; }
        public string Programa { get; set; }
        public string Competencia { get; set; }

        private ListBox lstSugerencias;
        private TextBox currentTextBox;

        // Conexión compartida
        private SqlConnection ConDB => Conexion.ConDB;

        public FrmInfoCelda()
        {
            InitializeComponent();
        }

        private void FrmInfoCelda_Load(object sender, EventArgs e)
        {
            // ListBox de sugerencias
            lstSugerencias = new ListBox
            {
                Visible = false,
                Height = 140,
                IntegralHeight = true,
                BorderStyle = BorderStyle.FixedSingle,
                TabStop = false
            };

            if (lstSugerencias.Parent == null)
                this.Controls.Add(lstSugerencias);

            // Confirmación por clic / doble clic / enter
            lstSugerencias.MouseDown += (s, me) =>
            {
                int i = lstSugerencias.IndexFromPoint(me.Location);
                if (i >= 0)
                {
                    lstSugerencias.SelectedIndex = i;
                    CommitSelection();
                }
            };
            lstSugerencias.DoubleClick += (s, ev2) => CommitSelection();
            lstSugerencias.KeyDown += LstSugerencias_KeyDown;

            // ===== Búsquedas “Id — Nombre” =====
            // Al tipear FICHA, limpiamos programa para evitar desincronización
            txtFicha.TextChanged += (s, ev) =>
            {
                if (currentTextBox != txtPrograma) txtPrograma.Clear();
                BuscarSugerencias(
                    txtFicha,
                    @"SELECT TOP 10
                          RTRIM(f.IdFicha) AS Id,
                          RTRIM(f.IdFicha) + ' — ' + ISNULL(RTRIM(p.Nombre),'') + 
                          ISNULL(' v' + RTRIM(f.Version),'') AS Texto
                      FROM Fichas f
                      LEFT JOIN Programas p ON p.IdPrograma = f.IdPrograma
                      WHERE f.IdFicha LIKE @b + '%' OR p.Nombre LIKE '%' + @b + '%'
                      ORDER BY f.IdFicha");
            };

            txtPrograma.TextChanged += (s, ev) => BuscarSugerencias(
                txtPrograma,
                @"SELECT TOP 10
                      RTRIM(p.IdPrograma) AS Id,
                      RTRIM(p.IdPrograma) + ' — ' + RTRIM(p.Nombre) AS Texto
                  FROM Programas p
                  WHERE p.IdPrograma LIKE @b + '%' OR p.Nombre LIKE '%' + @b + '%'
                  ORDER BY p.IdPrograma");

            txtCompetencia.TextChanged += (s, ev) => BuscarSugerencias(
                txtCompetencia,
                @"SELECT TOP 10
                      RTRIM(c.IdCompetencia) AS Id,
                      RTRIM(c.IdCompetencia) + ' — ' + LEFT(RTRIM(ISNULL(c.Nombre,'')), 80) AS Texto
                  FROM Competencias c
                  WHERE c.IdCompetencia LIKE @b + '%' OR c.Nombre LIKE '%' + @b + '%'
                  ORDER BY c.IdCompetencia");

            // Navegación con teclado
            txtFicha.KeyDown += TextBox_KeyDown;
            txtPrograma.KeyDown += TextBox_KeyDown;
            txtCompetencia.KeyDown += TextBox_KeyDown;

            // Ocultar la lista al salir de los textbox (si no se va al listbox)
            txtFicha.Leave += TextBox_Leave;
            txtPrograma.Leave += TextBox_Leave;
            txtCompetencia.Leave += TextBox_Leave;
        }

        private void TextBox_Leave(object s, EventArgs e)
        {
            if (this.ActiveControl != lstSugerencias)
                lstSugerencias.Visible = false;
        }

        private void EnsureOpen()
        {
            if (ConDB.State != ConnectionState.Open)
                ConDB.Open();
        }

        private void BuscarSugerencias(TextBox txt, string sql)
        {
            string b = txt.Text.Trim();
            if (string.IsNullOrEmpty(b))
            {
                lstSugerencias.Visible = false;
                lstSugerencias.DataSource = null;
                return;
            }

            currentTextBox = txt;

            if (lstSugerencias.Parent != txt.Parent)
                txt.Parent.Controls.Add(lstSugerencias);

            EnsureOpen();

            using (var cmd = new SqlCommand(sql, ConDB))
            {
                cmd.Parameters.AddWithValue("@b", b);
                var dt = new DataTable();
                new SqlDataAdapter(cmd).Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    lstSugerencias.Visible = false;
                    lstSugerencias.DataSource = null;
                    return;
                }

                lstSugerencias.DataSource = dt;
                lstSugerencias.DisplayMember = "Texto"; // lo visible
                lstSugerencias.ValueMember = "Id";      // el Id real

                // Posicionar debajo del TextBox
                Point screenBottomLeft = txt.PointToScreen(new Point(0, txt.Height));
                Point parentPoint = txt.Parent.PointToClient(screenBottomLeft);
                lstSugerencias.Location = parentPoint;
                lstSugerencias.Width = txt.Width;
                lstSugerencias.BringToFront();
                lstSugerencias.Visible = true;
            }
        }

        // === Autocomplete confirmado ===
        private void CommitSelection()
        {
            if (lstSugerencias.SelectedItem is DataRowView row && currentTextBox != null)
            {
                currentTextBox.Text = row["Id"].ToString(); // Solo el Id
                currentTextBox.SelectionStart = currentTextBox.TextLength;

                // Si se confirmó una FICHA, autocompletar PROGRAMA
                if (currentTextBox == txtFicha)
                    AutofillProgramaFromFicha();
            }
            lstSugerencias.Visible = false;
            currentTextBox?.Focus();
        }

        // Enter / Escape en TextBox
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (lstSugerencias.Visible && lstSugerencias.Items.Count > 0)
                    CommitSelection();

                // Si estás en Ficha y no elegiste de la lista, igual intenta autocompletar Programa
                if (sender == txtFicha)
                {
                    AutofillProgramaFromFicha();
                    txtPrograma.Focus();
                }
                else if (sender == txtPrograma)
                {
                    txtCompetencia.Focus();
                }
                else if (sender == txtCompetencia)
                {
                    btnGuardar.Focus();
                }

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Down && lstSugerencias.Visible)
            {
                lstSugerencias.Focus();
                if (lstSugerencias.Items.Count > 0) lstSugerencias.SelectedIndex = 0;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                lstSugerencias.Visible = false;
                e.Handled = true;
            }
        }

        // Enter / Escape en ListBox
        private void LstSugerencias_KeyDown(object sender, KeyEventArgs ke)
        {
            if (ke.KeyCode == Keys.Enter)
            {
                CommitSelection();
                ke.Handled = true;
            }
            else if (ke.KeyCode == Keys.Escape)
            {
                lstSugerencias.Visible = false;
                currentTextBox?.Focus();
                ke.Handled = true;
            }
        }

        // ====== AUTOCOMPLETE PROGRAMA DESDE FICHA ======
        private string GetProgramaDeFicha(string idFicha)
        {
            const string sql = @"SELECT RTRIM(IdPrograma) 
                                 FROM Fichas 
                                 WHERE IdFicha = @f";
            try
            {
                EnsureOpen();
                using (var cmd = new SqlCommand(sql, ConDB))
                {
                    cmd.Parameters.AddWithValue("@f", idFicha);
                    var result = cmd.ExecuteScalar();
                    return result?.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el programa de la ficha: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void AutofillProgramaFromFicha()
        {
            var idFicha = txtFicha.Text.Trim();
            if (string.IsNullOrEmpty(idFicha)) return;

            var idPrograma = GetProgramaDeFicha(idFicha);
            if (!string.IsNullOrEmpty(idPrograma))
            {
                txtPrograma.Text = idPrograma;
                txtPrograma.SelectionStart = txtPrograma.TextLength;
            }
            else
            {
                // Si la ficha no existe o no tiene programa, no toco txtPrograma
                // (opcional) MessageBox.Show("No se encontró el programa de la ficha.");
            }
        }

        // ===== VALIDACIONES =====
        private bool ExisteEnBD(string sql, Action<SqlParameterCollection> addParams)
        {
            try
            {
                EnsureOpen();
                using (var cmd = new SqlCommand(sql, ConDB))
                {
                    addParams(cmd.Parameters);
                    object result = cmd.ExecuteScalar();
                    return result != null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al verificar en BD: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private bool FichaPerteneceAPrograma(string idFicha, string idPrograma)
        {
            const string sql = @"SELECT 1 FROM Fichas WHERE IdFicha = @f AND IdPrograma = @p";
            return ExisteEnBD(sql, p =>
            {
                p.AddWithValue("@f", idFicha);
                p.AddWithValue("@p", idPrograma);
            });
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Longitudes reales
            if (txtFicha.Text.Length > 15)
            {
                MessageBox.Show("La Ficha no puede tener más de 15 caracteres.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            if (txtPrograma.Text.Length > 15)
            {
                MessageBox.Show("El Programa no puede tener más de 15 caracteres.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            if (txtCompetencia.Text.Length > 50)
            {
                MessageBox.Show("La Competencia no puede tener más de 50 caracteres.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }

            if (!ExisteEnBD("SELECT 1 FROM Fichas WHERE IdFicha = @v", p => p.AddWithValue("@v", txtFicha.Text)))
            { MessageBox.Show("La ficha no existe en la base de datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            if (!ExisteEnBD("SELECT 1 FROM Programas WHERE IdPrograma = @v", p => p.AddWithValue("@v", txtPrograma.Text)))
            { MessageBox.Show("El programa no existe en la base de datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            if (!ExisteEnBD("SELECT 1 FROM Competencias WHERE IdCompetencia = @v", p => p.AddWithValue("@v", txtCompetencia.Text)))
            { MessageBox.Show("La competencia no existe en la base de datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            if (!FichaPerteneceAPrograma(txtFicha.Text, txtPrograma.Text))
            {
                MessageBox.Show("La Ficha no pertenece al Programa indicado.", "Inconsistencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Ficha = txtFicha.Text;
            Programa = txtPrograma.Text;
            Competencia = txtCompetencia.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void txtCompetencia_Validated(object sender, EventArgs e) { }
    }
}
