namespace RecipeApp.CoreApi.Features.Ingredient.Cqrs;

public class GetIngredientsByIntroductionIdQueryHandlerNotification : INotification
{
    public IApiResultModel<IEnumerable<IngredientDto>> ApiResultModel { get; init; }
}
