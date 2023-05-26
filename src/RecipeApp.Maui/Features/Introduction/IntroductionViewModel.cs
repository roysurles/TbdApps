namespace RecipeApp.Maui.Features.Introduction;

public partial class IntroductionViewModel : BaseViewModel, IIntroductionViewModel
{
    protected readonly IIntroductionApiClientV1_0 _introductionApiClientV1_0;
    protected readonly ILogger<IntroductionViewModel> _logger;

    public IntroductionViewModel(IIntroductionApiClientV1_0 introductionApiClientV1_0
    , ILogger<IntroductionViewModel> logger)
    {
        _introductionApiClientV1_0 = introductionApiClientV1_0;
        _logger = logger;
    }

    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public IntroductionDto introduction = new();

    public async Task<IIntroductionViewModel> InitializeAsync(Guid introductionId)
    {
        _logger.LogInformation($"{nameof(IntroductionViewModel)}({{introductionId}})", introductionId);

        ApiResultMessages.Clear();

        if (Equals(Guid.Empty, introductionId))
            return SetIntroductionToNewDto();

        var response = await RefitExStaticMethods.TryInvokeApiAsync(
            () => _introductionApiClientV1_0.GetAsync(introductionId), ApiResultMessages);

        Introduction = response.Data;

        return this;
    }

    [RelayCommand]
    public async Task<IIntroductionViewModel> SaveIntroductionAsync()
    {
        try
        {
            _logger.LogInformation($"{nameof(SaveIntroductionAsync)}()");

            ResetForNextOperation();

            if (Introduction.TryValidateObject(ApiResultMessages).Equals(false))
                return this;

            var saveIntroductionTask = Introduction.IsNew
                ? RefitExStaticMethods.TryInvokeApiAsync(() => _introductionApiClientV1_0.InsertAsync(Introduction), ApiResultMessages)
                : RefitExStaticMethods.TryInvokeApiAsync(() => _introductionApiClientV1_0.UpdateAsync(Introduction), ApiResultMessages);

            await saveIntroductionTask;
            Introduction = saveIntroductionTask.Result.Data;

            var message = $"{Introduction.Title} saved";
            await App.Current.MainPage.DisplaySnackbar(message);
            var toast = Toast.Make(message);
            await toast.Show();
        }
        finally
        {
            SetIsBusy(false);
        }

        return this;
    }

    [RelayCommand]
    public async Task<IIntroductionViewModel> DeleteIntroductionAsync(Guid introductionId)
    {
        try
        {
            _logger.LogInformation($"{nameof(DeleteIntroductionAsync)}()");
            ResetForNextOperation();
            WeakReferenceMessenger.Default.Send(new IsBusyValueChangedMessage(IsBusy));

            if (Equals(introductionId, Guid.Empty))
            {
                await App.Current.MainPage.DisplayAlert("Delete", "There is nothing to Delete!", Constants.AlertButtonText.OK);
                //AddInformationMessage("There is nothing to Delete!", $"{nameof(IntroductionViewModel)}.{nameof(DeleteIntroductionAsync)}");
                return this;
            }

            await Task.Delay(3000);

            //var apiResult = await RefitExStaticMethods.TryInvokeApiAsync(() => _introductionApiClientV1_0.DeleteAsync(introductionId), ApiResultMessages);
            //if (apiResult.IsSuccessHttpStatusCode)
            //{
            //    var message = "Recipe deleted successfully!";
            //    await App.Current.MainPage.DisplaySnackbar(message);
            //    var toast = Toast.Make(message);
            //    await toast.Show();

            //    //AddInformationMessage("Introduction deleted successfully!", $"{nameof(IntroductionViewModel)}.{nameof(DeleteIntroductionAsync)}", 200);
            //}

            //return SetIntroductionToNewDto();


        }
        finally
        {
            IsBusy = false;
            WeakReferenceMessenger.Default.Send(new IsBusyValueChangedMessage(IsBusy));
        }

        return this;
    }

    protected IIntroductionViewModel SetIntroductionToNewDto()
    {
        Introduction = new IntroductionDto();
        return this;
    }
}

public interface IIntroductionViewModel : IBaseViewModel
{
    IntroductionDto Introduction { get; }

    Task<IIntroductionViewModel> InitializeAsync(Guid introductionId);

    Task<IIntroductionViewModel> SaveIntroductionAsync();

    IAsyncRelayCommand SaveIntroductionCommand { get; }

    Task<IIntroductionViewModel> DeleteIntroductionAsync(Guid introductionId);
}
