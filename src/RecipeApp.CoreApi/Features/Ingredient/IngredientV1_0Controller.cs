using Microsoft.AspNetCore.Mvc;

namespace RecipeApp.CoreApi.Features.Ingredient
{
    /// <summary>
    /// Ingredient Api Controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Ingredient", Name = "Ingredient")]
    [ApiController]
    public class IngredientV1_0Controller : ControllerBase
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
