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
                Thread.Sleep(1000);

                var passwordElm = instace.Driver.FindElement(@"[name=""password""]", 60, token);
                instace.Driver.Sendkeys(passwordElm, password, true, 60, token);
                Thread.Sleep(1000);

                instace.Driver.Click(@"#loginForm button[type=""submit""]", 60, token);
                _ = instace.Driver.FindElement($@"img[alt*=""{username.ToLower()}""]", 60, token);

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
                return null;
            }
        }
    }
}
