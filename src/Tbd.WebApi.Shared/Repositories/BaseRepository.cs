using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

using Tbd.Shared.Pagination;

namespace Tbd.WebApi.Shared.Repositories
{
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

        public static PaginationMetaDataModel CreatePaginationMetaDataModel() =>
            new();

        public static PaginationMetaDataModel CreatePaginationMetaDataModel(int pageNumber, int pageSize, int totalItemcount) =>
            new(pageNumber, pageSize, totalItemcount);
    }
}
