using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateStuff
{
    class Program : IProgram
    {
        static void Main(string[] args)
        {
            string logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");
            FileStream logger = File.OpenWrite(logFile);

            string[] projes = Directory.GetFiles(@"C:\Automation\AlertsProject", "*.csproj", SearchOption.AllDirectories);

            Log(ref logger, "--- Projects: ---" + Environment.NewLine + String.Join(Environment.NewLine, projes) + Environment.NewLine + Environment.NewLine);

            foreach (string proj in projes)
            {
                string projXml = File.ReadAllText(proj);

                if (projXml.Contains("<RestorePackages>true</RestorePackages>"))
                    projXml = projXml.Replace("<RestorePackages>true</RestorePackages>", "");
                else
                {
                    Log(ref logger, "No old nuget for " + proj);
                    continue;
                }

                if (projXml.Contains(enable))
                    projXml = projXml.Replace(enable, "");

                if (projXml.Contains("<Import Project=\"$(SolutionDir)\\.nuget\\NuGet.targets\" Condition=\"Exists('$(SolutionDir)\\.nuget\\NuGet.targets')\" />"))
                    projXml = projXml.Replace("<Import Project=\"$(SolutionDir)\\.nuget\\NuGet.targets\" Condition=\"Exists('$(SolutionDir)\\.nuget\\NuGet.targets')\" />", "");
            
                File.WriteAllText(proj, projXml);
                Log(ref logger, "Updated " + proj);
            }
            logger.Dispose();
        }

        private static void Log(ref FileStream logger, string msg) 
        {
            byte[] b = Encoding.ASCII.GetBytes(Environment.NewLine + msg);
            logger.Write(b, 0, b.Length);
        }

        string IProgram.Name
        {
            get { return "StamProg"; }
        }
        object IProgram.Run(object obj) 
        {
            return obj;
        }

        static string enable = @"<Target Name=""EnsureNuGetPackageBuildImports"" BeforeTargets=""PrepareForBuild"">
    <PropertyGroup>
      <ErrorText>This project references NuGet package(s) that are missing on this computer. Enable NuGet Package Restore to download them.  For more information, see http://go.microsoft.com/fwlink/?LinkID=322105. The missing file is {0}.</ErrorText>
    </PropertyGroup>
    <Error Condition=""!Exists('$(SolutionDir)\.nuget\NuGet.targets')"" Text=""$([System.String]::Format('$(ErrorText)', '$(SolutionDir)\.nuget\NuGet.targets'))"" />
  </Target>";
    }
}
