using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using RecipeApp.Shared.Features.Introduction;

using Tbd.Shared.ApiResult;
using Tbd.WebApi.Shared.Controllers;

namespace RecipeApp.CoreApi.Features.Introduction
{
    /// <summary>
    /// Introduction Api Controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Introduction")]
    [ApiController]
    [AllowAnonymous]
    public class IntroductionV1_0Controller : BaseApiController
    {
        protected readonly IIntroductionService _introductionService;

        public IntroductionV1_0Controller(IIntroductionService introductionService) =>
            _introductionService = introductionService;

        /// <summary>
        /// Gets IntroductionDto for the desired Introduction Id
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
    }
}
