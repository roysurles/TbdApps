using System;

namespace RecipeApp.Shared.Features.Ingredient
{
    public static class IngredientDtosExtensions
    {
        public static object ToInsertParameters(this IngredientDto ingredientDto
            , Guid id, string createdById, DateTime? createdOnUtc)
        {
            return new
            {
                Id = ingredientDto.Id = id,
                ingredientDto.IntroductionId,
                ingredientDto.SortOrder,
                ingredientDto.Measurement,
                ingredientDto.Description,
                CreatedById = ingredientDto.CreatedById = createdById,
                createdOnUtc = ingredientDto.CreatedOnUtc = createdOnUtc
            };
        }

        public static object ToUpdateParameters(this IngredientDto ingredientDto
            , string updatedById, DateTime? updatedOnUtc)
        {
            return new
            {
                ingredientDto.Id,
                ingredientDto.SortOrder,
                ingredientDto.Measurement,
                ingredientDto.Description,
                updatedById = ingredientDto.UpdatedById = updatedById,
                UpdatedOnUtc = ingredientDto.UpdatedOnUtc = updatedOnUtc
            };
        }
    }
}
