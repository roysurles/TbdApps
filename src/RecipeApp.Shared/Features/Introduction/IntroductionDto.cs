﻿namespace RecipeApp.Shared.Features.Introduction;

public class IntroductionDto : BaseDto
{
    [Required]
    [MaxLength(50, ErrorMessage = "Title cannot exceed 50 characters.")]
    public string Title { get; set; }

    [MaxLength(255, ErrorMessage = "Comment cannot exceed 255 characters.")]
    public string Comment { get; set; }
}
