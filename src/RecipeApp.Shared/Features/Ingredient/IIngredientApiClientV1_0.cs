namespace RecipeApp.Shared.Features.Ingredient;

public interface IIngredientApiClientV1_0
{
    [Get("/api/v1.0/Ingredient/{id}")]
    Task<ApiResultModel<IngredientDto>> GetAsync(Guid id);

    [Get("/api/v1.0/Ingredient/AllForIntroductionId/{introductionId}")]
    Task<ApiResultModel<List<IngredientDto>>> GetAllForIntroductionIdAsync(Guid introductionId);

    [Post("/api/v1.0/Ingredient")]
    Task<ApiResultModel<IngredientDto>> InsertAsync([Body] IngredientDto ingredientDto);

    [Put("/api/v1.0/Ingredient")]
    Task<ApiResultModel<IngredientDto>> UpdateAsync([Body] IngredientDto ingredientDto);

    [Put("/api/v1.0/Ingredient/Multiple")]
    Task<ApiResultModel<int>> UpdateMultipleAsync([Body] IngredientsDto ingredientsDto);

    [Delete("/api/v1.0/Ingredient/{id}")]
    Task<ApiResultModel<int>> DeleteAsync(Guid id);
}
