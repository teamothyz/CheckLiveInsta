using CheckLiveInsta;
using ChromeDriverLibrary;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace InstaChecker
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            InstaRequestService.Username = config["Username"] ?? string.Empty;
            var maxCheck = Convert.ToInt32(config["MaxCheck"]);
            if (maxCheck > 0) InstaRequestService.MaxCheck = maxCheck;

            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .Enrich.FromLogContext()
            .CreateLogger();
            ApplicationConfiguration.Initialize();
            Application.Run(new FrmMain());
        }
    }
}