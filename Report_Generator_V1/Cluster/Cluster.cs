using System;
using System.Diagnostics;
using System.IO;

namespace Report_Generator_V1
{
    public class Cluster
    {
        public void RunClusterAlgorithm()
        {
            string workingDirectory = Environment.CurrentDirectory;
            string fileName = Path.Combine(Directory.GetParent(workingDirectory).Parent.Parent.FullName, @"Report_Generator_V1\Cluster\Config\model.exe");

            //string fileName = @"C:\Users\Rodrigo\Desktop\GitMotta\SafraHackathon\SafraHackathon-master\Report_Generator_V1\Cluster\Config\model.exe";

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = fileName;

            // Descomentar a linha abaixo caso não for necessário o TRUNCATE na tabela de origem
            // startInfo.Arguments = "false";

            using (Process exeProcess = Process.Start(startInfo))
            {
                exeProcess.WaitForExit();
            }


            Console.WriteLine("Fim de executação");
            Console.ReadLine();
        }
    }
}
