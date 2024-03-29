﻿namespace RecipeApp.CoreApi.Features.Instruction.V1_0;

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

    public async Task<IApiResultModel<InstructionDto>> SelectAsync(Guid id, CancellationToken cancellationToken)
    {
        var memberName = $"{_className}.{nameof(SelectAsync)}";
        _logger.LogInformation($"{memberName}({id})");

        var apiResult = CreateApiResultModel<InstructionDto>();

        return id == Guid.Empty
            ? apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest)
                .AddErrorMessage("Id is required.", memberName, HttpStatusCode.BadRequest)
            : apiResult.SetHttpStatusCode(HttpStatusCode.OK)
                .SetData(await _instructionRepository.SelectAsync(id, cancellationToken).ConfigureAwait(false))
                .VerifyDataIsNotNull(ApiResultMessageModelTypeEnumeration.Error, source: memberName);
    }

    public async Task<IApiResultModel<IEnumerable<InstructionDto>>> SelectAllForIntroductionIdAsync(Guid introductionId, CancellationToken cancellationToken)
    {
        var memberName = $"{_className}.{nameof(SelectAllForIntroductionIdAsync)}";
        _logger.LogInformation($"{memberName}({introductionId})");

        var apiResult = CreateApiResultModel<IEnumerable<InstructionDto>>();

        return Equals(introductionId, Guid.Empty)
            ? apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest)
                .AddErrorMessage("Introduction Id is required.", memberName, HttpStatusCode.BadRequest)
            : apiResult.SetHttpStatusCode(HttpStatusCode.OK)
                .SetData(await _instructionRepository.SelectAllForIntroductionIdAsync(introductionId, cancellationToken).ConfigureAwait(false))
                .VerifyDataHasCount(ApiResultMessageModelTypeEnumeration.Information, source: memberName, setHttpStatusCode: false);
    }

    public async Task<IApiResultModel<InstructionDto>> InsertAsync(InstructionDto instructionDto, string createdById, CancellationToken cancellationToken)
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
                .SetData(await _instructionRepository.InsertAsync(instructionDto, createdById, cancellationToken).ConfigureAwait(false))
            : apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest);
    }

    public async Task<IApiResultModel<InstructionDto>> UpdateAsync(InstructionDto instructionDto, string updatedById, CancellationToken cancellationToken)
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
                .SetData(await _instructionRepository.UpdateAsync(instructionDto, updatedById, cancellationToken).ConfigureAwait(false))
            : apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest);
    }

    public async Task<IApiResultModel<int>> UpdateMultipleAsync(InstructionsDto instructionsDto, string updatedById, CancellationToken cancellationToken)
    {
        var memberName = $"{_className}.{nameof(UpdateAsync)}";
        _logger.LogInformation("{memberName}(InstructionsDto, {updatedById}, CancellationToken)", memberName, updatedById);

        var apiResult = CreateApiResultModel<int>().SetHttpStatusCode(HttpStatusCode.OK).SetData(0);

        return instructionsDto.Instructions.Count == 0
            ? apiResult
            : apiResult.SetData(await _instructionRepository.UpdateMultipleAsync(instructionsDto, updatedById, cancellationToken));
    }

    public async Task<IApiResultModel<int>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var memberName = $"{_className}.{nameof(DeleteAsync)}";
        _logger.LogInformation($"{memberName}({id})");

        var apiResult = CreateApiResultModel<int>();

        return id == Guid.Empty
            ? apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest)
                .AddErrorMessage("Id is required.", memberName, HttpStatusCode.BadRequest)
            : apiResult.SetHttpStatusCode(HttpStatusCode.OK)
                .SetData(await _instructionRepository.DeleteAsync(id, cancellationToken).ConfigureAwait(false));
    }
}

public interface IInstructionServiceV1_0
{
    Task<IApiResultModel<InstructionDto>> SelectAsync(Guid id, CancellationToken cancellationToken);

    Task<IApiResultModel<IEnumerable<InstructionDto>>> SelectAllForIntroductionIdAsync(Guid introductionId, CancellationToken cancellationToken);

    Task<IApiResultModel<InstructionDto>> InsertAsync(InstructionDto instructionDto, string createdById, CancellationToken cancellationToken);

    Task<IApiResultModel<InstructionDto>> UpdateAsync(InstructionDto instructionDto, string updatedById, CancellationToken cancellationToken);

    Task<IApiResultModel<int>> UpdateMultipleAsync(InstructionsDto instructionsDto, string updatedById, CancellationToken cancellationToken);

    Task<IApiResultModel<int>> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
