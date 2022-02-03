using MediatR;

using RecipeApp.Shared.Features.Ingredient;

using Tbd.Shared.ApiResult;

namespace RecipeApp.CoreApi.Features.Ingredient.Cqrs
{
    public class GetIngredientByIdQueryHandlerNotification : INotification
    {
        public IApiResultModel<IngredientDto> ApiResultModel { get; init; }
    }
}
