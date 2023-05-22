namespace RecipeApp.Maui;

/*  TODO:
 *      - TextColor to brush
 *      - Busy Indicator
 *          - Searching... please wait
 *      - SnackBar ... possible to move to top?  or hide keyboard
 *      - Swipe / Tap opens details
 *
 *      - global exception handler
 *      - Pagination
 *      - Session / Trace Id
 *      - Files / Feature Organization
 */
public partial class MainPageViewModel : BaseViewModel, IMainPageViewModel
{
    protected readonly ILogger<MainPageViewModel> _logger;

    public MainPageViewModel(IIntroductionSearchViewModel introductionSearchViewModel, ILogger<MainPageViewModel> logger)
    {
        IntroductionSearchViewModel = introductionSearchViewModel;
        _logger = logger;

        //App.Current.MainPage.DisplayAlert("Hello", "", "Cancel");
        //App.Current.MainPage.DisplayPromptAsync
        //App.Current.MainPage.DisplaySnackbar

        //SearchAsyncCommand = new AsyncRelayCommand<object>((object searchText) => SearchAsync(searchText));

        // Enumeration class nuget package

        // SearchBar xml
    }

    public IIntroductionSearchViewModel IntroductionSearchViewModel { get; protected set; }

    // TODO:  investigate if we can bind to IntroductionSearchViewModel.SearchAsync()  -- SearchCommand
    [RelayCommand]
    public async Task<IMainPageViewModel> SearchAsync()
    {
        try
        {
            IsBusy = true;

            await IntroductionSearchViewModel.SearchAsync();
        }
        finally
        {
            IsBusy = false;
        }

        return this;
    }

    [RelayCommand]
    public async Task DeleteIntroductionAsync(object introductionSearchResultDto)
    {
        var introduction = introductionSearchResultDto as IntroductionSearchResultDto;
        if (introduction is null)
        {
            await App.Current.MainPage.DisplayAlert("Mismatch", "Cannot convert introductionSearchResultDto to IntroductionSearchResultDto", "Ok");
            return;
        }

        await App.Current.MainPage.DisplayAlert("Delete", $"Delete {introduction.Title}?", "Ok");
    }

    [RelayCommand]
    public async Task EditIntroductionAsync(object introductionSearchResultDto)
    {
        var introduction = introductionSearchResultDto as IntroductionSearchResultDto;
        if (introduction is null)
        {
            await App.Current.MainPage.DisplayAlert("Mismatch", "Cannot convert introductionSearchResultDto to IntroductionSearchResultDto", "Ok");
            return;
        }

        await App.Current.MainPage.DisplayAlert("Edit", $"Navigate to {introduction.Title}?", "Ok");

        // https://www.youtube.com/watch?v=ddmZ6k1GIkM
        //await Shell.Current.Navigation.PushAsync
        await Shell.Current.GoToAsync($"{nameof(DetailsPage)}?IntroductionId={introduction.Id}");
    }
}

public interface IMainPageViewModel : IBaseViewModel
{
    IIntroductionSearchViewModel IntroductionSearchViewModel { get; }

    Task<IMainPageViewModel> SearchAsync();

    Task DeleteIntroductionAsync(object introductionSearchResultDto);

    Task EditIntroductionAsync(object introductionSearchResultDto);
}
