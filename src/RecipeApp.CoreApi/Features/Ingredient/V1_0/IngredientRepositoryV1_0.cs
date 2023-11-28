using System.Data;
using System.Data.SqlClient;

namespace RecipeApp.CoreApi.Features.Ingredient.V1_0;

internal class IngredientRepositoryV1_0 : BaseRepository, IIngredientRepositoryV1_0
{
    public IngredientRepositoryV1_0(string connectionString) : base(connectionString) { }

    public async Task<IngredientDto> SelectAsync(Guid id, CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition("IngredientSelect"
            , new { id }
            , commandType: CommandType.StoredProcedure
            , cancellationToken: cancellationToken);

        using var connection = await CreateConnectionAsync(cancellationToken).ConfigureAwait(false);

        return await connection.QuerySingleOrDefaultAsync<IngredientDto>(commandDefinition).ConfigureAwait(false);
    }

    public async Task<IEnumerable<IngredientDto>> SelectAllForIntroductionIdAsync(Guid introductionId, CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition("IngredientSelectAllForIntroductionId"
            , new { introductionId }
            , commandType: CommandType.StoredProcedure
            , cancellationToken: cancellationToken);

        using var connection = await CreateConnectionAsync(cancellationToken).ConfigureAwait(false);

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

        using var connection = await CreateConnectionAsync(cancellationToken).ConfigureAwait(false);

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

        using var connection = await CreateConnectionAsync(cancellationToken).ConfigureAwait(false);

        await connection.ExecuteAsync(commandDefinition).ConfigureAwait(false);

        return ingredientDto;
    }

    public async Task<int> UpdateMultipleAsync(IngredientsDto ingredientsDto, string updatedById, CancellationToken cancellationToken)
    {
        var result = 0;

        if (ingredientsDto.Ingredients.Count == 0)
            return result;

        var updatedOnUtc = DateTime.UtcNow;

        using var connection = await CreateConnectionAsync(cancellationToken).ConfigureAwait(false);
        using var transaction = await (connection as SqlConnection).BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

        foreach (var ingredientDto in ingredientsDto.Ingredients)
        {
            var commandDefinition = new CommandDefinition("IngredientUpdate"
                , ingredientDto.ToUpdateParameters(updatedById, updatedOnUtc)
                , transaction: transaction
                , commandType: CommandType.StoredProcedure
                , cancellationToken: cancellationToken);

            result += await connection.ExecuteAsync(commandDefinition).ConfigureAwait(false);
        }

        transaction.Commit();

        return result;
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