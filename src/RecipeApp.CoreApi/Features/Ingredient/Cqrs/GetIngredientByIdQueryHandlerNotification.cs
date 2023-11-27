namespace RecipeApp.CoreApi.Features.Ingredient.Cqrs;

public class GetIngredientByIdQueryHandlerNotification : INotification
{
    public IApiResultModel<IngredientDto> ApiResultModel { get; init; }
}
