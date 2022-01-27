using RecipeApp.Shared.Features.Introduction;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Tbd.Shared.Pagination;

namespace RecipeApp.CoreApi.Features.Introduction.V1_0
{
    public interface IIntroductionRepositoryV1_0
    {
        Task<(PaginationMetaDataModel PaginationMetaData, IEnumerable<IntroductionSearchResultDto> Data)> SearchAsync(IntroductionSearchRequestDto introductionSearchRequestDto, CancellationToken cancellationToken);

        Task<IntroductionDto> SelectAsync(Guid id, CancellationToken cancellationToken);

        Task<IntroductionDto> InsertAsync(IntroductionDto introductionDto, string createdById, CancellationToken cancellationToken);

        Task<IntroductionDto> UpdateAsync(IntroductionDto introductionDto, string updatedById, CancellationToken cancellationToken);

        Task<int> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
