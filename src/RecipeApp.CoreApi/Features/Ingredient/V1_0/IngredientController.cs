using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using RecipeApp.Shared.Features.Ingredient;

using Tbd.Shared.ApiResult;
using Tbd.WebApi.Shared.Controllers;

namespace RecipeApp.CoreApi.Features.Ingredient.V1_0
{
    /// <summary>
    /// Ingredient Api Controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Ingredient", Name = "Ingredient")]
    [ApiController]
    public class IngredientController : BaseApiController
    {
        protected readonly IIngredientV1_0Service _ingredientService;

        public IngredientController(IIngredientV1_0Service ingredientService) =>
            _ingredientService = ingredientService;

        /// <summary>
        /// Get IngredientDto for the desired Ingredient Id.
        /// </summary>
        /// <param name="id">Desired Ingredient Id</param>
        /// <returns>IApiResultModel of IngredientDto</returns>
        /// <response code="200">OK - returns IApiResultModel of IngredientDto</response>
        /// <response code="400">BadRequest - missing Id</response>
        /// <response code="404">NotFound - No data found for the desired Id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IApiResultModel<IngredientDto>>> GetAsync(Guid id) =>
            CreateActionResult(await _ingredientService.SelectAsync(id).ConfigureAwait(false));

        /// <summary>
        /// Get all IngredientDto for the desired Introduction Id.
        /// </summary>
        /// <param name="introductionId">Desired Introduction Id</param>
        /// <returns>IApiResultModel of IEnumerable of IngredientDto</returns>
        /// <response code="200">OK - returns IApiResultModel of IEnumerable of IngredientDto</response>
        /// <response code="400">BadRequest - missing Id</response>
        /// <response code="404">NotFound - No data found for the desired Introduction Id</response>
        [HttpGet("AllForIntroductionId/{introductionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IApiResultModel<IEnumerable<IngredientDto>>>> GetAllForIntroductionIdAsync(Guid introductionId) =>
            CreateActionResult(await _ingredientService.SelectAllForIntroductionIdAsync(introductionId).ConfigureAwait(false));

        /// <summary>
        /// Insert new Ingredient.
        /// </summary>
        /// <param name="ingredientDto">Desired Ingredient data.</param>
        /// <returns>IApiResultModel of IngredientDto</returns>
        /// <response code="201">Created - returns IApiResultModel of IngredientDto</response>
        /// <response code="400">BadRequest - Invalid request; See messages.</response>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IApiResultModel<IngredientDto>>> PostAsync([FromBody] IngredientDto ingredientDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return CreateActionResult(await _ingredientService.InsertAsync(ingredientDto, userId).ConfigureAwait(false));
        }

        /// <summary>
        /// Update Ingredient.
        /// </summary>
        /// <param name="ingredientDto">Desired Ingredient data.</param>
        /// <returns>IApiResultModel of IngredientDto</returns>
        /// <response code="200">OK - returns IApiResultModel of IngredientDto</response>
        /// <response code="400">BadRequest - Invalid request; See messages.</response>
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IApiResultModel<IngredientDto>>> PutAsync([FromBody] IngredientDto ingredientDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return CreateActionResult(await _ingredientService.UpdateAsync(ingredientDto, userId).ConfigureAwait(false));
        }

        /// <summary>
        /// Delete Ingredient record for the desired Ingredient Id.
        /// There is no error if the Id does not exist.
        /// </summary>
        /// <param name="id">Desired Ingredient Id</param>
        /// <returns>IApiResultModel of int</returns>
        /// <response code="200">OK - returns IApiResultModel of int</response>
        /// <response code="400">BadRequest - missing Id</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IApiResultModel<int>>> DeleteAsync(Guid id) =>
            CreateActionResult(await _ingredientService.DeleteAsync(id).ConfigureAwait(false));
    }
}
