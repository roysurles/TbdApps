namespace RecipeApp.Maui.Features.Instruction;

public partial class InstructionDto : BaseDto
{
    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public Guid introductionId;

    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public int sortOrder;

    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    [Required]
    [MaxLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
    public string description;
}
