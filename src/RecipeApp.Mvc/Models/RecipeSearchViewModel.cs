namespace RecipeApp.Mvc.Models;

public class RecipeSearchViewModel
{
    public RecipeSearchViewModel()
    {
        if (IntroductionSearchResult is null)
        {
            IntroductionSearchResult = new ApiResultModel<List<IntroductionSearchResultDto>>();

            IntroductionSearchResult.SetData([]);
            IntroductionSearchResult.SetMeta(1, 10, 0);
        }
    }

    public bool HasSearched { get; set; }

    public string SearchText { get; set; } = string.Empty;

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public ApiResultModel<List<IntroductionSearchResultDto>> IntroductionSearchResult { get; set; }
}
