namespace Tbd.WebApi.Shared.Repositories;

public class ApiLogRepository : BaseRepository
{
    protected ApiLogRepository(string connectionString) : base(connectionString) { }

    public static async Task<int> InsertApiLogDtoAsync(ApiLogDto apiLogDto, ILogger<ApiLogRepository> logger, string connectionString)
    {
        var affectedRows = 0;
        try
        {
            const string sql = @"INSERT INTO ApiLog
                                   (Id
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
                                   ,ExceptionData
                                   ,ElapsedMilliseconds
                                   ,HttpRequestBody
                                   ,HttpResponseBody)
                             VALUES
                                   (@Id
                                   ,@ConnectionId
                                   ,@TraceId
                                   ,@MachineName
                                   ,@UserAgent
                                   ,@Claims
                                   ,@LocalIpAddress
                                   ,@RemoteIpAddress
                                   ,@AssemblyName
                                   ,@Url
                                   ,@ControllerName
                                   ,@ActionName
                                   ,@ActionDateTimeOffset
                                   ,@HttpProtocol
                                   ,@HttpMethod
                                   ,@HttpStatusCode
                                   ,@ExceptionData
                                   ,@ElapsedMilliseconds
                                   ,@HttpRequestBody
                                   ,@HttpResponseBody)";

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = apiLogDto.Id;
            command.Parameters.Add("@ConnectionId", SqlDbType.NVarChar, 255).Value = apiLogDto.ConnectionId.IsNullToDbNull();
            command.Parameters.Add("@TraceId", SqlDbType.NVarChar, 255).Value = apiLogDto.TraceId.IsNullToDbNull();
            command.Parameters.Add("@MachineName", SqlDbType.NVarChar, 255).Value = apiLogDto.MachineName.IsNullToDbNull();
            command.Parameters.Add("@UserAgent", SqlDbType.NVarChar, 255).Value = apiLogDto.UserAgent.IsNullToDbNull();
            command.Parameters.Add("@Claims", SqlDbType.NVarChar, 255).Value = apiLogDto.Claims.IsNullToDbNull();
            command.Parameters.Add("@LocalIpAddress", SqlDbType.NVarChar, 50).Value = apiLogDto.LocalIpAddress.IsNullToDbNull();
            command.Parameters.Add("@RemoteIpAddress", SqlDbType.NVarChar, 50).Value = apiLogDto.RemoteIpAddress.IsNullToDbNull();
            command.Parameters.Add("@AssemblyName", SqlDbType.NVarChar, 255).Value = apiLogDto.AssemblyName.IsNullToDbNull();
            command.Parameters.Add("@Url", SqlDbType.NVarChar, 255).Value = apiLogDto.Url.IsNullToDbNull();
            command.Parameters.Add("@ControllerName", SqlDbType.NVarChar, 100).Value = apiLogDto.ControllerName.IsNullToDbNull();
            command.Parameters.Add("@ActionName", SqlDbType.NVarChar, 100).Value = apiLogDto.ActionName.IsNullToDbNull();
            command.Parameters.Add("@ActionDateTimeOffset", SqlDbType.DateTimeOffset).Value = apiLogDto.ActionDateTimeOffset;
            command.Parameters.Add("@HttpProtocol", SqlDbType.NVarChar, 25).Value = apiLogDto.HttpProtocol.IsNullToDbNull();
            command.Parameters.Add("@HttpMethod", SqlDbType.NVarChar, 10).Value = apiLogDto.HttpMethod.IsNullToDbNull();
            command.Parameters.Add("@HttpStatusCode", SqlDbType.Int).Value = apiLogDto.HttpStatusCode;
            command.Parameters.Add("@ExceptionData", SqlDbType.NVarChar).Value = apiLogDto.ExceptionData.IsNullToDbNull();
            command.Parameters.Add("@ElapsedMilliseconds", SqlDbType.Int).Value = apiLogDto.ElapsedMilliseconds;
            command.Parameters.Add("@HttpRequestBody", SqlDbType.NVarChar).Value = apiLogDto.HttpRequestBody.IsNullToDbNull();
            command.Parameters.Add("@HttpResponseBody", SqlDbType.NVarChar).Value = apiLogDto.HttpResponseBody.IsNullToDbNull();

            await connection.OpenAsync().ConfigureAwait(false);
            affectedRows = await command.ExecuteNonQueryAsync().ConfigureAwait(false);

            logger.LogInformation("Inserted {0} ApiLog Record(s)", 1);
            return affectedRows;
        }
        catch (Exception ex)
        {
            logger.LogInformation("Inserted 0 ApiLog Record(s); Swallowing exception.");
            logger.LogError(ex, "{methodName} encountered an exception: ", nameof(InsertApiLogDtoAsync));
            return affectedRows;
        }
    }
}
