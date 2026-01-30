namespace RecipeApp.CoreApi.UnitTests.NSub.Features.Introduction.V1_0;

public partial class IntroductionServiceTests : IClassFixture<TestClassFixture>
{
    protected readonly IServiceProvider _serviceProvider;
    protected readonly ITestOutputHelper _testOutputHelper;
    protected readonly ILogger<IntroductionServiceV1_0> _introductionServiceLogger;
    protected readonly IIntroductionRepositoryV1_0 _introductionRepositoryMock;
    protected readonly IIntroductionServiceV1_0 _introductionService;

    public IntroductionServiceTests(TestClassFixture testClassFixture, ITestOutputHelper testOutputHelper)
    {
        _serviceProvider = testClassFixture.ServiceProvider;
        _testOutputHelper = testOutputHelper;

        _introductionServiceLogger = new LoggerFactory().CreateLogger<IntroductionServiceV1_0>();

        _introductionRepositoryMock = Substitute.For<IIntroductionRepositoryV1_0>();

        _introductionService = new IntroductionServiceV1_0(_serviceProvider
            , _introductionServiceLogger
            , _introductionRepositoryMock);

        _testOutputHelper.WriteLine("IntroductionServiceTests initialized.");
    }
}
