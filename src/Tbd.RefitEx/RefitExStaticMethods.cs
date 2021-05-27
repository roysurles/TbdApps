using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Refit;

using Tbd.Shared.ApiResult;

namespace Tbd.RefitEx
{
    public static class RefitExStaticMethods
    {
        [SuppressMessage("Design", "RCS1090:Add call to 'ConfigureAwait' (or vice versa).", Justification = "<Pending>")]
        public static async Task<ApiResultModel<TResult>> TryInvokeApiAsync<TResult>(Func<Task<ApiResultModel<TResult>>> func
            , List<IApiResultMessageModel> apiResultMessages = null
            , TResult onExceptionDefaultData = default, bool onExceptionCreateNewDataIfNullDefault = true)
        {
            ApiResultModel<TResult> result = null;

            try
            {
                result = await func.Invoke();
            }
            catch (ApiException apiException)
            {
                //  TODO:  add apiException.Content deserialize to custom error and include in result.AddErrorMessage
                //  '{"error":{"code":"UnsupportedApiVersion","message":"The HTTP resource that matches the request URI 'https://localhost:44350/api/v1.0/Introduction' with API version '1.0' does not support HTTP method 'GET'.","innerError":null}}'
                result = await apiException.GetContentAsAsync<ApiResultModel<TResult>>();
                result.AddErrorMessage($"{apiException.Message}", apiException.Source, apiException.StatusCode);
                var data = EqualityComparer<TResult>.Default.Equals(onExceptionDefaultData, default) && onExceptionCreateNewDataIfNullDefault
                    ? Activator.CreateInstance<TResult>()
                    : onExceptionDefaultData;
                result.SetData(data);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (result is not null && apiResultMessages is not null)
                    apiResultMessages.AddRange(result.Messages);
            }

            return result;
        }
    }
}
