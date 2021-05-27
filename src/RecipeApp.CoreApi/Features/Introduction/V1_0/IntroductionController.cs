using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using RecipeApp.Shared.Features.Introduction;

using Tbd.Shared.ApiResult;
using Tbd.WebApi.Shared.Controllers;

namespace RecipeApp.CoreApi.Features.Introduction.V1_0
{
    /// <summary>
    /// Introduction Api Controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Introduction")]
    [ApiController]
    [AllowAnonymous]
    public class IntroductionController : BaseApiController
    {
        protected readonly IIntroductionV1_0Service _introductionService;

        public IntroductionController(IIntroductionV1_0Service introductionService) =>
            _introductionService = introductionService;

        /// <summary>
        /// Search Introductions based on criteria.
        /// </summary>
        /// <param name="introductionSearchRequestDto">Search criteria</param>
        /// <returns>IApiResultModel of IEnumerable of IntroductionSearchResultDto.</returns>
        /// <response code="200">OK - returns IApiResultModel of IEnumerable of IntroductionSearchResultDto.</response>
        [HttpPost("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IApiResultModel<IEnumerable<IntroductionSearchResultDto>>>> SearchAsync([FromBody] IntroductionSearchRequestDto introductionSearchRequestDto) =>
            CreateActionResult(await _introductionService.SearchAsync(introductionSearchRequestDto).ConfigureAwait(false), false);

        /// <summary>
        /// Get IntroductionDto for the desired Introduction Id.
        /// </summary>
        /// <param name="id">Desired Introduction Id</param>
        /// <returns>IApiResultModel of IntroductionDto</returns>
        /// <response code="200">OK - returns IApiResultModel of IntroductionDto</response>
        /// <response code="400">BadRequest - missing Id</response>
        /// <response code="404">NotFound - No data found for the desired Id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IApiResultModel<IntroductionDto>>> GetAsync(Guid id) =>
            CreateActionResult(await _introductionService.SelectAsync(id).ConfigureAwait(false));

        /// <summary>
        /// Insert new Introduction.
        /// </summary>
        /// <param name="introductionDto">Desired Introduction data.</param>
        /// <returns>IApiResultModel of IntroductionDto</returns>
        /// <response code="201">Created - returns IApiResultModel of IntroductionDto</response>
        /// <response code="400">BadRequest - Invalid request; See messages.</response>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IApiResultModel<IntroductionDto>>> PostAsync([FromBody] IntroductionDto introductionDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return CreateActionResult(await _introductionService.InsertAsync(introductionDto, userId).ConfigureAwait(false));
        }

        /// <summary>
        /// Update Introduction
        /// </summary>
        /// <param name="introductionDto">Desired Introduction data.</param>
        /// <returns>IApiResultModel of IntroductionDto</returns>
        /// <response code="200">OK - returns IApiResultModel of IntroductionDto</response>
        /// <response code="400">BadRequest - Invalid request; See messages.</response>
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IApiResultModel<IntroductionDto>>> PutAsync([FromBody] IntroductionDto introductionDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return CreateActionResult(await _introductionService.UpdateAsync(introductionDto, userId).ConfigureAwait(false));
        }

        /// <summary>
        /// Delete Introduction record for the desired Introduction Id.
        /// There is no error if the Id does not exist.
        /// </summary>
        /// <param name="id">Desired Introduction Id</param>
        /// <returns>IApiResultModel of int</returns>
        /// <response code="200">OK - returns IApiResultModel of int</response>
        /// <response code="400">BadRequest - missing Id</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IApiResultModel<int>>> DeleteAsync(Guid id) =>
            CreateActionResult(await _introductionService.DeleteAsync(id).ConfigureAwait(false));
    }
}
