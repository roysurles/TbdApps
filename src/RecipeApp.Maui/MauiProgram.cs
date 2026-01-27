namespace RecipeApp.Maui;

public static class MauiProgram
{
    public static string CoreApiUrl = "https://localhost:7133";

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

                fonts.AddFont("Font Awesome 6 Brands-Regular-400.otf", "FAB");
                fonts.AddFont("Font Awesome 6 Free-Regular-400.otf", "FAR");
                fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "FAS");
            });

        //var coreApiUrl = "https://localhost:44350";

        builder.Services.AddScoped<CustomMessageHandler>();

#if DEBUG
        builder.Logging.AddDebug();

        if (DeviceInfo.Platform == DevicePlatform.Android)
            CoreApiUrl = CoreApiUrl.Replace("localhost", "10.0.2.2");

        HttpsClientHandlerService httpsClientHandlerService = new();

        if (DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.Android)
        {
            builder.Services.AddHttpClient(Constants.HttpClientNames.CoreApi)
                .ConfigureHttpClient(configureClient => configureClient.BaseAddress = new Uri(CoreApiUrl))
                .ConfigureHttpMessageHandlerBuilder(configureBuilder => configureBuilder.PrimaryHandler = httpsClientHandlerService.GetPlatformMessageHandler())
                .AddHttpMessageHandler<CustomMessageHandler>();

            builder.Services.AddRefitClient<IIntroductionApiClientV1_0>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(CoreApiUrl))
                .ConfigureHttpMessageHandlerBuilder(configureBuilder => configureBuilder.PrimaryHandler = httpsClientHandlerService.GetPlatformMessageHandler())
                .AddHttpMessageHandler<CustomMessageHandler>();

            builder.Services.AddRefitClient<IIngredientApiClientV1_0>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(CoreApiUrl))
                .ConfigureHttpMessageHandlerBuilder(configureBuilder => configureBuilder.PrimaryHandler = httpsClientHandlerService.GetPlatformMessageHandler())
                .AddHttpMessageHandler<CustomMessageHandler>();

            builder.Services.AddRefitClient<IInstructionApiClientV1_0>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(CoreApiUrl))
                .ConfigureHttpMessageHandlerBuilder(configureBuilder => configureBuilder.PrimaryHandler = httpsClientHandlerService.GetPlatformMessageHandler())
                .AddHttpMessageHandler<CustomMessageHandler>();
        }
        else
        {
            builder.Services.AddHttpClientsWithoutPlatformMessageHandler();
        }
#else
            builder.Services.AddHttpClientsWithoutPlatformMessageHandler();
#endif

        builder.Services.AddTransient<IIntroductionSearchViewModel, IntroductionSearchViewModel>();
        builder.Services.AddTransient<IIntroductionViewModel, IntroductionViewModel>();
        builder.Services.AddTransient<IIngredientViewModel, IngredientViewModel>();
        builder.Services.AddTransient<IInstructionViewModel, InstructionViewModel>();

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<IMainPageViewModel, MainPageViewModel>();

        builder.Services.AddTransient<DetailsPage>();
        builder.Services.AddTransient<IDetailsPageViewModel, DetailsPageViewModel>();

        return builder.Build();
    }
}

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddHttpClientsWithoutPlatformMessageHandler(this IServiceCollection services)
    {
        services.AddHttpClient(Constants.HttpClientNames.CoreApi)
            .ConfigureHttpClient(configureClient => configureClient.BaseAddress = new Uri(MauiProgram.CoreApiUrl))
            .AddHttpMessageHandler<CustomMessageHandler>();

        services.AddRefitClient<IIntroductionApiClientV1_0>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(MauiProgram.CoreApiUrl))
            .AddHttpMessageHandler<CustomMessageHandler>();

        services.AddRefitClient<IIngredientApiClientV1_0>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(MauiProgram.CoreApiUrl))
            .AddHttpMessageHandler<CustomMessageHandler>();

        services.AddRefitClient<IInstructionApiClientV1_0>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(MauiProgram.CoreApiUrl))
            .AddHttpMessageHandler<CustomMessageHandler>();

        return services;
    }
}
