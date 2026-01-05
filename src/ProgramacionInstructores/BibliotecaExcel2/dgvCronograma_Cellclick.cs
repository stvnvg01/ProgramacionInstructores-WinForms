using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using Excel = Microsoft.Office.Interop.Excel;

namespace BibliotecaExcel2
{
    public class dgvCronograma_Cellclick
    {
        public class dgvCronograma_Cellclic
        {
            private Dictionary<string, bool[,]> cronogramasGuardados = new Dictionary<string, bool[,]>();

            public void CellClick(
                DataGridView dgvCronograma,
                DateTime fechaSeleccionada,
                int mesSeleccionado,  // ✅ Usa este parámetro en lugar de comboBox3.SelectedIndex
                bool incluirSabados,
                bool incluirDomingos,
                string idTexto,
                DataGridViewCellEventArgs e)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex > 0)
                {
                    var celda = dgvCronograma.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    string headerText = dgvCronograma.Columns[e.ColumnIndex].HeaderText;
                    string[] partes = headerText.Split('\n');

                    if (!int.TryParse(partes[1], out int dia))
                        return;

                    DateTime fechaBase = new DateTime(fechaSeleccionada.Year, mesSeleccionado, 1);
                    DateTime primerDia = fechaBase.AddDays(-(((int)fechaBase.DayOfWeek + 6) % 7));
                    DateTime fechaCelda = primerDia.AddDays(e.ColumnIndex - 1);

                    bool esSabado = fechaCelda.DayOfWeek == DayOfWeek.Saturday;
                    bool esDomingo = fechaCelda.DayOfWeek == DayOfWeek.Sunday;

                    if (fechaCelda.Month != mesSeleccionado ||
                        (esSabado && !incluirSabados) ||
                        (esDomingo && !incluirDomingos))
                    {
                        return;
                    }

                    string clave = $"{idTexto}_{fechaSeleccionada:yyyy}";
                    if (!cronogramasGuardados.ContainsKey(clave))
                    {
                        int dias = DateTime.DaysInMonth(fechaSeleccionada.Year, mesSeleccionado);
                        cronogramasGuardados[clave] = new bool[dgvCronograma.Rows.Count, dias];
                    }

                    // Calcular índice real del día dentro del mes
                    int diaIndex = 0;
                    for (int col = 1; col <= e.ColumnIndex; col++)
                    {
                        string h = dgvCronograma.Columns[col].HeaderText;
                        string[] p = h.Split('\n');
                        int d;
                        if (p.Length < 2 || !int.TryParse(p[1], out d)) continue;

                        DateTime f = primerDia.AddDays(col - 1);
                        if (f.Month == mesSeleccionado)  // ✅ Usa mesSeleccionado en lugar de comboBox3
                        {
                            if (col == e.ColumnIndex)
                                break;
                            diaIndex++;
                        }
                    }
                }
            }
        }
    }
}
