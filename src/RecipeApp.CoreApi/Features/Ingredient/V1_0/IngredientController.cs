namespace RecipeApp.CoreApi.Features.Ingredient.V1_0;

/// <summary>
/// Ingredient Api Controller
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/Ingredient", Name = "Ingredient")]
[ApiController]
[AllowAnonymous]
public class IngredientController : BaseApiController
{
    protected readonly IIngredientServiceV1_0 _ingredientService;
    protected readonly IMediator _mediator;

    public IngredientController(IIngredientServiceV1_0 ingredientService, IMediator mediator)
    {
        _ingredientService = ingredientService;
        _mediator = mediator;
    }

    /// <summary>
    /// Get IngredientDto for the desired Ingredient Id.
    /// </summary>
    /// <param name="id">Desired Ingredient Id</param>
    /// <param name="cancellationToken">CancellationToken in case client cancels this method</param>
    /// <returns>IApiResultModel of IngredientDto</returns>
    /// <response code="200">OK - returns IApiResultModel of IngredientDto</response>
    /// <response code="400">BadRequest - missing Id</response>
    /// <response code="404">NotFound - No data found for the desired Id</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IApiResultModel<IngredientDto>>> GetAsync(Guid id, CancellationToken cancellationToken) =>
        CreateActionResult(await _ingredientService.SelectAsync(id, cancellationToken).ConfigureAwait(false));
    // CreateActionResult(await _mediator.Send(new GetIngredientByIdQuery { Id = id }, cancellationToken));        // TODO: Example of CQRS

    /// <summary>
    /// Get all IngredientDto for the desired Introduction Id.
    /// </summary>
    /// <param name="introductionId">Desired Introduction Id</param>
    /// <param name="cancellationToken">CancellationToken in case client cancels this method</param>
    /// <returns>IApiResultModel of IEnumerable of IngredientDto</returns>
    /// <response code="200">OK - returns IApiResultModel of IEnumerable of IngredientDto</response>
    /// <response code="400">BadRequest - missing Id</response>
    /// <response code="404">NotFound - No data found for the desired Introduction Id</response>
    [HttpGet("AllForIntroductionId/{introductionId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IApiResultModel<IEnumerable<IngredientDto>>>> GetAllForIntroductionIdAsync(Guid introductionId, CancellationToken cancellationToken) =>
        CreateActionResult(await _ingredientService.SelectAllForIntroductionIdAsync(introductionId, cancellationToken).ConfigureAwait(false), false);
    // CreateActionResult(await _mediator.Send(new GetIngredientsByIntroductionIdQuery { IntroductionId = introductionId }, cancellationToken), false);    // TODO: Look into exception; Example of CQRS

    /// <summary>
    /// Insert new Ingredient.
    /// </summary>
    /// <param name="ingredientDto">Desired Ingredient data.</param>
    /// <param name="cancellationToken">CancellationToken in case client cancels this method</param>
    /// <returns>IApiResultModel of IngredientDto</returns>
    /// <response code="201">Created - returns IApiResultModel of IngredientDto</response>
    /// <response code="400">BadRequest - Invalid request; See messages.</response>
    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IApiResultModel<IngredientDto>>> PostAsync([FromBody] IngredientDto ingredientDto, CancellationToken cancellationToken) =>
        CreateActionResult(await _ingredientService.InsertAsync(ingredientDto, GetNameIdentifierClaimValue, cancellationToken).ConfigureAwait(false));

    /// <summary>
    /// Update Ingredient.
    /// </summary>
    /// <param name="ingredientDto">Desired Ingredient data.</param>
    /// <param name="cancellationToken">CancellationToken in case client cancels this method</param>
    /// <returns>IApiResultModel of IngredientDto</returns>
    /// <response code="200">OK - returns IApiResultModel of IngredientDto</response>
    /// <response code="400">BadRequest - Invalid request; See messages.</response>
    [HttpPut()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IApiResultModel<IngredientDto>>> PutAsync([FromBody] IngredientDto ingredientDto, CancellationToken cancellationToken) =>
        CreateActionResult(await _ingredientService.UpdateAsync(ingredientDto, GetNameIdentifierClaimValue, cancellationToken).ConfigureAwait(false));

    /// <summary>
    /// Update Ingredients.
    /// </summary>
    /// <param name="ingredientsDto">Desired list of Ingredient data.</param>
    /// <param name="cancellationToken">CancellationToken in case client cancels this method</param>
    /// <returns>IApiResultModel of int</returns>
    /// <response code="200">OK - returns IApiResultModel of int</response>
    [HttpPut("Multiple")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IApiResultModel<int>>> PutMultipleAsync([FromBody] IngredientsDto ingredientsDto, CancellationToken cancellationToken) =>
        CreateActionResult(await _ingredientService.UpdateMultipleAsync(ingredientsDto, GetNameIdentifierClaimValue, cancellationToken).ConfigureAwait(false));

    /// <summary>
    /// Delete Ingredient record for the desired Ingredient Id.
    /// There is no error if the Id does not exist.
    /// </summary>
    /// <param name="id">Desired Ingredient Id</param>
    /// <param name="cancellationToken">CancellationToken in case client cancels this method</param>
    /// <returns>IApiResultModel of int</returns>
    /// <response code="200">OK - returns IApiResultModel of int</response>
    /// <response code="400">BadRequest - missing Id</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IApiResultModel<int>>> DeleteAsync(Guid id, CancellationToken cancellationToken) =>
        CreateActionResult(await _ingredientService.DeleteAsync(id, cancellationToken).ConfigureAwait(false));
}
