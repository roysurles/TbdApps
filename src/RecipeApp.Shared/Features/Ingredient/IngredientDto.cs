namespace RecipeApp.Shared.Features.Ingredient;

public class IngredientDto : BaseDto
{
    public Guid IntroductionId { get; set; }

    public int SortOrder { get; set; }

    [Required]
    [MaxLength(50, ErrorMessage = "Measurement cannot exceed 50 characters.")]
    public string Measurement { get; set; }

    [Required]
    [MaxLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
    public string Description { get; set; }
}
