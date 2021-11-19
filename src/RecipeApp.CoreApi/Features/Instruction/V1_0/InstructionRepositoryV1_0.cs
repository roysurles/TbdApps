﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Dapper;

using RecipeApp.Shared.Features.Instruction;

using Tbd.WebApi.Shared.Repositories;

namespace RecipeApp.CoreApi.Features.Instruction.V1_0
{
    internal class InstructionRepositoryV1_0 : BaseRepository, IInstructionRepositoryV1_0
    {
        public InstructionRepositoryV1_0(string connectionString) : base(connectionString) { }

        public async Task<InstructionDto> SelectAsync(Guid id)
        {
            using var connection = await CreateConnectionAsync().ConfigureAwait(false);
            return await connection.QuerySingleOrDefaultAsync<InstructionDto>("InstructionSelect"
                   , new { id }
                   , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
        }

        public async Task<IEnumerable<InstructionDto>> SelectAllForIntroductionIdAsync(Guid introductionId)
        {
            using var connection = await CreateConnectionAsync().ConfigureAwait(false);
            return await connection.QueryAsync<InstructionDto>("InstructionSelectAllForIntroductionId"
                , new { introductionId }
                , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
        }

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

        public async Task<InstructionDto> UpdateAsync(InstructionDto instructionDto, string updatedById)
        {
            var updatedOnUtc = DateTime.UtcNow;

            using var connection = await CreateConnectionAsync().ConfigureAwait(false);

            await connection.ExecuteAsync("InstructionUpdate"
                , instructionDto.ToUpdateParameters(updatedById, updatedOnUtc)
                , commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            return instructionDto;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            using var connection = await CreateConnectionAsync().ConfigureAwait(false);
            return await connection.ExecuteScalarAsync<int>("InstructionDelete"
                , new { id }
                , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
        }
    }

    public interface IInstructionRepositoryV1_0
    {
        Task<InstructionDto> SelectAsync(Guid id);

        Task<IEnumerable<InstructionDto>> SelectAllForIntroductionIdAsync(Guid introductionId);

        Task<InstructionDto> InsertAsync(InstructionDto instructionDto, string createdById);

        Task<InstructionDto> UpdateAsync(InstructionDto instructionDto, string updatedById);

        Task<int> DeleteAsync(Guid id);
    }
}
