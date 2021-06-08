using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RecipeApp.BlazorWasmBootstrap.Features.Details;
using RecipeApp.BlazorWasmBootstrap.Features.Ingredient;
using RecipeApp.BlazorWasmBootstrap.Features.Instruction;
using RecipeApp.BlazorWasmBootstrap.Features.Introduction;
using RecipeApp.BlazorWasmBootstrap.Features.Shared;
using RecipeApp.BlazorWasmBootstrap.Features.Shared.IntroductionSearch;
using RecipeApp.BlazorWasmBootstrap.Features.Shared.MessageHandlers;
using RecipeApp.BlazorWasmBootstrap.Features.Shared.Session;
using RecipeApp.Shared.Features;

using Refit;

using Tbd.Shared.ApiResult;

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

            var apiUrlsOptionsModel = new ApiUrlsOptionsModel();
            builder.Configuration.GetSection("ApiUrls").Bind(apiUrlsOptionsModel);
            builder.Services.AddSingleton(_ => apiUrlsOptionsModel);

            builder.Services.AddSingleton<ISessionViewModel, SessionViewModel>();
            builder.Services.AddScoped<CustomMessageHandler>();
            builder.Services.AddTransient(typeof(IApiResultModel<>), typeof(ApiResultModel<>));
            builder.Services.AddTransient<Invoker>();
            builder.Services.AddTransient<Invoker2>();
            builder.Services.AddTransient<Invoker3>();

            builder.Services.AddRefitClient<IIntroductionApiClientV1_0>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiUrlsOptionsModel.CoreApiUrl))
                .AddHttpMessageHandler<CustomMessageHandler>();
            builder.Services.AddTransient<IIntroductionViewModel, IntroductionViewModel>();

            builder.Services.AddRefitClient<IIngredientApiClientV1_0>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiUrlsOptionsModel.CoreApiUrl))
                .AddHttpMessageHandler<CustomMessageHandler>();
            builder.Services.AddTransient<IIngredientViewModel, IngredientViewModel>();

            builder.Services.AddRefitClient<IInstructionApiClientV1_0>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiUrlsOptionsModel.CoreApiUrl))
                .AddHttpMessageHandler<CustomMessageHandler>();
            builder.Services.AddTransient<IInstructionViewModel, InstructionViewModel>();

            builder.Services.AddSingleton<IIntroductionSearchViewModel, IntroductionSearchViewModel>();

            builder.Services.AddTransient<IDetailsPageViewModel, DetailsPageViewModel>();

            await builder.Build().RunAsync();
        }
    }
}
