using Dapper;

using RecipeApp.Shared.Features.Introduction;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

using Tbd.Shared.Pagination;
using Tbd.WebApi.Shared.Repositories;

namespace RecipeApp.CoreApi.Features.Introduction.V1_0
{
    internal class IntroductionRepositoryV1_0 : BaseRepository, IIntroductionRepositoryV1_0
    {
        public IntroductionRepositoryV1_0(string connectionString) : base(connectionString) { }

        public async Task<(PaginationMetaDataModel PaginationMetaData, IEnumerable<IntroductionSearchResultDto> Data)> SearchAsync(IntroductionSearchRequestDto introductionSearchRequestDto
            , CancellationToken cancellationToken)
        {
            var (IsValid, ErrorMessages) = introductionSearchRequestDto.OrderByClause.IsValid();
            if (IsValid == false)
                throw new InvalidOperationException(string.Join(", ", ErrorMessages));

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

            using var connection = await CreateConnectionAsync(cancellationToken).ConfigureAwait(false);
            var commandDefinition = new CommandDefinition(sql
                , new
                {
                    introductionSearchRequestDto.SearchText,
                    introductionSearchRequestDto.Offset,
                    introductionSearchRequestDto.Fetch
                }
                , commandType: CommandType.Text
                , cancellationToken: cancellationToken);

            using var gridReader = await connection.QueryMultipleAsync(commandDefinition).ConfigureAwait(false);

            var totalItemCount = await gridReader.ReadSingleAsync<int>().ConfigureAwait(false);
            var data = await gridReader.ReadAsync<IntroductionSearchResultDto>().ConfigureAwait(false);

            return (new(introductionSearchRequestDto.PageNumber
                , introductionSearchRequestDto.PageSize
                , totalItemCount), data);
        }

        public async Task<IntroductionDto> SelectAsync(Guid id, CancellationToken cancellationToken)
        {
            var commandDefinition = new CommandDefinition("IntroductionSelect"
                        , new { id }
                        , commandType: CommandType.StoredProcedure
                        , cancellationToken: cancellationToken);
            using var connection = await CreateConnectionAsync(cancellationToken).ConfigureAwait(false);
            return await connection.QuerySingleOrDefaultAsync<IntroductionDto>(commandDefinition).ConfigureAwait(false);
        }

        public async Task<IntroductionDto> InsertAsync(IntroductionDto introductionDto, string createdById, CancellationToken cancellationToken)
        {
            if (introductionDto.Id == Guid.Empty)
                introductionDto.Id = Guid.NewGuid();

            var commandDefinition = new CommandDefinition("IntroductionInsert"
                        , introductionDto.ToInsertParameters(createdById, DateTime.UtcNow)
                        , commandType: CommandType.StoredProcedure
                        , cancellationToken: cancellationToken);
            using var connection = await CreateConnectionAsync(cancellationToken).ConfigureAwait(false);
            await connection.ExecuteAsync(commandDefinition).ConfigureAwait(false);

            return introductionDto;
        }

        public async Task<IntroductionDto> UpdateAsync(IntroductionDto introductionDto, string updatedById, CancellationToken cancellationToken)
        {
            var commandDefinition = new CommandDefinition("IntroductionUpdate"
                        , introductionDto.ToUpdateParameters(updatedById, DateTime.UtcNow)
                        , commandType: CommandType.StoredProcedure
                        , cancellationToken: cancellationToken);
            using var connection = await CreateConnectionAsync(cancellationToken).ConfigureAwait(false);
            await connection.ExecuteAsync(commandDefinition).ConfigureAwait(false);

            return introductionDto;
        }

        public async Task<int> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            using var connection = await CreateConnectionAsync(cancellationToken).ConfigureAwait(false);
            using var transaction = await (connection as SqlConnection).BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
            var commandDefinition = new CommandDefinition("IntroductionDelete"
                        , new { id }
                        , transaction: transaction
                        , commandType: CommandType.StoredProcedure
                        , cancellationToken: cancellationToken);

            var result = await connection.ExecuteScalarAsync<int>(commandDefinition).ConfigureAwait(false);

            transaction.Commit();

            return result;
        }
    }
}
