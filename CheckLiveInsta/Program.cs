using ChromeDriverLibrary;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Text;

namespace CheckLiveInsta
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(new ConfigurationBuilder()
                .AddJsonFile("appsettings.json").Build())
                .Enrich.FromLogContext()
                .CreateLogger();

                var config = new ConfigurationBuilder().AddJsonFile("account.config.json").Build();
                var username = config["Username"] ?? throw new Exception("Vui lòng config account trong account.config.json");
                var password = config["Password"] ?? throw new Exception("Vui lòng config account trong account.config.json");

                Console.OutputEncoding = Encoding.UTF8;
                Console.InputEncoding = Encoding.UTF8;
                Console.ForegroundColor = ConsoleColor.Green;

                string path = string.Empty;
                int totalThreads = 10;

                while (true)
                {
                    Console.Write("Nhập đường dẫn file dữ liệu: ");
                    path = Console.ReadLine() ?? string.Empty;
                    if (!File.Exists(path))
                    {
                        ConsoleExtension.WriteError("Không tìm thấy file. Kiểm tra lại đường dẫn");
                        Console.WriteLine();
                        continue;
                    }
                    break;
                }

                while (true)
                {
                    Console.Write("Nhập số luồng: ");
                    var threadStr = Console.ReadLine();
                    var success = int.TryParse(threadStr, out totalThreads);
                    if (success) break;
                    ConsoleExtension.WriteError("Số luồng không hợp lệ. Vui lòng nhập lại");
                    Console.WriteLine();
                }
                Console.WriteLine("Đang đăng nhập get cookie...");
                Console.ResetColor();

                var accounts = FileUtil.ReadData(path);
                var headers = InstaRequestService.LoginAndGetHeaders(username, password, CancellationToken.None);
                if (headers.Count == 0)
                {
                    ConsoleExtension.WriteError($"Đăng nhập get cookie {username} thất bại");
                    return;
                }
                InstaRequestService.CheckAccountLoop(accounts, headers, totalThreads);
            }
            catch (Exception ex)
            {
                ConsoleExtension.WriteError($"Lỗi chương trình: {ex.Message}");
            }
            finally
            {
                ChromeDriverInstance.KillAllChromes();
            }
        }
    }
}