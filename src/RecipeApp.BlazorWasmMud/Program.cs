using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MudBlazor.Services;

using RecipeApp.Shared.Features.Introduction;
using RecipeApp.Shared.Features.Session;
using RecipeApp.Shared.MessageHandlers;
using RecipeApp.Shared.Models;

using Refit;

using Tbd.Shared.ApiResult;

namespace RecipeApp.BlazorWasmMud
{
    public static class Program
    {
        [SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
        public static Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddMudServices();

            var apiUrlsOptionsModel = new ApiUrlsOptionsModel();
            builder.Configuration.GetSection("ApiUrls").Bind(apiUrlsOptionsModel);
            builder.Services.AddSingleton(_ => apiUrlsOptionsModel);

            builder.Services.AddSingleton<ISessionViewModel, SessionViewModel>();
            builder.Services.AddScoped<CustomMessageHandler>();
            builder.Services.AddTransient(typeof(IApiResultModel<>), typeof(ApiResultModel<>));

            builder.Services.AddRefitClient<IIntroductionApiClientV1_0>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiUrlsOptionsModel.CoreApiUrl))
                .AddHttpMessageHandler<CustomMessageHandler>();
            builder.Services.AddHttpClient<IntroductionApiClientNativeV1_0>(client =>
                client.BaseAddress = new Uri(apiUrlsOptionsModel.CoreApiUrl))
                .AddHttpMessageHandler<CustomMessageHandler>();
            builder.Services.AddTransient<IIntroductionViewModel, IntroductionViewModel>();
            builder.Services.AddSingleton<IIntroductionSearchViewModel, IntroductionSearchViewModel>();

            return builder.Build().RunAsync();
        }
    }
}
