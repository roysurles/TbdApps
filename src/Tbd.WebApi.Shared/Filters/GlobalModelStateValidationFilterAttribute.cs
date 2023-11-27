namespace Tbd.WebApi.Shared.Filters;

/// <summary>
/// Global Model State Validation Filter
/// </summary>
public sealed class GlobalModelStateValidationFilterAttribute : ActionFilterAttribute
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
        result.Messages.AddRange(errors.SelectMany(item => item.Errors.Select(error => error.Exception == null
            ? new ApiResultMessageModel { MessageType = ApiResultMessageModelTypeEnumeration.Error, Code = (int)HttpStatusCode.BadRequest, Source = item.Key, Message = error.ErrorMessage }
            : new ApiResultMessageModel
            {
                MessageType = ApiResultMessageModelTypeEnumeration.UnhandledException,
                Code = (int)HttpStatusCode.InternalServerError,
                //TODO:  UnhandledException = error.Exception
            })));

        context.Result = ((BaseApiController)context.Controller).BadRequest(result);
    }
}
