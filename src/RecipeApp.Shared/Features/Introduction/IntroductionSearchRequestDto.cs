﻿namespace RecipeApp.Shared.Features.Introduction;

public class IntroductionSearchRequestDto : PaginationRequestModel<IntroductionSearchResultDto>
{
    [MaxLength(50, ErrorMessage = "Search Text cannot exceed 50 characters.")]
    public string SearchText { get; set; }
}
