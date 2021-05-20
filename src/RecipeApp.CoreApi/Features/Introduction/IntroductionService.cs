using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using RecipeApp.Shared.Features.Introduction;

using Tbd.Shared.ApiResult;
using Tbd.Shared.Extensions;
using Tbd.WebApi.Shared.Services;

namespace RecipeApp.CoreApi.Features.Introduction
{
    public class IntroductionService : BaseService, IIntroductionService
    {
        protected readonly ILogger<IntroductionService> _logger;
        protected readonly IIntroductionRepository _introductionRepository;

        public IntroductionService(IServiceProvider serviceProvider
            , ILogger<IntroductionService> logger
            , IIntroductionRepository introductionRepository) : base(serviceProvider)
        {
            _logger = logger;
            _introductionRepository = introductionRepository;
        }

        public async Task<IApiResultModel<IEnumerable<IntroductionSearchResultDto>>> SearchAsync(IntroductionSearchRequestDto introductionSearchRequestDto)
        {
            _logger.LogInformation($"{nameof(SelectAsync)}({nameof(introductionSearchRequestDto)})");

            return CreateApiResultModel<IEnumerable<IntroductionSearchResultDto>>()
                .SetData(await _introductionRepository.SearchAsync(introductionSearchRequestDto).ConfigureAwait(false))
                .VerifyDataHasCount(ApiResultMessageModelTypeEnumeration.Information);
        }

        public async Task<IApiResultModel<IntroductionDto>> SelectAsync(Guid id)
        {
            _logger.LogInformation($"{nameof(SelectAsync)}({id})");

            var apiResult = CreateApiResultModel<IntroductionDto>();

            return id == Guid.Empty
                ? apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest)
                    .AddErrorMessage("Id is required.", $"{nameof(IntroductionService)}.{nameof(SelectAsync)}", HttpStatusCode.BadRequest)
                : apiResult.SetHttpStatusCode(HttpStatusCode.OK)
                    .SetData(await _introductionRepository.SelectAsync(id).ConfigureAwait(false))
                    .VerifyDataIsNotNull(ApiResultMessageModelTypeEnumeration.Error, source: $"{nameof(IntroductionService)}.{nameof(SelectAsync)}");
        }

        public async Task<IApiResultModel<IntroductionDto>> InsertAsync(IntroductionDto introductionDto, string createdById)
        {
            _logger.LogInformation($"{nameof(InsertAsync)}(IntroductionDto, {createdById})");

            var apiResult = CreateApiResultModel<IntroductionDto>();

            introductionDto.Id = Guid.NewGuid();

            return introductionDto.TryValidateObject(apiResult.Messages)
                ? apiResult.SetHttpStatusCode(HttpStatusCode.Created)
                    .SetData(await _introductionRepository.UpdateAsync(introductionDto, createdById).ConfigureAwait(false))
                : apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest);
        }

        public async Task<IApiResultModel<IntroductionDto>> UpdateAsync(IntroductionDto introductionDto, string updatedById)
        {
            _logger.LogInformation($"{nameof(UpdateAsync)}(IntroductionDto, {updatedById})");

            var apiResult = CreateApiResultModel<IntroductionDto>();

            if (introductionDto.Id == Guid.Empty)
            {
                return apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest)
                    .AddErrorMessage("Id is required.", $"{nameof(IntroductionService)}.{nameof(UpdateAsync)}", HttpStatusCode.BadRequest);
            }

            return introductionDto.TryValidateObject(apiResult.Messages)
                ? apiResult.SetHttpStatusCode(HttpStatusCode.OK)
                    .SetData(await _introductionRepository.UpdateAsync(introductionDto, updatedById).ConfigureAwait(false))
                : apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest);
        }

        public async Task<IApiResultModel<int>> DeleteAsync(Guid id)
        {
            _logger.LogInformation($"{nameof(DeleteAsync)}({id})");

            var apiResult = CreateApiResultModel<int>();

            return id == Guid.Empty
                ? apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest)
                    .AddErrorMessage("Id is required.", $"{nameof(IntroductionService)}.{nameof(DeleteAsync)}", HttpStatusCode.BadRequest)
                : apiResult.SetHttpStatusCode(HttpStatusCode.OK)
                    .SetData(await _introductionRepository.DeleteAsync(id).ConfigureAwait(false));
        }
    }

    public interface IIntroductionService
    {
        Task<IApiResultModel<IEnumerable<IntroductionSearchResultDto>>> SearchAsync(IntroductionSearchRequestDto introductionSearchRequestDto);

        Task<IApiResultModel<IntroductionDto>> SelectAsync(Guid id);

        Task<IApiResultModel<IntroductionDto>> InsertAsync(IntroductionDto introductionDto, string createdById);

        Task<IApiResultModel<IntroductionDto>> UpdateAsync(IntroductionDto introductionDto, string updatedById);

        Task<IApiResultModel<int>> DeleteAsync(Guid id);
    }
}
