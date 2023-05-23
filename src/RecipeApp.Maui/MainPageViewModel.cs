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
 *      - Enumeration class nuget package
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

        WeakReferenceMessenger.Default.Register<IsBusyValueChangedMessage>(this, (r, m) =>
        {
            IsBusy = m.Value;
        });
    }

    public IIntroductionSearchViewModel IntroductionSearchViewModel { get; protected set; }

    public IIntroductionViewModel IntroductionViewModel { get; protected set; }

    // TODO:  investigate if we can bind to IntroductionSearchViewModel.SearchAsync()  -- SearchCommand
    //[RelayCommand]
    //public async Task<IMainPageViewModel> SearchAsync()
    //{
    //    try
    //    {
    //        IsBusy = true;

    //        await IntroductionSearchViewModel.SearchAsync();
    //    }
    //    finally
    //    {
    //        IsBusy = false;
    //    }

    //    return this;
    //}

    [RelayCommand]
    public async Task DeleteIntroductionAsync(object introductionSearchResultDto)
    {
        var introduction = introductionSearchResultDto as IntroductionSearchResultDto;
        if (introduction is null)
        {
            await App.Current.MainPage.DisplayAlert("Mismatch", "Cannot convert introductionSearchResultDto to IntroductionSearchResultDto", Constants.AlertButtonText.OK);
            return;
        }

        bool confirm = await App.Current.MainPage.DisplayAlert("Delete", $"Delete {introduction.Title}?", Constants.AlertButtonText.OK, Constants.AlertButtonText.Cancel);
        if (!confirm)
            return;

        //try
        //{
        //    IsBusy = true;

        await IntroductionViewModel.DeleteIntroductionAsync(introduction.Id);
        await IntroductionSearchViewModel.SearchAsync();
        //}
        //finally
        //{
        //    IsBusy = false;
        //}
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

    IIntroductionViewModel IntroductionViewModel { get; }

    //Task<IMainPageViewModel> SearchAsync();

    Task DeleteIntroductionAsync(object introductionSearchResultDto);

    Task EditIntroductionAsync(object introductionSearchResultDto);
}
