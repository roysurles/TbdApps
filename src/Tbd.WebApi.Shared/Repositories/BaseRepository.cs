using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

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
    }
}
