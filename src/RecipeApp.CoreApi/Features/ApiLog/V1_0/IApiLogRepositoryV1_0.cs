namespace RecipeApp.CoreApi.Features.ApiLog.V1_0;

public interface IApiLogRepositoryV1_0
{
    Task<(PaginationMetaDataModel PaginationMetaData, IEnumerable<ApiLogDto> Data)> SearchAsync(ApiLogSearchRequestDto apiLogSearchRequestDto, CancellationToken cancellationToken);
}
