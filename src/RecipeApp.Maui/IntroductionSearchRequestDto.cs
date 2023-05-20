
namespace RecipeApp.Maui;

public class IntroductionSearchRequestDto : PaginationRequestModel<IntroductionSearchResultDto>
{
    [MaxLength(50, ErrorMessage = "Search Text cannot exceed 50 characters.")]
    public string SearchText { get; set; }

    public IntroductionSearchRequestDto SetSearchText(string searchText)
    {
        SearchText = searchText;
        return this;
    }
}
