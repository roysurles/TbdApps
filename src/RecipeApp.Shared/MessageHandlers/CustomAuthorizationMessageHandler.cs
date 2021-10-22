using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

using RecipeApp.Shared.Features.Session;

namespace RecipeApp.Shared.MessageHandlers
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/additional-scenarios?view=aspnetcore-5.0
    /// </summary>
    public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        protected readonly ISessionViewModel _sessionViewModel;

        public CustomAuthorizationMessageHandler(IAccessTokenProvider accessTokenProvider
            , NavigationManager navigationManager
            , ISessionViewModel sessionViewModel) : base(accessTokenProvider, navigationManager)
        {
            _sessionViewModel = sessionViewModel;
            _ = ConfigureHandler(authorizedUrls: new[] { "https://domain.Api1.net", "https://domain.Api2.net" },
                                 scopes: new[] { "openid", "profile", "Api1.read", "Api2.read" });
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Remove("X-Correlation-ID");
            request.Headers.Add("X-Correlation-ID", _sessionViewModel.TraceId.ToString());
            return base.SendAsync(request, cancellationToken);
        }
    }
}
