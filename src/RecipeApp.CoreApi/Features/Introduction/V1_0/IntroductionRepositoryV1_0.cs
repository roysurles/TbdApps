using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
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
        public async Task<(PaginationMetaDataModel PaginationMetaData, IEnumerable<IntroductionSearchResultDto> Data)> SearchAsync(IntroductionSearchRequestDto introductionSearchRequestDto)
        {
            using var connection = await CreateConnectionAsync().ConfigureAwait(false);
            using var gridReader = await connection.QueryMultipleAsync("IntroductionSearch"
                , new
                {
                    introductionSearchRequestDto.SearchText,
                    introductionSearchRequestDto.Offset,
                    introductionSearchRequestDto.Fetch
                }
                , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
            var totalItemCount = await gridReader.ReadSingleAsync<int>().ConfigureAwait(false);
            var data = await gridReader.ReadAsync<IntroductionSearchResultDto>().ConfigureAwait(false);

            return (CreatePaginationMetaDataModel(introductionSearchRequestDto.PageNumber
                , introductionSearchRequestDto.PageSize
                , totalItemCount), data);
        }

        [SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
        public async Task<IntroductionDto> SelectAsync(Guid id)
        {
            using var connection = await CreateConnectionAsync().ConfigureAwait(false);
            return await connection.QuerySingleOrDefaultAsync<IntroductionDto>("IntroductionSelect"
                        , new { id }
                        , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
        }

        [SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
        public async Task<IntroductionDto> InsertAsync(IntroductionDto introductionDto, string createdById)
        {
            if (introductionDto.Id == Guid.Empty)
                introductionDto.Id = Guid.NewGuid();

            using var connection = await CreateConnectionAsync().ConfigureAwait(false);

            await connection.ExecuteAsync("IntroductionInsert"
                , introductionDto.ToInsertParameters(createdById, DateTime.UtcNow)
                , commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            return introductionDto;
        }

        [SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
        public async Task<IntroductionDto> UpdateAsync(IntroductionDto introductionDto, string updatedById)
        {
            using var connection = await CreateConnectionAsync().ConfigureAwait(false);

            await connection.ExecuteAsync("IntroductionUpdate"
                , introductionDto.ToUpdateParameters(updatedById, DateTime.UtcNow)
                , commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            return introductionDto;
        }

        [SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
        public async Task<int> DeleteAsync(Guid id)
        {
            using var connection = await CreateConnectionAsync().ConfigureAwait(false);
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteScalarAsync<int>("IntroductionDelete"
                , new { id }
                , transaction
                , commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            transaction.Commit();

            return result;
        }
    }

    public interface IIntroductionRepositoryV1_0
    {
        Task<(PaginationMetaDataModel PaginationMetaData, IEnumerable<IntroductionSearchResultDto> Data)> SearchAsync(IntroductionSearchRequestDto introductionSearchRequestDto);

        Task<IntroductionDto> SelectAsync(Guid id);

        Task<IntroductionDto> InsertAsync(IntroductionDto introductionDto, string createdById);

        Task<IntroductionDto> UpdateAsync(IntroductionDto introductionDto, string updatedById);

        Task<int> DeleteAsync(Guid id);
    }
}
