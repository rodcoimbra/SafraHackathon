using Report_Generator_V1.Communication;
using Report_Generator_V1.Model.Client;
using Report_Generator_V1.Model.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Report_Generator_V1
{
    public static class Program
    {
        public static void Main()
        {
            //Acesso às APIs
            var clients = new List<ClientAccount>();

            var client1 = new ClientAccount("ZjlkM2NkOTYwMDg3NGFjMjgwM2QwM2NhNzA5Yjc4ZWI6MWEyMDc1ZTMtYjE1ZS00MzI0LTkwMmMtMGYxMmY4ZjA4MDgy");
            var client2 = new ClientAccount("ZTMzYjYxMWE4MTIwNGYzMThhMTVkNTcyOGI5OTg2NjE6MzE1OTFhZDItMWNiZC00NGU5LWEwMWMtMWJiY2MwOTg1NTQ0");
            var client3 = new ClientAccount("YmZjZmFlMGY1YjMzNDNlMGIzNTcxODg0MzU2ODJmZWU6NDZhMjllYjAtNDBkOS00MzJlLWE2M2ItODE4OTIxMTBhZTc4");
            var client4 = new ClientAccount("YThlM2JhYjIwNmQyNGE4OWI5YjExM2NjNDkzYTIzNTA6MTgyODY0NTktM2M2Ni00YmI1LTk0M2QtMDBkY2I0ZmRiNzZk");
            var client5 = new ClientAccount("YjVmYjFlM2IzNjcxNGFkMGEwOGU5ZmQ1NDFkMDAxNjA6YTcwNDEzYjktODgzNy00N2RmLTg4NWUtM2QwNDQ5ZmVhYmY1");

            clients.Add(client1);
            clients.Add(client2);
            clients.Add(client3);
            clients.Add(client4);
            clients.Add(client5);

            foreach (var client in clients)
            {
                SafraAPIsComm restApi = new SafraAPIsComm();
                string oAuth = restApi.OAuthRq(client.TokenAccount);
                var json = restApi.TransactionRq(oAuth);

                string accountId = String.Empty;
                string transactionId = String.Empty;
                string transactionAmount = String.Empty;
                string creditDebitInfo = String.Empty;
                string balanceAmount = String.Empty;
                string balanceCreditDebitInfo = String.Empty;

                string insertCreditQuery = String.Empty;
                string insertDebitQuery = String.Empty;

                foreach (var item in json.Data.Transaction)
                {
                    //info da transação
                    accountId = item.AccountId;
                    transactionId = item.TransactionId;
                    transactionAmount = item.Amount._Amount;
                    creditDebitInfo = item.CreditDebitIndicator;

                    //transacao sumarizada ao total
                    balanceAmount = item.Balance.Amount.Amount;
                    balanceCreditDebitInfo = item.Balance.CreditDebitIndicator;
                }

                //INSERT Queries
                if (creditDebitInfo.Equals("Credit"))
                    insertCreditQuery = String.Format("INSERT INTO `safra.transferencias`(conta, entrada, saida) VALUES({0}, {1}, {2});", accountId, transactionAmount, "0.00");

                if (creditDebitInfo.Equals("Debit"))
                    insertDebitQuery = String.Format("INSERT INTO `safra.transferencias`(conta, entrada, saida) VALUES({0}, {1}, {2});", accountId, "0.00", transactionAmount);
            }




            //Run no algoritmo de "clusterização"

            var pyScript = new Cluster();
            pyScript.RunClusterAlgorithm("", "");

            //



            //Geração do relatorio

            Database db = new Database();
            ReturnStructure returnstructure = db.Get_Accounts();

            Excel excel;

            if (returnstructure.Status)
            {

                List<Account> a = (List<Account>)returnstructure.Data;

                excel = new Excel();
                excel.Create_Report(a, @"C:\Users\luiz-pc\Desktop\teste.xlsx");
            }

            //Envio do e-mail

            var email = new Email();
            email.SendEmail("excelFile");

        }


    }
}
