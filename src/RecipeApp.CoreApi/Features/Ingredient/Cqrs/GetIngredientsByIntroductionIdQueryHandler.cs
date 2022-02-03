using MediatR;

using Microsoft.Extensions.Logging;

using RecipeApp.CoreApi.Features.Ingredient.V1_0;
using RecipeApp.Shared.Features.Ingredient;

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Tbd.Shared.ApiResult;

namespace RecipeApp.CoreApi.Features.Ingredient.Cqrs
{
    public class GetIngredientsByIntroductionIdQueryHandler : IRequestHandler<GetIngredientsByIntroductionIdQuery, IApiResultModel<IEnumerable<IngredientDto>>>
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IMediator _mediator;
        protected readonly ILogger<GetIngredientsByIntroductionIdQueryHandler> _logger;
        protected readonly IIngredientRepositoryV1_0 _ingredientRepository;

        public GetIngredientsByIntroductionIdQueryHandler(IServiceProvider serviceProvider
            , IMediator mediator
            , ILogger<GetIngredientsByIntroductionIdQueryHandler> logger
            , IIngredientRepositoryV1_0 ingredientRepository)
        {
            _serviceProvider = serviceProvider;
            _mediator = mediator;
            _logger = logger;
            _ingredientRepository = ingredientRepository;
        }

        public async Task<IApiResultModel<IEnumerable<IngredientDto>>> Handle(GetIngredientsByIntroductionIdQuery request, CancellationToken cancellationToken)
        {
            var memberName = $"{nameof(GetIngredientsByIntroductionIdQueryHandler)}.{nameof(Handle)}";
            _logger.LogInformation("{methodName}({id})", nameof(GetIngredientsByIntroductionIdQueryHandler), request.IntroductionId);

            var apiResult = _serviceProvider.CreateApiResultModel<IEnumerable<IngredientDto>>();

            _ = request.IntroductionId == Guid.Empty
                ? apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest)
                    .AddErrorMessage("Introduction Id is required.", memberName, HttpStatusCode.BadRequest)
                : apiResult.SetHttpStatusCode(HttpStatusCode.OK)
                    .SetData(await _ingredientRepository.SelectAllForIntroductionIdAsync(request.IntroductionId, cancellationToken))
                    .VerifyDataHasCount(ApiResultMessageModelTypeEnumeration.Information, source: $"{memberName}");

            await _mediator.Publish(new GetIngredientsByIntroductionIdQueryHandlerNotification { ApiResultModel = apiResult }, cancellationToken);

            return apiResult;
        }
    }
}
