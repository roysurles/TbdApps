namespace RecipeApp.Maui.Features.Ingredient;

public partial class IngredientsDto : ObservableObject
{
    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public ObservableCollection<IngredientDto> ingredients = new();
}
