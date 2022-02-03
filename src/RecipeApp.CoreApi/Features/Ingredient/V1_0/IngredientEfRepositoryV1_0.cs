using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using RecipeApp.Database.Ef.RecipeDb;
using RecipeApp.Shared.Features.Ingredient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RecipeApp.CoreApi.Features.Ingredient.V1_0
{
    internal class IngredientEfRepositoryV1_0 : IIngredientRepositoryV1_0
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ILogger<IngredientEfRepositoryV1_0> _logger;

        public IngredientEfRepositoryV1_0(IServiceProvider serviceProvider, ILogger<IngredientEfRepositoryV1_0> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task<IngredientDto> SelectAsync(Guid id, CancellationToken cancellationToken)
        {
            using var dbContext = CreateNewRecipeDbContext();

            // 1) query against table
            //var ingredientModel = await dbContext.Ingredients.SingleAsync(m => Equals(id, m.Id), cancellationToken);

            // 2) exec sproc
            //var ingredientModel = await dbContext.Ingredients.FromSqlInterpolated($"EXEC IngredientSelect {id}").SingleAsync(cancellationToken); // This causes exception -- see IntroductionEfRepositoryV1_0.SelectAsync for fix
            var query = dbContext.Ingredients.FromSqlInterpolated($"EXEC IngredientSelect {id}");
            var queryString = query.ToQueryString();        // interrogate the generated sql
            var data = await query.ToListAsync(cancellationToken);
            var ingredientModel = data.Single();

            var ingredientDto = new IngredientDto
            {
                Id = ingredientModel.Id,
                IntroductionId = ingredientModel.IntroductionId,
                Measurement = ingredientModel.Measurement,
                Description = ingredientModel.Description,
                CreatedById = ingredientModel.CreatedById,
                CreatedOnUtc = ingredientModel.CreatedOnUtc,
                UpdatedById = ingredientModel.UpdatedById,
                UpdatedOnUtc = ingredientModel.UpdatedOnUtc
            };

            return ingredientDto;
        }

        public async Task<IEnumerable<IngredientDto>> SelectAllForIntroductionIdAsync(Guid introductionId, CancellationToken cancellationToken)
        {
            using var dbContext = CreateNewRecipeDbContext();

            // 1) query against table
            //  var ingredientDtos = dbContext.Ingredients.Where(c => Equals(introductionId, c.IntroductionId))
            //    .Select(m => new IngredientDto { Id = m.Id, IntroductionId = m.IntroductionId, Measurement = m.Measurement, Description = m.Description, CreatedById = m.CreatedById, CreatedOnUtc = m.CreatedOnUtc, UpdatedById = m.UpdatedById, UpdatedOnUtc = m.UpdatedOnUtc })
            //    .ToListAsync(cancellationToken);

            // 2) exec sproc
            // Use the following technique to view the generated query and make sure it is parameterized to prevent sql injection
            var query = dbContext.Ingredients.FromSqlInterpolated($"EXEC IngredientSelectAllForIntroductionId {introductionId}");
            var queryString = query.ToQueryString();

            var ingredientDtos = (await query
                .ToListAsync(cancellationToken))
                .Select(m => new IngredientDto { Id = m.Id, IntroductionId = m.IntroductionId, Measurement = m.Measurement, Description = m.Description, CreatedById = m.CreatedById, CreatedOnUtc = m.CreatedOnUtc, UpdatedById = m.UpdatedById, UpdatedOnUtc = m.UpdatedOnUtc });

            return await Task.FromResult(ingredientDtos);
        }

        public async Task<IngredientDto> InsertAsync(IngredientDto ingredientDto, string createdById, CancellationToken cancellationToken)
        {
            ingredientDto.Id = Guid.NewGuid();
            ingredientDto.CreatedById = createdById;
            ingredientDto.CreatedOnUtc = DateTime.UtcNow;

            using var dbContext = CreateNewRecipeDbContext();

            await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC IngredientInsert {ingredientDto.Id}, {ingredientDto.IntroductionId}, {ingredientDto.Measurement}, {ingredientDto.Description}, {ingredientDto.CreatedById}, {ingredientDto.CreatedOnUtc}", cancellationToken);

            return ingredientDto;
        }

        public async Task<IngredientDto> UpdateAsync(IngredientDto ingredientDto, string updatedById, CancellationToken cancellationToken)
        {
            ingredientDto.UpdatedById = updatedById;
            ingredientDto.UpdatedOnUtc = DateTime.UtcNow;

            using var dbContext = CreateNewRecipeDbContext();

            await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC IngredientUpdate {ingredientDto.Id}, {ingredientDto.Measurement}, {ingredientDto.Description}, {ingredientDto.UpdatedById}, {ingredientDto.UpdatedOnUtc}", cancellationToken);

            return ingredientDto;
        }

        public async Task<int> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            using var dbContext = CreateNewRecipeDbContext();

            return await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC IngredientDelete {id}", cancellationToken);
        }

        protected RecipeDbContext CreateNewRecipeDbContext() =>
            _serviceProvider.GetRequiredService<RecipeDbContext>();
    }
}
