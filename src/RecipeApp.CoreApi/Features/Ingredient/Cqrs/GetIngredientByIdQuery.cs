using MediatR;

using RecipeApp.Shared.Features.Ingredient;

using System;

using Tbd.Shared.ApiResult;

namespace RecipeApp.CoreApi.Features.Ingredient.Cqrs
{
    public class GetIngredientByIdQuery : IRequest<IApiResultModel<IngredientDto>>
    {
        public Guid Id { get; set; }        // Ingredient Id
    }
}
