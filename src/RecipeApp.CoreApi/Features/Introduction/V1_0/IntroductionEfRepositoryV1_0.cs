using System.Data;

using Microsoft.Data.SqlClient;


namespace RecipeApp.CoreApi.Features.Introduction.V1_0;

internal class IntroductionEfRepositoryV1_0 : IIntroductionRepositoryV1_0
{
    protected readonly IServiceProvider _serviceProvider;
    protected readonly ILogger<IntroductionEfRepositoryV1_0> _logger;

    public IntroductionEfRepositoryV1_0(IServiceProvider serviceProvider, ILogger<IntroductionEfRepositoryV1_0> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task<(PaginationMetaDataModel PaginationMetaData, IEnumerable<IntroductionSearchResultDto> Data)> SearchAsync(IntroductionSearchRequestDto introductionSearchRequestDto, CancellationToken cancellationToken)
    {
        var sql = @$"
                IF RTRIM(@SearchText) = ''
                    SET @SearchText = NULL

                SELECT COUNT(Id) AS TotalItemCount
                FROM   dbo.Introduction
                WHERE  @SearchText IS NULL
                       OR @SearchText IS NOT NULL
                       AND Title LIKE '%' + @SearchText + '%'
                       OR @SearchText IS NOT NULL
                       AND Comment LIKE '%' + @SearchText + '%';

                SELECT    Introduction.Id
                         ,Introduction.Title
                         ,Introduction.Comment
                         ,IngredientsCount = ( SELECT COUNT(*) FROM dbo.Ingredient WHERE IntroductionId = Introduction.Id )
                         ,InstructionsCount = ( SELECT COUNT(*) FROM dbo.Instruction WHERE IntroductionId = Introduction.Id )
                FROM dbo.Introduction Introduction
                        WHERE @SearchText IS NULL
                              OR @SearchText IS NOT NULL
                              AND Title LIKE '%' + @SearchText + '%'
                              OR @SearchText IS NOT NULL
                              AND Comment LIKE '%' + @SearchText + '%'
                {introductionSearchRequestDto.OrderByClause.ToSqlString()}
                OFFSET @Offset ROWS FETCH NEXT @Fetch ROWS ONLY";

        using var dbContext = CreateNewRecipeDbContext();
        using var connection = dbContext.Database.GetDbConnection();

        using var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = sql;

        var parameters = new SqlParameter[] {
            new SqlParameter() {
                ParameterName = "@SearchText",
                SqlDbType =  SqlDbType.NVarChar,
                Size =  50,
                Direction = ParameterDirection.Input,
                Value = introductionSearchRequestDto.SearchText ?? string.Empty
            },
            new SqlParameter() {
                ParameterName = "@Offset",
                SqlDbType =  SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = introductionSearchRequestDto.Offset
            },
            new SqlParameter() {
                ParameterName = "@Fetch",
                SqlDbType =  SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = introductionSearchRequestDto.Fetch
            }
        };
        command.Parameters.AddRange(parameters);

        await connection.OpenAsync(cancellationToken);

        using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var totalItemCount = 0;
        var data = new List<IntroductionSearchResultDto>();

        while (await reader.ReadAsync(cancellationToken))
            totalItemCount = reader.GetInt32("TotalItemCount");

        await reader.NextResultAsync(cancellationToken); // next record set

        while (await reader.ReadAsync(cancellationToken))
        {

            data.Add(new IntroductionSearchResultDto
            {
                Id = reader.GetGuid("Id"),
                Title = reader.GetString("Title"),
                Comment = await reader.IsDBNullAsync("Comment", cancellationToken) ? null : reader.GetString("Comment"),
                IngredientsCount = reader.GetInt32("IngredientsCount"),
                InstructionsCount = reader.GetInt32("InstructionsCount")
            });
        }

        return (new PaginationMetaDataModel(introductionSearchRequestDto.PageNumber
                                            , introductionSearchRequestDto.PageSize
                                            , totalItemCount)
            , data);
    }

    public async Task<IntroductionDto> SelectAsync(Guid id, CancellationToken cancellationToken)
    {
        using var dbContext = CreateNewRecipeDbContext();

        // 1) query against table
        //var InstructionDto = await dbContext.Introductions.SingleAsync(m => Equals(id, m.Id), cancellationToken);

        // 2) exec sproc
        //var introductionModel = await dbContext.Introductions.FromSqlInterpolated($"EXEC IntroductionSelect {id}").SingleAsync(cancellationToken);  // This causes exception
        var query = dbContext.Introductions.FromSqlInterpolated($"EXEC IntroductionSelect {id}");
        var queryString = query.ToQueryString();        // interrogate the generated sql
        var data = await query.ToListAsync(cancellationToken);
        var introductionModel = data.Single();

        var introductionDto = new IntroductionDto
        {
            Id = introductionModel.Id,
            Title = introductionModel.Title,
            Comment = introductionModel.Comment,
            CreatedById = introductionModel.CreatedById,
            CreatedOnUtc = introductionModel.CreatedOnUtc,
            UpdatedById = introductionModel.UpdatedById,
            UpdatedOnUtc = introductionModel.UpdatedOnUtc
        };

        return introductionDto;
    }

    public async Task<IntroductionDto> InsertAsync(IntroductionDto introductionDto, string createdById, CancellationToken cancellationToken)
    {
        introductionDto.Id = Guid.NewGuid();
        introductionDto.CreatedById = createdById;
        introductionDto.CreatedOnUtc = DateTime.UtcNow;

        using var dbContext = CreateNewRecipeDbContext();

        await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC IntroductionInsert {introductionDto.Id}, {introductionDto.Title}, {introductionDto.Comment}, {introductionDto.CreatedById}, {introductionDto.CreatedOnUtc}", cancellationToken);

        return introductionDto;
    }

    public async Task<IntroductionDto> UpdateAsync(IntroductionDto introductionDto, string updatedById, CancellationToken cancellationToken)
    {
        introductionDto.UpdatedById = updatedById;
        introductionDto.UpdatedOnUtc = DateTime.UtcNow;

        using var dbContext = CreateNewRecipeDbContext();

        await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC IntroductionUpdate {introductionDto.Id}, {introductionDto.Title}, {introductionDto.Comment}, {introductionDto.UpdatedById}, {introductionDto.UpdatedOnUtc}", cancellationToken);

        return introductionDto;
    }

    public async Task<int> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        using var dbContext = CreateNewRecipeDbContext();
        using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        var result = await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC IntroductionDelete {id}", cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return result;
    }

    protected RecipeDbContext CreateNewRecipeDbContext() =>
        _serviceProvider.GetRequiredService<RecipeDbContext>();
}
