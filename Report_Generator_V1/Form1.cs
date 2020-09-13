using Report_Generator_V1.Model.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Report_Generator_V1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Database db = new Database();
            ReturnStructure returnstructure = db.Get_Accounts();

            if (returnstructure.Status)
            {

                List<Account> a = (List<Account>)returnstructure.Data;

                Excel excel = new Excel();
                excel.Create_Report(a, @"C:\Users\luiz-pc\Desktop\teste.xlsx");
            }

        }
    }
}
