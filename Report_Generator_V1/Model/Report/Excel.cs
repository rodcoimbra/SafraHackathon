using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Report_Generator_V1.Model.Report
{
    class Excel
    {
        public void Create_Report(List<Account>data, string save_path)
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
                Worksheet xlSheetGeneral = (Worksheet)xlSheets.Add(xlSheets[indexSheet], Type.Missing, Type.Missing);
                xlSheetGeneral.Name = "GENERAL";

                if (!Create_Header(xlSheetGeneral))
                {
                    xlSheetGeneral.Delete();
                    Exit_Excel(xlBook, xlApp);
                    return;
                }

                if(!Insert_Data(xlSheetGeneral, data))
                {
                    xlSheetGeneral.Delete();
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
                xlSheet.Columns[1].ColumnWidth = 12;
                xlSheet.Columns[2].ColumnWidth = 12;
                xlSheet.Columns[3].ColumnWidth = 12;
                xlSheet.Columns[4].ColumnWidth = 12;

                xlSheet.Cells[1, 1] = "ACCOUNT INFO";
                xlSheet.Cells[1, 2] = "BALANCE IN";
                xlSheet.Cells[1, 3] = "BALANCE OUT";
                xlSheet.Cells[1, 4] = "CLUSTER";

                xlSheet.Cells[1, 1].Font.Bold = true;
                xlSheet.Cells[1, 2].Font.Bold = true;
                xlSheet.Cells[1, 3].Font.Bold = true;
                xlSheet.Cells[1, 4].Font.Bold = true;
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
                for(int i = 0;i< data.Count;i++)
                {
                    Account row = data[i];

                    xlSheet.Cells[i+2,1] = row.Description;
                    xlSheet.Cells[i + 2, 2] = row.Balance_in;
                    xlSheet.Cells[i + 2, 3] = row.Balance_out;
                    xlSheet.Cells[i + 2, 4] = row.Cluster;

                    //for(int j = 0;j< row.Account_fields_count; j++)
                    //{
                    //    xlSheet.Cells[i+2,j+1] = row[i].ToString();
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not insert data into sheet: " + xlSheet.Name + "\n" + ex.Message, "Error insertind data", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
