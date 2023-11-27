namespace Tbd.WebApi.Shared.Repositories;

public abstract class BaseRepository
{
    protected readonly string _connectionString;

    protected BaseRepository(string connectionString) =>
        _connectionString = connectionString;

    public IDbConnection CreateConnection(bool open = true)
    {
        var sqlConnection = new SqlConnection(_connectionString);
        if (open)
            sqlConnection.Open();

        return sqlConnection;
    }

    public async Task<IDbConnection> CreateConnectionAsync(bool open = true)
    {
        var sqlConnection = new SqlConnection(_connectionString);
        if (open)
            await sqlConnection.OpenAsync().ConfigureAwait(false);

        return sqlConnection;
    }

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken, bool open = true)
    {
        var sqlConnection = new SqlConnection(_connectionString);
        if (open)
            await sqlConnection.OpenAsync(cancellationToken).ConfigureAwait(false);

        return sqlConnection;
    }

    public static PaginationMetaDataModel CreatePaginationMetaDataModel() =>
        new();

    public static PaginationMetaDataModel CreatePaginationMetaDataModel(int pageNumber, int pageSize, int totalItemcount) =>
        new(pageNumber, pageSize, totalItemcount);
}
