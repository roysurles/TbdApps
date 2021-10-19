using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Dapper;

using RecipeApp.Shared.Features.Introduction;

using Tbd.Shared.Pagination;
using Tbd.WebApi.Shared.Repositories;

namespace RecipeApp.CoreApi.Features.Introduction.V1_0
{
    internal class IntroductionRepositoryV1_0 : BaseRepository, IIntroductionRepositoryV1_0
    {
        public IntroductionRepositoryV1_0(string connectionString) : base(connectionString) { }

        [SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
        public async Task<(PaginationMetaDataModel PaginationMetaData, IEnumerable<IntroductionSearchResultDto> Data)> SearchAsync(IntroductionSearchRequestDto introductionSearchRequestDto
            , CancellationToken cancellationToken)
        {
            var commandDefinition = new CommandDefinition("IntroductionSearch"
                , new
                {
                    introductionSearchRequestDto.SearchText,
                    introductionSearchRequestDto.Offset,
                    introductionSearchRequestDto.Fetch
                }
                , commandType: CommandType.StoredProcedure
                , cancellationToken: cancellationToken);
            using var connection = await CreateConnectionAsync(cancellationToken).ConfigureAwait(false);
            using var gridReader = await connection.QueryMultipleAsync(commandDefinition).ConfigureAwait(false);
            var totalItemCount = await gridReader.ReadSingleAsync<int>().ConfigureAwait(false);
            var data = await gridReader.ReadAsync<IntroductionSearchResultDto>().ConfigureAwait(false);

            return (CreatePaginationMetaDataModel(introductionSearchRequestDto.PageNumber
                , introductionSearchRequestDto.PageSize
                , totalItemCount), data);
        }

        [SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
        public async Task<IntroductionDto> SelectAsync(Guid id, CancellationToken cancellationToken)
        {
            var commandDefinition = new CommandDefinition("IntroductionSelect"
                        , new { id }
                        , commandType: CommandType.StoredProcedure
                        , cancellationToken: cancellationToken);
            using var connection = await CreateConnectionAsync(cancellationToken).ConfigureAwait(false);
            return await connection.QuerySingleOrDefaultAsync<IntroductionDto>(commandDefinition).ConfigureAwait(false);
        }

        [SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
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

        [SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
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

        [SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
        public async Task<int> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var commandDefinition = new CommandDefinition("IntroductionDelete"
                        , new { id }
                        , commandType: CommandType.StoredProcedure
                        , cancellationToken: cancellationToken);
            using var connection = await CreateConnectionAsync(cancellationToken).ConfigureAwait(false);
            using var transaction = connection.BeginTransaction();
            var result = await connection.ExecuteScalarAsync<int>(commandDefinition).ConfigureAwait(false);

            transaction.Commit();

            return result;
        }
    }

    public interface IIntroductionRepositoryV1_0
    {
        Task<(PaginationMetaDataModel PaginationMetaData, IEnumerable<IntroductionSearchResultDto> Data)> SearchAsync(IntroductionSearchRequestDto introductionSearchRequestDto, CancellationToken cancellationToken);

        Task<IntroductionDto> SelectAsync(Guid id, CancellationToken cancellationToken);

        Task<IntroductionDto> InsertAsync(IntroductionDto introductionDto, string createdById, CancellationToken cancellationToken);

        Task<IntroductionDto> UpdateAsync(IntroductionDto introductionDto, string updatedById, CancellationToken cancellationToken);

        Task<int> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
