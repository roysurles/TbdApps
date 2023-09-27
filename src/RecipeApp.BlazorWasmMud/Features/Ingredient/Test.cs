

namespace RecipeApp.BlazorWasmMud.Features.Ingredient;

public class Test
{
    // , Func<IngredientDto, Task<IIngredientViewModel>> func
    public async Task MoveIngredientAsync(IngredientDto ingredientDto
        , Expression<Func<IngredientDto, Task<IIngredientViewModel>>> expr)
    {
        var func = expr.Compile();
        var answer = await func(ingredientDto);
    }
}
