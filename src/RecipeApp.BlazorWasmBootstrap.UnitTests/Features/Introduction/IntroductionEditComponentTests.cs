namespace RecipeApp.BlazorWasmBootstrap.UnitTests.Features.Introduction;

public class IntroductionEditComponentTests : TestContext
{
    protected readonly ITestOutputHelper _output;

    public IntroductionEditComponentTests(ITestOutputHelper output) =>
        _output = output;

    [Fact]
    public void Should_Render()
    {
        // Arrange
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddDefaultServices();

        var mockIntroductionApiClientV1_0 = new Mock<IIntroductionApiClientV1_0>();
        var mockLogger = new Mock<ILogger<IntroductionViewModel>>();

        // Act
        var cut = RenderComponent<IntroductionEditComponent>(parameters =>
            parameters.Add(p => p.IntroductionViewModel, new IntroductionViewModel(mockIntroductionApiClientV1_0.Object, mockLogger.Object)));

        // Assert
        var formHtml = cut.Find("form");

        _output.WriteLine("*** Finished");
    }
}
