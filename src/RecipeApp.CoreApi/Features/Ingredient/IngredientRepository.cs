﻿using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Dapper;

using RecipeApp.Shared.Features.Ingredient;

using Tbd.WebApi.Shared.Repositories;

namespace RecipeApp.CoreApi.Features.Ingredient
{
    internal class IngredientRepository : BaseRepository, IIngredientRepository
    {
        public IngredientRepository(string connectionString) : base(connectionString) { }

        [SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
        public async Task<IngredientDto> SelectAsync(Guid id)
        {
            using var connection = await CreateConnectionAsync().ConfigureAwait(false);
            return await connection.QueryFirstOrDefaultAsync<IngredientDto>("IngredientSelect"
                , new { id }
                , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
        }

        [SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
        public async Task<IngredientDto> InsertAsync(IngredientDto ingredientDto, string createdById)
        {
            var id = Guid.NewGuid();
            var createdOnUtc = DateTime.UtcNow;

            using var connection = await CreateConnectionAsync().ConfigureAwait(false);

            await connection.ExecuteAsync("IngredientInsert"
                , ingredientDto.ToInsertParameters(id, createdById, createdOnUtc)
                , commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            return ingredientDto;
        }

        [SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
        public async Task<IngredientDto> UpdateAsync(IngredientDto ingredientDto, string updatedById)
        {
            var updatedOnUtc = DateTime.UtcNow;

            using var connection = await CreateConnectionAsync().ConfigureAwait(false);

            await connection.ExecuteAsync("IngredientUpdate"
                , ingredientDto.ToUpdateParameters(updatedById, updatedOnUtc)
                , commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            return ingredientDto;
        }

        [SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
        public async Task<int> DeleteAsync(Guid id)
        {
            using var connection = await CreateConnectionAsync().ConfigureAwait(false);
            return await connection.ExecuteScalarAsync<int>("IngredientDelete"
                , new { id }
                , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
        }
    }

    public interface IIngredientRepository
    {
        Task<IngredientDto> SelectAsync(Guid id);

        Task<IngredientDto> InsertAsync(IngredientDto ingredientDto, string createdById);

        Task<IngredientDto> UpdateAsync(IngredientDto ingredientDto, string updatedById);

        Task<int> DeleteAsync(Guid id);
    }
}
