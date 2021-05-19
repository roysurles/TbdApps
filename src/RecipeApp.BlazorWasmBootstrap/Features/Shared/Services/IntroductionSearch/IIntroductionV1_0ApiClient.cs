using System.Collections.Generic;
using System.Threading.Tasks;

using RecipeApp.Shared.Features.Introduction;

using Refit;

using Tbd.Shared.ApiResult;

namespace RecipeApp.BlazorWasmBootstrap.Features.Shared.Services.IntroductionSearch
{
    public interface IIntroductionV1_0ApiClient
    {
        [Get("/api/v1.0/Introduction/search")]
        Task<IApiResultModel<IEnumerable<IntroductionSearchResultDto>>> SearchAsync([Body] IntroductionSearchRequestDto introductionSearchRequestDto);
    }
}
