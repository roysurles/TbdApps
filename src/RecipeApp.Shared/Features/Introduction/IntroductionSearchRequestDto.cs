using System.ComponentModel.DataAnnotations;

using Tbd.Shared.Pagination;

namespace RecipeApp.Shared.Features.Introduction
{
    public class IntroductionSearchRequestDto : PaginationRequestModel
    {
        [MaxLength(50, ErrorMessage = "Search Text cannot exceed 50 characters.")]
        public string SearchText { get; set; }
    }
}
