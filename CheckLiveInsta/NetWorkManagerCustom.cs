using OpenQA.Selenium.DevTools;
using OpenQA.Selenium;

namespace CheckLiveInsta
{
    public class NetWorkManagerCustom : INetwork
    {
        private readonly Lazy<DevToolsSession> session;

        private readonly List<NetworkRequestHandler> requestHandlers = new();

        private readonly List<NetworkResponseHandler> responseHandlers = new();

        private readonly List<NetworkAuthenticationHandler> authenticationHandlers = new();

        public event EventHandler<NetworkRequestSentEventArgs> NetworkRequestSent = null!;

        public event EventHandler<NetworkResponseReceivedEventArgs> NetworkResponseReceived = null!;

        public NetWorkManagerCustom(IWebDriver driver)
        {
            NetWorkManagerCustom networkManager = this;
            session = new Lazy<DevToolsSession>(delegate
            {
                if (networkManager.session == null || driver is not IDevTools devTools)
                {
                    throw new WebDriverException("Driver must implement IDevTools to use these features");
                }
                return devTools.GetDevToolsSession(101);
            });
        }

        public async Task StartMonitoring()
        {
            session.Value.Domains.Network.RequestPaused += OnRequestPaused;
            session.Value.Domains.Network.AuthRequired += OnAuthRequired;
            session.Value.Domains.Network.ResponsePaused += OnResponsePaused;
            await session.Value.Domains.Network.EnableFetchForAllPatterns();
            await session.Value.Domains.Network.EnableNetwork();
            await session.Value.Domains.Network.DisableNetworkCaching();
        }

        public async Task StopMonitoring()
        {
            session.Value.Domains.Network.ResponsePaused -= OnResponsePaused;
            session.Value.Domains.Network.AuthRequired -= OnAuthRequired;
            session.Value.Domains.Network.RequestPaused -= OnRequestPaused;
            await session.Value.Domains.Network.EnableNetworkCaching();
        }

        public void AddRequestHandler(NetworkRequestHandler handler)
        {
            requestHandlers.Add(handler);
        }

        public void ClearRequestHandlers()
        {
            requestHandlers.Clear();
        }

        public void AddAuthenticationHandler(NetworkAuthenticationHandler handler)
        {
            authenticationHandlers.Add(handler);
        }

        public void ClearAuthenticationHandlers()
        {
            authenticationHandlers.Clear();
        }

        public void AddResponseHandler(NetworkResponseHandler handler)
        {
            responseHandlers.Add(handler);
        }

        public void ClearResponseHandlers()
        {
            responseHandlers.Clear();
        }

        private async void OnAuthRequired(object? sender, AuthRequiredEventArgs e)
        {
            _ = e.RequestId;
            var uri = new Uri(e.Uri);
            bool successfullyAuthenticated = false;
            foreach (NetworkAuthenticationHandler authenticationHandler in authenticationHandlers)
            {
                if (authenticationHandler.UriMatcher(uri))
                {
                    PasswordCredentials credentials = authenticationHandler.Credentials as PasswordCredentials ?? new PasswordCredentials();
                    await session.Value.Domains.Network.ContinueWithAuth(e.RequestId, credentials.UserName, credentials.Password);
                    successfullyAuthenticated = true;
                    break;
                }
            }

            if (!successfullyAuthenticated)
            {
                await session.Value.Domains.Network.CancelAuth(e.RequestId);
            }
        }

        private async void OnRequestPaused(object? sender, RequestPausedEventArgs e)
        {
            if (NetworkRequestSent != null)
            {
                NetworkRequestSent(this, new NetworkRequestSentEventArgs(e.RequestData));
            }
            await session.Value.Domains.Network.ContinueRequestWithoutModification(e.RequestData);
        }

        private async void OnResponsePaused(object? sender, ResponsePausedEventArgs e)
        {
            if (e.ResponseData.Headers.Count > 0)
            {
                await session.Value.Domains.Network.AddResponseBody(e.ResponseData);
            }
            if (NetworkResponseReceived != null)
            {
                NetworkResponseReceived(this, new NetworkResponseReceivedEventArgs(e.ResponseData));
            }
            await session.Value.Domains.Network.ContinueResponseWithoutModification(e.ResponseData);
        }
    }
}
