namespace RecipeApp.Shared.Features.ApiLog;

public interface IApiLogApiClientV1_0
{
    [Post("/api/v1.0/ApiLog/search")]
    Task<ApiResultModel<List<ApiLogDto>>> SearchAsync([Body] ApiLogSearchRequestDto ApiLogSearchRequestDto);
}
