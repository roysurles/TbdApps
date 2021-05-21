using System.Collections.Generic;
using System.Threading.Tasks;

using RecipeApp.Shared.Features.Introduction;

using Refit;

using Tbd.Shared.ApiResult;

namespace RecipeApp.BlazorWasmBootstrap.Features.Shared.IntroductionSearch
{
    public interface IIntroductionV1_0ApiClient
    {
        [Post("/api/v1.0/Introduction/search")]
        Task<ApiResponse<ApiResultModel<IEnumerable<IntroductionSearchResultDto>>>> SearchAsync([Body] IntroductionSearchRequestDto introductionSearchRequestDto);

        //[Post("/api/v1.0/Introduction/search")]
        //Task<ApiResultModel<IEnumerable<IntroductionSearchResultDto>>> SearchAsync([Body] IntroductionSearchRequestDto introductionSearchRequestDto);
    }
}
