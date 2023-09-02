using Microsoft.Extensions.Configuration;
using Serilog;

namespace InstaChecker
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build())
            .Enrich.FromLogContext()
            .CreateLogger();
            ApplicationConfiguration.Initialize();
            Application.Run(new FrmMain());
        }
    }
}