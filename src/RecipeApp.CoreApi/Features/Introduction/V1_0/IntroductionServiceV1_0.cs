﻿using System;
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
    public class IntroductionServiceV1_0 : BaseService, IIntroductionServiceV1_0
    {
        protected readonly ILogger<IntroductionServiceV1_0> _logger;
        protected readonly IIntroductionRepositoryV1_0 _introductionRepository;

        public IntroductionServiceV1_0(IServiceProvider serviceProvider
            , ILogger<IntroductionServiceV1_0> logger
            , IIntroductionRepositoryV1_0 introductionRepository) : base(serviceProvider)
        {
            _logger = logger;
            _introductionRepository = introductionRepository;
        }

        public async Task<IApiResultModel<IEnumerable<IntroductionSearchResultDto>>> SearchAsync(IntroductionSearchRequestDto introductionSearchRequestDto)
        {
            _logger.LogInformation($"{nameof(SearchAsync)}({nameof(introductionSearchRequestDto)})");

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
                    .AddErrorMessage("Id is required.", $"{nameof(IntroductionServiceV1_0)}.{nameof(SelectAsync)}", HttpStatusCode.BadRequest)
                : apiResult.SetHttpStatusCode(HttpStatusCode.OK)
                    .SetData(await _introductionRepository.SelectAsync(id).ConfigureAwait(false))
                    .VerifyDataIsNotNull(ApiResultMessageModelTypeEnumeration.Error, source: $"{nameof(IntroductionServiceV1_0)}.{nameof(SelectAsync)}");
        }

        public async Task<IApiResultModel<IntroductionDto>> InsertAsync(IntroductionDto introductionDto, string createdById)
        {
            _logger.LogInformation($"{nameof(InsertAsync)}({nameof(introductionDto)}, {createdById})");

            var apiResult = CreateApiResultModel<IntroductionDto>();

            return introductionDto.TryValidateObject(apiResult.Messages)
                ? apiResult.SetHttpStatusCode(HttpStatusCode.Created)
                    .SetData(await _introductionRepository.InsertAsync(introductionDto, createdById).ConfigureAwait(false))
                : apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest);
        }

        public async Task<IApiResultModel<IntroductionDto>> UpdateAsync(IntroductionDto introductionDto, string updatedById)
        {
            _logger.LogInformation($"{nameof(UpdateAsync)}({nameof(introductionDto)}, {updatedById})");

            var apiResult = CreateApiResultModel<IntroductionDto>();

            if (introductionDto.Id == Guid.Empty)
            {
                return apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest)
                    .AddErrorMessage("Id is required.", $"{nameof(IntroductionServiceV1_0)}.{nameof(UpdateAsync)}", HttpStatusCode.BadRequest);
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
                    .AddErrorMessage("Id is required.", $"{nameof(IntroductionServiceV1_0)}.{nameof(DeleteAsync)}", HttpStatusCode.BadRequest)
                : apiResult.SetHttpStatusCode(HttpStatusCode.OK)
                    .SetData(await _introductionRepository.DeleteAsync(id).ConfigureAwait(false));
        }
    }

    public interface IIntroductionServiceV1_0
    {
        Task<IApiResultModel<IEnumerable<IntroductionSearchResultDto>>> SearchAsync(IntroductionSearchRequestDto introductionSearchRequestDto);

        Task<IApiResultModel<IntroductionDto>> SelectAsync(Guid id);

        Task<IApiResultModel<IntroductionDto>> InsertAsync(IntroductionDto introductionDto, string createdById);

        Task<IApiResultModel<IntroductionDto>> UpdateAsync(IntroductionDto introductionDto, string updatedById);

        Task<IApiResultModel<int>> DeleteAsync(Guid id);
    }
}