using System.Data;

namespace RecipeApp.CoreApi.Features.ApiLog.V1_0;

public class ApiLogRepositoryV1_0 : BaseRepository, IApiLogRepositoryV1_0
{
    public ApiLogRepositoryV1_0(string connectionString) : base(connectionString) { }

    public async Task<(PaginationMetaDataModel PaginationMetaData, IEnumerable<ApiLogDto> Data)> SearchAsync(
        ApiLogSearchRequestDto apiLogSearchRequestDto, CancellationToken cancellationToken)
    {
        var (IsValid, ErrorMessages) = apiLogSearchRequestDto.OrderByClause.IsValid();
        if (!IsValid)
            throw new InvalidOperationException(string.Join(", ", ErrorMessages));

        var sql = @$"
                SELECT COUNT(Id) AS TotalItemCount
                FROM   dbo.ApiLog

                SELECT Id
                      ,ConnectionId
                      ,TraceId
                      ,MachineName
                      ,UserAgent
                      ,Claims
                      ,LocalIpAddress
                      ,RemoteIpAddress
                      ,AssemblyName
                      ,Url
                      ,ControllerName
                      ,ActionName
                      ,ActionDateTimeOffset
                      ,HttpProtocol
                      ,HttpMethod
                      ,HttpStatusCode
                      --,ExceptionData
                      ,ElapsedMilliseconds
                      --,HttpRequestBody
                      --,HttpResponseBody
                FROM Recipe.dbo.ApiLog
                {apiLogSearchRequestDto.OrderByClause.ToSqlString()}
                OFFSET @Offset ROWS FETCH NEXT @Fetch ROWS ONLY";

        using var connection = await CreateConnectionAsync(cancellationToken).ConfigureAwait(false);
        var commandDefinition = new CommandDefinition(sql
            , new
            {
                apiLogSearchRequestDto.Offset,
                apiLogSearchRequestDto.Fetch
            }
            , commandType: CommandType.Text
            , cancellationToken: cancellationToken);

        await using var gridReader = await connection.QueryMultipleAsync(commandDefinition).ConfigureAwait(false);

        var totalItemCount = await gridReader.ReadSingleAsync<int>().ConfigureAwait(false);
        var data = await gridReader.ReadAsync<ApiLogDto>().ConfigureAwait(false);

        return (new(apiLogSearchRequestDto.PageNumber
        , apiLogSearchRequestDto.PageSize
        , totalItemCount), data);
    }
}
