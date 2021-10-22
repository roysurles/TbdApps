using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RecipeApp.BlazorWasmBootstrap.Features.Details;
using RecipeApp.BlazorWasmBootstrap.Features.Ingredient;
using RecipeApp.BlazorWasmBootstrap.Features.Instruction;
using RecipeApp.Shared.Features.Introduction;
using RecipeApp.Shared.Features.Session;
using RecipeApp.Shared.MessageHandlers;
using RecipeApp.Shared.Models;

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
            //if (!builder.HostEnvironment.IsProduction())
            //    IdentityModelEventSource.ShowPII = true;      // For debugging Identity related exceptions...
            var defaultJsonDeSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            //builder.Services.AddOidcAuthentication(options =>
            //{
            //    options.ProviderOptions.Authority = "https://login.domain.com/";
            //    options.ProviderOptions.ClientId = "ClientId";
            //    options.ProviderOptions.ResponseType = "code";
            //    options.ProviderOptions.DefaultScopes.Add("openid");
            //    options.ProviderOptions.DefaultScopes.Add("profile");
            //    options.ProviderOptions.DefaultScopes.Add("Api1.read");
            //    options.ProviderOptions.DefaultScopes.Add("Api2.read");

            //    options.ProviderOptions.PostLogoutRedirectUri = "/";
            //    options.UserOptions.RoleClaim = "role";
            //})
            //.AddAccountClaimsPrincipalFactory<CustomAccountClaimsPrincipalFactory>();

            var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
            builder.Services.AddScoped(_ => httpClient);

            var appSettingsJsonFile = builder.HostEnvironment.IsProduction()
                ? "appsettings.json"
                : builder.HostEnvironment.IsStaging()
                    ? "appsettings.staging.json"
                    : builder.HostEnvironment.IsDevelopment()
                        ? "appsettings.development.json"
                        : throw new ArgumentOutOfRangeException("HostEnvironment", $"Unknown HostEnvironment: {builder.HostEnvironment.Environment}");
            using var response = await httpClient.GetAsync(appSettingsJsonFile);
            using var stream = await response.Content.ReadAsStreamAsync();
            builder.Configuration.AddJsonStream(stream);

            var apiUrlsOptionsModel = new ApiUrlsOptionsModel();
            builder.Configuration.GetSection("ApiUrls").Bind(apiUrlsOptionsModel);
            builder.Services.AddSingleton(_ => apiUrlsOptionsModel);

            builder.Services.AddSingleton<ISessionViewModel, SessionViewModel>();
            builder.Services.AddScoped<CustomMessageHandler>();
            builder.Services.AddTransient(typeof(IApiResultModel<>), typeof(ApiResultModel<>));

            builder.Services.AddRefitClient<IIntroductionApiClientV1_0>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiUrlsOptionsModel.CoreApiUrl))
                .AddHttpMessageHandler<CustomMessageHandler>();

            builder.Services.AddHttpClient<IIntroductionApiClientNativeV1_0>(client =>
                client.BaseAddress = new Uri(apiUrlsOptionsModel.CoreApiUrl))
                .AddHttpMessageHandler<CustomMessageHandler>()
                .AddTypedClient<IIntroductionApiClientNativeV1_0>(client => new IntroductionApiClientNativeV1_0(client, "/api/v1.0/Introduction", defaultJsonDeSerializerOptions: defaultJsonDeSerializerOptions));

            builder.Services.AddTransient<IIntroductionViewModel, IntroductionViewModel>();
            builder.Services.AddSingleton<IIntroductionSearchViewModel, IntroductionSearchViewModel>();

            builder.Services.AddRefitClient<IIngredientApiClientV1_0>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiUrlsOptionsModel.CoreApiUrl))
                .AddHttpMessageHandler<CustomMessageHandler>();
            builder.Services.AddTransient<IIngredientViewModel, IngredientViewModel>();

            builder.Services.AddRefitClient<IInstructionApiClientV1_0>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiUrlsOptionsModel.CoreApiUrl))
                .AddHttpMessageHandler<CustomMessageHandler>();
            builder.Services.AddTransient<IInstructionViewModel, InstructionViewModel>();

            builder.Services.AddTransient<IDetailsPageViewModel, DetailsPageViewModel>();

            await builder.Build().RunAsync();
        }
    }
}
