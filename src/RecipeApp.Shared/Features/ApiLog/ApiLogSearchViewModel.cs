namespace RecipeApp.Shared.Features.ApiLog;

public class ApiLogSearchViewModel : BaseViewModel, IApiLogSearchViewModel
{
    protected readonly IApiLogApiClientV1_0 _apiLogApiClientV1_0;
    protected readonly ILogger<ApiLogSearchViewModel> _logger;

    public ApiLogSearchViewModel(IApiLogApiClientV1_0 apiLogApiClientV1_0, ILogger<ApiLogSearchViewModel> logger)
    {
        _apiLogApiClientV1_0 = apiLogApiClientV1_0;
        _logger = logger;
    }

    public bool HasSearched { get; protected set; }

    public ApiLogSearchRequestDto ApiLogSearchRequestDto { get; } =
        new ApiLogSearchRequestDto { PageNumber = 1, PageSize = 10 };

    public IApiResultModel<List<ApiLogDto>> ApiLogSearchResult { get; protected set; } =
        new ApiResultModel<List<ApiLogDto>>()
        .SetMeta(1, 10, 0)
        .SetData([]);

    public async Task SearchAsync(int pageNumber = 1, int pageSize = 10)
    {
        _logger.LogInformation($"{nameof(SearchAsync)}({pageNumber}, {pageSize})");

        ClearApiResultMessages();

        ApiLogSearchRequestDto.OrderByClause.Clear();
        ApiLogSearchRequestDto.SetPagination(pageNumber, pageSize)
            .OrderByClause
                .AddOrderByDescending(p => p.ActionDateTimeOffset);

        ApiLogSearchResult = await RefitExStaticMethods.TryInvokeApiAsync(
            () => _apiLogApiClientV1_0.SearchAsync(ApiLogSearchRequestDto), ApiResultMessages);

        HasSearched = true;
    }
}

public interface IApiLogSearchViewModel : IBaseViewModel
{
    bool HasSearched { get; }

    ApiLogSearchRequestDto ApiLogSearchRequestDto { get; }

    IApiResultModel<List<ApiLogDto>> ApiLogSearchResult { get; }

    Task SearchAsync(int pageNumber = 1, int pageSize = 10);
}
