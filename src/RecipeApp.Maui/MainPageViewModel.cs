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

    public MainPageViewModel(IIntroductionSearchViewModel introductionSearchViewModel, ILogger<MainPageViewModel> logger)
    {
        IntroductionSearchViewModel = introductionSearchViewModel;
        _logger = logger;

        //SearchAsyncCommand = new AsyncRelayCommand<object>((object searchText) => SearchAsync(searchText));

        WeakReferenceMessenger.Default.Register<IsBusyValueChangedMessage>(this, (r, m) => IsBusy = m.Value);

        // TODO:  remove testing messages
        AddInformationMessage("Info 1", code: 200);
        AddInformationMessage("Info 2", code: 200);
        AddInformationMessage("Info 3", code: 200);
        AddInformationMessage("Info 4", code: 200);
        AddErrorMessage("Error 1", code: 600);
        AddErrorMessage("Error 2", code: 600);
        AddErrorMessage("Error 3", code: 600);
        AddErrorMessage("Error 4", code: 600);
        AddInformationMessage("Info 1", code: 200);
        AddInformationMessage("Info 2", code: 200);
        AddInformationMessage("Info 3", code: 200);
        AddInformationMessage("Info 4", code: 200);
        AddErrorMessage("Error 1", code: 600);
        AddErrorMessage("Error 2", code: 600);
        AddErrorMessage("Error 3", code: 600);
        AddErrorMessage("Error 4", code: 600);
        AddInformationMessage("Info 1", code: 200);
        AddInformationMessage("Info 2", code: 200);
        AddInformationMessage("Info 3", code: 200);
        AddInformationMessage("Info 4", code: 200);
        AddErrorMessage("Error 1", code: 600);
        AddErrorMessage("Error 2", code: 600);
        AddErrorMessage("Error 3", code: 600);
        AddErrorMessage("Error 4", code: 600);
        // ******************************
    }

    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public IIntroductionSearchViewModel introductionSearchViewModel;

    [RelayCommand]
    public async Task NavigatedToAsync(object obj)
    {
        try
        {
            _logger.LogInformation("{NavigatedToAsync}()", nameof(NavigatedToAsync));
            if (!IntroductionSearchViewModel.HasSearched)
            {
                return;
            }

            ResetForNextOperation();
            await IntroductionSearchViewModel.SearchAsync();
            //await App.Current.MainPage.DisplayAlert("MainPageViewModel.NavigatedToAsync", "MainPageViewModel.NavigatedToAsync", Constants.AlertButtonText.OK);
        }
        finally
        {
            SetIsBusy(false);
        }
    }

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

    Task NavigatedToAsync(object obj);

    Task EditIntroductionAsync(object introductionSearchResultDto);
}
