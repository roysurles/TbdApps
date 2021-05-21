using Tbd.Shared.Pagination;

namespace RecipeApp.Shared.Features.Introduction
{
    public class IntroductionSearchRequestDto : PaginationRequestModel
    {
        public string SearchText { get; set; }
    }
}
