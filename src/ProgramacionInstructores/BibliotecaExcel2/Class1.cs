using ProgramacionInstructores;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace BibliotecaExcel2
{
    public class Class1
    {
        // === Conexión compartida del proyecto ===
        private SqlConnection ConDB => Conexion.ConDB;

        // === Colores (pueden ser inyectados desde el formulario) ===
        private Dictionary<string, Color> coloresAsignados = new Dictionary<string, Color>();

        private readonly List<Color> paletaColores = new List<Color>
        {
            Color.LightBlue, Color.LightGreen, Color.LightCoral, Color.LightGoldenrodYellow,
            Color.LightPink, Color.LightSalmon, Color.LightSeaGreen, Color.LightSkyBlue,
            Color.LightSteelBlue, Color.LightCyan, Color.Plum, Color.Khaki
        };

        // ------------------------------------------------------------
        //  GENERAR A ESCRITORIO (original)
        // ------------------------------------------------------------
        public void GenerarExel(
            DataGridView dgvCronograma,
            string nombreInstructor,
            string institucionEdu,
            string mesSeleccionado,
            string IdInstructor,
            int mesNumero,
            int anio)
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
                sheet.Name = "Asistencia";

                // =======================
                // ENCABEZADOS/TÍTULOS
                // =======================
                Excel.Range rangoInstitucion1 = sheet.Range["L3", "Y3"];
                rangoInstitucion1.Merge();
                rangoInstitucion1.Value = "";
                rangoInstitucion1.Font.Bold = true;
                rangoInstitucion1.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                rangoInstitucion1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                Excel.Range rangoInstitucion3 = sheet.Range["L5", "Y5"];
                rangoInstitucion3.Merge();
                rangoInstitucion3.Value = nombreInstructor;
                rangoInstitucion3.Font.Bold = true;
                rangoInstitucion3.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                rangoInstitucion3.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                Excel.Range rangoInstitucion8 = sheet.Range["C7", "I7"];
                rangoInstitucion8.Merge();
                rangoInstitucion8.Value = "PERIODO:";
                rangoInstitucion8.Font.Bold = true;
                rangoInstitucion8.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                rangoInstitucion8.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                Excel.Range rangoInstitucion9 = sheet.Range["J7", "M7"];
                rangoInstitucion9.Merge();
                rangoInstitucion9.Value = "01/12/2025";
                rangoInstitucion9.Font.Bold = true;
                rangoInstitucion9.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                rangoInstitucion9.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                Excel.Range rangoInstitucion10 = sheet.Range["N7", "O7"];
                rangoInstitucion10.Merge();
                rangoInstitucion10.Value = "AL";
                rangoInstitucion10.Font.Bold = true;
                rangoInstitucion10.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                Excel.Range rangoInstitucion11 = sheet.Range["P7", "S7"];
                rangoInstitucion11.Merge();
                rangoInstitucion11.Value = "31/12/2025";
                rangoInstitucion11.Font.Bold = true;
                rangoInstitucion11.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                rangoInstitucion11.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                Excel.Range rangoInstitucion12 = sheet.Range["T7", "U7"];
                rangoInstitucion12.Merge();
                rangoInstitucion12.Value = "AÑO:";
                rangoInstitucion12.Font.Bold = true;
                rangoInstitucion12.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                Excel.Range rangoInstitucion13 = sheet.Range["V7", "Y7"];
                rangoInstitucion13.Merge();
                rangoInstitucion13.Value = "2025";
                rangoInstitucion13.Font.Bold = true;
                rangoInstitucion13.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                rangoInstitucion13.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                Excel.Range titleRange = sheet.Range["D1", "AJ1"];
                titleRange.Merge();
                titleRange.Value = "CRONOGRAMA";
                titleRange.Font.Size = 18;
                titleRange.Font.Bold = true;
                titleRange.Interior.Color = ColorTranslator.ToOle(Color.White);
                titleRange.Font.Color = ColorTranslator.ToOle(Color.Red);
                titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // CABECERA HORAS
                Excel.Range rangoInstitucion14 = sheet.Range["B9", "B12"];
                rangoInstitucion14.Merge();
                rangoInstitucion14.Value = "Horas";
                rangoInstitucion14.Font.Bold = true;
                rangoInstitucion14.Font.Size = 20;
                rangoInstitucion14.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                rangoInstitucion14.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                rangoInstitucion14.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                string[] horas = {
                    "06:00 am - 07:00 am","07:00 am - 08:00 am","08:00 am - 09:00 am","09:00 am - 10:00 am",
                    "10:00 am - 11:00 am","11:00 am - 12:00 pm","12:00 pm - 01:00 pm","01:00 pm - 02:00 pm",
                    "02:00 pm - 03:00 pm","03:00 pm - 04:00 pm","04:00 pm - 05:00 pm","05:00 pm - 06:00 pm",
                    "06:00 pm - 07:00 pm","07:00 pm - 08:00 pm","08:00 pm - 09:00 pm","09:00 pm - 10:00 pm",
                };
                for (int i = 0; i < horas.Length; i++) sheet.Cells[13 + i, 2] = horas[i];

                // Mes en C9:AN9 con validación
                string[] meses = {
                    "Enero","Febrero","Marzo","Abril","Mayo","Junio",
                    "Julio","Agosto","Septiembre","Octubre","Noviembre","Diciembre"
                };
                string listaMeses = string.Join(",", meses);
                Excel.Range celdaLista = sheet.get_Range("C9", "AN9");
                Excel.Range celdaMesActual = sheet.get_Range("C9", "AN9");
                celdaLista.Merge();
                celdaMesActual.Value = mesSeleccionado.ToUpper();
                celdaLista.Font.Size = 16;
                celdaLista.Font.Bold = true;
                celdaLista.Interior.Color = ColorTranslator.ToOle(Color.Blue);
                celdaLista.Font.Color = ColorTranslator.ToOle(Color.White);
                celdaLista.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                Excel.Validation validacion = celdaLista.Validation;
                validacion.Delete();
                validacion.Add(Excel.XlDVType.xlValidateList, Excel.XlDVAlertStyle.xlValidAlertStop,
                               Excel.XlFormatConditionOperator.xlBetween, listaMeses, Type.Missing);
                validacion.IgnoreBlank = true; validacion.InCellDropdown = true;

                // Semanas
                Excel.Range SEMANA1 = sheet.Range["C10", "I10"]; SEMANA1.Merge(); SEMANA1.Value = "SEMANA-1"; SEMANA1.Font.Bold = true; SEMANA1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; SEMANA1.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; SEMANA1.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                Excel.Range SEMANA2 = sheet.Range["J10", "P10"]; SEMANA2.Merge(); SEMANA2.Value = "SEMANA-2"; SEMANA2.Font.Bold = true; SEMANA2.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; SEMANA2.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; SEMANA2.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                Excel.Range SEMANA3 = sheet.Range["Q10", "W10"]; SEMANA3.Merge(); SEMANA3.Value = "SEMANA-3"; SEMANA3.Font.Bold = true; SEMANA3.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; SEMANA3.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; SEMANA3.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                Excel.Range SEMANA4 = sheet.Range["X10", "AD10"]; SEMANA4.Merge(); SEMANA4.Value = "SEMANA-4"; SEMANA4.Font.Bold = true; SEMANA4.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; SEMANA4.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; SEMANA4.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                Excel.Range SEMANA5 = sheet.Range["AE10", "AK10"]; SEMANA5.Merge(); SEMANA5.Value = "SEMANA-5"; SEMANA5.Font.Size = 10; SEMANA5.RowHeight = 33; SEMANA5.Font.Bold = true; SEMANA5.WrapText = true; SEMANA5.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; SEMANA5.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; SEMANA5.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                Excel.Range SEMANA6 = sheet.Range["AL10", "AN10"]; SEMANA6.Merge(); SEMANA6.Value = "SEMANA-6"; SEMANA6.Font.Size = 10; SEMANA6.RowHeight = 33; SEMANA6.Font.Bold = true; SEMANA6.WrapText = true; SEMANA6.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; SEMANA6.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; SEMANA6.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                sheet.Cells[29, 2] = "HORAS DIARIAS:";
                sheet.Cells[30, 2] = "HORAS SEMANALES:";
                sheet.Cells[31, 2] = "HORAS MENSUALES:";

                // Encabezado días
                string[] dias = { "=J7" };
                for (int i = 0; i < dias.Length; i++)
                {
                    var celda = sheet.Cells[12, 3 + i];
                    celda.Value = dias[i];
                    celda.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    celda.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                }

                Excel.Range fullTable = sheet.Range["B9", "AN" + (14 + horas.Length)];
                fullTable.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                // Anchos
                sheet.Columns.AutoFit();
                string[] cols = { "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN" };
                foreach (var c in cols) sheet.Columns[c].ColumnWidth = 3;

                // Fechas
                sheet.Range["J7"].FormulaLocal = @"=FECHANUMERO(""1 ""&$C$9&"" ""&AÑO(HOY()))-(DIASEM(FECHANUMERO(""1 ""&$C$9&"" ""&AÑO(HOY()));2)-1)";
                sheet.Range["P7"].FormulaLocal = @"=FIN.MES(FECHANUMERO(""1 ""&$C$9&"" ""&AÑO(HOY()));0)+(7-DIASEM(FIN.MES(FECHANUMERO(""1 ""&$C$9&"" ""&AÑO(HOY()));0);2))+3";
                sheet.Range["P7"].NumberFormat = "dd/mm/yyyy";
                sheet.Range["V7"].FormulaLocal = "=AÑO(HOY())";

                Excel.Range rangoInstitucion15 = sheet.Range["C11", "AN11"];
                rangoInstitucion15.FormulaLocal = "=TEXTO(C12; \"ddd\")";
                rangoInstitucion15.Font.Bold = true;
                rangoInstitucion15.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                rangoInstitucion15.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                string columnaAnterior = "C";
                for (int i = 0; i < 38; i++)
                {
                    int colNum = 3 + i;
                    string colActual = ObtenerNombreColumna(colNum);
                    string formula = $"=SI({columnaAnterior}12<$P$7;{columnaAnterior}12+1;\"\")";
                    Excel.Range celda = sheet.Range[$"{colActual}12"];
                    celda.FormulaLocal = formula;
                    celda.NumberFormat = "d";
                    celda.Font.Bold = true;
                    celda.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    celda.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    columnaAnterior = colActual;
                }
                sheet.Range["C12"].FormulaLocal = "=J7";

                for (int i = 0; i < 38; i++)
                {
                    int colNum = 3 + i;
                    string colLetra = ObtenerNombreColumna(colNum);
                    string formula = $"=SUMA({colLetra}13:{colLetra}28)";
                    sheet.Range[$"{colLetra}29"].FormulaLocal = formula;
                    sheet.Range[$"{colLetra}29"].Font.Bold = true;
                    sheet.Range[$"{colLetra}29"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                }

                Excel.Range SUMSEM1 = sheet.Range["C30", "I30"]; SUMSEM1.Merge(); SUMSEM1.FormulaLocal = "=SUMA(C29:I29)"; SUMSEM1.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                Excel.Range SUMSEM2 = sheet.Range["J30", "P30"]; SUMSEM2.Merge(); SUMSEM2.FormulaLocal = "=SUMA(J29:P29)"; SUMSEM2.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                Excel.Range SUMSEM3 = sheet.Range["Q30", "W30"]; SUMSEM3.Merge(); SUMSEM3.FormulaLocal = "=SUMA(Q29:W29)"; SUMSEM3.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                Excel.Range SUMSEM4 = sheet.Range["X30", "AD30"]; SUMSEM4.Merge(); SUMSEM4.FormulaLocal = "=SUMA(X29:AD29)"; SUMSEM4.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                Excel.Range SUMSEM5 = sheet.Range["AE30", "AK30"]; SUMSEM5.Merge(); SUMSEM5.FormulaLocal = "=SUMA(AE29:AK29)"; SUMSEM5.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                Excel.Range SUMSEM6 = sheet.Range["AL30", "AN30"]; SUMSEM6.Merge(); SUMSEM6.FormulaLocal = "=SUMA(AL29:AN29)"; SUMSEM6.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                Excel.Range SUMMES1 = sheet.Range["C31", "AN31"]; SUMMES1.Merge(); SUMMES1.FormulaLocal = "=SUMA(C29:AN29)"; SUMMES1.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // Copia del DataGridView
                for (int i = 0; i < dgvCronograma.Rows.Count; i++)
                {
                    for (int j = 1; j < dgvCronograma.Columns.Count; j++)
                    {
                        var celdaDataGrid = dgvCronograma.Rows[i].Cells[j];
                        Excel.Range excelCelda = sheet.Cells[i + 13, j + 2];
                        excelCelda.Value = (celdaDataGrid.Style.BackColor != Color.White &&
                                            celdaDataGrid.Style.BackColor != Color.LightGray) ? "1" : "";
                        excelCelda.Interior.Color = ColorTranslator.ToOle(celdaDataGrid.Style.BackColor);
                    }
                }

                // =======================
                // TABLA DERECHA (AP:AS...)   *** USA IdCompetencia para la CLAVE ***
                // =======================
                SqlConnection con = ConDB;
                try
                {
                    if (con.State == System.Data.ConnectionState.Open) con.Close();
                    con.Open();

                    string query = @"
SELECT 
    f.IdFicha            AS FICHA,
    p.Nombre             AS PROGRAMA,
    cd.IdCompetencia     AS COMPETENCIA_ID,    -- *** ID para color ***
    c.Nombre             AS COMPETENCIA_NOM,   -- útil para mostrar
    SUM(cd.DuracionBloque) AS HORAS
FROM CronogramaDetalle cd
INNER JOIN Fichas f       ON cd.IdFicha = f.IdFicha
INNER JOIN Programas p    ON f.IdPrograma = p.IdPrograma
INNER JOIN Competencias c ON cd.IdCompetencia = c.IdCompetencia
WHERE cd.IdInstructor = @IdInstructor
  AND cd.Anio = @Anio
  AND cd.Mes  = @Mes
GROUP BY f.IdFicha, p.Nombre, cd.IdCompetencia, c.Nombre
ORDER BY f.IdFicha, p.Nombre, c.Nombre";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        string idTexto = IdInstructor.Split('-')[0].Trim();
                        int idInstructorInt = int.Parse(idTexto);
                        cmd.Parameters.AddWithValue("@IdInstructor", idInstructorInt);
                        cmd.Parameters.AddWithValue("@Anio", anio);
                        cmd.Parameters.AddWithValue("@Mes", mesNumero);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            int fila = 10; // AP10
                            sheet.Cells[9, 42] = "FICHA";
                            sheet.Cells[9, 43] = "PROGRAMA";
                            sheet.Cells[9, 44] = "COMPETENCIA";
                            sheet.Cells[9, 45] = "HORAS";

                            Excel.Range header = sheet.Range["AP9", "AS9"];
                            header.Font.Bold = true;
                            header.Interior.Color = ColorTranslator.ToOle(Color.LightGray);
                            header.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                            while (reader.Read())
                            {
                                string ficha = reader["FICHA"].ToString().Trim();
                                string programa = reader["PROGRAMA"].ToString().Trim();
                                string competenciaId = reader["COMPETENCIA_ID"].ToString().Trim();
                                string competenciaNm = reader["COMPETENCIA_NOM"].ToString().Trim();
                                string horasStr = reader["HORAS"] != DBNull.Value ? reader["HORAS"].ToString().Trim() : "";

                                sheet.Cells[fila, 42] = ficha;
                                sheet.Cells[fila, 43] = programa;
                                sheet.Cells[fila, 44] = string.IsNullOrEmpty(competenciaNm) ? competenciaId : competenciaNm;
                                sheet.Cells[fila, 45] = horasStr;

                                // *** Color por clave (Ficha + IdCompetencia) ***
                                Color colorAsignado = ObtenerColorPorFichaCompetencia(ficha, competenciaId);
                                Excel.Range colorCell = sheet.Cells[fila, 46];
                                colorCell.Interior.Color = ColorTranslator.ToOle(colorAsignado);
                                colorCell.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                                fila++;
                            }

                            Excel.Range dataRange = sheet.Range["AP9", $"AS{fila - 1}"];
                            dataRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                            dataRange.Columns.AutoFit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener datos: " + ex.Message);
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open) con.Close();
                }

                // Bordes gruesos de varios rangos (igual a tu implementación)
                string[] rangos = {
                    "C10:I12","C13:I28","C29:I29","B9:B28",
                    "C11:AN12","C10:AN10","J10:P12","Q10:W12",
                    "X10:AD12","AE10:AN12","J13:P28","Q13:W28",
                    "X13:AD28","AE13:AN28","J29:P29","Q29:W29",
                    "X29:AD29","AE29:AN29","C30:P30","J30:W30",
                    "Q30:AD30","X30:AN30","B29:B31","B29:B30",
                    "B30:B31","B31:AN31","B9:AN12","AL10:AN28",
                    "X29:AK29","X30:AK30"
                };
                foreach (var r in rangos)
                {
                    Excel.Range celdas = sheet.Range[r];
                    celdas.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);
                }

                string ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Cronograma.xlsx");
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

        // ------------------------------------------------------------
        //  DEVUELVE RUTA (usa el método anterior)
        // ------------------------------------------------------------
        public string GenerarExelRuta(
            DataGridView dgvCronograma,
            string nombreInstructor,
            string institucionEdu,
            string mesSeleccionado,
            string IdInstructor,
            int mesNumero,
            int anio)
        {
            GenerarExel(dgvCronograma, nombreInstructor, institucionEdu, mesSeleccionado, IdInstructor, mesNumero, anio);
            string ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Cronograma.xlsx");
            return ruta;
        }

        // ------------------------------------------------------------
        //  GENERAR A UNA RUTA ESPECÍFICA (con diccionario externo)
        // ------------------------------------------------------------
        public string GenerarExelAPath(
            DataGridView dgvCronograma,
            string nombreInstructor,
            string institucionEdu,
            string mesSeleccionado,
            string IdInstructor,
            int mesNumero,
            int anio,
            string outputPath,
            Dictionary<string, Color> externalColorMap = null)   // <= OPCIONAL
        {
            // si viene un mapa externo (del formulario), úsalo para asegurar mismos colores
            if (externalColorMap != null)
                coloresAsignados = externalColorMap;

            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet sheet = null;

            try
            {
                excelApp = new Excel.Application();
                if (excelApp == null) throw new Exception("Excel no está instalado.");
                excelApp.DisplayAlerts = false;

                workbook = excelApp.Workbooks.Add();
                sheet = (Excel.Worksheet)workbook.Sheets[1];
                sheet.Name = "Asistencia";

                // ====== (MISMO CONTENIDO QUE EN GenerarExel) ======
                // -- Encabezados / formato / fórmulas / copiado del grid --
                // (Para mantener fidelidad, se replica el mismo bloque de arriba)

                Excel.Range rangoInstitucion1 = sheet.Range["L3", "Y3"];
                rangoInstitucion1.Merge(); rangoInstitucion1.Value = "";
                rangoInstitucion1.Font.Bold = true; rangoInstitucion1.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                rangoInstitucion1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                Excel.Range rangoInstitucion3 = sheet.Range["L5", "Y5"];
                rangoInstitucion3.Merge(); rangoInstitucion3.Value = nombreInstructor;
                rangoInstitucion3.Font.Bold = true; rangoInstitucion3.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                rangoInstitucion3.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                Excel.Range rangoInstitucion8 = sheet.Range["C7", "I7"];
                rangoInstitucion8.Merge(); rangoInstitucion8.Value = "PERIODO:";
                rangoInstitucion8.Font.Bold = true; rangoInstitucion8.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                rangoInstitucion8.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                Excel.Range rangoInstitucion9 = sheet.Range["J7", "M7"];
                rangoInstitucion9.Merge(); rangoInstitucion9.Value = "01/12/2025";
                rangoInstitucion9.Font.Bold = true; rangoInstitucion9.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                rangoInstitucion9.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                Excel.Range rangoInstitucion10 = sheet.Range["N7", "O7"];
                rangoInstitucion10.Merge(); rangoInstitucion10.Value = "AL";
                rangoInstitucion10.Font.Bold = true; rangoInstitucion10.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                Excel.Range rangoInstitucion11 = sheet.Range["P7", "S7"];
                rangoInstitucion11.Merge(); rangoInstitucion11.Value = "31/12/2025";
                rangoInstitucion11.Font.Bold = true; rangoInstitucion11.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                rangoInstitucion11.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                Excel.Range rangoInstitucion12 = sheet.Range["T7", "U7"];
                rangoInstitucion12.Merge(); rangoInstitucion12.Value = "AÑO:";
                rangoInstitucion12.Font.Bold = true; rangoInstitucion12.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                Excel.Range rangoInstitucion13 = sheet.Range["V7", "Y7"];
                rangoInstitucion13.Merge(); rangoInstitucion13.Value = "2025";
                rangoInstitucion13.Font.Bold = true; rangoInstitucion13.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                rangoInstitucion13.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                Excel.Range titleRange = sheet.Range["D1", "AJ1"];
                titleRange.Merge(); titleRange.Value = "CRONOGRAMA";
                titleRange.Font.Size = 18; titleRange.Font.Bold = true;
                titleRange.Interior.Color = ColorTranslator.ToOle(Color.White);
                titleRange.Font.Color = ColorTranslator.ToOle(Color.Red);
                titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                Excel.Range rangoInstitucion14 = sheet.Range["B9", "B12"];
                rangoInstitucion14.Merge(); rangoInstitucion14.Value = "Horas";
                rangoInstitucion14.Font.Bold = true; rangoInstitucion14.Font.Size = 20;
                rangoInstitucion14.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                rangoInstitucion14.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                rangoInstitucion14.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                string[] horas = {
                    "06:00 am - 07:00 am","07:00 am - 08:00 am","08:00 am - 09:00 am","09:00 am - 10:00 am",
                    "10:00 am - 11:00 am","11:00 am - 12:00 pm","12:00 pm - 01:00 pm","01:00 pm - 02:00 pm",
                    "02:00 pm - 03:00 pm","03:00 pm - 04:00 pm","04:00 pm - 05:00 pm","05:00 pm - 06:00 pm",
                    "06:00 pm - 07:00 pm","07:00 pm - 08:00 pm","08:00 pm - 09:00 pm","09:00 pm - 10:00 pm",
                };
                for (int i = 0; i < horas.Length; i++) sheet.Cells[13 + i, 2] = horas[i];

                string[] meses = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
                string listaMeses = string.Join(",", meses);
                Excel.Range celdaLista = sheet.get_Range("C9", "AN9");
                Excel.Range celdaMesActual = sheet.get_Range("C9", "AN9");
                celdaLista.Merge();
                celdaMesActual.Value = mesSeleccionado.ToUpper();
                celdaLista.Font.Size = 16; celdaLista.Font.Bold = true;
                celdaLista.Interior.Color = ColorTranslator.ToOle(Color.Blue);
                celdaLista.Font.Color = ColorTranslator.ToOle(Color.White);
                celdaLista.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                Excel.Validation validacion = celdaLista.Validation;
                validacion.Delete();
                validacion.Add(Excel.XlDVType.xlValidateList, Excel.XlDVAlertStyle.xlValidAlertStop,
                               Excel.XlFormatConditionOperator.xlBetween, listaMeses, Type.Missing);
                validacion.IgnoreBlank = true; validacion.InCellDropdown = true;

                Excel.Range SEMANA1 = sheet.Range["C10", "I10"]; SEMANA1.Merge(); SEMANA1.Value = "SEMANA-1"; SEMANA1.Font.Bold = true; SEMANA1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; SEMANA1.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; SEMANA1.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                Excel.Range SEMANA2 = sheet.Range["J10", "P10"]; SEMANA2.Merge(); SEMANA2.Value = "SEMANA-2"; SEMANA2.Font.Bold = true; SEMANA2.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; SEMANA2.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; SEMANA2.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                Excel.Range SEMANA3 = sheet.Range["Q10", "W10"]; SEMANA3.Merge(); SEMANA3.Value = "SEMANA-3"; SEMANA3.Font.Bold = true; SEMANA3.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; SEMANA3.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; SEMANA3.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                Excel.Range SEMANA4 = sheet.Range["X10", "AD10"]; SEMANA4.Merge(); SEMANA4.Value = "SEMANA-4"; SEMANA4.Font.Bold = true; SEMANA4.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; SEMANA4.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; SEMANA4.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                Excel.Range SEMANA5 = sheet.Range["AE10", "AK10"]; SEMANA5.Merge(); SEMANA5.Value = "SEMANA-5"; SEMANA5.Font.Size = 10; SEMANA5.RowHeight = 33; SEMANA5.Font.Bold = true; SEMANA5.WrapText = true; SEMANA5.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; SEMANA5.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; SEMANA5.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                Excel.Range SEMANA6 = sheet.Range["AL10", "AN10"]; SEMANA6.Merge(); SEMANA6.Value = "SEMANA-6"; SEMANA6.Font.Size = 10; SEMANA6.RowHeight = 33; SEMANA6.Font.Bold = true; SEMANA6.WrapText = true; SEMANA6.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; SEMANA6.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; SEMANA6.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                sheet.Cells[29, 2] = "HORAS DIARIAS:";
                sheet.Cells[30, 2] = "HORAS SEMANALES:";
                sheet.Cells[31, 2] = "HORAS MENSUALES:";

                string[] dias = { "=J7" };
                for (int i = 0; i < dias.Length; i++)
                {
                    var celda = sheet.Cells[12, 3 + i];
                    celda.Value = dias[i];
                    celda.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    celda.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                }

                Excel.Range fullTable = sheet.Range["B9", "AN" + (14 + horas.Length)];
                fullTable.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                sheet.Columns.AutoFit();
                string[] cols = { "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN" };
                foreach (var c in cols) sheet.Columns[c].ColumnWidth = 3;

                sheet.Range["J7"].FormulaLocal = @"=FECHANUMERO(""1 ""&$C$9&"" ""&AÑO(HOY()))-(DIASEM(FECHANUMERO(""1 ""&$C$9&"" ""&AÑO(HOY()));2)-1)";
                sheet.Range["P7"].FormulaLocal = @"=FIN.MES(FECHANUMERO(""1 ""&$C$9&"" ""&AÑO(HOY()));0)+(7-DIASEM(FIN.MES(FECHANUMERO(""1 ""&$C$9&"" ""&AÑO(HOY()));0);2))+3";
                sheet.Range["P7"].NumberFormat = "dd/mm/yyyy";
                sheet.Range["V7"].FormulaLocal = "=AÑO(HOY())";

                Excel.Range rangoInstitucion15 = sheet.Range["C11", "AN11"];
                rangoInstitucion15.FormulaLocal = "=TEXTO(C12; \"ddd\")";
                rangoInstitucion15.Font.Bold = true;
                rangoInstitucion15.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                rangoInstitucion15.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                string columnaAnterior = "C";
                for (int i = 0; i < 38; i++)
                {
                    int colNum = 3 + i;
                    string colActual = ObtenerNombreColumna(colNum);
                    string formula = $"=SI({columnaAnterior}12<$P$7;{columnaAnterior}12+1;\"\")";
                    Excel.Range celda = sheet.Range[$"{colActual}12"];
                    celda.FormulaLocal = formula;
                    celda.NumberFormat = "d";
                    celda.Font.Bold = true;
                    celda.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    celda.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    columnaAnterior = colActual;
                }
                sheet.Range["C12"].FormulaLocal = "=J7";

                for (int i = 0; i < 38; i++)
                {
                    int colNum = 3 + i;
                    string colLetra = ObtenerNombreColumna(colNum);
                    string formula = $"=SUMA({colLetra}13:{colLetra}28)";
                    sheet.Range[$"{colLetra}29"].FormulaLocal = formula;
                    sheet.Range[$"{colLetra}29"].Font.Bold = true;
                    sheet.Range[$"{colLetra}29"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                }

                Excel.Range SUMSEM1 = sheet.Range["C30", "I30"]; SUMSEM1.Merge(); SUMSEM1.FormulaLocal = "=SUMA(C29:I29)"; SUMSEM1.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                Excel.Range SUMSEM2 = sheet.Range["J30", "P30"]; SUMSEM2.Merge(); SUMSEM2.FormulaLocal = "=SUMA(J29:P29)"; SUMSEM2.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                Excel.Range SUMSEM3 = sheet.Range["Q30", "W30"]; SUMSEM3.Merge(); SUMSEM3.FormulaLocal = "=SUMA(Q29:W29)"; SUMSEM3.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                Excel.Range SUMSEM4 = sheet.Range["X30", "AD30"]; SUMSEM4.Merge(); SUMSEM4.FormulaLocal = "=SUMA(X29:AD29)"; SUMSEM4.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                Excel.Range SUMSEM5 = sheet.Range["AE30", "AK30"]; SUMSEM5.Merge(); SUMSEM5.FormulaLocal = "=SUMA(AE29:AK29)"; SUMSEM5.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                Excel.Range SUMSEM6 = sheet.Range["AL30", "AN30"]; SUMSEM6.Merge(); SUMSEM6.FormulaLocal = "=SUMA(AL29:AN29)"; SUMSEM6.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                Excel.Range SUMMES1 = sheet.Range["C31", "AN31"]; SUMMES1.Merge(); SUMMES1.FormulaLocal = "=SUMA(C29:AN29)"; SUMMES1.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                for (int i = 0; i < dgvCronograma.Rows.Count; i++)
                {
                    for (int j = 1; j < dgvCronograma.Columns.Count; j++)
                    {
                        var celdaDataGrid = dgvCronograma.Rows[i].Cells[j];
                        Excel.Range excelCelda = sheet.Cells[i + 13, j + 2];
                        excelCelda.Value = (celdaDataGrid.Style.BackColor != Color.White &&
                                            celdaDataGrid.Style.BackColor != Color.LightGray) ? "1" : "";
                        excelCelda.Interior.Color = ColorTranslator.ToOle(celdaDataGrid.Style.BackColor);
                    }
                }

                // === BLOQUE DE TABLA DERECHA (mismo query con IdCompetencia) ===
                SqlConnection con = ConDB;
                try
                {
                    if (con.State == System.Data.ConnectionState.Open) con.Close();
                    con.Open();

                    string query = @"
SELECT 
    f.IdFicha            AS FICHA,
    p.Nombre             AS PROGRAMA,
    cd.IdCompetencia     AS COMPETENCIA_ID,    -- *** ID para color ***
    c.Nombre             AS COMPETENCIA_NOM,   -- útil para mostrar
    SUM(cd.DuracionBloque) AS HORAS
FROM CronogramaDetalle cd
INNER JOIN Fichas f       ON cd.IdFicha = f.IdFicha
INNER JOIN Programas p    ON f.IdPrograma = p.IdPrograma
INNER JOIN Competencias c ON cd.IdCompetencia = c.IdCompetencia
WHERE cd.IdInstructor = @IdInstructor
  AND cd.Anio = @Anio
  AND cd.Mes  = @Mes
GROUP BY f.IdFicha, p.Nombre, cd.IdCompetencia, c.Nombre
ORDER BY f.IdFicha, p.Nombre, c.Nombre";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        string idTexto = IdInstructor.Split('-')[0].Trim();
                        int idInstructorInt = int.Parse(idTexto);
                        cmd.Parameters.AddWithValue("@IdInstructor", idInstructorInt);
                        cmd.Parameters.AddWithValue("@Anio", anio);
                        cmd.Parameters.AddWithValue("@Mes", mesNumero);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            int fila = 10; // AP10
                            sheet.Cells[9, 42] = "FICHA";
                            sheet.Cells[9, 43] = "PROGRAMA";
                            sheet.Cells[9, 44] = "COMPETENCIA";
                            sheet.Cells[9, 45] = "HORAS";

                            Excel.Range header = sheet.Range["AP9", "AS9"];
                            header.Font.Bold = true;
                            header.Interior.Color = ColorTranslator.ToOle(Color.LightGray);
                            header.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                            while (reader.Read())
                            {
                                string ficha = reader["FICHA"].ToString().Trim();
                                string programa = reader["PROGRAMA"].ToString().Trim();
                                string competenciaId = reader["COMPETENCIA_ID"].ToString().Trim();
                                string competenciaNm = reader["COMPETENCIA_NOM"].ToString().Trim();
                                string horasStr = reader["HORAS"] != DBNull.Value ? reader["HORAS"].ToString().Trim() : "";

                                sheet.Cells[fila, 42] = ficha;
                                sheet.Cells[fila, 43] = programa;
                                sheet.Cells[fila, 44] = string.IsNullOrEmpty(competenciaNm) ? competenciaId : competenciaNm;
                                sheet.Cells[fila, 45] = horasStr;

                                Color colorAsignado = ObtenerColorPorFichaCompetencia(ficha, competenciaId);
                                Excel.Range colorCell = sheet.Cells[fila, 46];
                                colorCell.Interior.Color = ColorTranslator.ToOle(colorAsignado);
                                colorCell.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                                fila++;
                            }

                            Excel.Range dataRange = sheet.Range["AP9", $"AS{fila - 1}"];
                            dataRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                            dataRange.Columns.AutoFit();
                        }
                    }
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open) con.Close();
                }

                string[] rangos = {
                    "C10:I12","C13:I28","C29:I29","B9:B28",
                    "C11:AN12","C10:AN10","J10:P12","Q10:W12",
                    "X10:AD12","AE10:AN12","J13:P28","Q13:W28",
                    "X13:AD28","AE13:AN28","J29:P29","Q29:W29",
                    "X29:AD29","AE29:AN29","C30:P30","J30:W30",
                    "Q30:AD30","X30:AN30","B29:B31","B29:B30",
                    "B30:B31","B31:AN31","B9:AN12","AL10:AN28",
                    "X29:AK29","X30:AK30"
                };
                foreach (var r in rangos)
                {
                    Excel.Range celdas = sheet.Range[r];
                    celdas.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);
                }

                excelApp.CalculateFull(); // fuerza cálculo
                workbook.SaveAs(outputPath);
                return outputPath;
            }
            finally
            {
                try { if (workbook != null) workbook.Close(false); } catch { }
                try { if (excelApp != null) excelApp.Quit(); } catch { }

                if (sheet != null) Marshal.FinalReleaseComObject(sheet);
                if (workbook != null) Marshal.FinalReleaseComObject(workbook);
                if (excelApp != null) Marshal.FinalReleaseComObject(excelApp);

                sheet = null; workbook = null; excelApp = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        // ------------------------------------------------------------
        //  HELPERS
        // ------------------------------------------------------------
        private static string MakeKey(string ficha, string competencia)
            => $"{(ficha ?? "").Trim()}_{(competencia ?? "").Trim()}";

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

        // CLAVE DE COLOR: Ficha + **IdCompetencia** (no el nombre)
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
    }
}
