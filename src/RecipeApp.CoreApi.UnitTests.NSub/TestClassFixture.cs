namespace RecipeApp.CoreApi.UnitTests.NSub;

public class TestClassFixture
{
    public IServiceProvider ServiceProvider;

    private readonly IConfiguration _configuration;

    public TestClassFixture()
    {
        var services = new ServiceCollection();

        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();

        services.AddSingleton(_configuration);
        services.AddTransient(typeof(IApiResultModel<>), typeof(ApiResultModel<>));

        ServiceProvider = services.BuildServiceProvider();
    }
}
