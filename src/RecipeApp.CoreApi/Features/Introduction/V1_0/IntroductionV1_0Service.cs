using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using RecipeApp.Shared.Features.Introduction;

using Tbd.Shared.ApiResult;
using Tbd.Shared.Extensions;
using Tbd.WebApi.Shared.Services;

namespace RecipeApp.CoreApi.Features.Introduction.V1_0
{
    public class IntroductionV1_0Service : BaseService, IIntroductionV1_0Service
    {
        protected readonly ILogger<IntroductionV1_0Service> _logger;
        protected readonly IIntroductionV1_0Repository _introductionRepository;

        public IntroductionV1_0Service(IServiceProvider serviceProvider
            , ILogger<IntroductionV1_0Service> logger
            , IIntroductionV1_0Repository introductionRepository) : base(serviceProvider)
        {
            _logger = logger;
            _introductionRepository = introductionRepository;
        }

        public async Task<IApiResultModel<IEnumerable<IntroductionSearchResultDto>>> SearchAsync(IntroductionSearchRequestDto introductionSearchRequestDto)
        {
            _logger.LogInformation($"{nameof(SelectAsync)}({nameof(introductionSearchRequestDto)})");

            var (PaginationMetaData, Data) = await _introductionRepository.SearchAsync(introductionSearchRequestDto).ConfigureAwait(false);

            return CreateApiResultModel<IEnumerable<IntroductionSearchResultDto>>()
                .SetMeta(PaginationMetaData)
                .SetData(Data)
                .VerifyDataHasCount(ApiResultMessageModelTypeEnumeration.Information, setHttpStatusCode: false);
        }

        public async Task<IApiResultModel<IntroductionDto>> SelectAsync(Guid id)
        {
            _logger.LogInformation($"{nameof(SelectAsync)}({id})");

            var apiResult = CreateApiResultModel<IntroductionDto>();

            return id == Guid.Empty
                ? apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest)
                    .AddErrorMessage("Id is required.", $"{nameof(IntroductionV1_0Service)}.{nameof(SelectAsync)}", HttpStatusCode.BadRequest)
                : apiResult.SetHttpStatusCode(HttpStatusCode.OK)
                    .SetData(await _introductionRepository.SelectAsync(id).ConfigureAwait(false))
                    .VerifyDataIsNotNull(ApiResultMessageModelTypeEnumeration.Error, source: $"{nameof(IntroductionV1_0Service)}.{nameof(SelectAsync)}");
        }

        public async Task<IApiResultModel<IntroductionDto>> InsertAsync(IntroductionDto introductionDto, string createdById)
        {
            _logger.LogInformation($"{nameof(InsertAsync)}(IntroductionDto, {createdById})");

            var apiResult = CreateApiResultModel<IntroductionDto>();

            return introductionDto.TryValidateObject(apiResult.Messages)
                ? apiResult.SetHttpStatusCode(HttpStatusCode.Created)
                    .SetData(await _introductionRepository.InsertAsync(introductionDto, createdById).ConfigureAwait(false))
                : apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest);
        }

        public async Task<IApiResultModel<IntroductionDto>> UpdateAsync(IntroductionDto introductionDto, string updatedById)
        {
            _logger.LogInformation($"{nameof(UpdateAsync)}(IntroductionDto, {updatedById})");

            var apiResult = CreateApiResultModel<IntroductionDto>();

            if (introductionDto.Id == Guid.Empty)
            {
                return apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest)
                    .AddErrorMessage("Id is required.", $"{nameof(IntroductionV1_0Service)}.{nameof(UpdateAsync)}", HttpStatusCode.BadRequest);
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
                    .AddErrorMessage("Id is required.", $"{nameof(IntroductionV1_0Service)}.{nameof(DeleteAsync)}", HttpStatusCode.BadRequest)
                : apiResult.SetHttpStatusCode(HttpStatusCode.OK)
                    .SetData(await _introductionRepository.DeleteAsync(id).ConfigureAwait(false));
        }
    }

    public interface IIntroductionV1_0Service
    {
        Task<IApiResultModel<IEnumerable<IntroductionSearchResultDto>>> SearchAsync(IntroductionSearchRequestDto introductionSearchRequestDto);

        Task<IApiResultModel<IntroductionDto>> SelectAsync(Guid id);

        Task<IApiResultModel<IntroductionDto>> InsertAsync(IntroductionDto introductionDto, string createdById);

        Task<IApiResultModel<IntroductionDto>> UpdateAsync(IntroductionDto introductionDto, string updatedById);

        Task<IApiResultModel<int>> DeleteAsync(Guid id);
    }
}
