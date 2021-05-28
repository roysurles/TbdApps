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
    public class InstructionV1_0Service : BaseService, IInstructionV1_0Service
    {
        protected readonly ILogger<InstructionV1_0Service> _logger;
        protected readonly IInstructionV1_0Repository _instructionRepository;

        public InstructionV1_0Service(IServiceProvider serviceProvider
            , ILogger<InstructionV1_0Service> logger
            , IInstructionV1_0Repository instructionRepository) : base(serviceProvider)
        {
            _logger = logger;
            _instructionRepository = instructionRepository;
        }

        public async Task<IApiResultModel<InstructionDto>> SelectAsync(Guid id)
        {
            _logger.LogInformation($"{nameof(SelectAsync)}({id})");

            var apiResult = CreateApiResultModel<InstructionDto>();

            return id == Guid.Empty
                ? apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest)
                    .AddErrorMessage("Id is required.", $"{nameof(InstructionV1_0Service)}.{nameof(SelectAsync)}", HttpStatusCode.BadRequest)
                : apiResult.SetHttpStatusCode(HttpStatusCode.OK)
                    .SetData(await _instructionRepository.SelectAsync(id).ConfigureAwait(false))
                    .VerifyDataIsNotNull(ApiResultMessageModelTypeEnumeration.Error, source: $"{nameof(InstructionV1_0Service)}.{nameof(SelectAsync)}");
        }

        public async Task<IApiResultModel<IEnumerable<InstructionDto>>> SelectAllForIntroductionIdAsync(Guid introductionId)
        {
            _logger.LogInformation($"{nameof(SelectAllForIntroductionIdAsync)}({introductionId})");

            var apiResult = CreateApiResultModel<IEnumerable<InstructionDto>>();

            return introductionId == Guid.Empty
                ? apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest)
                    .AddErrorMessage("Id is required.", $"{nameof(InstructionV1_0Service)}.{nameof(SelectAsync)}", HttpStatusCode.BadRequest)
                : apiResult.SetHttpStatusCode(HttpStatusCode.OK)
                    .SetData(await _instructionRepository.SelectAllForIntroductionIdAsync(introductionId).ConfigureAwait(false))
                    .VerifyDataIsNotNull(ApiResultMessageModelTypeEnumeration.Error, source: $"{nameof(InstructionV1_0Service)}.{nameof(SelectAsync)}");
        }

        public async Task<IApiResultModel<InstructionDto>> InsertAsync(InstructionDto instructionDto, string createdById)
        {
            _logger.LogInformation($"{nameof(InsertAsync)}({nameof(instructionDto)}, {createdById})");

            var apiResult = CreateApiResultModel<InstructionDto>();

            return instructionDto.TryValidateObject(apiResult.Messages)
                ? apiResult.SetHttpStatusCode(HttpStatusCode.Created)
                    .SetData(await _instructionRepository.InsertAsync(instructionDto, createdById).ConfigureAwait(false))
                : apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest);
        }

        public async Task<IApiResultModel<InstructionDto>> UpdateAsync(InstructionDto instructionDto, string updatedById)
        {
            _logger.LogInformation($"{nameof(UpdateAsync)}({nameof(instructionDto)}, {updatedById})");

            var apiResult = CreateApiResultModel<InstructionDto>();

            if (instructionDto.Id == Guid.Empty)
            {
                return apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest)
                    .AddErrorMessage("Id is required.", $"{nameof(InstructionV1_0Service)}.{nameof(UpdateAsync)}", HttpStatusCode.BadRequest);
            }

            return instructionDto.TryValidateObject(apiResult.Messages)
                ? apiResult.SetHttpStatusCode(HttpStatusCode.OK)
                    .SetData(await _instructionRepository.UpdateAsync(instructionDto, updatedById).ConfigureAwait(false))
                : apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest);
        }

        public async Task<IApiResultModel<int>> DeleteAsync(Guid id)
        {
            _logger.LogInformation($"{nameof(DeleteAsync)}({id})");

            var apiResult = CreateApiResultModel<int>();

            return id == Guid.Empty
                ? apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest)
                    .AddErrorMessage("Id is required.", $"{nameof(InstructionV1_0Service)}.{nameof(DeleteAsync)}", HttpStatusCode.BadRequest)
                : apiResult.SetHttpStatusCode(HttpStatusCode.OK)
                    .SetData(await _instructionRepository.DeleteAsync(id).ConfigureAwait(false));
        }
    }

    public interface IInstructionV1_0Service
    {
        Task<IApiResultModel<InstructionDto>> SelectAsync(Guid id);

        Task<IApiResultModel<IEnumerable<InstructionDto>>> SelectAllForIntroductionIdAsync(Guid introductionId);

        Task<IApiResultModel<InstructionDto>> InsertAsync(InstructionDto instructionDto, string createdById);

        Task<IApiResultModel<InstructionDto>> UpdateAsync(InstructionDto instructionDto, string updatedById);

        Task<IApiResultModel<int>> DeleteAsync(Guid id);
    }
}
