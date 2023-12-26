namespace RecipeApp.Maui.Features.Instruction;

public partial class InstructionViewModel : BaseViewModel, IInstructionViewModel
{
    protected readonly IInstructionApiClientV1_0 _instructionApiClientV1_0;
    protected readonly ILogger<InstructionViewModel> _logger;
    protected Guid _introductionId = Guid.Empty;

    public InstructionViewModel(IInstructionApiClientV1_0 instructionApiClientV1_0, ILogger<InstructionViewModel> logger)
    {
        _instructionApiClientV1_0 = instructionApiClientV1_0;
        _logger = logger;
    }

    public bool IsIntroductionNew =>
        Equals(Guid.Empty, _introductionId);

    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public ObservableCollection<InstructionDto> instructions = new();

    public async Task<IInstructionViewModel> InitializeAsync(Guid introductionId)
    {
        try
        {
            _logger.LogInformation("{InstructionViewModel}({introductionId})", nameof(InstructionViewModel), introductionId);

            _introductionId = introductionId;
            ResetForNextOperation();
            Instructions.Clear();

            if (Equals(Guid.Empty, _introductionId))
                return this;

            var response = await RefitExStaticMethods.TryInvokeApiAsync(
                () => _instructionApiClientV1_0.GetAllForIntroductionIdAsync(introductionId), ApiResultMessages);

            Instructions.AddRange(response.Data.OrderBy(item => item.SortOrder));
        }
        finally
        {
            IsBusy = false;
        }

        return this;
    }

    [RelayCommand]
    public async Task AddInstructionAsync()
    {
        _logger.LogInformation("{AddInstruction}()", nameof(AddInstructionAsync));

        ClearApiResultMessages();

        if (IsIntroductionNew)
        {
            await App.Current.MainPage.DisplayAlert("Add", "Save introduction, before adding instructions.", Constants.AlertButtonText.OK);
            return;
        }

        if (Instructions.Any(x => x.IsNew))
        {
            await App.Current.MainPage.DisplayAlert("Add", "Save unsaved instruction before adding another.", Constants.AlertButtonText.OK);
            return;
        }

        Instructions.Add(new InstructionDto { IntroductionId = _introductionId, SortOrder = Instructions.Count + 1 });

        // TODO cleanup: await App.Current.MainPage.DisplayAlert("Add", $"AddInstructionAsync", Constants.AlertButtonText.OK);
    }

    [RelayCommand]
    public async Task SaveInstructionAsync(object args)
    {
        await App.Current.MainPage.DisplayAlert("Save", $"SaveInstructionAsync {args}?", Constants.AlertButtonText.OK);
    }

    [RelayCommand]
    public async Task DeleteInstructionAsync(object args)
    {
        //await App.Current.MainPage.DisplayAlert("Delete", $"DeleteInstructionAsync {args}?", Constants.AlertButtonText.OK);
        try
        {
            _logger.LogInformation($"{nameof(DeleteInstructionAsync)}({nameof(args)})");
            ClearApiResultMessages();

            var instructionDto = args as InstructionDto;

            if (!(await App.Current.MainPage.DisplayAlert("Delete", "Are you sure you want to delete this instruction?", "Yes", "No")))
                return;

            if (instructionDto.IsNew)
            {
                Instructions.Remove(instructionDto);
                return;
            }

            // TODO EXCEPTION:  *** this throws exception if one of the inputs (description or measurement) has focus ***

            IsBusy = true;
            // await IngredientViewModel.DeleteIngredientAsync(ingredientDto);
            var index = Instructions.IndexOf(instructionDto);
            var response = await RefitExStaticMethods.TryInvokeApiAsync(() => _instructionApiClientV1_0.DeleteAsync(instructionDto.Id), ApiResultMessages);
            if (response.IsSuccessHttpStatusCode)
                Instructions.RemoveAt(index);
            // ***************************************************************

            if (ApiResultMessages.Any(m => m.MessageType == ApiResultMessageModelTypeEnumeration.Error))
                await App.Current.MainPage.DisplaySnackbar("Instruction deleted successfully!");
            //var errorMessages = ApiResultMessages.Where(m => m.MessageType == ApiResultMessageModelTypeEnumeration.Error);
            //if (errorMessages.Any())
            //{
            //    foreach (var errorMessage in errorMessages)
            //        await JSRuntime.ToastAsync(new ToastModel(ToastType.error, "Ingredient", errorMessage.Message));
            //    return;
            //}

            await App.Current.MainPage.DisplaySnackbar("Instruction deleted successfully!");
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
    public async Task MoveInstructionFirstAsync(object args)
    {
        await App.Current.MainPage.DisplayAlert("Move First", $"MoveInstructionFirstAsync {args}", Constants.AlertButtonText.OK);
    }

    [RelayCommand]
    public async Task MoveInstructionUpAsync(object args)
    {
        await App.Current.MainPage.DisplayAlert("Move Up", $"MoveInstructionUpAsync {args}", Constants.AlertButtonText.OK);
    }

    [RelayCommand]
    public async Task MoveInstructionDownAsync(object args)
    {
        await App.Current.MainPage.DisplayAlert("Move Down", $"MoveInstructionDownAsync {args}", Constants.AlertButtonText.OK);
    }

    [RelayCommand]
    public async Task MoveInstructionLastAsync(object args)
    {
        await App.Current.MainPage.DisplayAlert("Move Last", $"MoveInstructionLastAsync {args}", Constants.AlertButtonText.OK);
    }
}

public interface IInstructionViewModel : IBaseViewModel
{
    bool IsIntroductionNew { get; }

    ObservableCollection<InstructionDto> Instructions { get; }

    Task<IInstructionViewModel> InitializeAsync(Guid introductionId);

    Task AddInstructionAsync();

    IAsyncRelayCommand AddInstructionCommand { get; }

    Task SaveInstructionAsync(object args);

    IAsyncRelayCommand<object> SaveInstructionCommand { get; }

    Task DeleteInstructionAsync(object args);

    IAsyncRelayCommand<object> DeleteInstructionCommand { get; }

    Task MoveInstructionFirstAsync(object args);

    IAsyncRelayCommand<object> MoveInstructionFirstCommand { get; }

    Task MoveInstructionUpAsync(object args);

    IAsyncRelayCommand<object> MoveInstructionUpCommand { get; }

    Task MoveInstructionDownAsync(object args);

    IAsyncRelayCommand<object> MoveInstructionDownCommand { get; }

    Task MoveInstructionLastAsync(object args);

    IAsyncRelayCommand<object> MoveInstructionLastCommand { get; }
}
