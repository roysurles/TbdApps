namespace RecipeApp.Maui;

public partial class DetailsPage : ContentPage
{
    protected readonly IDetailsPageViewModel _detailsPageViewModel;

    public DetailsPage(IDetailsPageViewModel detailsPageViewModel)
    {
        InitializeComponent();
        BindingContext = _detailsPageViewModel = detailsPageViewModel;
    }
}