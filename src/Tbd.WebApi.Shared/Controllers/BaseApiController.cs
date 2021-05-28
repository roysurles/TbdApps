using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RecipeApp.Shared.Extensions;

using Tbd.Shared.ApiResult;

namespace Tbd.WebApi.Shared.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        /// <summary>
        /// Create new instance of IApiResultModel&lt;T&gt;
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <returns>IApiResultModel&lt;T&gt;</returns>
        protected virtual IApiResultModel<T> CreateApiResultModel<T>() =>
            this.GetRequiredService<IApiResultModel<T>>();

        /// <summary>
        /// Create ActionResult
        /// </summary>
        /// <typeparam name="T">Generic</typeparam>
        /// <param name="data">T</param>
        /// <param name="checkForNonSuccess">bool; if true, StatusCode will be updated to the first error's StatusCode</param>
        /// <returns>ActionResult&lt;IApiResultModel&lt;T&gt;&gt;</returns>
        protected virtual ActionResult<IApiResultModel<T>> CreateActionResult<T>(T data, bool checkForNonSuccess = true) =>
            CreateActionResult(CreateApiResultModel<T>().SetData(data), checkForNonSuccess);

        /// <summary>
        /// Create ActionResult
        /// </summary>
        /// <typeparam name="T">Generic</typeparam>
        /// <param name="apiResult">IApiResultModel&lt;T&gt;</param>
        /// <param name="checkForNonSuccess">bool; if true, StatusCode will be updated to the first error's StatusCode</param>
        /// <returns>ActionResult&lt;IApiResultModel&lt;T&gt;&gt;</returns>
        protected virtual ActionResult<IApiResultModel<T>> CreateActionResult<T>(IApiResultModel<T> apiResult, bool checkForNonSuccess = true)
        {
            var objectResult = StatusCode((int)apiResult.HttpStatusCode, apiResult);
            if (!checkForNonSuccess)
                return objectResult;

            var firstNonSuccessCode = apiResult.Messages.Find(item => item.Code.HasValue && (item.Code < 200 || item.Code >= 300))?.Code;
            if (firstNonSuccessCode.HasValue)
                objectResult = StatusCode(firstNonSuccessCode.Value, apiResult);

            return objectResult;
        }

        /// <summary>
        /// Get ClaimTypes.NameIdentifier claim.  Returns null if it does not exist.
        /// </summary>
        protected virtual Claim GetNameIdentifierClaim =>
            User.FindFirst(ClaimTypes.NameIdentifier);

        /// <summary>
        /// Get ClaimTypes.NameIdentifier claim value.  Returns null if it does not exist.
        /// </summary>
        protected virtual string GetNameIdentifierClaimValue =>
            GetNameIdentifierClaim?.Value;

        /// <summary>
        /// Get desired claim.  Returns null if the claim does not exist.
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        protected virtual Claim GetClaim(string claim) =>
            User.FindFirst(claim);

        /// <summary>
        /// Get desired claim value.  Returns null if the claim does not exist.
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        protected virtual string GetClaimValue(string claim) =>
            GetClaim(claim)?.Value;
    }
}
