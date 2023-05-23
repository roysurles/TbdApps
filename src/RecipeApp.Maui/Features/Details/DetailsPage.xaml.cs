namespace RecipeApp.Maui.Features.Details;

public partial class DetailsPage : ContentPage
{
    protected readonly IDetailsPageViewModel _detailsPageViewModel;

    public DetailsPage(IDetailsPageViewModel detailsPageViewModel)
    {
        InitializeComponent();
        BindingContext = _detailsPageViewModel = detailsPageViewModel;
    }

    private async void IntroductionEditComponent_SaveIntroductionClicked(object sender, EventArgs e)
    {
        await App.Current.MainPage.DisplayAlert("Save", $"Save {_detailsPageViewModel.IntroductionId}?", "Ok");
    }
}