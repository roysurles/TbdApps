namespace RecipeApp.Maui.Features.Instruction;

public partial class InstructionsDto : ObservableObject
{
    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public ObservableCollection<InstructionDto> instructions = new();
}
