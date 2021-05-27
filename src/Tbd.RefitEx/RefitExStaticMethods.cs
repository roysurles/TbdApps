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
            , List<IApiResultMessageModel> apiResultMessages = null)
        {
            ApiResultModel<TResult> result = null;

            try
            {
                result = await func.Invoke();
            }
            catch (ApiException apiException)
            {
                result = await apiException.GetContentAsAsync<ApiResultModel<TResult>>();
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
