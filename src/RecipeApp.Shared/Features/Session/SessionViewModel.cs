﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Logging;

using RecipeApp.Shared.Models;

using Tbd.Shared.ApiResult;

namespace RecipeApp.Shared.Features.Session
{
    public class SessionViewModel : BaseViewModel, ISessionViewModel
    {
        protected readonly ILogger<SessionViewModel> _logger;

        public SessionViewModel(NavigationManager navigationManager
            , IWebAssemblyHostEnvironment hostEnvironment, ILogger<SessionViewModel> logger)
        {
            NavigationManager = navigationManager;
            HostEnvironment = hostEnvironment;
            _logger = logger;
            navigationManager.LocationChanged += (sender, e) =>
            {
                logger.LogInformation($"{nameof(navigationManager.LocationChanged)}({navigationManager.Uri})");
                logger.LogInformation($"{nameof(navigationManager.LocationChanged)}.TraceId was {TraceId}");
                NewTraceId();
                logger.LogInformation($"{nameof(navigationManager.LocationChanged)}.TraceId is {TraceId}");
            };
        }

        public event EventHandler StateHasChangedEvent;

        public NavigationManager NavigationManager { get; protected set; }

        public IWebAssemblyHostEnvironment HostEnvironment { get; protected set; }

        public Guid SessionId { get; } = Guid.NewGuid();

        public Guid TraceId { get; protected set; } = Guid.NewGuid();

        public void OnStateHasChanged() =>
            StateHasChangedEvent?.Invoke(this, EventArgs.Empty);

        public void NewTraceId()
        {
            TraceId = Guid.NewGuid();
            OnStateHasChanged();
        }

        public ISessionViewModel HandleException(Exception ex
            , List<IApiResultMessageModel> apiResultMessages = null
            , string callerComponentName = null
            , [CallerMemberName] string callerMemberName = null
            , params KeyValuePair<string, string>[] additionalData)
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

            var fullSourceName = $"{callerComponentName}.{callerMemberName}";

            var message = HostEnvironment.IsProduction()
                ? "Oops, we encountered an unhandled exception!  Please try again in a few minutes."
                : $"{fullSourceName} encountered exception: {ex}{NewLineHtml}StackTrace: {ex.StackTrace}{dictionaryAsString}".Replace(Environment.NewLine, NewLineHtml);

            if (HostEnvironment.IsProduction().Equals(false))
                _logger.LogError(ex, $"{fullSourceName}{NewLineHtml}{message}".Replace(NewLineHtml, Environment.NewLine));

            apiResultMessages?.Add(new ApiResultMessageModel
            {
                MessageType = ApiResultMessageModelTypeEnumeration.UnhandledException,
                Code = 600,
                Source = fullSourceName,
                Message = message
            });

            return this;
        }

        public Task TryInvokeAsync(Action @try, Action @finally
            , List<IApiResultMessageModel> apiResultMessages = null
            , string callerComponentName = null
            , [CallerMemberName] string callerMemberName = null
            , params KeyValuePair<string, string>[] additionalData) =>
            TryInvokeInternalAsync(@try, @finally);

        public Task TryInvokeAsync(Func<Task> @try, Action @finally
            , List<IApiResultMessageModel> apiResultMessages = null
            , string callerComponentName = null
            , [CallerMemberName] string callerMemberName = null
            , params KeyValuePair<string, string>[] additionalData) =>
            TryInvokeInternalAsync(@try, @finally);

        public Task TryInvokeAsync(Action @try, Func<Task> @finally
            , List<IApiResultMessageModel> apiResultMessages = null
            , string callerComponentName = null
            , [CallerMemberName] string callerMemberName = null
            , params KeyValuePair<string, string>[] additionalData) =>
            TryInvokeInternalAsync(@try, @finally);

        public Task TryInvokeAsync(Func<Task> @try, Func<Task> @finally
            , List<IApiResultMessageModel> apiResultMessages = null
            , string callerComponentName = null
            , [CallerMemberName] string callerMemberName = null
            , params KeyValuePair<string, string>[] additionalData) =>
            TryInvokeInternalAsync(@try, @finally);

        [SuppressMessage("Design", "RCS1090:Add call to 'ConfigureAwait' (or vice versa).", Justification = "<Pending>")]
        protected async Task TryInvokeInternalAsync(MulticastDelegate @try, MulticastDelegate @finally
            , List<IApiResultMessageModel> apiResultMessages = null
            , string callerComponentName = null
            , [CallerMemberName] string callerMemberName = null
            , params KeyValuePair<string, string>[] additionalData)
        {
            try
            {
                await InvokeAsync(@try);
            }
            catch (Exception ex)
            {
                HandleException(ex, apiResultMessages, callerComponentName, callerMemberName, additionalData);
            }
            finally
            {
                await InvokeAsync(@finally);
            }
        }

        [SuppressMessage("Usage", "RCS1229:Use async/await when necessary.", Justification = "<Pending>")]
        protected static Task InvokeAsync(MulticastDelegate @delegate)
        {
            switch (@delegate)
            {
                case null:
                    return Task.CompletedTask;

                case Action action:
                    action.Invoke();
                    return Task.CompletedTask;

                case Func<Task> func:
                    return func.Invoke();

                default:
                    {
                        try
                        {
                            return @delegate.DynamicInvoke() as Task ?? Task.CompletedTask;
                        }
                        catch (TargetInvocationException e)
                        {
                            return Task.FromException(e.InnerException!);
                        }
                    }
            }
        }
    }

    public interface ISessionViewModel : IBaseViewModel
    {
        event EventHandler StateHasChangedEvent;

        NavigationManager NavigationManager { get; }

        IWebAssemblyHostEnvironment HostEnvironment { get; }

        Guid SessionId { get; }

        Guid TraceId { get; }

        void NewTraceId();

        Task TryInvokeAsync(Action @try, Action @finally
            , List<IApiResultMessageModel> apiResultMessages = null
            , string callerComponentName = null
            , [CallerMemberName] string callerMemberName = null
            , params KeyValuePair<string, string>[] additionalData);

        Task TryInvokeAsync(Func<Task> @try, Action @finally
            , List<IApiResultMessageModel> apiResultMessages = null
            , string callerComponentName = null
            , [CallerMemberName] string callerMemberName = null
            , params KeyValuePair<string, string>[] additionalData);

        Task TryInvokeAsync(Action @try, Func<Task> @finally
            , List<IApiResultMessageModel> apiResultMessages = null
            , string callerComponentName = null
            , [CallerMemberName] string callerMemberName = null
            , params KeyValuePair<string, string>[] additionalData);

        Task TryInvokeAsync(Func<Task> @try, Func<Task> @finally
            , List<IApiResultMessageModel> apiResultMessages = null
            , string callerComponentName = null
            , [CallerMemberName] string callerMemberName = null
            , params KeyValuePair<string, string>[] additionalData);

        ISessionViewModel HandleException(Exception ex
            , List<IApiResultMessageModel> apiResultMessages = null
            , string callerComponentName = null
            , [CallerMemberName] string callerMemberName = null
            , params KeyValuePair<string, string>[] additionalData);
    }
}
