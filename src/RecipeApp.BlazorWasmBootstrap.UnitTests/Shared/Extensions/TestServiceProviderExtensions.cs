namespace RecipeApp.BlazorWasmBootstrap.UnitTests.Shared.Extensions;

public static class TestServiceProviderExtensions
{
    public static IServiceCollection AddDefaultServices(this IServiceCollection services)
    {
        services.AddSingleton<IWebAssemblyHostEnvironment, TestWebAssemblyHostEnvironment>();
        services.AddSingleton<ISessionViewModel, SessionViewModel>();

        return services;
    }
}
