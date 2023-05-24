namespace RecipeApp.Maui.Features.Details;

[QueryProperty("IntroductionId", "IntroductionId")]
public partial class DetailsPageViewModel : BaseViewModel, IDetailsPageViewModel
{
    protected readonly ILogger<DetailsPageViewModel> _logger;

    public DetailsPageViewModel(IIntroductionViewModel introductionViewModel, ILogger<DetailsPageViewModel> logger)
    {
        IntroductionViewModel = introductionViewModel;
        _logger = logger;

        WeakReferenceMessenger.Default.Register<IsBusyValueChangedMessage>(this, (r, m) =>
        {
            IsBusy = m.Value;
        });
    }

    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public string introductionId;

    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public IIntroductionViewModel introductionViewModel;

    [ObservableProperty]
    public string title;

    [RelayCommand]
    public async Task InitializeAsync()
    {
        _logger.LogInformation($"{nameof(InitializeAsync)}({IntroductionId})");
        //return App.Current.MainPage.DisplayAlert("InitializeAsync", $"InitializeAsync", Constants.AlertButtonText.OK);

        await IntroductionViewModel.InitializeAsync(Guid.Parse(IntroductionId));
        //var initializeIntroductionViewModelTask = IntroductionViewModel.InitializeAsync(IntroductionId);
        //var initializeIngredientViewModelTask = IngredientViewModel.InitializeAsync(_introductionId);
        //var initializeInstructionViewModelTask = InstructionViewModel.InitializeAsync(_introductionId);

        //await Task.WhenAll(initializeIntroductionViewModelTask, initializeIngredientViewModelTask, initializeInstructionViewModelTask);

        await App.Current.MainPage.DisplayAlert("InitializeAsync", $"InitializeAsync: {IntroductionViewModel.Introduction.Title}", Constants.AlertButtonText.OK);
        Title = IntroductionViewModel.Introduction.Title;

    }

    [RelayCommand]
    public Task SaveIntroductionAsync()
    {
        return App.Current.MainPage.DisplayAlert("Save", $"Save {IntroductionId}?", Constants.AlertButtonText.OK);
    }

    [RelayCommand]
    public Task DeleteIntroductionAsync()
    {
        return App.Current.MainPage.DisplayAlert("Delete", $"Delete {IntroductionId}?", Constants.AlertButtonText.OK);
    }
}

public interface IDetailsPageViewModel : IBaseViewModel
{
    string IntroductionId { get; }

    string Title { get; set; }

    IIntroductionViewModel IntroductionViewModel { get; }

    Task InitializeAsync();

    Task SaveIntroductionAsync();

    Task DeleteIntroductionAsync();
}

