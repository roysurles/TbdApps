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

    public bool IsIntroductionNew =>
        Equals(Guid.Empty, _introductionId);

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

    //[ObservableProperty]

    [RelayCommand]
    public async Task AddIngredientAsync()
    {
        _logger.LogInformation("{AddIngredient}()", nameof(AddIngredientAsync));

        ClearApiResultMessages();

        if (IsIntroductionNew)
        {
            await App.Current.MainPage.DisplayAlert("Add", "Save introduction, before adding ingredients.", Constants.AlertButtonText.OK);
            return;
        }

        if (Ingredients.Any(x => x.IsNew))
        {
            await App.Current.MainPage.DisplayAlert("Add", "Save unsaved ingredient before adding another.", Constants.AlertButtonText.OK);
            return;
        }

        Ingredients.Add(new IngredientDto { IntroductionId = _introductionId, SortOrder = Ingredients.Count + 1 });

        // TODO cleanup: await App.Current.MainPage.DisplayAlert("Add", $"AddIngredientAsync", Constants.AlertButtonText.OK);
    }

    [RelayCommand]
    public async Task SaveIngredientAsync(object args)
    {
        await App.Current.MainPage.DisplayAlert("Save", $"SaveIngredientAsync {args}?", Constants.AlertButtonText.OK);
    }

    [RelayCommand]
    public async Task DeleteIngredientAsync(object args)
    {
        // await App.Current.MainPage.DisplayAlert("Delete", $"DeleteIngredientAsync {args}?", Constants.AlertButtonText.OK);
        try
        {
            _logger.LogInformation($"{nameof(DeleteIngredientAsync)}({nameof(args)})");
            ClearApiResultMessages();

            var ingredientDto = args as IngredientDto;

            if (!(await App.Current.MainPage.DisplayAlert("Delete", "Are you sure you want to delete this ingredient?", "Yes", "No")))
                return;

            if (ingredientDto.IsNew)
            {
                Ingredients.Remove(ingredientDto);
                return;
            }

            // TODO EXCEPTION:  *** this throws exception if one of the inputs (description or measurement) has focus ***

            IsBusy = true;
            // await IngredientViewModel.DeleteIngredientAsync(ingredientDto);
            var index = Ingredients.IndexOf(ingredientDto);
            var response = await RefitExStaticMethods.TryInvokeApiAsync(() => _ingredientApiClientV1_0.DeleteAsync(ingredientDto.Id), ApiResultMessages);
            if (response.IsSuccessHttpStatusCode)
                Ingredients.RemoveAt(index);
            // ***************************************************************

            if (ApiResultMessages.Any(m => m.MessageType == ApiResultMessageModelTypeEnumeration.Error))
                await App.Current.MainPage.DisplaySnackbar("Ingredient deleted successfully!");
            //var errorMessages = ApiResultMessages.Where(m => m.MessageType == ApiResultMessageModelTypeEnumeration.Error);
            //if (errorMessages.Any())
            //{
            //    foreach (var errorMessage in errorMessages)
            //        await JSRuntime.ToastAsync(new ToastModel(ToastType.error, "Ingredient", errorMessage.Message));
            //    return;
            //}

            await App.Current.MainPage.DisplaySnackbar("Ingredient deleted successfully!");
            // TODO:  toast replacement - await JSRuntime.ToastAsync(new ToastModel(ToastType.info, "Ingredient", "Deleted successfully!"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred: ");
            await App.Current.MainPage.DisplaySnackbar("Unhandled exception occurred!");
            // TODO implement: SessionViewModel.HandleException(ex, IngredientViewModel.ApiResultMessages, ComponentName);
        }
        finally
        {
            IsBusy = false;
        }
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
    bool IsIntroductionNew { get; }

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
