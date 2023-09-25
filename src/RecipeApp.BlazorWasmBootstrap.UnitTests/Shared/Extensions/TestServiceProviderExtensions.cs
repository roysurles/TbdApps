namespace RecipeApp.BlazorWasmBootstrap.UnitTests.Shared.Extensions;

public static class TestServiceProviderExtensions
{
    public static TestServiceProvider AddDefaultServices(this TestServiceProvider testServiceProvider)
    {
        testServiceProvider.AddSingleton<IWebAssemblyHostEnvironment, TestWebAssemblyHostEnvironment>();
        testServiceProvider.AddSingleton<ISessionViewModel, SessionViewModel>();

        return testServiceProvider;
    }
}
