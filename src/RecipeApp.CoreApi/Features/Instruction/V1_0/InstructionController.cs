using Microsoft.AspNetCore.Mvc;

namespace RecipeApp.CoreApi.Features.Instruction.V1_0
{
    /// <summary>
    /// Instruction Api Controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Instruction")]
    [ApiController]
    public class InstructionController : ControllerBase
    {
        /// <summary>
        /// Initial comments...
        /// </summary>
        /// <returns>int</returns>
        /// <response code="200">OK</response>
        [HttpGet]
        public ActionResult<int> Get() => Ok(1);
    }
}
