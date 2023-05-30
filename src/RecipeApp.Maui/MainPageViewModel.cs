namespace RecipeApp.Maui;

/*  TODO:
 *      - TextColor to brush
 *      - Busy Indicator
 *          - Searching... please wait
 *      - SnackBar ... possible to move to top?  or hide keyboard
 *      - Swipe / Tap opens details
 *
 *      - Card Control - include session
 *      - global exception handler
 *      - Pagination
 *      - Session / Trace Id
 *      - Files / Feature Organization
 *      - Enumeration class nuget package
 *      - Blazor exceptions should be [...] button
 */
public partial class MainPageViewModel : BaseViewModel, IMainPageViewModel
{
    protected readonly ILogger<MainPageViewModel> _logger;

    public MainPageViewModel(IIntroductionSearchViewModel introductionSearchViewModel, IIntroductionViewModel introductionViewModel, ILogger<MainPageViewModel> logger)
    {
        IntroductionSearchViewModel = introductionSearchViewModel;
        IntroductionViewModel = introductionViewModel;
        _logger = logger;

        //SearchAsyncCommand = new AsyncRelayCommand<object>((object searchText) => SearchAsync(searchText));

        WeakReferenceMessenger.Default.Register<IsBusyValueChangedMessage>(this, (r, m) => IsBusy = m.Value);
    }

    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public IIntroductionSearchViewModel introductionSearchViewModel;

    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public IIntroductionViewModel introductionViewModel;

    [RelayCommand]
    public async Task EditIntroductionAsync(object introductionSearchResultDto)
    {
        var introduction = introductionSearchResultDto as IntroductionSearchResultDto;
        if (introduction is null)
        {
            await App.Current.MainPage.DisplayAlert("Mismatch", "Cannot convert introductionSearchResultDto to IntroductionSearchResultDto", Constants.AlertButtonText.OK);
            return;
        }

        //await App.Current.MainPage.DisplayAlert("Edit", $"Navigate to {introduction.Title}?", Constants.AlertButtonText.OK);

        // https://www.youtube.com/watch?v=ddmZ6k1GIkM
        //await Shell.Current.Navigation.PushAsync
        await Shell.Current.GoToAsync($"{nameof(DetailsPage)}?IntroductionId={introduction.Id}");
    }
}

public interface IMainPageViewModel : IBaseViewModel
{
    IIntroductionSearchViewModel IntroductionSearchViewModel { get; }

    IIntroductionViewModel IntroductionViewModel { get; }

    Task EditIntroductionAsync(object introductionSearchResultDto);
}
