namespace RecipeApp.BlazorWasmBootstrap.UnitTests.Features.Index;

public class IndexPageTests : TestContext
{
    protected readonly ITestOutputHelper _output;

    public IndexPageTests(ITestOutputHelper output) =>
        _output = output;

    [Fact]
    public void Should_Render()
    {
        // Arrange
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddDefaultServices();

        var mockIntroductionApiClientV1_0 = new Mock<IIntroductionApiClientV1_0>();
        var mockIntroductionApiClientNativeV1_0 = new Mock<IIntroductionApiClientNativeV1_0>();
        var mockLogger = new Mock<ILogger<IntroductionSearchViewModel>>();
        Services.AddTransient<IIntroductionSearchViewModel>(_ => new IntroductionSearchViewModel(mockIntroductionApiClientV1_0.Object
            , mockIntroductionApiClientNativeV1_0.Object
            , mockLogger.Object));

        // Act
        var cut = RenderComponent<IndexPage>();

        // Assert
        var divHtml = cut.Find("div");
    }
}
