using System.Data;
using System.Threading;
using System.Threading.Tasks;

using Dapper;

namespace Tbd.WebApi.Shared.Repositories;

public class DatabaseHealthCheckRepository : BaseRepository
{
    public DatabaseHealthCheckRepository(string connectionString) : base(connectionString) { }

    public async Task CheckDatabaseAsync(CancellationToken cancellationToken)
    {
        using var connection = await CreateConnectionAsync(cancellationToken).ConfigureAwait(false);
        var commandDefinition = new CommandDefinition("SELECT 1", commandType: CommandType.Text, cancellationToken: cancellationToken);
        await connection.ExecuteScalarAsync(commandDefinition);
    }
}
