namespace RecipeApp.CoreApi.UnitTests.Features.Instruction.V1_0;

public partial class InstructionServiceTests : IClassFixture<TestClassFixture>
{
    protected readonly IServiceProvider _serviceProvider;
    protected readonly ITestOutputHelper _testOutputHelper;
    protected readonly ILogger<InstructionServiceV1_0> _instructionServiceLogger;
    protected readonly Mock<IInstructionRepositoryV1_0> _instructionRepositoryMock;
    protected readonly IInstructionServiceV1_0 _instructionService;

    public InstructionServiceTests(TestClassFixture testClassFixture, ITestOutputHelper testOutputHelper)
    {
        _serviceProvider = testClassFixture.ServiceProvider;
        _testOutputHelper = testOutputHelper;

        _instructionServiceLogger = new LoggerFactory().CreateLogger<InstructionServiceV1_0>();

        _instructionRepositoryMock = new Mock<IInstructionRepositoryV1_0>();

        _instructionService = new InstructionServiceV1_0(_serviceProvider
            , _instructionServiceLogger
            , _instructionRepositoryMock.Object);

        _testOutputHelper.WriteLine("InstructionServiceTests initialized.");
    }
}
