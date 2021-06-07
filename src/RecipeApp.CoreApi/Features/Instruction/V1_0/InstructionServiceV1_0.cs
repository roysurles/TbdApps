using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using RecipeApp.Shared.Features.Instruction;

using Tbd.Shared.ApiResult;
using Tbd.Shared.Extensions;
using Tbd.WebApi.Shared.Services;

namespace RecipeApp.CoreApi.Features.Instruction.V1_0
{
    public class InstructionServiceV1_0 : BaseService, IInstructionServiceV1_0
    {
        protected readonly ILogger<InstructionServiceV1_0> _logger;
        protected readonly IInstructionRepositoryV1_0 _instructionRepository;
        protected readonly string _className = nameof(InstructionServiceV1_0);

        public InstructionServiceV1_0(IServiceProvider serviceProvider
            , ILogger<InstructionServiceV1_0> logger
            , IInstructionRepositoryV1_0 instructionRepository) : base(serviceProvider)
        {
            _logger = logger;
            _instructionRepository = instructionRepository;
        }

        public async Task<IApiResultModel<InstructionDto>> SelectAsync(Guid id)
        {
            var memberName = $"{_className}.{nameof(SelectAsync)}";
            _logger.LogInformation($"{memberName}({id})");

            var apiResult = CreateApiResultModel<InstructionDto>();

            return id == Guid.Empty
                ? apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest)
                    .AddErrorMessage("Id is required.", memberName, HttpStatusCode.BadRequest)
                : apiResult.SetHttpStatusCode(HttpStatusCode.OK)
                    .SetData(await _instructionRepository.SelectAsync(id).ConfigureAwait(false))
                    .VerifyDataIsNotNull(ApiResultMessageModelTypeEnumeration.Error, source: memberName);
        }

        public async Task<IApiResultModel<IEnumerable<InstructionDto>>> SelectAllForIntroductionIdAsync(Guid introductionId)
        {
            var memberName = $"{_className}.{nameof(SelectAllForIntroductionIdAsync)}";
            _logger.LogInformation($"{memberName}({introductionId})");

            var apiResult = CreateApiResultModel<IEnumerable<InstructionDto>>();

            return Equals(introductionId, Guid.Empty)
                ? apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest)
                    .AddErrorMessage("Introduction Id is required.", memberName, HttpStatusCode.BadRequest)
                : apiResult.SetHttpStatusCode(HttpStatusCode.OK)
                    .SetData(await _instructionRepository.SelectAllForIntroductionIdAsync(introductionId).ConfigureAwait(false))
                    .VerifyDataHasCount(ApiResultMessageModelTypeEnumeration.Information, source: memberName, setHttpStatusCode: false);
        }

        public async Task<IApiResultModel<InstructionDto>> InsertAsync(InstructionDto instructionDto, string createdById)
        {
            var memberName = $"{_className}.{nameof(InsertAsync)}";
            _logger.LogInformation($"{memberName}({nameof(instructionDto)}, {createdById})");

            var apiResult = CreateApiResultModel<InstructionDto>();

            if (instructionDto.IntroductionId == Guid.Empty)
            {
                return apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest)
                    .AddErrorMessage("Introduction Id is required.", memberName, HttpStatusCode.BadRequest);
            }

            return instructionDto.TryValidateObject(apiResult.Messages)
                ? apiResult.SetHttpStatusCode(HttpStatusCode.Created)
                    .SetData(await _instructionRepository.InsertAsync(instructionDto, createdById).ConfigureAwait(false))
                : apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest);
        }

        public async Task<IApiResultModel<InstructionDto>> UpdateAsync(InstructionDto instructionDto, string updatedById)
        {
            var memberName = $"{_className}.{nameof(UpdateAsync)}";
            _logger.LogInformation($"{memberName}({nameof(instructionDto)}, {updatedById})");

            var apiResult = CreateApiResultModel<InstructionDto>();

            if (instructionDto.IntroductionId == Guid.Empty)
            {
                return apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest)
                    .AddErrorMessage("Introduction Id is required.", memberName, HttpStatusCode.BadRequest);
            }

            if (instructionDto.Id == Guid.Empty)
            {
                return apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest)
                    .AddErrorMessage("Id is required.", memberName, HttpStatusCode.BadRequest);
            }

            return instructionDto.TryValidateObject(apiResult.Messages)
                ? apiResult.SetHttpStatusCode(HttpStatusCode.OK)
                    .SetData(await _instructionRepository.UpdateAsync(instructionDto, updatedById).ConfigureAwait(false))
                : apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest);
        }

        public async Task<IApiResultModel<int>> DeleteAsync(Guid id)
        {
            var memberName = $"{_className}.{nameof(DeleteAsync)}";
            _logger.LogInformation($"{memberName}({id})");

            var apiResult = CreateApiResultModel<int>();

            return id == Guid.Empty
                ? apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest)
                    .AddErrorMessage("Id is required.", memberName, HttpStatusCode.BadRequest)
                : apiResult.SetHttpStatusCode(HttpStatusCode.OK)
                    .SetData(await _instructionRepository.DeleteAsync(id).ConfigureAwait(false));
        }
    }

    public interface IInstructionServiceV1_0
    {
        Task<IApiResultModel<InstructionDto>> SelectAsync(Guid id);

        Task<IApiResultModel<IEnumerable<InstructionDto>>> SelectAllForIntroductionIdAsync(Guid introductionId);

        Task<IApiResultModel<InstructionDto>> InsertAsync(InstructionDto instructionDto, string createdById);

        Task<IApiResultModel<InstructionDto>> UpdateAsync(InstructionDto instructionDto, string updatedById);

        Task<IApiResultModel<int>> DeleteAsync(Guid id);
    }
}
