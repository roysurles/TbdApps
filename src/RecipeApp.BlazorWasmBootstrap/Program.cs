using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

using RecipeApp.BlazorWasmBootstrap.Features.Shared.Services;

namespace RecipeApp.BlazorWasmBootstrap
{
    public static class Program
    {
        [SuppressMessage("AsyncUsage", "AsyncFixer01:Unnecessary async/await usage", Justification = "<Pending>")]
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddSingleton<ISessionService, SessionService>();

            await builder.Build().RunAsync();
        }
    }
}
