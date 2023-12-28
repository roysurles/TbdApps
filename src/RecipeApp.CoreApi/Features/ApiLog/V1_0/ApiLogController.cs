namespace RecipeApp.CoreApi.Features.ApiLog.V1_0;

/// <summary>
/// Api Log Api Controller
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/ApiLog")]
[ApiController]
[AllowAnonymous]
public class ApiLogController : BaseApiController
{
    protected readonly IApiLogServiceV1_0 _apiLogService;

    public ApiLogController(IApiLogServiceV1_0 apiLogService) =>
        _apiLogService = apiLogService;

    /// <summary>
    /// Search ApiLog based on criteria.
    /// </summary>
    /// <param name="apiLogSearchRequestDto">Search criteria</param>
    /// <param name="cancellationToken">CancellationToken in case client cancels this method</param>
    /// <returns>IApiResultModel of IEnumerable of ApiLogDto.</returns>
    /// <response code="200">OK - returns IApiResultModel of IEnumerable of ApiLogDto.</response>
    /// <response code="500">InternalServerError - returns IApiResultModel of object for any unhandled exception.</response>
    [HttpPost("search")]
    [ProducesResponseType(typeof(IApiResultModel<IEnumerable<ApiLogDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiResultModel<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IApiResultModel<IEnumerable<ApiLogDto>>>> SearchAsync([FromBody] ApiLogSearchRequestDto apiLogSearchRequestDto, CancellationToken cancellationToken) =>
        CreateActionResult(await _apiLogService.SearchAsync(apiLogSearchRequestDto, cancellationToken).ConfigureAwait(false), false);
}
