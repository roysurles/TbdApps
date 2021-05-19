using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Dapper;

using RecipeApp.Shared.Features.Instruction;

using Tbd.WebApi.Shared.Repositories;

namespace RecipeApp.CoreApi.Features.Instruction
{
    internal class InstructionRepository : BaseRepository, IInstructionRepository
    {
        public InstructionRepository(string connectionString) : base(connectionString) { }

        [SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
        public async Task<InstructionDto> SelectAsync(Guid id)
        {
            using var connection = await CreateConnectionAsync().ConfigureAwait(false);
            return await connection.QueryFirstOrDefaultAsync<InstructionDto>("InstructionSelect"
                   , new { id }
                   , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
        }

        [SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
        public async Task<InstructionDto> InsertAsync(InstructionDto instructionDto, string createdById)
        {
            var id = Guid.NewGuid();
            var createdOnUtc = DateTime.UtcNow;

            using var connection = await CreateConnectionAsync().ConfigureAwait(false);

            await connection.ExecuteAsync("InstructionInsert"
                , instructionDto.ToInsertParameters(id, createdById, createdOnUtc)
                , commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            return instructionDto;
        }

        [SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
        public async Task<InstructionDto> UpdateAsync(InstructionDto instructionDto, string updatedById)
        {
            var updatedOnUtc = DateTime.UtcNow;

            using var connection = await CreateConnectionAsync().ConfigureAwait(false);

            await connection.ExecuteAsync("InstructionUpdate"
                , instructionDto.ToUpdateParameters(updatedById, updatedOnUtc)
                , commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            return instructionDto;
        }

        [SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
        public async Task<int> DeleteAsync(Guid id)
        {
            using var connection = await CreateConnectionAsync().ConfigureAwait(false);
            return await connection.ExecuteScalarAsync<int>("InstructionDelete"
                , new { id }
                , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
        }
    }

    public interface IInstructionRepository
    {
        Task<InstructionDto> SelectAsync(Guid id);

        Task<InstructionDto> InsertAsync(InstructionDto instructionDto, string createdById);

        Task<InstructionDto> UpdateAsync(InstructionDto instructionDto, string updatedById);

        Task<int> DeleteAsync(Guid id);
    }
}
