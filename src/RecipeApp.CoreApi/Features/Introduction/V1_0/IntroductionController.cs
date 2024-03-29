﻿namespace RecipeApp.CoreApi.Features.Introduction.V1_0;

/// <summary>
/// Introduction Api Controller
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/Introduction")]
[ApiController]
[AllowAnonymous]
public class IntroductionController : BaseApiController
{
    protected readonly IIntroductionServiceV1_0 _introductionService;

    public IntroductionController(IIntroductionServiceV1_0 introductionService) =>
        _introductionService = introductionService;

    /// <summary>
    /// Search Introductions based on criteria.
    /// </summary>
    /// <param name="introductionSearchRequestDto">Search criteria</param>
    /// <param name="cancellationToken">CancellationToken in case client cancels this method</param>
    /// <returns>IApiResultModel of IEnumerable of IntroductionSearchResultDto.</returns>
    /// <response code="200">OK - returns IApiResultModel of IEnumerable of IntroductionSearchResultDto.</response>
    [HttpPost("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IApiResultModel<IEnumerable<IntroductionSearchResultDto>>>> SearchAsync([FromBody] IntroductionSearchRequestDto introductionSearchRequestDto, CancellationToken cancellationToken) =>
        CreateActionResult(await _introductionService.SearchAsync(introductionSearchRequestDto, cancellationToken).ConfigureAwait(false), false);

    /// <summary>
    /// Get IntroductionDto for the desired Introduction Id.
    /// </summary>
    /// <param name="id">Desired Introduction Id</param>
    /// <param name="cancellationToken">CancellationToken in case client cancels this method</param>
    /// <returns>IApiResultModel of IntroductionDto</returns>
    /// <response code="200">OK - returns IApiResultModel of IntroductionDto</response>
    /// <response code="400">BadRequest - missing Id</response>
    /// <response code="404">NotFound - No data found for the desired Id</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IApiResultModel<IntroductionDto>>> GetAsync(Guid id, CancellationToken cancellationToken) =>
        CreateActionResult(await _introductionService.SelectAsync(id, cancellationToken).ConfigureAwait(false));

    /// <summary>
    /// Insert new Introduction.
    /// </summary>
    /// <param name="introductionDto">Desired Introduction data.</param>
    /// <param name="cancellationToken">CancellationToken in case client cancels this method</param>
    /// <returns>IApiResultModel of IntroductionDto</returns>
    /// <response code="201">Created - returns IApiResultModel of IntroductionDto</response>
    /// <response code="400">BadRequest - Invalid request; See messages.</response>
    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IApiResultModel<IntroductionDto>>> PostAsync([FromBody] IntroductionDto introductionDto, CancellationToken cancellationToken) =>
        CreateActionResult(await _introductionService.InsertAsync(introductionDto, GetNameIdentifierClaimValue, cancellationToken).ConfigureAwait(false));

    /// <summary>
    /// Update Introduction
    /// </summary>
    /// <param name="introductionDto">Desired Introduction data.</param>
    /// <param name="cancellationToken">CancellationToken in case client cancels this method</param>
    /// <returns>IApiResultModel of IntroductionDto</returns>
    /// <response code="200">OK - returns IApiResultModel of IntroductionDto</response>
    /// <response code="400">BadRequest - Invalid request; See messages.</response>
    [HttpPut()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IApiResultModel<IntroductionDto>>> PutAsync([FromBody] IntroductionDto introductionDto, CancellationToken cancellationToken) =>
        CreateActionResult(await _introductionService.UpdateAsync(introductionDto, GetNameIdentifierClaimValue, cancellationToken).ConfigureAwait(false));

    /// <summary>
    /// Delete Introduction record for the desired Introduction Id.
    /// There is no error if the Id does not exist.
    /// </summary>
    /// <param name="id">Desired Introduction Id</param>
    /// <param name="cancellationToken">CancellationToken in case client cancels this method</param>
    /// <returns>IApiResultModel of int</returns>
    /// <response code="200">OK - returns IApiResultModel of int</response>
    /// <response code="400">BadRequest - missing Id</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IApiResultModel<int>>> DeleteAsync(Guid id, CancellationToken cancellationToken) =>
        CreateActionResult(await _introductionService.DeleteAsync(id, cancellationToken).ConfigureAwait(false));
}
