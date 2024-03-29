﻿namespace RecipeApp.Maui.Features.Ingredient;

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
    public async Task<IIngredientViewModel> AddIngredientAsync()
    {
        try
        {
            _logger.LogInformation("{methodName}", nameof(AddIngredientAsync));

            ResetForNextOperation(false);

            if (IsIntroductionNew)
            {
                await this.DisplayOkAlertAsync("Add", "Save introduction, before adding ingredients.");
                return this;
            }

            if (Ingredients.Any(x => x.IsNew))
            {
                await this.DisplayOkAlertAsync("Add", "Save unsaved ingredient before adding another.");
                return this;
            }

            Ingredients.Add(new IngredientDto { IntroductionId = _introductionId, SortOrder = Ingredients.Count + 1 });
        }
        catch (Exception ex)
        {
            AddUnhandledException(ex, _logger);
        }

        return this;
    }

    [RelayCommand]
    public async Task<IIngredientViewModel> SaveIngredientAsync(object args)
    {
        await App.Current.MainPage.DisplayAlert("Save", $"SaveIngredientAsync {args}?", Constants.AlertButtonText.OK);
        return this;
    }

    [RelayCommand]
    public async Task<IIngredientViewModel> DeleteIngredientAsync(object args)
    {
        try
        {
            _logger.LogInformation($"{nameof(DeleteIngredientAsync)}({nameof(args)})");

            var ingredientDto = args as IngredientDto;

            if (!(await App.Current.MainPage.DisplayAlert("Delete", "Are you sure you want to delete this ingredient?", "Yes", "No")))
                return this;

            if (ingredientDto.IsNew)
            {
                Ingredients.Remove(ingredientDto);
                return this;
            }

            // TODO EXCEPTION:  *** this throws exception if one of the inputs (description or measurement) has focus ***
            ResetForNextOperation(true);

            var index = Ingredients.IndexOf(ingredientDto);
            var response = await RefitExStaticMethods.TryInvokeApiAsync(() => _ingredientApiClientV1_0.DeleteAsync(ingredientDto.Id), ApiResultMessages);
            if (response.IsSuccessHttpStatusCode)
                Ingredients.RemoveAt(index);

            using var _ = await this.ShowSnackbarAndToastAsync("Ingredient deleted successfully!");
        }
        catch (Exception ex)
        {
            // TODO implement: SessionViewModel.HandleException(ex, IngredientViewModel.ApiResultMessages, ComponentName);
            AddUnhandledException(ex, _logger);
            using var _ = await this.ShowSnackbarAndToastAsync("Unhandled exception occurred");
        }
        finally
        {
            SetIsBusy(false);
        }

        return this;
    }

    [RelayCommand]
    public async Task<IIngredientViewModel> MoveIngredientFirstAsync(object args)
    {
        await App.Current.MainPage.DisplayAlert("Move First", $"MoveIngredientFirstAsync {args}", Constants.AlertButtonText.OK);
        return this;
    }

    [RelayCommand]
    public async Task<IIngredientViewModel> MoveIngredientUpAsync(object args)
    {
        await App.Current.MainPage.DisplayAlert("Move Up", $"MoveIngredientUpAsync {args}", Constants.AlertButtonText.OK);
        return this;
    }

    [RelayCommand]
    public async Task<IIngredientViewModel> MoveIngredientDownAsync(object args)
    {
        await App.Current.MainPage.DisplayAlert("Move Down", $"MoveIngredientDownAsync {args}", Constants.AlertButtonText.OK);
        return this;
    }

    [RelayCommand]
    public async Task<IIngredientViewModel> MoveIngredientLastAsync(object args)
    {
        await App.Current.MainPage.DisplayAlert("Move Last", $"MoveIngredientLastAsync {args}", Constants.AlertButtonText.OK);
        return this;
    }

    protected async Task<IIngredientViewModel> ResequenceIngredientsSortOrderAsync()
    {
        var index = 0;
        foreach (var item in Ingredients)
            item.SortOrder = ++index;

        var ingredientsDto = new IngredientsDto { Ingredients = Ingredients };
        await RefitExStaticMethods.TryInvokeApiAsync(() => _ingredientApiClientV1_0.UpdateMultipleAsync(ingredientsDto), ApiResultMessages);

        return this;
    }
}

public interface IIngredientViewModel : IBaseViewModel
{
    bool IsIntroductionNew { get; }

    ObservableCollection<IngredientDto> Ingredients { get; }

    Task<IIngredientViewModel> InitializeAsync(Guid introductionId);

    Task<IIngredientViewModel> AddIngredientAsync();

    IAsyncRelayCommand AddIngredientCommand { get; }

    Task<IIngredientViewModel> SaveIngredientAsync(object args);

    IAsyncRelayCommand<object> SaveIngredientCommand { get; }

    Task<IIngredientViewModel> DeleteIngredientAsync(object args);

    IAsyncRelayCommand<object> DeleteIngredientCommand { get; }

    Task<IIngredientViewModel> MoveIngredientFirstAsync(object args);

    IAsyncRelayCommand<object> MoveIngredientFirstCommand { get; }

    Task<IIngredientViewModel> MoveIngredientUpAsync(object args);

    IAsyncRelayCommand<object> MoveIngredientUpCommand { get; }

    Task<IIngredientViewModel> MoveIngredientDownAsync(object args);

    IAsyncRelayCommand<object> MoveIngredientDownCommand { get; }

    Task<IIngredientViewModel> MoveIngredientLastAsync(object args);

    IAsyncRelayCommand<object> MoveIngredientLastCommand { get; }
}
