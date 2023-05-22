namespace RecipeApp.Maui.Features.Details;

public partial class DetailsPage : ContentPage
{
    protected readonly IDetailsPageViewModel _detailsPageViewModel;

    public DetailsPage(IDetailsPageViewModel detailsPageViewModel)
    {
        InitializeComponent();
        BindingContext = _detailsPageViewModel = detailsPageViewModel;
    }
}