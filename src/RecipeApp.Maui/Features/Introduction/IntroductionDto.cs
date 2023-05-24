
namespace RecipeApp.Maui.Features.Introduction;

public partial class IntroductionDto : BaseDto
{
    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    [Required]
    [MaxLength(50, ErrorMessage = "Title cannot exceed 50 characters.")]
    public string title;

    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    [MaxLength(255, ErrorMessage = "Comment cannot exceed 255 characters.")]
    public string comment;
}
