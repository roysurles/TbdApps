using CommunityToolkit.Maui;

using Microsoft.Extensions.Logging;

namespace RecipeApp.Maui;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        var coreApiUrl = "https://localhost:44350";

        builder.Services.AddScoped<CustomMessageHandler>();

#if DEBUG
        builder.Logging.AddDebug();

        if (DeviceInfo.Platform == DevicePlatform.Android)
            coreApiUrl = coreApiUrl.Replace("localhost", "10.0.2.2");

        HttpsClientHandlerService httpsClientHandlerService = new();

        if (DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.Android)
        {
            builder.Services.AddHttpClient(Constants.HttpClientNames.CoreApi)
                .ConfigureHttpClient(configureClient => configureClient.BaseAddress = new Uri(coreApiUrl))
                .ConfigureHttpMessageHandlerBuilder(configureBuilder => configureBuilder.PrimaryHandler = httpsClientHandlerService.GetPlatformMessageHandler())
                .AddHttpMessageHandler<CustomMessageHandler>();

            builder.Services.AddRefitClient<IIntroductionApiClientV1_0>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(coreApiUrl))
                .ConfigureHttpMessageHandlerBuilder(configureBuilder => configureBuilder.PrimaryHandler = httpsClientHandlerService.GetPlatformMessageHandler())
                .AddHttpMessageHandler<CustomMessageHandler>();
        }
        else
        {
            builder.Services.AddHttpClient(Constants.HttpClientNames.CoreApi)
                .ConfigureHttpClient(configureClient => configureClient.BaseAddress = new Uri(coreApiUrl))
                .AddHttpMessageHandler<CustomMessageHandler>();

            builder.Services.AddRefitClient<IIntroductionApiClientV1_0>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(coreApiUrl))
                .AddHttpMessageHandler<CustomMessageHandler>();
        }
#else
            builder.Services.AddHttpClient(Constants.HttpClientNames.CoreApi)
                .ConfigureHttpClient(configureClient => configureClient.BaseAddress = new Uri(coreApiUrl))
                .AddHttpMessageHandler<CustomMessageHandler>();

            builder.Services.AddRefitClient<IIntroductionApiClientV1_0>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(coreApiUrl))
                .AddHttpMessageHandler<CustomMessageHandler>();
#endif


        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<IMainPageViewModel, MainPageViewModel>();

        return builder.Build();
    }
}
