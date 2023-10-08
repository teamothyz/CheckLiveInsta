using OpenQA.Selenium.Chrome;
using SeleniumUndetectedChromeDriver;
using System.Diagnostics;

namespace ChromeDriverLibrary
{
    public class ChromeDriverInstance
    {
        private static readonly object _lockUserDir = new();

        public static MyChromeDriver GetInstance(int positionX,
            int positionY,
            bool isMaximize = false,
            string? proxy = null,
            bool isHeadless = true,
            List<string>? extensionPaths = null,
            bool disableImg = true,
            bool privateMode = true,
            string? userDataDir = null,
            string? profile = null,
            bool isDeleteProfile = true,
            bool keepOneWindow = false,
            CancellationToken? token = null)
        {
            MyChromeDriver myDriver = new();
            if (userDataDir == null)
            {
                myDriver.ProfileDir = GetUserDir();
                myDriver.IsDeleteProfile = isDeleteProfile;
            }
            else
            {
                myDriver.ProfileDir = Path.Combine(userDataDir, profile ?? "Default");
                myDriver.IsDeleteProfile = isDeleteProfile;
            }
            try
            {
                token ??= CancellationToken.None;
                var proxyInfo = new List<string>();

                var options = new ChromeOptions();
                options.AddArgument("--remote-debugging-port=9222"); 
                if (!string.IsNullOrEmpty(proxy))
                {
                    var prefix = string.Empty;
                    if (proxy.Contains("http://") || proxy.Contains("https://"))
                    {
                        proxy = proxy.Replace("http://", "");
                        prefix = "http://";
                    }
                    else if (proxy.Contains("socks5://"))
                    {
                        proxy = proxy.Replace("socks5://", "");
                        prefix = "socks5://";
                    }
                    else throw new Exception("unsupported proxy type");

                    proxyInfo = proxy.Split(":").ToList();
                    if (proxyInfo.Count != 2 && proxyInfo.Count != 4) throw new Exception("invalid proxy format");

                    options.AddArgument($"--proxy-server={prefix}{proxyInfo[0]}:{proxyInfo[1]}");
                }

                var basePath = AppDomain.CurrentDomain.BaseDirectory;

                var extensions = new List<string>();
                extensions.AddRange(extensionPaths ?? new List<string>());
                if (proxyInfo.Count == 4) extensions.Add($"{basePath}/chromedriver/proxyauth");
                if (extensions.Count > 0) options.AddArguments($"--load-extension={string.Join(",", extensions)}");

                if (privateMode) options.AddArgument("--incognito");
                if (disableImg) options.AddArgument("--blink-settings=imagesEnabled=false");
                if (!string.IsNullOrWhiteSpace(profile)) options.AddArgument($"--profile-directory={profile}");

                var chromeDriverPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "chromedriver", "chromedriver.exe");
                myDriver.Driver = UndetectedChromeDriver.Create(driverExecutablePath: chromeDriverPath,
                    userDataDir: userDataDir,
                    headless: isHeadless,
                    hideCommandPromptWindow: true,
                    options: options);
                if (!isMaximize)
                {
                    myDriver.Driver.Manage().Window.Position = new System.Drawing.Point(positionX, positionY);
                    myDriver.Driver.Manage().Window.Size = new System.Drawing.Size(300, 300);
                }
                else
                {
                    myDriver.Driver.Manage().Window.Maximize();
                }
                Thread.Sleep(1000);
                if (proxyInfo.Count == 4)
                {
                    foreach (var window in myDriver.Driver.WindowHandles)
                    {
                        myDriver.Driver.SwitchTo().Window(window);
                        Task.Delay(1000, token.Value).Wait(token.Value);
                        var title = (string)myDriver.Driver.ExecuteScript("return document.title");
                        if (title.Contains("Proxy Auto Auth"))
                        {
                            myDriver.Driver.FindElement("#login", 30, token.Value);
                            myDriver.Driver.ExecuteScript($"localStorage['proxy_login'] = '{proxyInfo[2]}';" +
                            $"localStorage['proxy_password'] = '{proxyInfo[3]}';" +
                            $"localStorage['proxy_retry'] = '5'");
                            Task.Delay(500, token.Value).Wait(token.Value);
                            myDriver.Driver.ExecuteScript($"localStorage['proxy_locked'] = false;");
                            Task.Delay(500, token.Value).Wait(token.Value);

                            myDriver.Driver.Close();
                            myDriver.Driver.SwitchTo().Window(myDriver.Driver.WindowHandles.First());
                            Task.Delay(1000, token.Value).Wait(token.Value);
                            break;
                        }
                    }
                }

                if (keepOneWindow)
                {
                    while (myDriver.Driver.WindowHandles.Count > 1)
                    {
                        myDriver.Driver.Close();
                        Thread.Sleep(1000);
                        myDriver.Driver.SwitchTo().Window(myDriver.Driver.WindowHandles.First());
                    }
                }
            }
            catch
            {
                Close(myDriver).Wait();
                throw;
            }
            return myDriver;
        }

        private static string GetUserDir()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var folder = Path.Combine(basePath, "profiles");
            var container = Path.Combine(folder, Guid.NewGuid().ToString());
            if (!Directory.Exists(folder))
            {
                lock (_lockUserDir)
                {
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                }
            }
            Directory.CreateDirectory(container);
            return container;
        }

        public static async Task Close(MyChromeDriver myDriver)
        {
            if (myDriver == null) return;
            try
            {
                myDriver.Driver?.Close();
                await Task.Delay(1000).ConfigureAwait(false);
                myDriver.Driver?.Quit();
                await Task.Delay(1000).ConfigureAwait(false);
                myDriver.Driver?.Dispose();
                await Task.Delay(1000).ConfigureAwait(false);
            }
            catch { }

            if (myDriver.IsDeleteProfile && Directory.Exists(myDriver.ProfileDir))
            {
                try
                {
                    Directory.Delete(myDriver.ProfileDir, true);
                }
                catch { }
            }
        }

        public static void KillAllChromes(bool deleteTempFolder = true)
        {
            try
            {
                var process = new Process();
                process.StartInfo.FileName = "chromedriver/kill.bat";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.WaitForExit(10000);
            }
            catch { }

            if (deleteTempFolder)
            {
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var folder = Path.Combine(basePath, "profiles");
                try
                {
                    if (!Directory.Exists(folder)) return;
                    Directory.Delete(folder, true);
                }
                catch { }
            }
        }
    }
}
