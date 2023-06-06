namespace RecipeApp.Maui.Features.Shared.XamlComponents;

public partial class PaginationComponentButtonProperties : ObservableObject
{
    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public string text;

    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public Color backgroundColor = Colors.LightBlue;
}
