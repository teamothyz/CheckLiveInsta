using ChromeDriverLibrary;
using Serilog;
using System.Net;

namespace CheckLiveInsta
{
    public class InstaRequestService
    {
        public static Dictionary<string, string> LoginAndGetHeaders(string username, string password, CancellationToken token)
        {
            Dictionary<string, string> headers = new();
            MyChromeDriver instace = null!;
            try
            {
                var isLoggedIn = false;
                instace = ChromeDriverInstance.GetInstance(0, 0, isMaximize: true, isHeadless: true, disableImg: false, keepOneWindow: true, privateMode: false);

                var networkManager = new NetWorkManagerCustom(instace.Driver);
                networkManager.NetworkRequestSent += (sender, e) =>
                {
                    if (e.RequestUrl == $"https://www.instagram.com/api/v1/users/web_profile_info/?username=DangHuong_73825")
                    {
                        foreach (var header in e.RequestHeaders)
                        {
                            headers.Add(header.Key, header.Value);
                        }
                        isLoggedIn = true;
                    }
                };

                instace.Driver.GoToUrl("https://www.instagram.com/");
                var usernameElm = instace.Driver.FindElement(@"[name=""username""]", 60, token);
                instace.Driver.Sendkeys(usernameElm, username, true, 60, token);

                var passwordElm = instace.Driver.FindElement(@"[name=""password""]", 60, token);
                instace.Driver.Sendkeys(passwordElm, password, true, 60, token);

                instace.Driver.Click("#loginForm button", 60, token);

                networkManager.StartMonitoring().Wait(token);
                instace.Driver.Url = "https://www.instagram.com/DangHuong_73825";
                var endTime = DateTime.Now.AddMinutes(2);
                while (!isLoggedIn && DateTime.Now < endTime)
                {
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Login account {username} error: {ex}");
            }
            finally
            {
                instace.Close();
            }
            return headers;
        }

        public static void CheckAccountLoop(List<Tuple<string, string>> accounts, Dictionary<string, string> headers, int totalThreads)
        {
            try
            {
                var countLocker = new object();
                var count = 0;

                var statusLocker = new object();
                var live = 0;
                var die = 0;
                var error = 0;
                var total = 0;

                FileUtil.Init();

                var tasks = new List<Task>();
                for (int i = 0; i < totalThreads; i++)
                {
                    tasks.Add(Task.Run(() =>
                    {
                        while (true)
                        {
                            Tuple<string, string> account = null!;
                            lock (countLocker)
                            {
                                if (count >= accounts.Count) return;
                                account = accounts[count];
                                count++;
                                if (account == null) return;
                            }
                            var success = CheckAccount(account.Item1, headers).Result;
                            FileUtil.WriteGeneral(account, success);

                            switch (success)
                            {
                                case true:
                                    lock (statusLocker)
                                    {
                                        live++;
                                        total++;
                                    }
                                    FileUtil.WriteLive(account);
                                    break;
                                case false:
                                    lock (statusLocker)
                                    {
                                        die++;
                                        total++;
                                    }
                                    FileUtil.WriteDie(account);
                                    break;
                                default:
                                    lock (statusLocker)
                                    {
                                        error++;
                                        total++;
                                    }
                                    FileUtil.WriteError(account);
                                    break;
                            }
                            lock (statusLocker)
                            {
                                ConsoleExtension.WriteInfo(accounts.Count, total, live, die, error);
                            }
                            Thread.Sleep(100);
                        }
                    }));
                }
                Task.WaitAll(tasks.ToArray());
            }
            catch (Exception ex)
            {
                Log.Error($"Loop checking account error: {ex}");
            }
            finally
            {
                FileUtil.Close();
            }
        }

        public static async Task<bool?> CheckAccount(string username, Dictionary<string, string> headers)
        {
            try
            {
                using var client = new HttpClient();
                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
                var res = await client.GetAsync($"https://www.instagram.com/api/v1/users/web_profile_info/?username={username}");
                var content = await res.Content.ReadAsStringAsync();
                return res.StatusCode switch
                {
                    HttpStatusCode.OK => true,
                    HttpStatusCode.NotFound => false,
                    _ => null
                };
            }
            catch (Exception ex)
            {
                Log.Error($"Checking account {username} error: {ex}");
                ConsoleExtension.WriteError($"Error: {username}");
                return null;
            }
        }
    }
}
