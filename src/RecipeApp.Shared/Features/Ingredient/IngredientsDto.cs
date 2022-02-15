using System.Collections.Generic;

namespace RecipeApp.Shared.Features.Ingredient
{
    public class IngredientsDto
    {
        public List<IngredientDto> Ingredients { get; set; } =
            new List<IngredientDto>();
    }
}
