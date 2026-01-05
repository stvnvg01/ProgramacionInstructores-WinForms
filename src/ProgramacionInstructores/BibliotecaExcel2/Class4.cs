using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Threading.Tasks;

namespace BibliotecaExcel2
{
    public class Class4
    {
        public void GenerarExel(DataGridView dgvCronograma, string nombreInstructor, string IntitucionEdu, string mesSeleccionado)
        {
            try
            {
                Excel.Application excelApp = new Excel.Application();
                if (excelApp == null)
                {
                    MessageBox.Show("Excel no está instalado.");
                    return;
                }

                Excel.Workbook workbook = excelApp.Workbooks.Add();
                Excel.Worksheet sheet = workbook.Sheets[1];
                sheet.Name = "Cronograma";

                // TITULOS ENCABEZADO
                Excel.Range rangoInstitucion = sheet.Range["C3", "K3"];
                rangoInstitucion.Merge(); // Fusionar celdas
                rangoInstitucion.Value = "SEDE:";
                rangoInstitucion.Font.Bold = true;
                rangoInstitucion.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; // Centrado
                rangoInstitucion.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                Excel.Range rangoInstitucion1 = sheet.Range["L3", "Y3"];
                rangoInstitucion1.Merge(); // Fusionar celdas
                rangoInstitucion1.Value = "";
                rangoInstitucion1.Font.Bold = true;
                rangoInstitucion1.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; // Centrado
                rangoInstitucion1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                Excel.Range rangoInstitucion2 = sheet.Range["C5", "K5"];
                rangoInstitucion2.Merge(); // Fusionar celdas
                rangoInstitucion2.Value = "DOCENTE:";
                rangoInstitucion2.Font.Bold = true;
                rangoInstitucion2.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; // Centrado
                rangoInstitucion2.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                Excel.Range rangoInstitucion3 = sheet.Range["L5", "Y5"];
                rangoInstitucion3.Merge(); // Fusionar celdas
                rangoInstitucion3.Value = nombreInstructor;
                rangoInstitucion3.Font.Bold = true;
                rangoInstitucion3.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; // Centrado
                rangoInstitucion3.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                Excel.Range rangoInstitucion8 = sheet.Range["C7", "I7"];
                rangoInstitucion8.Merge(); // Fusionar celdas
                rangoInstitucion8.Value = "PERIODO:";
                rangoInstitucion8.Font.Bold = true;
                rangoInstitucion8.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; // Centrado
                rangoInstitucion8.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                Excel.Range rangoInstitucion9 = sheet.Range["J7", "M7"];
                rangoInstitucion9.Merge(); // Fusionar celdas
                rangoInstitucion9.Value = "01/12/2025";
                rangoInstitucion9.Font.Bold = true;
                rangoInstitucion9.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; // Centrado
                rangoInstitucion9.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                Excel.Range rangoInstitucion10 = sheet.Range["N7", "O7"];
                rangoInstitucion10.Merge(); // Fusionar celdas
                rangoInstitucion10.Value = "AL";
                rangoInstitucion10.Font.Bold = true;
                rangoInstitucion10.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; // Centrado

                Excel.Range rangoInstitucion11 = sheet.Range["P7", "S7"];
                rangoInstitucion11.Merge(); // Fusionar celdas
                rangoInstitucion11.Value = "31/12/2025";
                rangoInstitucion11.Font.Bold = true;
                rangoInstitucion11.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; // Centrado
                rangoInstitucion11.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                Excel.Range rangoInstitucion12 = sheet.Range["T7", "U7"];
                rangoInstitucion12.Merge(); // Fusionar celdas
                rangoInstitucion12.Value = "AÑO:";
                rangoInstitucion12.Font.Bold = true;
                rangoInstitucion12.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; // Centrado

                Excel.Range rangoInstitucion13 = sheet.Range["V7", "Y7"];
                rangoInstitucion13.Merge(); // Fusionar celdas
                rangoInstitucion13.Value = "2025";
                rangoInstitucion13.Font.Bold = true;
                rangoInstitucion13.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; // Centrado
                rangoInstitucion13.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                // TÍTULO PRINCIPAL
                Excel.Range titleRange = sheet.Range["D1", "AJ1"];
                titleRange.Merge();
                titleRange.Value = "CRONOGRAMA";
                titleRange.Font.Size = 18;
                titleRange.Font.Bold = true;
                titleRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                titleRange.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // CABECERA DE TABLA
                Excel.Range rangoInstitucion14 = sheet.Range["B9", "B12"];
                rangoInstitucion14.Merge(); // Fusionar celdas
                rangoInstitucion14.Value = "Horas";
                rangoInstitucion14.Font.Bold = true;
                rangoInstitucion14.Font.Size = 20;
                rangoInstitucion14.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                rangoInstitucion14.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                rangoInstitucion14.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                string[] horas = {
                      "06:00 am - 10:00 am", "10:00 am - 02:00 pm", "02:00 pm - 06:00 pm", "06:00 pm - 10:00 pm",
                };

                for (int i = 0; i < horas.Length; i++)
                {
                    sheet.Cells[13 + i, 2] = horas[i];
                }

                // Crear la validación de datos con los meses
                string[] meses = {
                    "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                    "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
                };

                // Convertir array a una cadena separada por comas
                string listaMeses = string.Join(",", meses);

                //// Definir la celda de destino (por ejemplo AG14)
                Excel.Range celdaLista = sheet.get_Range("c9", "AN9");
                Excel.Range celdaMesActual = sheet.get_Range("C9", "AN9");
                celdaLista.Merge();
                //string mesActual = DateTime.Now.ToString("MMMM").ToUpper(); // Ej: MAYO
                // Mostrar el mes actual
                celdaMesActual.Value = mesSeleccionado.ToUpper(); // <-- USA EL PARÁMETRO mesSeleccionado AQUÍ
                celdaLista.Font.Size = 16;
                celdaLista.Font.Bold = true;
                celdaLista.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                celdaLista.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                celdaLista.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // Crear validación de lista
                Excel.Validation validacion = celdaLista.Validation;
                validacion.Delete(); // Eliminar cualquier validación previa
                validacion.Add(
                    Excel.XlDVType.xlValidateList,
                    Excel.XlDVAlertStyle.xlValidAlertStop,
                    Excel.XlFormatConditionOperator.xlBetween,
                    listaMeses,
                    Type.Missing
                );
                validacion.IgnoreBlank = true;
                validacion.InCellDropdown = true;
                validacion.InputTitle = "Seleccionar mes";
                validacion.ErrorTitle = "Mes inválido";
                validacion.ErrorMessage = "Por favor, selecciona un mes válido de la lista.";

                Excel.Range SEMANA1 = sheet.Range["C10", "I10"];
                SEMANA1.Merge(); // Fusionar celdas
                SEMANA1.Value = "SEMANA-1";
                SEMANA1.Font.Bold = true;
                SEMANA1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                SEMANA1.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                SEMANA1.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                Excel.Range SEMANA2 = sheet.Range["J10", "P10"];
                SEMANA2.Merge(); // Fusionar celdas
                SEMANA2.Value = "SEMANA-2";
                SEMANA2.Font.Bold = true;
                SEMANA2.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                SEMANA2.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                SEMANA2.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                Excel.Range SEMANA3 = sheet.Range["Q10", "W10"];
                SEMANA3.Merge(); // Fusionar celdas
                SEMANA3.Value = "SEMANA-3";
                SEMANA3.Font.Bold = true;
                SEMANA3.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                SEMANA3.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                SEMANA3.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                Excel.Range SEMANA4 = sheet.Range["X10", "AD10"];
                SEMANA4.Merge(); // Fusionar celdas
                SEMANA4.Value = "SEMANA-4";
                SEMANA4.Font.Bold = true;
                SEMANA4.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                SEMANA4.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                SEMANA4.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                Excel.Range SEMANA5 = sheet.Range["AE10", "AK10"];
                SEMANA5.Merge(); // Fusionar celdas
                SEMANA5.Value = "SEMANA-5";
                SEMANA5.Font.Size = 10;
                SEMANA5.RowHeight = 33;  // Aumentar alto
                SEMANA5.Font.Bold = true;
                SEMANA5.WrapText = true; // Ajustar texto en la celda
                SEMANA5.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                SEMANA5.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                SEMANA5.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                Excel.Range SEMANA6 = sheet.Range["AL10", "AN10"];
                SEMANA6.Merge(); // Fusionar celdas
                SEMANA6.Value = "SEMANA-6";
                SEMANA6.Font.Size = 10;
                SEMANA6.RowHeight = 33;  // Aumentar alto
                SEMANA6.Font.Bold = true;
                SEMANA6.WrapText = true; // Ajustar texto en la celda
                SEMANA6.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                SEMANA6.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                SEMANA6.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                sheet.Cells[17, 2] = "HORAS DIARIAS:";
                sheet.Cells[18, 2] = "HORAS SEMANALES:";
                sheet.Cells[19, 2] = "HORAS MENSUALES:";

                // ENCABEZADO DE DÍAS (SEMANA 1)
                string[] dias = { "=J7" };
                for (int i = 0; i < dias.Length; i++)
                {
                    var celda = sheet.Cells[12, 3 + i];
                    celda.Value = dias[i];
                    celda.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    celda.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                }

                // Estética: bordes
                Excel.Range fullTable = sheet.Range["B9", "AN" + (14 + horas.Length)];
                fullTable.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;


                // Autoajustar columnas
                sheet.Columns.AutoFit();
                sheet.Columns["C"].ColumnWidth = 3;
                sheet.Columns["D"].ColumnWidth = 3;
                sheet.Columns["E"].ColumnWidth = 3;
                sheet.Columns["F"].ColumnWidth = 3;
                sheet.Columns["G"].ColumnWidth = 3;
                sheet.Columns["H"].ColumnWidth = 3;
                sheet.Columns["I"].ColumnWidth = 3;
                sheet.Columns["J"].ColumnWidth = 3;
                sheet.Columns["K"].ColumnWidth = 3;
                sheet.Columns["L"].ColumnWidth = 3;
                sheet.Columns["M"].ColumnWidth = 3;
                sheet.Columns["N"].ColumnWidth = 3;
                sheet.Columns["O"].ColumnWidth = 3;
                sheet.Columns["P"].ColumnWidth = 3;
                sheet.Columns["Q"].ColumnWidth = 3;
                sheet.Columns["R"].ColumnWidth = 3;
                sheet.Columns["S"].ColumnWidth = 3;
                sheet.Columns["T"].ColumnWidth = 3;
                sheet.Columns["U"].ColumnWidth = 3;
                sheet.Columns["V"].ColumnWidth = 3;
                sheet.Columns["W"].ColumnWidth = 3;
                sheet.Columns["X"].ColumnWidth = 3;
                sheet.Columns["Y"].ColumnWidth = 3;
                sheet.Columns["Z"].ColumnWidth = 3;
                sheet.Columns["AA"].ColumnWidth = 3;
                sheet.Columns["AB"].ColumnWidth = 3;
                sheet.Columns["AC"].ColumnWidth = 3;
                sheet.Columns["AD"].ColumnWidth = 3;
                sheet.Columns["AE"].ColumnWidth = 3;
                sheet.Columns["AF"].ColumnWidth = 3;
                sheet.Columns["AG"].ColumnWidth = 3;
                sheet.Columns["AH"].ColumnWidth = 3;
                sheet.Columns["AI"].ColumnWidth = 3;
                sheet.Columns["AJ"].ColumnWidth = 3;
                sheet.Columns["AK"].ColumnWidth = 3;
                sheet.Columns["AL"].ColumnWidth = 3;
                sheet.Columns["AM"].ColumnWidth = 3;
                sheet.Columns["AN"].ColumnWidth = 3;

                sheet.Range["J7"].FormulaLocal =
                    @"=FECHANUMERO(""1 ""&$C$9&"" ""&AÑO(HOY()))-(DIASEM(FECHANUMERO(""1 ""&$C$9&"" ""&AÑO(HOY()));2)-1)";

                sheet.Range["P7"].FormulaLocal =
                    @"=FIN.MES(FECHANUMERO(""1 ""&$C$9&"" ""&AÑO(HOY()));0)+(7-DIASEM(FIN.MES(FECHANUMERO(""1 ""&$C$9&"" ""&AÑO(HOY()));0);2))+3";

                sheet.Range["P7"].NumberFormat = "dd/mm/yyyy";
                sheet.Range["V7"].FormulaLocal = "=AÑO(HOY())";

                Excel.Range rangoInstitucion15 = sheet.Range["C11", "AN11"];
                rangoInstitucion15.FormulaLocal = "=TEXTO(C12; \"ddd\")";
                rangoInstitucion15.Font.Bold = true;
                rangoInstitucion15.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                rangoInstitucion15.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                string columnaAnterior = "C";

                for (int i = 0; i < 38; i++) // D10 a AG10
                {
                    int colNum = 3 + i; // desde D (col 4)
                    string colActual = ObtenerNombreColumna(colNum);

                    // Fórmula: suma 1 al valor anterior si es menor a P6
                    string formula = $"=SI({columnaAnterior}12<$P$7;{columnaAnterior}12+1;\"\")";

                    Excel.Range celda = sheet.Range[$"{colActual}12"];
                    celda.FormulaLocal = formula;

                    // Formato personalizado: solo mostrar el día
                    celda.NumberFormat = "d";

                    // Estética
                    celda.Font.Bold = true;
                    celda.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    celda.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    columnaAnterior = colActual;
                }
                sheet.Range["C12"].FormulaLocal = "=J7";

                for (int i = 0; i < 38; i++) // C a AG son 31 columnas
                {
                    int colNum = 3 + i; // C = 3
                    string colLetra = ObtenerNombreColumna(colNum);

                    string formula = $"=SUMA({colLetra}13:{colLetra}16)";
                    sheet.Range[$"{colLetra}17"].FormulaLocal = formula;

                    // Opcional: formato
                    sheet.Range[$"{colLetra}17"].Font.Bold = true;
                    sheet.Range[$"{colLetra}17"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                }
                //J28-P28, Q28-W28, X28-AD28, AE20-AG28
                Excel.Range SUMSEM1 = sheet.Range["C18", "I18"];
                SUMSEM1.Merge(); // Fusionar celdas
                SUMSEM1.FormulaLocal = "=SUMA(C17:I17)";
                SUMSEM1.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                Excel.Range SUMSEM2 = sheet.Range["J18", "P18"];
                SUMSEM2.Merge(); // Fusionar celdas
                SUMSEM2.FormulaLocal = "=SUMA(J17:P17)";
                SUMSEM2.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                Excel.Range SUMSEM3 = sheet.Range["Q18", "W18"];
                SUMSEM3.Merge(); // Fusionar celdas
                SUMSEM3.FormulaLocal = "=SUMA(Q17:W17)";
                SUMSEM3.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                Excel.Range SUMSEM4 = sheet.Range["X18", "AD18"];
                SUMSEM4.Merge(); // Fusionar celdas
                SUMSEM4.FormulaLocal = "=SUMA(X17:AD17)";
                SUMSEM4.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                Excel.Range SUMSEM5 = sheet.Range["AE18", "AK18"];
                SUMSEM5.Merge(); // Fusionar celdas
                SUMSEM5.FormulaLocal = "=SUMA(AE17:AK17)";
                SUMSEM5.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                Excel.Range SUMSEM6 = sheet.Range["AL18", "AN18"];
                SUMSEM6.Merge(); // Fusionar celdas
                SUMSEM6.FormulaLocal = "=SUMA(AL17:AN17)";
                SUMSEM6.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                Excel.Range SUMMES1 = sheet.Range["C19", "AN19"];
                SUMMES1.Merge(); // Fusionar celdas
                SUMMES1.FormulaLocal = "=SUMA(C17:AN17)";
                SUMMES1.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // Crear la tabla de horas y días
                for (int i = 0; i < dgvCronograma.Rows.Count; i++)
                {
                    for (int j = 1; j < dgvCronograma.Columns.Count; j++)  // Saltamos la columna de horas
                    {
                        var celdaDataGrid = dgvCronograma.Rows[i].Cells[j];
                        Excel.Range excelCelda = sheet.Cells[i + 13, j + 2]; // Ajusta el índice según sea necesario

                        // Copiar valor (si la celda está marcada, agregar "1")
                        excelCelda.Value = (celdaDataGrid.Style.BackColor != Color.White && celdaDataGrid.Style.BackColor != Color.LightGray) ? "4" : "";


                        // Copiar color de fondo
                        excelCelda.Interior.Color = System.Drawing.ColorTranslator.ToOle(celdaDataGrid.Style.BackColor);
                    }
                }

                // Crear un array con los rangos que necesitan bordes
                string[] rangos = {
                    "C10:I12", "C13:I16", "C18:I18", "B9:B16",
                    "C11:AN12", "C10:AN10", "J10:P12", "Q10:W12",
                    "X10:AD12", "AE10:AN12", "J13:P16", "Q13:W16",
                    "X13:AD16", "AE13:AN16", "J17:P17", "Q17:W17",
                    "X17:AD17", "AE17:AK17", "C18:P18", "J18:W18",
                    "Q18:AD18", "X18:AN18", "B18:B19", "B17:B18",
                    "B18:B19", "B19:AN19", "B9:AN12", "AL10:AN16",
                    "AL18:AN18","AL17:AN17",
                };

                // Aplicar bordes gruesos a todos los rangos
                foreach (var rango in rangos)
                {
                    Excel.Range celdas = sheet.Range[rango];
                    celdas.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);
                }

                // Guardar archivo
                string ruta = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Cronograma.xlsx";
                workbook.SaveAs(ruta);
                workbook.Close(false);
                excelApp.Quit();

                MessageBox.Show("Archivo generado en el escritorio.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private string ObtenerNombreColumna(int numeroColumna)
        {
            string nombre = "";
            while (numeroColumna > 0)
            {
                int mod = (numeroColumna - 1) % 26;
                nombre = Convert.ToChar(65 + mod) + nombre;
                numeroColumna = (numeroColumna - mod) / 26;
            }
            return nombre;
        }

        private string GetExcelColumnName(int columnNumber)
        {
            string columnName = "";
            while (columnNumber > 0)
            {
                int modulo = (columnNumber - 1) % 26;
                columnName = Convert.ToChar(65 + modulo) + columnName;
                columnNumber = (columnNumber - modulo) / 26;
            }
            return columnName;
        }
    }
}

