using CommunityToolkit.Maui.Alerts;

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
 */
public partial class MainPageViewModel : ObservableObject, IMainPageViewModel
{
    protected readonly IIntroductionApiClientV1_0 _introductionApiClientV1_0;
    protected readonly ILogger<MainPageViewModel> _logger;

    public MainPageViewModel(IIntroductionApiClientV1_0 introductionApiClientV1_0, ILogger<MainPageViewModel> logger)
    {
        _introductionApiClientV1_0 = introductionApiClientV1_0;
        _logger = logger;

        //App.Current.MainPage.DisplayAlert("Hello", "", "Cancel");
        //App.Current.MainPage.DisplayPromptAsync
        //App.Current.MainPage.DisplaySnackbar

        //SearchAsyncCommand = new AsyncRelayCommand<object>((object searchText) => SearchAsync(searchText));

        // Enumeration class nuget package

        // SearchBar xml
    }

    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public bool isBusy;

    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public bool hasSearched;

    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public string filterText;

    // For now, intialize searchText = "-", because searchbar does not fire on empty string for Android
    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public string searchText = "-";

    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public int pageNumber = 1;

    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public int pageSize = 10;

    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public bool isInitialTextVisible = true;

    public IntroductionSearchRequestDto IntroductionSearchRequestDto { get; } =
        new IntroductionSearchRequestDto { PageNumber = 1, PageSize = 10 };

    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public ObservableCollection<IntroductionSearchResultDto> introductionSearchResults = new();

    //public IApiResultModel<List<IntroductionSearchResultDto>> IntroductionSearchResult { get; protected set; } =
    //        new ApiResultModel<List<IntroductionSearchResultDto>>()
    //            .SetMeta(1, 10, 0)
    //            .SetData(new List<IntroductionSearchResultDto>());

    //public IApiResultModel<List<IntroductionSearchResultDto>> FilteredIntroductionSearchResult
    //{
    //    get
    //    {
    //        if (string.IsNullOrWhiteSpace(FilterText))
    //            return IntroductionSearchResult;

    //        var filteredData = IntroductionSearchResult.Data.Where(item => item.Title.Contains(FilterText, StringComparison.InvariantCultureIgnoreCase)).ToList();
    //        return new ApiResultModel<List<IntroductionSearchResultDto>>()
    //            .SetData(filteredData)
    //            .SetMeta(IntroductionSearchResult.Meta);
    //    }
    //}

    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public ObservableCollection<IApiResultMessageModel> apiResultMessages = new();

    public IntroductionDto Introduction { get; protected set; } = new();

    [RelayCommand]
    public async Task<IMainPageViewModel> SearchAsync()
    {
        try
        {
            _logger.LogInformation("{methodName}({pageNumber}, {pageSize})", nameof(SearchAsync), PageNumber, PageSize);
            IntroductionSearchResults.Clear();
            IsInitialTextVisible = false;
            ResetForNextOperation();

            var cleanSearchText = string.Equals(SearchText?.ToString().Trim(), "-") ? string.Empty : SearchText?.ToString().Trim();

            IntroductionSearchRequestDto.SetSearchText(cleanSearchText)
                                        .SetPagination(PageNumber, PageSize)
                .OrderByClause.Clear()
                              .AddOrderByAscending(p => p.Title)
                              .AddOrderByDescending(p => p.InstructionsCount);

            var introductionSearchResult = await RefitExStaticMethods.TryInvokeApiAsync(
                () => _introductionApiClientV1_0.SearchAsync(IntroductionSearchRequestDto), ApiResultMessages);

            IntroductionSearchResults.AddRange(introductionSearchResult.Data);

            HasSearched = true;
            var foundResultsTest = $"Found {introductionSearchResult.Meta.TotalItemCount:#,##0} results";
            await App.Current.MainPage.DisplaySnackbar(foundResultsTest);
            var toast = Toast.Make(foundResultsTest);       // not very friendly.. get rid of .Net and display towards top?
            await toast.Show();
            //https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/alerts/toast?tabs=android
        }
        finally
        {
            IsBusy = false;
        }

        return this;
    }

    [RelayCommand]
    public async Task IntroductionSearchResultTappedAsync(object tappedIntroductionSearchResultDto)
    {
        var tappedIntroduction = tappedIntroductionSearchResultDto as IntroductionSearchResultDto;
        if (tappedIntroduction is null)
        {
            await App.Current.MainPage.DisplayAlert("Mismatch", "Cannot convert tappedIntroductionSearchResultDto to IntroductionSearchResultDto", "Ok");
            return;
        }

        await App.Current.MainPage.DisplayAlert("Tapped", $"{tappedIntroduction.Title} tapped...", "Ok");

        // https://www.youtube.com/watch?v=ddmZ6k1GIkM
        //await Shell.Current.Navigation.PushAsync
        await Shell.Current.GoToAsync($"{nameof(DetailsPage)}?IntroductionId={tappedIntroduction.Id}");
    }

    [RelayCommand]
    public async Task DeleteIntroductionAsync(object introductionSearchResultDto)
    {
        var introduction = introductionSearchResultDto as IntroductionSearchResultDto;
        if (introduction is null)
        {
            await App.Current.MainPage.DisplayAlert("Mismatch", "Cannot convert introductionSearchResultDto to IntroductionSearchResultDto", "Ok");
            return;
        }

        await App.Current.MainPage.DisplayAlert("Delete", $"Delete {introduction.Title}?", "Ok");
    }

    [RelayCommand]
    public async Task EditIntroductionAsync(object introductionSearchResultDto)
    {
        var introduction = introductionSearchResultDto as IntroductionSearchResultDto;
        if (introduction is null)
        {
            await App.Current.MainPage.DisplayAlert("Mismatch", "Cannot convert introductionSearchResultDto to IntroductionSearchResultDto", "Ok");
            return;
        }

        await App.Current.MainPage.DisplayAlert("Edit", $"Navigate to {introduction.Title}?", "Ok");

        // https://www.youtube.com/watch?v=ddmZ6k1GIkM
        //await Shell.Current.Navigation.PushAsync
        await Shell.Current.GoToAsync($"{nameof(DetailsPage)}?IntroductionId={introduction.Id}");
    }

    public async Task<IMainPageViewModel> InitializeAsync(Guid introductionId)
    {
        try
        {
            _logger.LogInformation("{methodName}({id})", nameof(InitializeAsync), introductionId);
            ResetForNextOperation();

            if (Equals(Guid.Empty, introductionId))
                return SetIntroductionToNewDto();

            var response = await RefitExStaticMethods.TryInvokeApiAsync(
                () => _introductionApiClientV1_0.GetAsync(introductionId), ApiResultMessages);

            Introduction = response.Data;
        }
        finally
        {
            IsBusy = false;
        }

        return this;
    }

    public async Task<IMainPageViewModel> SaveIntroductionAsync()
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
        }
        finally
        {
            IsBusy = false;
        }

        return this;
    }

    //public async Task<IMainPageViewModel> DeleteIntroductionAsync()
    //{
    //    try
    //    {
    //        _logger.LogInformation($"{nameof(DeleteIntroductionAsync)}()");
    //        ResetForNextOperation();

    //        if (Introduction?.IsNew == true)
    //        {
    //            AddInformationMessage("There is nothing to Delete!", $"{nameof(MainPageViewModel)}.{nameof(DeleteIntroductionAsync)}");
    //            return this;
    //        }

    //        var apiResult = await RefitExStaticMethods.TryInvokeApiAsync(() => _introductionApiClientV1_0.DeleteAsync(Introduction.Id), ApiResultMessages);
    //        if (apiResult.IsSuccessHttpStatusCode)
    //            AddInformationMessage("Introduction deleted successfully!", $"{nameof(MainPageViewModel)}.{nameof(DeleteIntroductionAsync)}", 200);
    //    }
    //    finally
    //    {
    //        IsBusy = false;
    //    }

    //    return SetIntroductionToNewDto();
    //}

    protected IMainPageViewModel ClearApiResultMessages()
    {
        ApiResultMessages.Clear();
        return this;
    }

    protected IMainPageViewModel AddApiResultMessage(ApiResultMessageModelTypeEnumeration apiResultMessageType, string message, string source = null, int? code = null)
    {
        ApiResultMessages.Add(new ApiResultMessageModel
        {
            MessageType = apiResultMessageType,
            Message = message,
            Source = source,
            Code = code
        });
        return this;
    }

    protected IMainPageViewModel AddInformationMessage(string message, string source = null, int? code = null) =>
        AddApiResultMessage(ApiResultMessageModelTypeEnumeration.Information, message, source, code);

    protected IMainPageViewModel AddWarningMessage(string message, string source = null, int? code = null) =>
        AddApiResultMessage(ApiResultMessageModelTypeEnumeration.Warning, message, source, code);

    protected IMainPageViewModel AddErrorMessage(string message, string source = null, int? code = null) =>
        AddApiResultMessage(ApiResultMessageModelTypeEnumeration.Error, message, source, code);

    protected IMainPageViewModel AddMessages(IEnumerable<ApiResultMessageModel> messages)
    {
        ApiResultMessages.AddRange(messages);
        return this;
    }

    protected IMainPageViewModel ResetForNextOperation()
    {
        IsBusy = true;
        ApiResultMessages.Clear();

        return this;
    }

    protected IMainPageViewModel SetIntroductionToNewDto()
    {
        Introduction = new IntroductionDto();
        return this;
    }
}

public interface IMainPageViewModel
{
    bool IsBusy { get; }

    bool HasSearched { get; }

    string FilterText { get; set; }

    string SearchText { get; set; }

    int PageNumber { get; set; }

    int PageSize { get; set; }

    IntroductionSearchRequestDto IntroductionSearchRequestDto { get; }

    ObservableCollection<IntroductionSearchResultDto> IntroductionSearchResults { get; set; }

    //IApiResultModel<List<IntroductionSearchResultDto>> IntroductionSearchResult { get; }

    //IApiResultModel<List<IntroductionSearchResultDto>> FilteredIntroductionSearchResult { get; }

    ObservableCollection<IApiResultMessageModel> ApiResultMessages { get; }

    IntroductionDto Introduction { get; }

    Task<IMainPageViewModel> SearchAsync();

    Task IntroductionSearchResultTappedAsync(object tappedIntroductionSearchResultDto);

    Task DeleteIntroductionAsync(object introductionSearchResultDto);

    Task EditIntroductionAsync(object introductionSearchResultDto);

    Task<IMainPageViewModel> InitializeAsync(Guid introductionId);

    Task<IMainPageViewModel> SaveIntroductionAsync();

    //Task<IMainPageViewModel> DeleteIntroductionAsync();
}
