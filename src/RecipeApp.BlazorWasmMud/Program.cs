using System;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MudBlazor.Services;

using RecipeApp.Shared.Features.Session;
using RecipeApp.Shared.Models;

namespace RecipeApp.BlazorWasmMud
{
    public static class Program
    {
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

            return builder.Build().RunAsync();
        }
    }
}
