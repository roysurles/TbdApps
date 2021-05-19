using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

using RecipeApp.BlazorWasmBootstrap.Features.Shared.MessageHandlers;
using RecipeApp.BlazorWasmBootstrap.Features.Shared.Services;
using RecipeApp.BlazorWasmBootstrap.Features.Shared.Services.IntroductionSearch;

using Refit;

namespace RecipeApp.BlazorWasmBootstrap
{
    public static class Program
    {
        [SuppressMessage("AsyncUsage", "AsyncFixer01:Unnecessary async/await usage", Justification = "<Pending>")]
        [SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddSingleton<ISessionService, SessionService>();

            // TODO:  Bind ApiUrls from appsettings & make environments
            builder.Services.AddRefitClient<IIntroductionV1_0ApiClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:44350"))
                .AddHttpMessageHandler<CustomMessageHandler>();
            builder.Services.AddSingleton<IIntroductionSearchViewModel, IntroductionSearchViewModel>();

            await builder.Build().RunAsync();
        }
    }
}
