using Tbd.Shared.Pagination;

namespace RecipeApp.Shared.Features.Introduction
{
    public class IntroductionSearchRequestDto
    {
        public string SearchText { get; set; }

        public IPaginationRequestModel PaginationRequest { get; } =
            new PaginationRequestModel();
    }
}
