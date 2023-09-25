
namespace RecipeApp.Shared.Features.Ingredient;

public class IngredientViewModel : BaseViewModel, IIngredientViewModel
{
    protected readonly IIngredientApiClientV1_0 _ingredientApiClientV1_0;
    protected readonly ILogger<IngredientViewModel> _logger;
    protected Guid _introductionId = Guid.Empty;

    public IngredientViewModel(IIngredientApiClientV1_0 ingredientApiClientV1_0
        , ILogger<IngredientViewModel> logger)
    {
        _ingredientApiClientV1_0 = ingredientApiClientV1_0;
        _logger = logger;
    }

    public bool IsIntroductionNew =>
        Equals(Guid.Empty, _introductionId);

    public ObservableCollection<IngredientDto> Ingredients { get; protected set; } =
        new ObservableCollection<IngredientDto>();

    public async Task<IIngredientViewModel> InitializeAsync(Guid introductionId)
    {
        _logger.LogInformation("{IngredientViewModel}({introductionId})", nameof(IngredientViewModel), introductionId);

        ApiResultMessages.Clear();
        Ingredients.Clear();
        _introductionId = introductionId;

        if (Equals(Guid.Empty, _introductionId))
            return this;

        var response = await RefitExStaticMethods.TryInvokeApiAsync(
            () => _ingredientApiClientV1_0.GetAllForIntroductionIdAsync(introductionId), ApiResultMessages);

        Ingredients.AddRange(response.Data.OrderBy(item => item.SortOrder));

        return this;
    }

    public IIngredientViewModel AddIngredient()
    {
        _logger.LogInformation("{AddIngredient}()", nameof(AddIngredient));

        ClearApiResultMessages();

        Ingredients.Add(new IngredientDto { IntroductionId = _introductionId, SortOrder = Ingredients.Count + 1 });

        return this;
    }

    public async Task<IIngredientViewModel> SaveIngredientAsync(IngredientDto ingredientDto)
    {
        _logger.LogInformation("{SaveIngredientAsync}({ingredientDto})", nameof(SaveIngredientAsync), nameof(ingredientDto));

        ClearApiResultMessages();

        if (ingredientDto.TryValidateObject(ApiResultMessages).Equals(false))
            return this;

        var index = Ingredients.IndexOf(ingredientDto);

        var saveIngredientTask = ingredientDto.IsNew
            ? RefitExStaticMethods.TryInvokeApiAsync(() => _ingredientApiClientV1_0.InsertAsync(ingredientDto), ApiResultMessages)
            : RefitExStaticMethods.TryInvokeApiAsync(() => _ingredientApiClientV1_0.UpdateAsync(ingredientDto), ApiResultMessages);

        await saveIngredientTask;

        if (saveIngredientTask.Result.IsSuccessHttpStatusCode)
            Ingredients[index] = saveIngredientTask.Result.Data;

        return this;
    }

    public async Task<IIngredientViewModel> DeleteIngredientAsync(IngredientDto ingredientDto)
    {
        _logger.LogInformation("{DeleteIngredientAsync}({ingredientDto})", nameof(DeleteIngredientAsync), nameof(ingredientDto));

        ClearApiResultMessages();

        var index = Ingredients.IndexOf(ingredientDto);

        var response = await RefitExStaticMethods.TryInvokeApiAsync(() => _ingredientApiClientV1_0.DeleteAsync(ingredientDto.Id), ApiResultMessages);

        if (response.IsSuccessHttpStatusCode)
            Ingredients.RemoveAt(index);

        return this;
    }

    public async Task<IIngredientViewModel> MoveIngredientFirstAsync(IngredientDto ingredientDto)
    {
        _logger.LogInformation("{MoveIngredientFirstAsync}({ingredientDto})", nameof(MoveIngredientFirstAsync), nameof(ingredientDto));

        ClearApiResultMessages();

        if (ingredientDto.IsNew)
            return AddInformationMessage("Please save the ingredient before moving it.") as IIngredientViewModel;

        if (ingredientDto.SortOrder.Equals(1))
            return AddInformationMessage("This ingredient is already first.") as IIngredientViewModel;

        if (ingredientDto.TryValidateObject(ApiResultMessages).Equals(false))
            return this;

        var currentIndex = Ingredients.IndexOf(ingredientDto);
        Ingredients.Move(currentIndex, 0);

        return await ResequenceIngredientsSortOrderAsync();
    }

    public async Task<IIngredientViewModel> MoveIngredientPreviousAsync(IngredientDto ingredientDto)
    {
        _logger.LogInformation("{MoveIngredientPreviousAsync}({ingredientDto})", nameof(MoveIngredientPreviousAsync), nameof(ingredientDto));

        ClearApiResultMessages();

        if (ingredientDto.IsNew)
            return AddInformationMessage("Please save the ingredient before moving it.") as IIngredientViewModel;

        if (ingredientDto.SortOrder.Equals(1))
            return AddInformationMessage("This ingredient is already first.") as IIngredientViewModel;

        if (ingredientDto.TryValidateObject(ApiResultMessages).Equals(false))
            return this;

        var currentIndex = Ingredients.IndexOf(ingredientDto);
        Ingredients.Move(currentIndex, currentIndex - 1);

        return await ResequenceIngredientsSortOrderAsync();
    }

    public async Task<IIngredientViewModel> MoveIngredientNextAsync(IngredientDto ingredientDto)
    {
        _logger.LogInformation("{MoveIngredientNextAsync}({ingredientDto})", nameof(MoveIngredientNextAsync), nameof(ingredientDto));

        ClearApiResultMessages();

        if (ingredientDto.IsNew)
            return AddInformationMessage("Please save the ingredient before moving it.") as IIngredientViewModel;

        if (ingredientDto.SortOrder.Equals(Ingredients.Count))
            return AddInformationMessage("This ingredient is already last.") as IIngredientViewModel;

        if (ingredientDto.TryValidateObject(ApiResultMessages).Equals(false))
            return this;

        var currentIndex = Ingredients.IndexOf(ingredientDto);
        Ingredients.Move(currentIndex, currentIndex + 1);

        return await ResequenceIngredientsSortOrderAsync();
    }

    public async Task<IIngredientViewModel> MoveIngredientLastAsync(IngredientDto ingredientDto)
    {
        _logger.LogInformation("{MoveIngredientLastAsync}({ingredientDto})", nameof(MoveIngredientLastAsync), nameof(ingredientDto));

        ClearApiResultMessages();

        if (ingredientDto.IsNew)
            return AddInformationMessage("Please save the ingredient before moving it.") as IIngredientViewModel;

        if (ingredientDto.SortOrder.Equals(Ingredients.Count))
            return AddInformationMessage("This ingredient is already last.") as IIngredientViewModel;

        if (ingredientDto.TryValidateObject(ApiResultMessages).Equals(false))
            return this;

        var currentIndex = Ingredients.IndexOf(ingredientDto);
        Ingredients.Move(currentIndex, Ingredients.Count - 1);

        return await ResequenceIngredientsSortOrderAsync();
    }

    protected async Task<IIngredientViewModel> ResequenceIngredientsSortOrderAsync()
    {
        var index = 0;
        foreach (var item in Ingredients)
            item.SortOrder = ++index;

        var ingredientsDto = new IngredientsDto { Ingredients = Ingredients.ToList() };
        await RefitExStaticMethods.TryInvokeApiAsync(() => _ingredientApiClientV1_0.UpdateMultipleAsync(ingredientsDto), ApiResultMessages);

        return this;
    }
}

public interface IIngredientViewModel : IBaseViewModel
{
    bool IsIntroductionNew { get; }

    ObservableCollection<IngredientDto> Ingredients { get; }

    Task<IIngredientViewModel> InitializeAsync(Guid introductionId);

    IIngredientViewModel AddIngredient();

    Task<IIngredientViewModel> SaveIngredientAsync(IngredientDto ingredientDto);

    Task<IIngredientViewModel> DeleteIngredientAsync(IngredientDto ingredientDto);

    Task<IIngredientViewModel> MoveIngredientFirstAsync(IngredientDto ingredientDto);

    Task<IIngredientViewModel> MoveIngredientPreviousAsync(IngredientDto ingredientDto);

    Task<IIngredientViewModel> MoveIngredientNextAsync(IngredientDto ingredientDto);

    Task<IIngredientViewModel> MoveIngredientLastAsync(IngredientDto ingredientDto);
}
