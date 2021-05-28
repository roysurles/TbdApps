using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using RecipeApp.Shared.Features.Instruction;

using Tbd.Shared.ApiResult;
using Tbd.WebApi.Shared.Controllers;

namespace RecipeApp.CoreApi.Features.Instruction.V1_0
{
    /// <summary>
    /// Instruction Api Controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Instruction")]
    [ApiController]
    [AllowAnonymous]
    public class InstructionController : BaseApiController
    {
        protected readonly IInstructionV1_0Service _instructionService;

        public InstructionController(IInstructionV1_0Service instructionService) =>
            _instructionService = instructionService;

        /// <summary>
        /// Get InstructionDto for the desired Instruction Id.
        /// </summary>
        /// <param name="id">Desired Instruction Id</param>
        /// <returns>IApiResultModel of InstructionDto</returns>
        /// <response code="200">OK - returns IApiResultModel of InstructionDto</response>
        /// <response code="400">BadRequest - missing Id</response>
        /// <response code="404">NotFound - No data found for the desired Id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IApiResultModel<InstructionDto>>> GetAsync(Guid id) =>
            CreateActionResult(await _instructionService.SelectAsync(id).ConfigureAwait(false));

        /// <summary>
        /// Get all InstructionDto for the desired Introduction Id.
        /// </summary>
        /// <param name="introductionId">Desired Introduction Id</param>
        /// <returns>IApiResultModel of IEnumerable of InstructionDto</returns>
        /// <response code="200">OK - returns IApiResultModel of IEnumerable of InstructionDto</response>
        /// <response code="400">BadRequest - missing Id</response>
        /// <response code="404">NotFound - No data found for the desired Introduction Id</response>
        [HttpGet("AllForIntroductionId/{introductionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IApiResultModel<IEnumerable<InstructionDto>>>> GetAllForIntroductionIdAsync(Guid introductionId) =>
            CreateActionResult(await _instructionService.SelectAllForIntroductionIdAsync(introductionId).ConfigureAwait(false));

        /// <summary>
        /// Insert new Instruction.
        /// </summary>
        /// <param name="instructionDto">Desired Instruction data.</param>
        /// <returns>IApiResultModel of InstructionDto</returns>
        /// <response code="201">Created - returns IApiResultModel of InstructionDto</response>
        /// <response code="400">BadRequest - Invalid request; See messages.</response>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IApiResultModel<InstructionDto>>> PostAsync([FromBody] InstructionDto instructionDto) =>
            CreateActionResult(await _instructionService.InsertAsync(instructionDto, GetNameIdentifierClaimValue).ConfigureAwait(false));

        /// <summary>
        /// Update Instruction.
        /// </summary>
        /// <param name="instructionDto">Desired Instruction data.</param>
        /// <returns>IApiResultModel of InstructionDto</returns>
        /// <response code="200">OK - returns IApiResultModel of InstructionDto</response>
        /// <response code="400">BadRequest - Invalid request; See messages.</response>
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IApiResultModel<InstructionDto>>> PutAsync([FromBody] InstructionDto instructionDto) =>
            CreateActionResult(await _instructionService.UpdateAsync(instructionDto, GetNameIdentifierClaimValue).ConfigureAwait(false));

        /// <summary>
        /// Delete Instruction record for the desired Instruction Id.
        /// There is no error if the Id does not exist.
        /// </summary>
        /// <param name="id">Desired Instruction Id</param>
        /// <returns>IApiResultModel of int</returns>
        /// <response code="200">OK - returns IApiResultModel of int</response>
        /// <response code="400">BadRequest - missing Id</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IApiResultModel<int>>> DeleteAsync(Guid id) =>
            CreateActionResult(await _instructionService.DeleteAsync(id).ConfigureAwait(false));
    }
}
