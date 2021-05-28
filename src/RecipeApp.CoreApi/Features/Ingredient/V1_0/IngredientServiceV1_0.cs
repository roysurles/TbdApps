using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using RecipeApp.Shared.Features.Ingredient;

using Tbd.Shared.ApiResult;
using Tbd.Shared.Extensions;
using Tbd.WebApi.Shared.Services;

namespace RecipeApp.CoreApi.Features.Ingredient.V1_0
{
    public class IngredientServiceV1_0 : BaseService, IIngredientServiceV1_0
    {
        protected readonly ILogger<IngredientServiceV1_0> _logger;
        protected readonly IIngredientRepositoryV1_0 _ingredientRepository;

        public IngredientServiceV1_0(IServiceProvider serviceProvider
            , ILogger<IngredientServiceV1_0> logger
            , IIngredientRepositoryV1_0 ingredientRepository) : base(serviceProvider)
        {
            _logger = logger;
            _ingredientRepository = ingredientRepository;
        }

        public async Task<IApiResultModel<IngredientDto>> SelectAsync(Guid id)
        {
            _logger.LogInformation($"{nameof(SelectAsync)}({id})");

            var apiResult = CreateApiResultModel<IngredientDto>();

            return id == Guid.Empty
                ? apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest)
                    .AddErrorMessage("Id is required.", $"{nameof(IngredientServiceV1_0)}.{nameof(SelectAsync)}", HttpStatusCode.BadRequest)
                : apiResult.SetHttpStatusCode(HttpStatusCode.OK)
                    .SetData(await _ingredientRepository.SelectAsync(id).ConfigureAwait(false))
                    .VerifyDataIsNotNull(ApiResultMessageModelTypeEnumeration.Error, source: $"{nameof(IngredientServiceV1_0)}.{nameof(SelectAsync)}");
        }

        public async Task<IApiResultModel<IEnumerable<IngredientDto>>> SelectAllForIntroductionIdAsync(Guid introductionId)
        {
            _logger.LogInformation($"{nameof(SelectAllForIntroductionIdAsync)}({introductionId})");

            var apiResult = CreateApiResultModel<IEnumerable<IngredientDto>>();

            return introductionId == Guid.Empty
                ? apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest)
                    .AddErrorMessage("Id is required.", $"{nameof(IngredientServiceV1_0)}.{nameof(SelectAsync)}", HttpStatusCode.BadRequest)
                : apiResult.SetHttpStatusCode(HttpStatusCode.OK)
                    .SetData(await _ingredientRepository.SelectAllForIntroductionIdAsync(introductionId).ConfigureAwait(false))
                    .VerifyDataIsNotNull(ApiResultMessageModelTypeEnumeration.Error, source: $"{nameof(IngredientServiceV1_0)}.{nameof(SelectAsync)}");
        }

        public async Task<IApiResultModel<IngredientDto>> InsertAsync(IngredientDto ingredientDto, string createdById)
        {
            _logger.LogInformation($"{nameof(InsertAsync)}({nameof(ingredientDto)}, {createdById})");

            var apiResult = CreateApiResultModel<IngredientDto>();

            return ingredientDto.TryValidateObject(apiResult.Messages)
                ? apiResult.SetHttpStatusCode(HttpStatusCode.Created)
                    .SetData(await _ingredientRepository.InsertAsync(ingredientDto, createdById).ConfigureAwait(false))
                : apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest);
        }

        public async Task<IApiResultModel<IngredientDto>> UpdateAsync(IngredientDto ingredientDto, string updatedById)
        {
            _logger.LogInformation($"{nameof(UpdateAsync)}({nameof(ingredientDto)}, {updatedById})");

            var apiResult = CreateApiResultModel<IngredientDto>();

            if (ingredientDto.Id == Guid.Empty)
            {
                return apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest)
                    .AddErrorMessage("Id is required.", $"{nameof(IngredientServiceV1_0)}.{nameof(UpdateAsync)}", HttpStatusCode.BadRequest);
            }

            return ingredientDto.TryValidateObject(apiResult.Messages)
                ? apiResult.SetHttpStatusCode(HttpStatusCode.OK)
                    .SetData(await _ingredientRepository.UpdateAsync(ingredientDto, updatedById).ConfigureAwait(false))
                : apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest);
        }

        public async Task<IApiResultModel<int>> DeleteAsync(Guid id)
        {
            _logger.LogInformation($"{nameof(DeleteAsync)}({id})");

            var apiResult = CreateApiResultModel<int>();

            return id == Guid.Empty
                ? apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest)
                    .AddErrorMessage("Id is required.", $"{nameof(IngredientServiceV1_0)}.{nameof(DeleteAsync)}", HttpStatusCode.BadRequest)
                : apiResult.SetHttpStatusCode(HttpStatusCode.OK)
                    .SetData(await _ingredientRepository.DeleteAsync(id).ConfigureAwait(false));
        }
    }

    public interface IIngredientServiceV1_0
    {
        Task<IApiResultModel<IngredientDto>> SelectAsync(Guid id);

        Task<IApiResultModel<IEnumerable<IngredientDto>>> SelectAllForIntroductionIdAsync(Guid introductionId);

        Task<IApiResultModel<IngredientDto>> InsertAsync(IngredientDto ingredientDto, string createdById);

        Task<IApiResultModel<IngredientDto>> UpdateAsync(IngredientDto ingredientDto, string updatedById);

        Task<IApiResultModel<int>> DeleteAsync(Guid id);
    }
}
