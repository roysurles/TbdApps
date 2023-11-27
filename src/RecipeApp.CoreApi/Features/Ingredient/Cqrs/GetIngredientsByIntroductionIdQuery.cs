namespace RecipeApp.CoreApi.Features.Ingredient.Cqrs;

public class GetIngredientsByIntroductionIdQuery : IRequest<IApiResultModel<IEnumerable<IngredientDto>>>
{
    public Guid IntroductionId { get; set; }        // Introduction Id
}
