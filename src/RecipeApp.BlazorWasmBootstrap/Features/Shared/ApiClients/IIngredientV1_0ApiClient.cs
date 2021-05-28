using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using RecipeApp.Shared.Features.Ingredient;

using Refit;

using Tbd.Shared.ApiResult;

namespace RecipeApp.BlazorWasmBootstrap.Features.Shared.ApiClients
{
    public interface IIngredientV1_0ApiClient
    {
        [Get("/api/v1.0/Ingredient/{id}")]
        Task<ApiResultModel<IngredientDto>> GetAsync(Guid id);

        [Get("/api/v1.0/Ingredient/AllForIntroductionId/{introductionId}")]
        Task<ApiResultModel<IEnumerable<IngredientDto>>> GetAllForIntroductionIdAsync(Guid introductionId);

        [Post("/api/v1.0/Ingredient")]
        Task<ApiResultModel<IngredientDto>> InsertAsync([Body] IngredientDto ingredientDto);

        [Put("/api/v1.0/Ingredient")]
        Task<ApiResultModel<IngredientDto>> UpdateAsync([Body] IngredientDto ingredientDto);

        [Delete("/api/v1.0/Ingredient/{id}")]
        Task<ApiResultModel<int>> DeleteAsync(Guid id);
    }
}
