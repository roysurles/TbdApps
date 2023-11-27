namespace RecipeApp.BlazorWasmBootstrap.UnitTests.Shared;

public class TestWebAssemblyHostEnvironment : IWebAssemblyHostEnvironment
{
    public string Environment { get; } = "Development";

    public string BaseAddress { get; } = "https://localhost:5001";
}
