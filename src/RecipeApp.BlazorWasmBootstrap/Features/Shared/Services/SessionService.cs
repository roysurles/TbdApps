using System;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace RecipeApp.BlazorWasmBootstrap.Features.Shared.Services
{
    public class SessionService : ISessionService
    {
        private const string ComponentName = nameof(SessionService);

        protected readonly ILogger<SessionService> _logger;

        public SessionService(NavigationManager navigationManager, IJSRuntime jsRuntime
            , IWebAssemblyHostEnvironment hostEnvironment, ILogger<SessionService> logger)
        {
            NavigationManager = navigationManager;
            JSRuntime = jsRuntime;
            HostEnvironment = hostEnvironment;
            _logger = logger;

            navigationManager.LocationChanged += (sender, e) =>
            {
                logger.LogInformation($"{ComponentName}.{nameof(navigationManager.LocationChanged)}({navigationManager.Uri})");
                logger.LogInformation($"{ComponentName}.{nameof(navigationManager.LocationChanged)}.TraceId was {TraceId}");
                TraceId = Guid.NewGuid();
                logger.LogInformation($"{ComponentName}.{nameof(navigationManager.LocationChanged)}.TraceId is {TraceId}");
            };
        }

        public NavigationManager NavigationManager { get; protected set; }

        public IJSRuntime JSRuntime { get; protected set; }

        public IWebAssemblyHostEnvironment HostEnvironment { get; protected set; }

        public Guid SessionId { get; } = Guid.NewGuid();

        public Guid TraceId { get; protected set; } = Guid.NewGuid();

        public void NewId() => TraceId = Guid.NewGuid();
    }

    public interface ISessionService
    {
        NavigationManager NavigationManager { get; }

        IJSRuntime JSRuntime { get; }

        IWebAssemblyHostEnvironment HostEnvironment { get; }

        Guid SessionId { get; }

        Guid TraceId { get; }

        void NewId();
    }
}
