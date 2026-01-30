namespace RecipeApp.CoreApi.UnitTests.NSub.Features.Ingredient.V1_0;

public partial class IngredientServiceTests : IClassFixture<TestClassFixture>
{
    protected readonly IServiceProvider _serviceProvider;
    protected readonly ITestOutputHelper _testOutputHelper;
    protected readonly ILogger<IngredientServiceV1_0> _ingredientServiceLogger;
    protected readonly IIngredientRepositoryV1_0 _ingredientRepositoryMock;
    protected readonly IIngredientServiceV1_0 _ingredientService;

    public IngredientServiceTests(TestClassFixture testClassFixture, ITestOutputHelper testOutputHelper)
    {
        _serviceProvider = testClassFixture.ServiceProvider;
        _testOutputHelper = testOutputHelper;

        _ingredientServiceLogger = new LoggerFactory().CreateLogger<IngredientServiceV1_0>();

        _ingredientRepositoryMock = Substitute.For<IIngredientRepositoryV1_0>();

        _ingredientService = new IngredientServiceV1_0(_serviceProvider
            , _ingredientServiceLogger
            , _ingredientRepositoryMock);

        _testOutputHelper.WriteLine("IngredientServiceTests initialized.");
    }
}