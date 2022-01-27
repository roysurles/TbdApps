using Dapper;

using RecipeApp.Shared.Features.Ingredient;

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

using Tbd.WebApi.Shared.Repositories;

namespace RecipeApp.CoreApi.Features.Ingredient.V1_0
{
    internal class IngredientRepositoryV1_0 : BaseRepository, IIngredientRepositoryV1_0
    {
        public IngredientRepositoryV1_0(string connectionString) : base(connectionString) { }

        public async Task<IngredientDto> SelectAsync(Guid id, CancellationToken cancellationToken)
        {
            var commandDefinition = new CommandDefinition("IngredientSelect"
                , new { id }
                , commandType: CommandType.StoredProcedure
                , cancellationToken: cancellationToken);

            using var connection = await CreateConnectionAsync().ConfigureAwait(false);

            return await connection.QuerySingleOrDefaultAsync<IngredientDto>(commandDefinition).ConfigureAwait(false);
        }

        public async Task<IEnumerable<IngredientDto>> SelectAllForIntroductionIdAsync(Guid introductionId, CancellationToken cancellationToken)
        {
            var commandDefinition = new CommandDefinition("IngredientSelectAllForIntroductionId"
                , new { introductionId }
                , commandType: CommandType.StoredProcedure
                , cancellationToken: cancellationToken);

            using var connection = await CreateConnectionAsync().ConfigureAwait(false);

            return await connection.QueryAsync<IngredientDto>(commandDefinition).ConfigureAwait(false);
        }

        public async Task<IngredientDto> InsertAsync(IngredientDto ingredientDto, string createdById, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();
            var createdOnUtc = DateTime.UtcNow;

            var commandDefinition = new CommandDefinition("IngredientInsert"
                , ingredientDto.ToInsertParameters(id, createdById, createdOnUtc)
                , commandType: CommandType.StoredProcedure
                , cancellationToken: cancellationToken);

            using var connection = await CreateConnectionAsync().ConfigureAwait(false);

            await connection.ExecuteAsync(commandDefinition).ConfigureAwait(false);

            return ingredientDto;
        }

        public async Task<IngredientDto> UpdateAsync(IngredientDto ingredientDto, string updatedById, CancellationToken cancellationToken)
        {
            var updatedOnUtc = DateTime.UtcNow;

            var commandDefinition = new CommandDefinition("IngredientUpdate"
                , ingredientDto.ToUpdateParameters(updatedById, updatedOnUtc)
                , commandType: CommandType.StoredProcedure
                , cancellationToken: cancellationToken);

            using var connection = await CreateConnectionAsync().ConfigureAwait(false);

            await connection.ExecuteAsync(commandDefinition).ConfigureAwait(false);

            return ingredientDto;
        }

        public async Task<int> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var commandDefinition = new CommandDefinition("IngredientDelete"
                , new { id }
                , commandType: CommandType.StoredProcedure
                , cancellationToken: cancellationToken);

            using var connection = await CreateConnectionAsync().ConfigureAwait(false);

            return await connection.ExecuteScalarAsync<int>(commandDefinition).ConfigureAwait(false);
        }
    }
}