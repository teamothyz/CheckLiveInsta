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
                instace = ChromeDriverInstance.GetInstance(0, 0, isMaximize: false, isHeadless: true, disableImg: false, keepOneWindow: true, privateMode: false);

                var networkManager = new NetWorkManagerCustom(instace.Driver);
                networkManager.NetworkRequestSent += (sender, e) =>
                {
                    if (e.RequestUrl == $"https://www.instagram.com/api/v1/users/web_profile_info/?username=danghuong_73825")
                    {
                        account.Headers.Clear();
                        foreach (var header in e.RequestHeaders)
                        {
                            account.Headers.Add(header.Key, header.Value);
                        }
                        if (account.Headers.ContainsKey("cookie"))
                        {
                            account.Cookie = account.Headers["cookie"];
                        }
                        if (account.Headers.ContainsKey("Cookie"))
                        {
                            account.Cookie = account.Headers["Cookie"];
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

                var endTimeLogin = DateTime.Now.AddMinutes(1);
                while (DateTime.Now < endTimeLogin)
                {
                    try
                    {
                        _ = instace.Driver.FindElement($@"img[alt*=""{account.Username.ToLower()}""]", 3, token);
                        account.Status = LoginStatus.Success;
                        break;
                    }
                    catch
                    {
                        try
                        {
                            _ = instace.Driver.FindElement("#loginForm > span > div", 3, token);
                            account.Status = LoginStatus.Die;
                            break;
                        }
                        catch
                        {
                            if (instace.Driver.Url.Contains("accounts/suspended", StringComparison.OrdinalIgnoreCase))
                            {
                                account.Status = LoginStatus.Die;
                                break;
                            }
                        }
                    }
                }

                if (account.Status == LoginStatus.Success)
                {
                    networkManager.StartMonitoring().Wait(token);
                    instace.Driver.GoToUrl("https://www.instagram.com/danghuong_73825");
                    var endTime = DateTime.Now.AddMinutes(1);
                    while (account.Status != LoginStatus.Success && DateTime.Now < endTime)
                    {
                        Thread.Sleep(1000);
                    }
                }
                if (account.Status == LoginStatus.Die || account.Status == LoginStatus.Success) return;
                else
                {
                    account.Status = LoginStatus.Failed;
                    return;
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
                        var user = JsonConvert.DeserializeObject<JObject>(content)?.SelectToken("data.user")?.ToString();
                        return user switch
                        {
                            "" => false,
                            null => null,
                            _ => true
                        };
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
