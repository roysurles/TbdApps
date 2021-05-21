
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

using RecipeApp.BlazorWasmBootstrap.Features.Shared.Models;

using Tbd.Shared.ApiResult;

namespace RecipeApp.BlazorWasmBootstrap.Features.Shared.Session
{
    public class SessionViewModel : BaseViewModel, ISessionViewModel
    {
        protected readonly ILogger<SessionViewModel> _logger;

        public SessionViewModel(NavigationManager navigationManager, IJSRuntime jsRuntime
            , IWebAssemblyHostEnvironment hostEnvironment, ILogger<SessionViewModel> logger)
        {
            NavigationManager = navigationManager;
            JSRuntime = jsRuntime;
            HostEnvironment = hostEnvironment;
            _logger = logger;
            navigationManager.LocationChanged += (sender, e) =>
            {
                logger.LogInformation($"{nameof(navigationManager.LocationChanged)}({navigationManager.Uri})");
                logger.LogInformation($"{nameof(navigationManager.LocationChanged)}.TraceId was {TraceId}");
                TraceId = Guid.NewGuid();
                logger.LogInformation($"{nameof(navigationManager.LocationChanged)}.TraceId is {TraceId}");
            };
        }

        public NavigationManager NavigationManager { get; protected set; }

        public IJSRuntime JSRuntime { get; protected set; }

        public IWebAssemblyHostEnvironment HostEnvironment { get; protected set; }

        public Guid SessionId { get; } = Guid.NewGuid();

        public Guid TraceId { get; protected set; } = Guid.NewGuid();

        public void NewId() => TraceId = Guid.NewGuid();

        public void HandleException(List<IApiResultMessageModel> apiResultMessages, Exception ex, string source
            , [CallerMemberName] string callerMemberName = null, params KeyValuePair<string, string>[] additionalData)
        {
            const string NewLineHtml = "<br />";

            //if (ex is AccessTokenNotAvailableException accessTokenNotAvailableException)
            //{
            //    accessTokenNotAvailableException.Redirect();
            //    return;
            //}

            var dictionary = new Dictionary<string, string>(additionalData)
            {
                { "DateTimeOffset", DateTimeOffset.Now.ToString() },
                { "SessionId", SessionId.ToString() },
                { "TraceId", TraceId.ToString() }
            };

            var dictionaryAsString = dictionary?.Count > 0
                ? $"{NewLineHtml}Additional Data: {NewLineHtml}{string.Join(NewLineHtml, dictionary.Select(x => $"{x.Key} = {x.Value}"))}"
                : string.Empty;

            var combinedSource = $"{source}.{callerMemberName}";

            var message = HostEnvironment.IsProduction()
                ? "Oops, we encountered an unhandled exception!  Please try again in a few minutes."
                : $"{combinedSource} encountered exception: {ex}{NewLineHtml}StackTrace: {ex.StackTrace}{dictionaryAsString}".Replace(Environment.NewLine, NewLineHtml);

            if (HostEnvironment.IsProduction().Equals(false))
                _logger.LogError(ex, $"{combinedSource}{NewLineHtml}{message}".Replace(NewLineHtml, Environment.NewLine));

            apiResultMessages?.Add(new ApiResultMessageModel
            {
                MessageType = ApiResultMessageModelTypeEnumeration.UnhandledException,
                Code = 600,
                Source = combinedSource,
                Message = message
            });
        }
    }

    public interface ISessionViewModel : IBaseViewModel
    {
        NavigationManager NavigationManager { get; }

        IJSRuntime JSRuntime { get; }

        IWebAssemblyHostEnvironment HostEnvironment { get; }

        Guid SessionId { get; }

        Guid TraceId { get; }

        void NewId();

        void HandleException(List<IApiResultMessageModel> apiResultMessages, Exception ex, string source
            , [CallerMemberName] string callerMemberName = null, params KeyValuePair<string, string>[] additionalData);
    }
}
