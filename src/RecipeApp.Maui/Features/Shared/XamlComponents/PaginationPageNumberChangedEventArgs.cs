namespace RecipeApp.Maui.Features.Shared.XamlComponents;

public class PaginationPageNumberChangedEventArgs : EventArgs
{
    public int PageNumber { get; set; }

    public int PreviousPageNumber { get; set; }
}
