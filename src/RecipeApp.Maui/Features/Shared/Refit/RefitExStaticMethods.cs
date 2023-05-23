namespace RecipeApp.Maui.Features.Shared.Refit;

public static class RefitExStaticMethods
{
    public static async Task<ApiResultModel<TResult>> TryInvokeApiAsync<TResult>(Func<Task<ApiResultModel<TResult>>> func
        , ICollection<IApiResultMessageModel> apiResultMessages = null
        , TResult onExceptionDefaultData = default, bool onExceptionCreateNewDataIfNullDefault = true)
    {
        ApiResultModel<TResult> result = null;

        try
        {
            result = await func.Invoke();
        }
        //catch (AccessTokenNotAvailableException atnaex)  // TODO
        //{
        //    atnaex.Redirect();
        //}
        catch (ApiException apiException)
        {
            //  TODO:  add apiException.Content deserialize to custom error and include in result.AddErrorMessage
            //  '{"error":{"code":"UnsupportedApiVersion","message":"The HTTP resource that matches the request URI 'https://localhost:44350/api/v1.0/Introduction' with API version '1.0' does not support HTTP method 'GET'.","innerError":null}}'
            result = await apiException.GetContentAsAsync<ApiResultModel<TResult>>();
            result.AddErrorMessage($"{apiException.Message}", apiException.Source, apiException.StatusCode);

            /*
             *  Possibility for exception when TResult is IEnumerable<TDto>
                    var dtoType = type.GenericTypeArguments[0].GetType();
                    var listType = typeof(List<>).MakeGenericType(new[] { type });
                    var newResult = Activator.CreateInstance(listType);
             */

            var data = EqualityComparer<TResult>.Default.Equals(onExceptionDefaultData, default) && onExceptionCreateNewDataIfNullDefault
                ? Activator.CreateInstance<TResult>()
                : onExceptionDefaultData;
            result.SetData(data);
        }
        catch (Exception ex)
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
