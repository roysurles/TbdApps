namespace RecipeApp.CoreApi.UnitTests.Features.Ingredient.V1_0;

public partial class IngredientServiceTests
{
    protected readonly ServiceProvider _serviceProvider;
    protected readonly ILogger<IngredientServiceV1_0> _ingredientServiceLogger;
    protected readonly Mock<IIngredientRepositoryV1_0> _ingredientRepositoryMock;
    protected readonly IIngredientServiceV1_0 _ingredientService;

    public IngredientServiceTests(ServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        _ingredientServiceLogger = new LoggerFactory().CreateLogger<IngredientServiceV1_0>();

        _ingredientRepositoryMock = new Mock<IIngredientRepositoryV1_0>();

        _ingredientService = new IngredientServiceV1_0(_serviceProvider
            , _ingredientServiceLogger
            , _ingredientRepositoryMock.Object);
    }
}
