using MediatR;

using RecipeApp.Shared.Features.Ingredient;

using System;
using System.Collections.Generic;

using Tbd.Shared.ApiResult;

namespace RecipeApp.CoreApi.Features.Ingredient.Cqrs
{
    public class GetIngredientsByIntroductionIdQuery : IRequest<IApiResultModel<IEnumerable<IngredientDto>>>
    {
        public Guid IntroductionId { get; set; }        // Introduction Id
    }
}
