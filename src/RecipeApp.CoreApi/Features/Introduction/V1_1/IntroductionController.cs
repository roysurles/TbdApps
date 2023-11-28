namespace RecipeApp.CoreApi.Features.Introduction.V1_1;

/// <summary>
/// Introduction Api Controller
/// </summary>
[ApiVersion("1.1")]
[Route("api/v{version:apiVersion}/Introduction")]
[ApiController]
public class IntroductionController : ControllerBase
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
