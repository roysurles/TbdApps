namespace RecipeApp.Maui.Features.Ingredient;

public partial class IngredientViewModel : BaseViewModel, IIngredientViewModel
{
    protected readonly IIngredientApiClientV1_0 _ingredientApiClientV1_0;
    protected readonly ILogger<IngredientViewModel> _logger;
    protected Guid _introductionId = Guid.Empty;

    public IngredientViewModel(IIngredientApiClientV1_0 ingredientApiClientV1_0, ILogger<IngredientViewModel> logger)
    {
        _ingredientApiClientV1_0 = ingredientApiClientV1_0;
        _logger = logger;
    }

    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public ObservableCollection<IngredientDto> ingredients = new();

    public async Task<IIngredientViewModel> InitializeAsync(Guid introductionId)
    {
        try
        {
            _logger.LogInformation("{IngredientViewModel}({introductionId})", nameof(IngredientViewModel), introductionId);

            _introductionId = introductionId;
            ResetForNextOperation();
            Ingredients.Clear();

            if (Equals(Guid.Empty, _introductionId))
                return this;

            var response = await RefitExStaticMethods.TryInvokeApiAsync(
                () => _ingredientApiClientV1_0.GetAllForIntroductionIdAsync(introductionId), ApiResultMessages);

            Ingredients.AddRange(response.Data.OrderBy(item => item.SortOrder));
        }
        finally
        {
            IsBusy = false;
        }

        return this;
    }

    //public IIngredientViewModel AddIngredient()
    //{
    //    _logger.LogInformation("{AddIngredient}()", nameof(AddIngredient));

    //    ClearApiResultMessages();

    //    Ingredients.Add(new IngredientDto { IntroductionId = _introductionId, SortOrder = Ingredients.Count + 1 });

    //    return this;
    //}

    //public async Task<IIngredientViewModel> SaveIngredientAsync(IngredientDto ingredientDto)
    //{
    //    _logger.LogInformation("{SaveIngredientAsync}({ingredientDto})", nameof(SaveIngredientAsync), nameof(ingredientDto));

    //    ClearApiResultMessages();

    //    if (ingredientDto.TryValidateObject(ApiResultMessages).Equals(false))
    //        return this;

    //    var index = Ingredients.IndexOf(ingredientDto);

    //    var saveIngredientTask = ingredientDto.IsNew
    //        ? RefitExStaticMethods.TryInvokeApiAsync(() => _ingredientApiClientV1_0.InsertAsync(ingredientDto), ApiResultMessages)
    //        : RefitExStaticMethods.TryInvokeApiAsync(() => _ingredientApiClientV1_0.UpdateAsync(ingredientDto), ApiResultMessages);

    //    await saveIngredientTask;

    //    if (saveIngredientTask.Result.IsSuccessHttpStatusCode)
    //        Ingredients[index] = saveIngredientTask.Result.Data;

    //    return this;
    //}

    [RelayCommand]
    public async Task AddIngredientAsync()
    {
        await App.Current.MainPage.DisplayAlert("Add", $"AddIngredientAsync", Constants.AlertButtonText.OK);
    }

    [RelayCommand]
    public async Task SaveIngredientAsync(object args)
    {
        await App.Current.MainPage.DisplayAlert("Save", $"SaveIngredientAsync {args}?", Constants.AlertButtonText.OK);
    }

    [RelayCommand]
    public async Task DeleteIngredientAsync(object args)
    {
        await App.Current.MainPage.DisplayAlert("Delete", $"DeleteIngredientAsync {args}?", Constants.AlertButtonText.OK);
    }

    [RelayCommand]
    public async Task MoveIngredientFirstAsync(object args)
    {
        await App.Current.MainPage.DisplayAlert("Move First", $"MoveIngredientFirstAsync {args}", Constants.AlertButtonText.OK);
    }

    [RelayCommand]
    public async Task MoveIngredientUpAsync(object args)
    {
        await App.Current.MainPage.DisplayAlert("Move Up", $"MoveIngredientUpAsync {args}", Constants.AlertButtonText.OK);
    }

    [RelayCommand]
    public async Task MoveIngredientDownAsync(object args)
    {
        await App.Current.MainPage.DisplayAlert("Move Down", $"MoveIngredientDownAsync {args}", Constants.AlertButtonText.OK);
    }

    [RelayCommand]
    public async Task MoveIngredientLastAsync(object args)
    {
        await App.Current.MainPage.DisplayAlert("Move Last", $"MoveIngredientLastAsync {args}", Constants.AlertButtonText.OK);
    }
}

public interface IIngredientViewModel : IBaseViewModel
{
    ObservableCollection<IngredientDto> Ingredients { get; }

    Task<IIngredientViewModel> InitializeAsync(Guid introductionId);

    Task AddIngredientAsync();

    IAsyncRelayCommand AddIngredientCommand { get; }

    Task SaveIngredientAsync(object args);

    IAsyncRelayCommand<object> SaveIngredientCommand { get; }

    Task DeleteIngredientAsync(object args);

    IAsyncRelayCommand<object> DeleteIngredientCommand { get; }

    Task MoveIngredientFirstAsync(object args);

    IAsyncRelayCommand<object> MoveIngredientFirstCommand { get; }

    Task MoveIngredientUpAsync(object args);

    IAsyncRelayCommand<object> MoveIngredientUpCommand { get; }

    Task MoveIngredientDownAsync(object args);

    IAsyncRelayCommand<object> MoveIngredientDownCommand { get; }

    Task MoveIngredientLastAsync(object args);

    IAsyncRelayCommand<object> MoveIngredientLastCommand { get; }
}
