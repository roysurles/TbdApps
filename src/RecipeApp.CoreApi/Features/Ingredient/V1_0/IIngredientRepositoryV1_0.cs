using RecipeApp.Shared.Features.Ingredient;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RecipeApp.CoreApi.Features.Ingredient.V1_0
{
    public interface IIngredientRepositoryV1_0
    {
        Task<IngredientDto> SelectAsync(Guid id, CancellationToken cancellationToken);

        Task<IEnumerable<IngredientDto>> SelectAllForIntroductionIdAsync(Guid introductionId, CancellationToken cancellationToken);

        Task<IngredientDto> InsertAsync(IngredientDto ingredientDto, string createdById, CancellationToken cancellationToken);

        Task<IngredientDto> UpdateAsync(IngredientDto ingredientDto, string updatedById, CancellationToken cancellationToken);

        Task<int> UpdateMultipleAsync(IngredientsDto ingredientsDto, string updatedById, CancellationToken cancellationToken);

        Task<int> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
