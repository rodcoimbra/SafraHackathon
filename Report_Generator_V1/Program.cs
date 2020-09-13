using Report_Generator_V1.Communication;
using Report_Generator_V1.Model.Client;
using Report_Generator_V1.Model.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Report_Generator_V1.Model.Database;
using Ubiety.Dns.Core.Records.NotUsed;
using System.Configuration;

namespace Report_Generator_V1
{
    public static class Program
    {
        public static async Task Main()
        {
            //Acesso às APIs
            var clients = new List<ClientAccount>();

            var client1 = new ClientAccount(ConfigurationManager.AppSettings["client1Auth"], ConfigurationManager.AppSettings["client1Token"], ConfigurationManager.AppSettings["client1endpoint"]);

            var client2 = new ClientAccount(ConfigurationManager.AppSettings["client2Auth"], ConfigurationManager.AppSettings["client2Token"], ConfigurationManager.AppSettings["client2endpoint"]);

            var client3 = new ClientAccount(ConfigurationManager.AppSettings["client3Auth"], ConfigurationManager.AppSettings["client3Token"], ConfigurationManager.AppSettings["client3endpoint"]);


            clients.Add(client1);
            clients.Add(client2);
            clients.Add(client3);

            foreach (var client in clients)
            {
                SafraAPIsComm restApi = new SafraAPIsComm();
                string oAuth = restApi.OAuthRq(client.TokenAccount);
                var json = restApi.TransactionRq(oAuth, client.AccountEndpoint);

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

                ////INSERT Queries
                //if (creditDebitInfo.Equals("Credit"))
                //    insertCreditQuery = String.Format("INSERT INTO `safra.transferencias`(conta, entrada, saida) VALUES({0}, {1}, {2});", accountId, transactionAmount, "0.00");

                //if (creditDebitInfo.Equals("Debit"))
                //    insertDebitQuery = String.Format("INSERT INTO `safra.transferencias`(conta, entrada, saida) VALUES({0}, {1}, {2});", accountId, "0.00", transactionAmount);
            }




            //Run no algoritmo de "clusterização"

            var pyScript = new Cluster();
            pyScript.RunClusterAlgorithm();

            //



            //Geração do relatorio
            ReturnStructure returnstructure = new ReturnStructure();

            Database db = new Database();
            await Task.Run(() => returnstructure = db.Get_Accounts());

            if (returnstructure.Status)
            {
                List<Account> list_of_accounts = (List<Account>)returnstructure.Data;

                Excel excel = new Excel();
                await Task.Run(() => excel.Create_Report(list_of_accounts, @"C:\Users\luiz-pc\Desktop\teste.xlsx"));

                GC.Collect();
                GC.WaitForPendingFinalizers();

            }

            //Envio do e-mail

            var email = new Email();
            email.SendEmail("excelFile");

        }


    }
}
