using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace Report_Generator_V1.Model.Report
{
    public class Email
    {
        public void SendEmail(string excelFile)
        {
            try
            {
                var fromAddress = new MailAddress(ConfigurationManager.AppSettings["emailLogin"], "From Name");
                var toAddress = new MailAddress(ConfigurationManager.AppSettings["emailLogin"], "To Name");
                int qtd_emails = Int16.Parse(ConfigurationManager.AppSettings["qtdEmails"]);

                string fromPassword = ConfigurationManager.AppSettings["emailSenha"];
                string subject = "Relatório - previsão de saúde financeira " + DateTime.Now.ToString("dd-MM-yyyy hh:mm");
                
                const string body = "Caro,\n\nRelatório em anexo.\n\nAtenciosamente.";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 20000
                };



                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,


                })
                {
                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment(excelFile);
                    message.Attachments.Add(attachment);

                    for (int i = 0; i < qtd_emails; i++)
                    {
                        message.CC.Add(ConfigurationManager.AppSettings[String.Format("email{0}", i)]);
                    }


                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }



    }
}
