using ChromeDriverLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System.Net;

namespace CheckLiveInsta
{
    public class InstaRequestService
    {
        public static void LoginAndGetHeaders(Account account, CancellationToken token)
        {
            MyChromeDriver instace = null!;
            try
            {
                instace = ChromeDriverInstance.GetInstance(0, 0, isMaximize: true, isHeadless: true, disableImg: false, keepOneWindow: true, privateMode: false);

                var networkManager = new NetWorkManagerCustom(instace.Driver);
                networkManager.NetworkRequestSent += (sender, e) =>
                {
                    if (e.RequestUrl == $"https://www.instagram.com/api/v1/users/web_profile_info/?username=DangHuong_73825")
                    {
                        account.Headers.Clear();
                        foreach (var header in e.RequestHeaders)
                        {
                            account.Headers.Add(header.Key, header.Value);
                        }
                        account.Status = LoginStatus.Success;
                    }
                };

                instace.Driver.GoToUrl("https://www.instagram.com/");
                var usernameElm = instace.Driver.FindElement(@"[name=""username""]", 60, token);
                instace.Driver.Sendkeys(usernameElm, account.Username, true, 60, token);
                Thread.Sleep(1000);

                var passwordElm = instace.Driver.FindElement(@"[name=""password""]", 60, token);
                instace.Driver.Sendkeys(passwordElm, account.Password, true, 60, token);
                Thread.Sleep(1000);

                instace.Driver.Click(@"#loginForm button[type=""submit""]", 60, token);
                _ = instace.Driver.FindElement($@"img[alt*=""{account.Username.ToLower()}""]", 60, token);

                networkManager.StartMonitoring().Wait(token);
                instace.Driver.Url = "https://www.instagram.com/DangHuong_73825";
                var endTime = DateTime.Now.AddMinutes(1);
                while (account.Status != LoginStatus.Success && DateTime.Now < endTime)
                {
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Login account {account.Username} error: {ex}");
                account.Status = LoginStatus.Failed;
            }
            finally
            {
                instace?.Close();
            }
        }

        public static async Task<bool?> CheckAccount(string username, Dictionary<string, string> headers, CancellationToken token)
        {
            try
            {
                using var client = new HttpClient();
                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
                var res = await client.GetAsync($"https://www.instagram.com/api/v1/users/web_profile_info/?username={username}", token);
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    try
                    {
                        var content = await res.Content.ReadAsStringAsync(token);
                        var status = JsonConvert.DeserializeObject<JObject>(content)?["status"]?.ToString();
                        return status == "ok";
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"Scan {username} error: {ex.Message}");
                        return null;
                    }
                }
                else if (res.StatusCode == HttpStatusCode.NotFound) return false;
                else
                {
                    Log.Error($"Scan {username} error: {res.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Checking account {username} error: {ex}");
                return null;
            }
        }
    }
}
