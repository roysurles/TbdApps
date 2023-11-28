namespace RecipeApp.CoreApi.Features.Instruction.V1_0;

/// <summary>
/// Instruction Api Controller
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/Instruction")]
[ApiController]
[AllowAnonymous]
public class InstructionController : BaseApiController
{
    protected readonly IInstructionServiceV1_0 _instructionService;

    public InstructionController(IInstructionServiceV1_0 instructionService) =>
        _instructionService = instructionService;

    /// <summary>
    /// Get InstructionDto for the desired Instruction Id.
    /// </summary>
    /// <param name="id">Desired Instruction Id</param>
    /// <param name="cancellationToken">CancellationToken in case client cancels this method</param>
    /// <returns>IApiResultModel of InstructionDto</returns>
    /// <response code="200">OK - returns IApiResultModel of InstructionDto</response>
    /// <response code="400">BadRequest - missing Id</response>
    /// <response code="404">NotFound - No data found for the desired Id</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IApiResultModel<InstructionDto>>> GetAsync(Guid id, CancellationToken cancellationToken) =>
        CreateActionResult(await _instructionService.SelectAsync(id, cancellationToken).ConfigureAwait(false));

    /// <summary>
    /// Get all InstructionDto for the desired Introduction Id.
    /// </summary>
    /// <param name="introductionId">Desired Introduction Id</param>
    /// <param name="cancellationToken">CancellationToken in case client cancels this method</param>
    /// <returns>IApiResultModel of IEnumerable of InstructionDto</returns>
    /// <response code="200">OK - returns IApiResultModel of IEnumerable of InstructionDto</response>
    /// <response code="400">BadRequest - missing Id</response>
    /// <response code="404">NotFound - No data found for the desired Introduction Id</response>
    [HttpGet("AllForIntroductionId/{introductionId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IApiResultModel<IEnumerable<InstructionDto>>>> GetAllForIntroductionIdAsync(Guid introductionId, CancellationToken cancellationToken) =>
        CreateActionResult(await _instructionService.SelectAllForIntroductionIdAsync(introductionId, cancellationToken).ConfigureAwait(false), false);

    /// <summary>
    /// Insert new Instruction.
    /// </summary>
    /// <param name="instructionDto">Desired Instruction data.</param>
    /// <param name="cancellationToken">CancellationToken in case client cancels this method</param>
    /// <returns>IApiResultModel of InstructionDto</returns>
    /// <response code="201">Created - returns IApiResultModel of InstructionDto</response>
    /// <response code="400">BadRequest - Invalid request; See messages.</response>
    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IApiResultModel<InstructionDto>>> PostAsync([FromBody] InstructionDto instructionDto, CancellationToken cancellationToken) =>
        CreateActionResult(await _instructionService.InsertAsync(instructionDto, GetNameIdentifierClaimValue, cancellationToken).ConfigureAwait(false));

    /// <summary>
    /// Update Instruction.
    /// </summary>
    /// <param name="instructionDto">Desired Instruction data.</param>
    /// <param name="cancellationToken">CancellationToken in case client cancels this method</param>
    /// <returns>IApiResultModel of InstructionDto</returns>
    /// <response code="200">OK - returns IApiResultModel of InstructionDto</response>
    /// <response code="400">BadRequest - Invalid request; See messages.</response>
    [HttpPut()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IApiResultModel<InstructionDto>>> PutAsync([FromBody] InstructionDto instructionDto, CancellationToken cancellationToken) =>
        CreateActionResult(await _instructionService.UpdateAsync(instructionDto, GetNameIdentifierClaimValue, cancellationToken).ConfigureAwait(false));

    /// <summary>
    /// Update Instructions.
    /// </summary>
    /// <param name="instructionsDto">Desired list of Instruction data.</param>
    /// <param name="cancellationToken">CancellationToken in case client cancels this method</param>
    /// <returns>IApiResultModel of int</returns>
    /// <response code="200">OK - returns IApiResultModel of int</response>
    [HttpPut("Multiple")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IApiResultModel<int>>> PutMultipleAsync([FromBody] InstructionsDto instructionsDto, CancellationToken cancellationToken) =>
        CreateActionResult(await _instructionService.UpdateMultipleAsync(instructionsDto, GetNameIdentifierClaimValue, cancellationToken).ConfigureAwait(false));

    /// <summary>
    /// Delete Instruction record for the desired Instruction Id.
    /// There is no error if the Id does not exist.
    /// </summary>
    /// <param name="id">Desired Instruction Id</param>
    /// <param name="cancellationToken">CancellationToken in case client cancels this method</param>
    /// <returns>IApiResultModel of int</returns>
    /// <response code="200">OK - returns IApiResultModel of int</response>
    /// <response code="400">BadRequest - missing Id</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IApiResultModel<int>>> DeleteAsync(Guid id, CancellationToken cancellationToken) =>
        CreateActionResult(await _instructionService.DeleteAsync(id, cancellationToken).ConfigureAwait(false));
}
