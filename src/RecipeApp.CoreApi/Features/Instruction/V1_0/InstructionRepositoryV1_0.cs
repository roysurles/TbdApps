using Dapper;

using RecipeApp.Shared.Features.Instruction;

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

using Tbd.WebApi.Shared.Repositories;

namespace RecipeApp.CoreApi.Features.Instruction.V1_0
{
    internal class InstructionRepositoryV1_0 : BaseRepository, IInstructionRepositoryV1_0
    {
        public InstructionRepositoryV1_0(string connectionString) : base(connectionString) { }

        public async Task<InstructionDto> SelectAsync(Guid id, CancellationToken cancellationToken)
        {
            var commandDefinition = new CommandDefinition("InstructionSelect"
                , new { id }
                , commandType: CommandType.StoredProcedure
                , cancellationToken: cancellationToken);

            using var connection = await CreateConnectionAsync().ConfigureAwait(false);

            return await connection.QuerySingleOrDefaultAsync<InstructionDto>(commandDefinition).ConfigureAwait(false);
        }

        public async Task<IEnumerable<InstructionDto>> SelectAllForIntroductionIdAsync(Guid introductionId, CancellationToken cancellationToken)
        {
            var commandDefinition = new CommandDefinition("InstructionSelectAllForIntroductionId"
                , new { introductionId }
                , commandType: CommandType.StoredProcedure
                , cancellationToken: cancellationToken);

            using var connection = await CreateConnectionAsync().ConfigureAwait(false);

            return await connection.QueryAsync<InstructionDto>(commandDefinition).ConfigureAwait(false);
        }

        public async Task<InstructionDto> InsertAsync(InstructionDto instructionDto, string createdById, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();
            var createdOnUtc = DateTime.UtcNow;

            var commandDefinition = new CommandDefinition("InstructionInsert"
                , instructionDto.ToInsertParameters(id, createdById, createdOnUtc)
                , commandType: CommandType.StoredProcedure
                , cancellationToken: cancellationToken);

            using var connection = await CreateConnectionAsync().ConfigureAwait(false);

            await connection.ExecuteAsync(commandDefinition).ConfigureAwait(false);

            return instructionDto;
        }

        public async Task<InstructionDto> UpdateAsync(InstructionDto instructionDto, string updatedById, CancellationToken cancellationToken)
        {
            var updatedOnUtc = DateTime.UtcNow;

            var commandDefinition = new CommandDefinition("InstructionUpdate"
                , instructionDto.ToUpdateParameters(updatedById, updatedOnUtc)
                , commandType: CommandType.StoredProcedure
                , cancellationToken: cancellationToken);

            using var connection = await CreateConnectionAsync().ConfigureAwait(false);

            await connection.ExecuteAsync(commandDefinition).ConfigureAwait(false);

            return instructionDto;
        }

        public async Task<int> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var commandDefinition = new CommandDefinition("InstructionDelete"
                , new { id }
                , commandType: CommandType.StoredProcedure
                , cancellationToken: cancellationToken);

            using var connection = await CreateConnectionAsync().ConfigureAwait(false);

            return await connection.ExecuteScalarAsync<int>(commandDefinition).ConfigureAwait(false);
        }
    }
}
