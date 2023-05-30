namespace RecipeApp.Maui.Features.Details;

/*  TODO
 *      - Expander - different colors based on IsExpanded
 */

[QueryProperty("IntroductionId", "IntroductionId")]
public partial class DetailsPageViewModel : BaseViewModel, IDetailsPageViewModel
{
    protected readonly ILogger<DetailsPageViewModel> _logger;

    public DetailsPageViewModel(IIntroductionViewModel introductionViewModel
        , IIngredientViewModel ingredientViewModel
        , IInstructionViewModel instructionViewModel
        , ILogger<DetailsPageViewModel> logger)
    {
        IntroductionViewModel = introductionViewModel;
        IngredientViewModel = ingredientViewModel;
        InstructionViewModel = instructionViewModel;
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
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public IIngredientViewModel ingredientViewModel;

    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public IInstructionViewModel instructionViewModel;

    [RelayCommand]
    public async Task InitializeAsync()
    {
        try
        {
            _logger.LogInformation("{InitializeAsync}({IntroductionId})", nameof(InitializeAsync), IntroductionId);
            ResetForNextOperation();

            var introductionIdAsGuid = Guid.Parse(IntroductionId);
            var initializeIntroductionViewModelTask = IntroductionViewModel.InitializeAsync(introductionIdAsGuid);
            var initializeIngredientViewModelTask = IngredientViewModel.InitializeAsync(introductionIdAsGuid);
            var initializeInstructionViewModelTask = InstructionViewModel.InitializeAsync(introductionIdAsGuid);

            await Task.WhenAll(initializeIntroductionViewModelTask, initializeIngredientViewModelTask, initializeInstructionViewModelTask);

            //await App.Current.MainPage.DisplayAlert("InitializeAsync", $"InitializeAsync: {IntroductionViewModel.Introduction.Title}", Constants.AlertButtonText.OK);
        }
        finally
        {
            IsBusy = false;
            WeakReferenceMessenger.Default.Send(new IsBusyValueChangedMessage(IsBusy));
        }
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

    IIntroductionViewModel IntroductionViewModel { get; }

    IIngredientViewModel IngredientViewModel { get; }

    IInstructionViewModel InstructionViewModel { get; }

    Task InitializeAsync();

    IAsyncRelayCommand InitializeCommand { get; }

    Task DeleteIntroductionAsync();
}

