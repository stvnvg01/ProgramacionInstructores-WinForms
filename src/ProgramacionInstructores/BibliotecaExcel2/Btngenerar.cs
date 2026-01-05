using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BibliotecaExcel2
{
    public class GeneradorCronograma
    {
        // Eventos para manejar los CheckedChanged
        public event EventHandler CheckBox1CheckedChanged;
        public event EventHandler CheckBox2CheckedChanged;

        // Metodo principal para generar el cronograma
        public void GenerarCronograma(
            DataGridView dgvCronograma,
            CheckBox checkBox1,
            CheckBox checkBox2,
            TextBox IdPersona,
            DateTimePicker dateTimePicker1,
            ComboBox comboBox3,
            ComboBox comboBox2,
            CheckBox chk1Hora,
            CheckBox chk2Horas,
            CheckBox chk4Horas,
            Dictionary<string, bool[,]> cronogramasGuardados,
            Action<DayOfWeek, bool> aplicarColorSabadoODomingo,
            Action ajustarAlturaDataGridView)
        {
            // Reiniciar checkboxes de sabado y domingo
            ManejarCheckBoxes(checkBox1, checkBox2, false);

            // Reaplicar logica visual
            aplicarColorSabadoODomingo(DayOfWeek.Saturday, false);
            aplicarColorSabadoODomingo(DayOfWeek.Sunday, false);

            // Validación de ID
            if (string.IsNullOrWhiteSpace(IdPersona.Text))
            {
                MessageBox.Show("Ingrese un ID");
                return;
            }

            // Configuración inicial
            int año = dateTimePicker1.Value.Year;
            int mes = comboBox3.SelectedIndex + 1;
            int diasMes = DateTime.DaysInMonth(año, mes);

            // Limpiar DataGridView
            dgvCronograma.Columns.Clear();
            dgvCronograma.Rows.Clear();

            // Configurar columnas (días del mes)
            ConfigurarColumnas(dgvCronograma, año, mes);

            // Configurar filas (horas del día)
            ConfigurarFilas(dgvCronograma, comboBox2);

            // Configuración visual del DataGridView
            ConfigurarAparienciaDataGridView(dgvCronograma);

            // Cargar o inicializar estado del cronograma
            CargarEstadoCronograma(dgvCronograma, IdPersona, dateTimePicker1, cronogramasGuardados, año, mes, diasMes);

            // Habilitar controles
            checkBox1.Enabled = true;
            checkBox2.Enabled = true;
            chk1Hora.Enabled = true;
            chk2Horas.Enabled = true;
            chk4Horas.Enabled = true;

            // Asegurar que al menos un checkbox de horas este seleccionado
            if (!chk1Hora.Checked && !chk2Horas.Checked && !chk4Horas.Checked)
            {
                chk1Hora.Checked = true;
            }

            // Ajustar altura del DataGridView
            ajustarAlturaDataGridView();
        }

        private void ManejarCheckBoxes(CheckBox checkBox1, CheckBox checkBox2, bool estado)
        {
            checkBox1.CheckedChanged -= CheckBox1CheckedChanged;
            checkBox2.CheckedChanged -= CheckBox2CheckedChanged;

            checkBox1.Checked = estado;
            checkBox2.Checked = estado;

            checkBox1.CheckedChanged += CheckBox1CheckedChanged;
            checkBox2.CheckedChanged += CheckBox2CheckedChanged;
        }

        private void ConfigurarFilas(DataGridView dgv, ComboBox comboBoxIntervalo)
        {
            int intervalo = 1; // Siempre 1 hora
            List<string> horas = GenerarIntervalosHorarios(intervalo);
            foreach (var hora in horas)
            {
                dgv.Rows.Add(hora);
            }
        }

        private List<string> GenerarIntervalosHorarios(int intervalo)
        {
            List<string> horas = new List<string>();
            DateTime horaInicio = DateTime.Parse("06:00");
            DateTime horaFin = DateTime.Parse("22:00");

            while (horaInicio < horaFin)
            {
                DateTime horaFinBloque = horaInicio.AddHours(intervalo);
                if (horaFinBloque > horaFin) break;

                horas.Add($"{horaInicio:hh\\:mm} - {horaFinBloque:hh\\:mm}");
                horaInicio = horaFinBloque;
            }

            return horas;
        }

        private void ConfigurarAparienciaDataGridView(DataGridView dgv)
        {
            dgv.Columns[0].Width = 72;
            for (int i = 1; i < dgv.Columns.Count; i++)
            {
                dgv.Columns[i].Width = 23;
            }

            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgv.Columns[0].ReadOnly = true;
        }
        
        private void CargarEstadoCronograma(
    DataGridView dgv,
    TextBox txtId,
    DateTimePicker dateTimePicker,
    Dictionary<string, bool[,]> cronogramasGuardados,
    int año,
    int mes,
    int diasMes)
        {
            string clave = $"{txtId.Text}_{dateTimePicker.Value:yyyyMM}";
            bool[,] estado = cronogramasGuardados.ContainsKey(clave)
                ? cronogramasGuardados[clave]
                : new bool[16, diasMes];

            if (!cronogramasGuardados.ContainsKey(clave))
            {
                cronogramasGuardados[clave] = estado;
            }

            Color[] coloresPorHora = GenerarColoresPorHora();

            for (int fila = 0; fila < dgv.Rows.Count; fila++)
            {
                int estadoColIndex = 0;

                for (int col = 1; col < dgv.Columns.Count; col++)
                {
                    var celda = dgv.Rows[fila].Cells[col];
                    string[] partes = dgv.Columns[col].HeaderText.Split('\n');

                    if (partes.Length >= 2 && int.TryParse(partes[1], out int dia))
                    {
                        // Detectar si es del mes anterior o siguiente usando el marcador invisible
                        bool esMesAnteriorOSiguiente = partes.Length == 3 && partes[2].Contains("\u200B");

                        // Determinar el mes y año correctos
                        int mesCol = mes;
                        int añoCol = año;

                        if (esMesAnteriorOSiguiente)
                        {
                            // Verificar si es mes anterior o siguiente basándose en el día
                            if (dia > 15) // Probablemente es del mes anterior
                            {
                                mesCol = mes == 1 ? 12 : mes - 1;
                                añoCol = mes == 1 ? año - 1 : año;
                            }
                            else // Probablemente es del mes siguiente
                            {
                                mesCol = mes == 12 ? 1 : mes + 1;
                                añoCol = mes == 12 ? año + 1 : año;
                            }
                        }

                        // Crear la fecha real
                        DateTime fechaCol;
                        try
                        {
                            fechaCol = new DateTime(añoCol, mesCol, dia);
                        }
                        catch
                        {
                            // Si no se puede crear la fecha, marcar como deshabilitada
                            celda.Style.BackColor = Color.LightGray;
                            celda.ReadOnly = true;
                            continue;
                        }

                        // Verificar si es fin de semana basándose en la letra del día
                        string letraDia = partes[0].ToUpper();
                        bool esFinDeSemana = letraDia == "S" || letraDia == "D";

                        // Si es del mes actual Y no es fin de semana
                        if (fechaCol.Month == mes && fechaCol.Year == año && !esFinDeSemana)
                        {
                            // Celda habilitada - puede ser marcada/desmarcada
                            bool marcado = fila < estado.GetLength(0) && estadoColIndex < estado.GetLength(1)
                                ? estado[fila, estadoColIndex]
                                : false;

                            celda.Style.BackColor = marcado ? Color.LightGreen : Color.White;
                            celda.ReadOnly = false;
                            estadoColIndex++;
                        }
                        else
                        {
                            // Celda deshabilitada - mes anterior/siguiente o fin de semana
                            celda.Style.BackColor = Color.LightGray;
                            celda.ReadOnly = true;
                            celda.Style.ForeColor = Color.DarkGray;
                        }
                    }
                    else
                    {
                        // Si no se puede parsear el día, deshabilitar la celda
                        celda.Style.BackColor = Color.LightGray;
                        celda.ReadOnly = true;
                        celda.Style.ForeColor = Color.DarkGray;
                    }
                }

                // Aplicar color de fondo para las filas (horas)
                dgv.Rows[fila].DefaultCellStyle.BackColor = coloresPorHora[fila % coloresPorHora.Length];
            }
        }

        // Método auxiliar mejorado para verificar si es fin de semana
        private bool EsFinDeSemana(DateTime fecha)
        {
            return fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday;
        }

        // Método para actualizar el estado de las columnas de fin de semana
        public void ActualizarEstadoFinDeSemana(DataGridView dgv, CheckBox checkBoxSabado, CheckBox checkBoxDomingo)
        {
            for (int fila = 0; fila < dgv.Rows.Count; fila++)
            {
                for (int col = 1; col < dgv.Columns.Count; col++)
                {
                    var celda = dgv.Rows[fila].Cells[col];
                    string[] partes = dgv.Columns[col].HeaderText.Split('\n');

                    if (partes.Length >= 1)
                    {
                        string letraDia = partes[0].ToUpper();

                        // Si es sábado
                        if (letraDia == "S")
                        {
                            if (checkBoxSabado.Checked)
                            {
                                celda.Style.BackColor = Color.White;
                                celda.ReadOnly = false;
                            }
                            else
                            {
                                celda.Style.BackColor = Color.LightGray;
                                celda.ReadOnly = true;
                                celda.Style.ForeColor = Color.DarkGray;
                            }
                        }
                        // Si es domingo
                        else if (letraDia == "D")
                        {
                            if (checkBoxDomingo.Checked)
                            {
                                celda.Style.BackColor = Color.White;
                                celda.ReadOnly = false;
                            }
                            else
                            {
                                celda.Style.BackColor = Color.LightGray;
                                celda.ReadOnly = true;
                                celda.Style.ForeColor = Color.DarkGray;
                            }
                        }
                    }
                }
            }
        }


        //actualizar el método ConfigurarColumnas para mejor detección
        private void ConfigurarColumnas(DataGridView dgv, int año, int mes)
        {
            dgv.Columns.Add("Horas", "Horas");

            DateTime primerDiaMes = new DateTime(año, mes, 1);
            int offset = ((int)primerDiaMes.DayOfWeek + 6) % 7; // Lunes=0
            DateTime diaInicio = primerDiaMes.AddDays(-offset);

            string[] diasSemana = { "L", "M", "X", "J", "V", "S", "D" };

            for (int i = 0; i < 38; i++)
            {
                DateTime fecha = diaInicio.AddDays(i);
                int diaSemana = ((int)fecha.DayOfWeek + 6) % 7;

                string nombre;
                string marcaInvisible = "\u200B"; // Carácter invisible

                if (fecha.Month != mes) // Mes anterior o siguiente
                {
                    nombre = $"{diasSemana[diaSemana]}\n{fecha.Day}\n{marcaInvisible}";
                }
                else // Mes actual
                {
                    nombre = $"{diasSemana[diaSemana]}\n{fecha.Day}";
                }

                dgv.Columns.Add($"D{i}", nombre);
            }
        }
        private bool EsDiaDelMes(string headerText, int año, int mes, out DateTime fecha)
        {
            fecha = DateTime.MinValue;
            string[] partes = headerText.Split('\n');

            if (partes.Length < 2 || !int.TryParse(partes[1], out int dia))
                return false;

            // Verificar que el día existe en el mes
            if (dia < 1 || dia > DateTime.DaysInMonth(año, mes))
                return false;

            try
            {
                fecha = new DateTime(año, mes, dia);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private Color[] GenerarColoresPorHora()
        {
            return new Color[]
            {
                Color.White, Color.White, Color.White, Color.White,
                Color.White, Color.White, Color.White, Color.White,
                Color.White, Color.White, Color.White, Color.White,
                Color.White, Color.White, Color.White, Color.White
            };
        }
    }
}
