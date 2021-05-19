using System;

using Microsoft.AspNetCore.Mvc;

namespace RecipeApp.CoreApi.Features.Introduction
{
    /// <summary>
    /// Introduction Api Controller
    /// </summary>
    [ApiVersion("1.1")]
    [Route("api/v{version:apiVersion}/Introduction")]
    [ApiController]
    public class IntroductionV1_1Controller : ControllerBase
    {
        /// <summary>
        /// Initial comments...
        /// </summary>
        /// <returns>int</returns>
        /// <response code="200">OK</response>
        [HttpGet]
        public ActionResult<int> Get() => Ok("1.1");

        [HttpGet("Throw")]
        public ActionResult<int> Throw() => throw new Exception("testing...");
    }
}
