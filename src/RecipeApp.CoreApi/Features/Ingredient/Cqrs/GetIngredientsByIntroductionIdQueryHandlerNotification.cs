using MediatR;

using RecipeApp.Shared.Features.Ingredient;

using System.Collections.Generic;

using Tbd.Shared.ApiResult;

namespace RecipeApp.CoreApi.Features.Ingredient.Cqrs
{
    public class GetIngredientsByIntroductionIdQueryHandlerNotification : INotification
    {
        public IApiResultModel<IEnumerable<IngredientDto>> ApiResultModel { get; init; }
    }
}
