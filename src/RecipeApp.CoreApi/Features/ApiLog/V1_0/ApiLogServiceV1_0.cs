
namespace RecipeApp.CoreApi.Features.ApiLog.V1_0;

public class ApiLogServiceV1_0 : BaseService, IApiLogServiceV1_0
{
    protected readonly ILogger<ApiLogServiceV1_0> _logger;
    protected readonly IApiLogRepositoryV1_0 _apiLogRepository;

    public ApiLogServiceV1_0(IServiceProvider serviceProvider
        , ILogger<ApiLogServiceV1_0> logger
        , IApiLogRepositoryV1_0 apiLogRepository) : base(serviceProvider)
    {
        _logger = logger;
        _apiLogRepository = apiLogRepository;
    }

    public async Task<IApiResultModel<IEnumerable<ApiLogDto>>> SearchAsync(ApiLogSearchRequestDto apiLogSearchRequestDto
        , CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(SearchAsync));

        var (PaginationMetaData, Data) = await _apiLogRepository.SearchAsync(apiLogSearchRequestDto, cancellationToken).ConfigureAwait(false);

        return CreateApiResultModel<IEnumerable<ApiLogDto>>()
            .SetMeta(PaginationMetaData)
            .SetData(Data);
    }
}

public interface IApiLogServiceV1_0
{
    Task<IApiResultModel<IEnumerable<ApiLogDto>>> SearchAsync(ApiLogSearchRequestDto apiLogSearchRequestDto, CancellationToken cancellationToken);
}
