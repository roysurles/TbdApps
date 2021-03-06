using System.Linq;
using System.Net;

using Microsoft.AspNetCore.Mvc.Filters;

using RecipeApp.Shared.Extensions;

using Tbd.Shared.ApiResult;
using Tbd.WebApi.Shared.Controllers;

namespace Tbd.WebApi.Shared.Filters
{
    /// <summary>
    /// Global Model State Validation Filter
    /// </summary>
    public sealed class GlobalModelStateValidationFilter : ActionFilterAttribute
    {
        /// <summary>
        /// OnActionExecuting
        /// </summary>
        /// <param name="context">ActionExecutingContext</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;
            if (modelState?.IsValid != false)
                return;

            var result = context.GetRequiredService<IApiResultModel<object>>()
                .SetHttpStatusCode(HttpStatusCode.BadRequest);

            var errors = modelState.Select(item => new { item.Key, item.Value.Errors }).ToList();
            foreach (var apiResultErrorModel in errors.SelectMany(item => item.Errors.Select(error => error.Exception == null
                ? new ApiResultMessageModel { MessageType = ApiResultMessageModelTypeEnumeration.Error, Code = (int)HttpStatusCode.BadRequest, Source = item.Key, Message = error.ErrorMessage }
                : new ApiResultMessageModel
                {
                    MessageType = ApiResultMessageModelTypeEnumeration.UnhandledException,
                    Code = (int)HttpStatusCode.InternalServerError,
                    //TODO:  UnhandledException = error.Exception
                })))
            {
                result.Messages.Add(apiResultErrorModel);
            }

            context.Result = ((BaseApiController)context.Controller).BadRequest(result);
        }
    }
}
