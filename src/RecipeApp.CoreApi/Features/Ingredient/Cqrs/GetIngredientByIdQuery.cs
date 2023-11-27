namespace RecipeApp.CoreApi.Features.Ingredient.Cqrs;

public class GetIngredientByIdQuery : IRequest<IApiResultModel<IngredientDto>>
{
    public Guid Id { get; set; }        // Ingredient Id
}
