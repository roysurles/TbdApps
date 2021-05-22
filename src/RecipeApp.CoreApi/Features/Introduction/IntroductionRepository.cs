using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Dapper;

using RecipeApp.Shared.Features.Introduction;

using Tbd.Shared.Pagination;
using Tbd.WebApi.Shared.Repositories;

namespace RecipeApp.CoreApi.Features.Introduction
{
    internal class IntroductionRepository : BaseRepository, IIntroductionRepository
    {
        public IntroductionRepository(string connectionString) : base(connectionString) { }

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

            //const int totalItemCount = 200;
            //var data = new List<IntroductionSearchResultDto>()
            //{
            //    new IntroductionSearchResultDto{ Id = Guid.NewGuid(), Title = "Title1", Comment = "Comment1", IngredientsCount = 3, InstructionsCount = 4 },
            //    new IntroductionSearchResultDto{ Id = Guid.NewGuid(), Title = "Title2", Comment = "Comment2", IngredientsCount = 7, InstructionsCount = 5 },
            //    new IntroductionSearchResultDto{ Id = Guid.NewGuid(), Title = "Title3", Comment = "Comment3", IngredientsCount = 9, InstructionsCount = 10 }
            //};

            //return (CreatePaginationMetaDataModel(introductionSearchRequestDto.PageNumber
            //    , introductionSearchRequestDto.PageSize
            //    , totalItemCount), data);
        }

        [SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
        public async Task<IntroductionDto> SelectAsync(Guid id)
        {
            using var connection = await CreateConnectionAsync().ConfigureAwait(false);
            return await connection.QueryFirstOrDefaultAsync<IntroductionDto>("IntroductionSelect"
                        , new { id }
                        , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
        }

        [SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
        public async Task<IntroductionDto> InsertAsync(IntroductionDto introductionDto, string createdById)
        {
            var id = Guid.NewGuid();
            var createdOnUtc = DateTime.UtcNow;

            using var connection = await CreateConnectionAsync().ConfigureAwait(false);

            await connection.ExecuteAsync("IntroductionInsert"
                , introductionDto.ToInsertParameters(id, createdById, createdOnUtc)
                , commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            return introductionDto;
        }

        [SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
        public async Task<IntroductionDto> UpdateAsync(IntroductionDto introductionDto, string updatedById)
        {
            var updatedOnUtc = DateTime.UtcNow;

            using var connection = await CreateConnectionAsync().ConfigureAwait(false);

            await connection.ExecuteAsync("IntroductionUpdate"
                , introductionDto.ToUpdateParameters(updatedById, updatedOnUtc)
                , commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            return introductionDto;
        }

        [SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
        public async Task<int> DeleteAsync(Guid id)
        {
            using var connection = await CreateConnectionAsync().ConfigureAwait(false);
            using var transaction = connection.BeginTransaction();

            return await connection.ExecuteScalarAsync<int>("IntroductionDelete"
                , new { id }
                , transaction
                , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
        }
    }

    public interface IIntroductionRepository
    {
        Task<(PaginationMetaDataModel PaginationMetaData, IEnumerable<IntroductionSearchResultDto> Data)> SearchAsync(IntroductionSearchRequestDto introductionSearchRequestDto);

        Task<IntroductionDto> SelectAsync(Guid id);

        Task<IntroductionDto> InsertAsync(IntroductionDto introductionDto, string createdById);

        Task<IntroductionDto> UpdateAsync(IntroductionDto introductionDto, string updatedById);

        Task<int> DeleteAsync(Guid id);
    }
}
