using Report_Generator_V1.Communication;
using Report_Generator_V1.Model.Client;
using Report_Generator_V1.Model.Report;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Report_Generator_V1.Model.Database;
using System.Configuration;
using System.IO;

namespace Report_Generator_V1
{
    public static class Program
    {
        public static async Task Main()
        {

            List<Account> list_accounts = new List<Account>();

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

                double client_in = 0;
                double client_out = 0;

                foreach (var item in json.Data.Transaction)
                {
                    //info da transação
                    accountId = item.AccountId;
                    transactionId = item.TransactionId;
                    transactionAmount = item.Amount._Amount;
                    creditDebitInfo = item.CreditDebitIndicator;


                    if (creditDebitInfo.Equals("Credit", StringComparison.OrdinalIgnoreCase))
                    {
                        client_in += Double.Parse(transactionAmount, System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else if (creditDebitInfo.Equals("Debit", StringComparison.OrdinalIgnoreCase))
                    {
                        client_out += Double.Parse(transactionAmount, System.Globalization.CultureInfo.InvariantCulture);
                    }
                }

                Account account = new Account();
                account.Balance_in = client_in;
                account.Balance_out = client_out;
                account.Description = accountId;

                list_accounts.Add(account);
            }

            Database db = new Database();
            ReturnStructure returnstructure = new ReturnStructure();

            await Task.Run(() => returnstructure = db.Set_Accounts(list_accounts));


            //Run no algoritmo de "clusterização"
            //-----------------------------------
            var pyScript = new Cluster();
            pyScript.RunClusterAlgorithm();

            String exe_location = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            await Task.Run(() => returnstructure = db.Get_Accounts());
            if (returnstructure.Status)
            {
                List<Account> list_of_accounts = (List<Account>)returnstructure.Data;

                Excel excel = new Excel();
                await Task.Run(() => excel.Create_Report(list_of_accounts, Path.Combine(exe_location, @"relatorio saude financeira.xlsx")));

                GC.Collect();
                GC.WaitForPendingFinalizers();

            }

            //Envio do e-mail
            var email = new Email();
            email.SendEmail(Path.Combine(exe_location, @"relatorio saude financeira.xlsx"));

        }


    }
}
