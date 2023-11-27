namespace RecipeApp.Shared.Features.Introduction;

public interface IIntroductionApiClientV1_0
{
    [Post("/api/v1.0/Introduction/search")]
    Task<ApiResultModel<List<IntroductionSearchResultDto>>> SearchAsync([Body] IntroductionSearchRequestDto introductionSearchRequestDto);

    [Get("/api/v1.0/Introduction/{id}")]
    Task<ApiResultModel<IntroductionDto>> GetAsync(Guid id);

    [Post("/api/v1.0/Introduction")]
    Task<ApiResultModel<IntroductionDto>> InsertAsync([Body] IntroductionDto introductionDto);

    [Put("/api/v1.0/Introduction")]
    Task<ApiResultModel<IntroductionDto>> UpdateAsync([Body] IntroductionDto introductionDto);

    [Delete("/api/v1.0/Introduction/{id}")]
    Task<ApiResultModel<int>> DeleteAsync(Guid id);
}
