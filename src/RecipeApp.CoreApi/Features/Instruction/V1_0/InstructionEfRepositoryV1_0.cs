﻿namespace RecipeApp.CoreApi.Features.Instruction.V1_0;

internal class InstructionEfRepositoryV1_0 : IInstructionRepositoryV1_0
{
    protected readonly IServiceProvider _serviceProvider;
    protected readonly ILogger<InstructionEfRepositoryV1_0> _logger;

    public InstructionEfRepositoryV1_0(IServiceProvider serviceProvider, ILogger<InstructionEfRepositoryV1_0> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task<InstructionDto> SelectAsync(Guid id, CancellationToken cancellationToken)
    {
        using var dbContext = CreateNewRecipeDbContext();

        // 1) query against table
        //var InstructionDto = await dbContext.Instructions.SingleAsync(m => Equals(id, m.Id), cancellationToken);

        // 2) exec sproc
        var instructionModel = await dbContext.Instructions.FromSqlInterpolated($"EXEC InstructionSelect {id}").SingleAsync(cancellationToken); // This causes exception -- see IntroductionEfRepositoryV1_0.SelectAsync for fix

        var instructionDto = new InstructionDto
        {
            Id = instructionModel.Id,
            IntroductionId = instructionModel.IntroductionId,
            SortOrder = instructionModel.SortOrder,
            Description = instructionModel.Description,
            CreatedById = instructionModel.CreatedById,
            CreatedOnUtc = instructionModel.CreatedOnUtc,
            UpdatedById = instructionModel.UpdatedById,
            UpdatedOnUtc = instructionModel.UpdatedOnUtc
        };

        return instructionDto;
    }

    public async Task<IEnumerable<InstructionDto>> SelectAllForIntroductionIdAsync(Guid introductionId, CancellationToken cancellationToken)
    {
        using var dbContext = CreateNewRecipeDbContext();

        // 1) query against table
        //  var instructionDtos = dbContext.Instructions.Where(c => Equals(introductionId, c.IntroductionId))
        //    .Select(m => new InstructionDto { Id = m.Id, IntroductionId = m.IntroductionId, Description = m.Description, CreatedById = m.CreatedById, CreatedOnUtc = m.CreatedOnUtc, UpdatedById = m.UpdatedById, UpdatedOnUtc = m.UpdatedOnUtc })
        //    .ToListAsync(cancellationToken);

        // 2) exec sproc
        // Use the following technique to view the generated query and make sure it is parameterized to prevent sql injection
        var query = dbContext.Instructions.FromSqlInterpolated($"EXEC InstructionSelectAllForIntroductionId {introductionId}");
        var queryString = query.ToQueryString();

        var instructionDtos = (await query
            .ToListAsync(cancellationToken))
            .Select(m => new InstructionDto { Id = m.Id, IntroductionId = m.IntroductionId, SortOrder = m.SortOrder, Description = m.Description, CreatedById = m.CreatedById, CreatedOnUtc = m.CreatedOnUtc, UpdatedById = m.UpdatedById, UpdatedOnUtc = m.UpdatedOnUtc });

        return await Task.FromResult(instructionDtos);
    }

    public async Task<InstructionDto> InsertAsync(InstructionDto instructionDto, string createdById, CancellationToken cancellationToken)
    {
        instructionDto.Id = Guid.NewGuid();
        instructionDto.CreatedById = createdById;
        instructionDto.CreatedOnUtc = DateTime.UtcNow;

        using var dbContext = CreateNewRecipeDbContext();

        await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC InstructionInsert {instructionDto.Id}, {instructionDto.IntroductionId}, {instructionDto.SortOrder}, {instructionDto.Description}, {instructionDto.CreatedById}, {instructionDto.CreatedOnUtc}", cancellationToken);

        return instructionDto;
    }

    public async Task<InstructionDto> UpdateAsync(InstructionDto instructionDto, string updatedById, CancellationToken cancellationToken)
    {
        instructionDto.UpdatedById = updatedById;
        instructionDto.UpdatedOnUtc = DateTime.UtcNow;

        using var dbContext = CreateNewRecipeDbContext();

        await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC InstructionUpdate {instructionDto.Id}, {instructionDto.SortOrder}, {instructionDto.Description}, {instructionDto.UpdatedById}, {instructionDto.UpdatedOnUtc}", cancellationToken);

        return instructionDto;
    }

    public async Task<int> UpdateMultipleAsync(InstructionsDto instructionsDto, string updatedById, CancellationToken cancellationToken)
    {
        var result = 0;

        if (instructionsDto.Instructions.Count == 0)
            return result;

        var updatedOnUtc = DateTime.UtcNow;

        using var dbContext = CreateNewRecipeDbContext();
        using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        foreach (var instructionDto in instructionsDto.Instructions)
            result += await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC InstructionUpdate {instructionDto.Id}, {instructionDto.SortOrder}, {instructionDto.Description}, {updatedById}, {updatedOnUtc}", cancellationToken);

        transaction.Commit();

        return result;
    }

    public async Task<int> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        using var dbContext = CreateNewRecipeDbContext();

        return await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC InstructionDelete {id}", cancellationToken);
    }

    protected RecipeDbContext CreateNewRecipeDbContext() =>
        _serviceProvider.GetRequiredService<RecipeDbContext>();
}
