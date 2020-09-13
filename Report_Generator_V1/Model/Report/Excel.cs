using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Report_Generator_V1.Model.Database;

namespace Report_Generator_V1.Model.Report
{
    class Excel
    {
        public void Create_Report(List<Account> data, string save_path)
        {
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            Workbook xlBook = null;
            Sheets xlSheets;
            int indexSheet = 1;

            try
            {
                xlBook = xlApp.Workbooks.Add();
                xlSheets = xlBook.Sheets;
                xlApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not create a new excel:\n" + ex.Message, "Error creating a new Excel file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Exit_Excel(xlBook, xlApp);
                return;
            }

            try
            {
                Worksheet xlSheet_listagem = (Worksheet)xlSheets.Add(xlSheets[indexSheet], Type.Missing, Type.Missing);
                xlSheet_listagem.Name = "listagem_formulas";

                Worksheet xlSheet_resume = (Worksheet)xlSheets.Add(xlSheets[indexSheet], Type.Missing, Type.Missing);
                xlSheet_resume.Name = "resumo_consolidado";

                if (!Create_Header(xlSheet_resume))
                {
                    xlSheet_listagem.Delete();
                    Exit_Excel(xlBook, xlApp);
                    return;
                }

                if (!Insert_Data(xlSheet_listagem, data))
                {
                    xlSheet_listagem.Delete();
                    Exit_Excel(xlBook, xlApp);
                    return;
                }

                if (!Insert_ScatterChart(xlApp, xlSheet_listagem, xlSheet_resume, data))
                {
                    xlSheet_listagem
                        .Delete();
                    Exit_Excel(xlBook, xlApp);
                    return;
                }

                if (!Insert_PizzaChart(xlApp, xlSheet_listagem, xlSheet_resume, data))
                {
                    xlSheet_listagem
                        .Delete();
                    Exit_Excel(xlBook, xlApp);
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating new sheet in workbook:\n" + ex.Message, "Error building Excel sheet", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Exit_Excel(xlBook, xlApp);
                return;
            }

            try
            {
                xlApp.DisplayAlerts = false;
                xlBook.SaveAs(save_path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                xlApp.DisplayAlerts = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving workbook:\n" + ex.Message, "Error saving workbook", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Exit_Excel(xlBook, xlApp);

            }

            return;
        }

        private Boolean Create_Header(Worksheet xlSheet)
        {

            try
            {


                xlSheet.Cells[1, 1] = "RESUMO CONSOLIDADO DE PREVISÃO DE SAÚDE FINANCEIRA";


                Range aux_range;

                for (int i = 1; i <= 12; i++)
                {
                    aux_range = xlSheet.Columns[i];
                    aux_range.Interior.Color = Color.FromArgb(123, 123, 123);
                }

                aux_range = xlSheet.Range[xlSheet.Cells[1, 1], xlSheet.Cells[7, 12]];
                aux_range.Merge();

                aux_range.HorizontalAlignment = HorizontalAlignment.Center;
                aux_range.VerticalAlignment = VerticalAlignment.Center;

                aux_range.Interior.Color = Color.FromArgb(123, 123, 123);
                aux_range.Font.Color = Color.FromArgb(1, 123, 123);
                aux_range.Font.Size = 18;

                aux_range = xlSheet.Range[xlSheet.Cells[8, 1], xlSheet.Cells[8, 12]];
                aux_range.Merge();
                aux_range.Interior.Color = Color.FromArgb(123, 123, 123);

                aux_range = xlSheet.Range[xlSheet.Cells[9, 1], xlSheet.Cells[9, 12]];
                aux_range.Interior.Color = Color.FromArgb(123, 123, 123);
                aux_range.Font.Color = Color.FromArgb(255, 255, 255);
                aux_range.Font.Size = 8;

            }
            catch (Exception)
            {
                MessageBox.Show("Could not build header for the sheet.", "Error Building Excel Sheet", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private Boolean Insert_Data(Worksheet xlSheet, List<Account> data)
        {

            try
            {
                xlSheet.Cells[1, 1] = "ACCOUNT";
                ((Range)xlSheet.Cells[1, 1]).BorderAround2();
                xlSheet.Cells[1, 2] = "OUT(R$)";
                ((Range)xlSheet.Cells[1, 2]).BorderAround2();
                xlSheet.Cells[1, 3] = "IN(R$)";
                ((Range)xlSheet.Cells[1, 3]).BorderAround2();
                xlSheet.Cells[1, 4] = "CLUSTER";
                ((Range)xlSheet.Cells[1, 4]).BorderAround2();
                xlSheet.Cells[1, 5] = "EXECUTION TIME";
                ((Range)xlSheet.Cells[1, 5]).BorderAround2();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not insert data header into sheet: " + xlSheet.Name + "\n" + ex.Message, "Error inserting data header", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            try
            {
                for (int i = 0; i < data.Count; i++)
                {
                    Account row = data[i];

                    xlSheet.Cells[i + 2, 1] = row.Description;
                    ((Range)xlSheet.Cells[i + 2, 1]).BorderAround2();
                    xlSheet.Cells[i + 2, 2] = row.Balance_out;
                    ((Range)xlSheet.Cells[i + 2, 2]).BorderAround2();
                    xlSheet.Cells[i + 2, 3] = row.Balance_in;
                    ((Range)xlSheet.Cells[i + 2, 3]).BorderAround2();
                    xlSheet.Cells[i + 2, 4] = row.Cluster;
                    ((Range)xlSheet.Cells[i + 2, 4]).BorderAround2();
                    xlSheet.Cells[i + 2, 5] = row.time_exec;
                    ((Range)xlSheet.Cells[i + 2, 5]).BorderAround2();


                    //for(int j = 0;j< row.Account_fields_count; j++)
                    //{
                    //    xlSheet.Cells[i+2,j+1] = row[i].ToString();
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not insert data into sheet: " + xlSheet.Name + "\n" + ex.Message, "Error inserting data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private Boolean Insert_ScatterChart(Microsoft.Office.Interop.Excel.Application xlApp, Worksheet xlSheetData, Worksheet xlSheetchart, List<Account> data)
        {

            int first_column = data[0].Account_fields_count + 2;
            int index = 0;
            //neutro  //investidor  //Risco iminente  //risco 2  //Possivel  //Risco 1
            Color[] grad_colors = new Color[]{Color.FromArgb(247, 222, 220), Color.FromArgb(181, 142, 65), Color.FromArgb(192, 0, 0),
                                   Color.FromArgb(247, 1, 220), Color.FromArgb(2, 222, 220), Color.FromArgb(3, 222, 220) };

            Dictionary<String, int> clusters = new Dictionary<string, int>();

            try
            {

                for (int i = 0; i < data.Count; i++)
                {
                    Account row = data[i];

                    if (!clusters.ContainsKey(row.Cluster))
                    {
                        clusters.Add(row.Cluster, index++);
                    }
                }

                index = 0;
                xlSheetData.Cells[1, index + first_column] = "Relação de Entrada (IN) e Saída (OUT) de contas PF Banco Safra";
                xlSheetData.Cells[2, index + first_column] = "IN";
                ((Range)xlSheetData.Cells[2, index + first_column]).BorderAround2();
                foreach (string key in clusters.Keys)
                {
                    xlSheetData.Cells[2, ++index + first_column] = key;
                    ((Range)xlSheetData.Cells[2, index + first_column]).BorderAround2();
                }


                for (int i = 0; i < data.Count; i++)
                {
                    Account row = data[i];

                    xlSheetData.Cells[i + 3, first_column] = row.Balance_in;
                    ((Range)xlSheetData.Cells[i + 3, first_column]).BorderAround2();

                    for (int j = 0; j < clusters.Count; j++)
                    {
                        if (j == clusters[row.Cluster])
                        {
                            xlSheetData.Cells[i + 3, j + first_column + 1] = row.Balance_out;

                        }
                        else
                        {
                            xlSheetData.Cells[i + 3, j + first_column + 1] = "";
                        }
                        ((Range)xlSheetData.Cells[i + 3, j + first_column + 1]).BorderAround2();
                    }
                }


                for (int i = 0; i <= clusters.Count; i++)
                {
                    Range range_h = (Range)xlSheetData.Columns[i + first_column];
                    range_h.ColumnWidth = 12;
                }

                Range aux_range = xlSheetData.Range[xlSheetData.Cells[1, first_column], xlSheetData.Cells[1, first_column + clusters.Count]];
                aux_range.Merge();
                aux_range = xlSheetData.Range[xlSheetData.Cells[1, first_column], xlSheetData.Cells[data.Count + 2, first_column + clusters.Count]];

                aux_range.BorderAround2();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not replicate the data to build the chart into sheet: " + xlSheetData.Name + "\n" + ex.Message, "Error inserting data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            try
            {

                var charts = xlSheetchart.ChartObjects() as Microsoft.Office.Interop.Excel.ChartObjects;
                var chartObject = charts.Add(5, 160, 565, 300) as Microsoft.Office.Interop.Excel.ChartObject;
                var chart = chartObject.Chart;

                // Set chart range.
                var range = xlSheetData.Range[xlSheetData.Cells[2, first_column], xlSheetData.Cells[data.Count + 2, first_column + clusters.Count]];
                chart.SetSourceData(range);

                // Set chart properties.
                chart.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlXYScatter;
                chart.ChartWizard(Source: range,
                    Title: "Relação de Entrada (IN) e Saída (OUT) de contas PF Banco Safra",
                    CategoryTitle: "ENTRADA R$ (IN)",
                    ValueTitle: "SAÍDA R$ (OUT)");
                chart.ChartStyle = 245;
                chart.ChartWizard(Type.Missing, Type.Missing, Type.Missing, XlRowCol.xlColumns, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                for (int i = 1; i <= clusters.Count; i++)
                {

                    chart.FullSeriesCollection(i).Select();
                    xlApp.Selection.Format.Line.Visible = true;
                    xlApp.Selection.Format.Line.ForeColor.RGB = grad_colors[i - 1];

                    xlApp.Selection.Format.Fill.Visible = true;
                    xlApp.Selection.Format.Fill.ForeColor.RGB = grad_colors[i - 1];

                    xlApp.Selection.Format.Glow.Radius = 0;

                }



            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not build the chart into sheet: " + xlSheetData.Name + "\n" + ex.Message, "Error building chart", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            return true;
        }

        private Boolean Insert_PizzaChart(Microsoft.Office.Interop.Excel.Application xlApp, Worksheet xlSheetData, Worksheet xlSheetchart, List<Account> data)
        {

            int index = 0;
            Color[] grad_colors = new Color[]{Color.FromArgb(247, 222, 220), Color.FromArgb(181, 142, 65), Color.FromArgb(192, 0, 0),
                                   Color.FromArgb(247, 1, 220), Color.FromArgb(2, 222, 220), Color.FromArgb(3, 222, 220) };

            Dictionary<String, int> clusters = new Dictionary<string, int>();
            Dictionary<String, double> percentage = new Dictionary<string, double>();

            for (int i = 0; i < data.Count; i++)
            {
                Account row = data[i];

                if (!clusters.ContainsKey(row.Cluster))
                {
                    clusters.Add(row.Cluster, index++);
                }
            }

            foreach (string key in clusters.Keys)
            {
                int value = data.Count(x => x.Cluster == key);
                percentage.Add(key, value);
            }
            percentage.Add("total", data.Count);

            int first_column = data[0].Account_fields_count + clusters.Count + 8;
            int index_column = 0;
            int index_row = 0;


            try
            {

                index_column = 0;
                index_row = 1;

                xlSheetData.Cells[index_row++, first_column] = "% Público/Cluster";
                xlSheetData.Cells[index_row, first_column] = "cluster";
                ((Range)xlSheetData.Cells[index_row, first_column]).BorderAround2();
                xlSheetData.Cells[index_row, first_column + 1] = "%";
                ((Range)xlSheetData.Cells[index_row, first_column + 1]).BorderAround2();
                index_row++;

                foreach (String key in clusters.Keys)
                {
                    xlSheetData.Cells[index_row, first_column] = key;
                    ((Range)xlSheetData.Cells[index_row, first_column]).BorderAround2();
                    xlSheetData.Cells[index_row, first_column + 1] = (percentage[key] / percentage["total"]);
                    ((Range)xlSheetData.Cells[index_row, first_column + 1]).BorderAround2();
                    index_row++;
                }



                Range aux_range = xlSheetData.Range[xlSheetData.Cells[1, first_column], xlSheetData.Cells[1, first_column + 1]];
                aux_range.Merge();
                aux_range = xlSheetData.Range[xlSheetData.Cells[1, first_column], xlSheetData.Cells[percentage.Count + 1, first_column + 1]];

                aux_range.BorderAround2();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not replicate the data to build the chart into sheet: " + xlSheetData.Name + "\n" + ex.Message, "Error inserting data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            try
            {

                var charts = xlSheetchart.ChartObjects() as Microsoft.Office.Interop.Excel.ChartObjects;
                var chartObject = charts.Add(5, 470, 565, 300) as Microsoft.Office.Interop.Excel.ChartObject;
                var chart = chartObject.Chart;

                // Set chart range.
                var range = xlSheetData.Range[xlSheetData.Cells[2, first_column], xlSheetData.Cells[percentage.Count + 1, first_column + 1]];
                chart.SetSourceData(range);

                // Set chart properties.
                chart.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlPie;
                chart.ChartWizard(Source: range,
                    Title: "% people / Cluster");
                chart.ChartStyle = 257;
                chart.SetElement(Microsoft.Office.Core.MsoChartElementType.msoElementDataLabelOutSideEnd);

                for (int i = 1; i < percentage.Count; i++)
                {
                    chart.FullSeriesCollection(1).Points(i).Select();

                    xlApp.Selection.Format.Fill.Visible = true;
                    xlApp.Selection.Format.Fill.ForeColor.RGB = grad_colors[i - 1];

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not build the chart into sheet: " + xlSheetData.Name + "\n" + ex.Message, "Error building chart", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            return true;
        }


        private void Exit_Excel(Workbook xlBook, Microsoft.Office.Interop.Excel.Application xlApp)
        {
            try { xlBook.Close(); } catch (Exception) { xlBook = null; }
            try { xlApp.Quit(); } catch (Exception) { xlApp = null; }
            return;
        }
    }
}
