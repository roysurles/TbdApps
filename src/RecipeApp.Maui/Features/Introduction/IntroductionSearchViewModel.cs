namespace RecipeApp.Maui.Features.Introduction;

public partial class IntroductionSearchViewModel : BaseViewModel, IIntroductionSearchViewModel
{
    protected readonly IIntroductionApiClientV1_0 _introductionApiClientV1_0;
    protected readonly ILogger<IntroductionSearchViewModel> _logger;
    protected readonly IntroductionSearchRequestDto _introductionSearchRequestDto = new() { PageNumber = 1, PageSize = 10 };

    public IntroductionSearchViewModel(IIntroductionApiClientV1_0 introductionpiClientV1_0, ILogger<IntroductionSearchViewModel> logger)
    {
        _introductionApiClientV1_0 = introductionpiClientV1_0;
        _logger = logger;
    }

    [ObservableProperty]
    //[NotifyPropertyChangedFor("IsPaginationVisible")]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public bool hasSearched;

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
    public int pageCount;

    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public int totalItemCount;

    //[ObservableProperty]
    //[NotifyPropertyChangedFor("IsPaginationVisible")]
    //public new bool isBusy;

    public bool IsPaginationVisible => !IsBusy && HasSearched;

    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public ObservableCollection<IntroductionSearchResultDto> introductionSearchResults = new();

    [RelayCommand]
    public async Task PageNumberChangedAsync(object args)
    {
        PageNumber = (args as PaginationPageNumberChangedEventArgs).PageNumber;
        await SearchAsync();
    }

    [RelayCommand]
    public async Task<IIntroductionSearchViewModel> SearchAsync()
    {
        try
        {
            _logger.LogInformation("{methodName}({pageNumber}, {pageSize})", nameof(SearchAsync), PageNumber, PageSize);
            ResetForNextOperation().RaisePropertyChangedFor("IsPaginationVisible");
            HasSearched = true;
            IntroductionSearchResults.Clear();

            var cleanSearchText = string.Equals(SearchText?.ToString().Trim(), "-") ? string.Empty : SearchText?.ToString().Trim();

            _introductionSearchRequestDto.SetSearchText(cleanSearchText)
                                         .SetPagination(PageNumber, PageSize)
                .OrderByClause.Clear()
                              .AddOrderByAscending(p => p.Title)
                              .AddOrderByDescending(p => p.InstructionsCount);

            var introductionSearchResult = await RefitExStaticMethods.TryInvokeApiAsync(
                () => _introductionApiClientV1_0.SearchAsync(_introductionSearchRequestDto), ApiResultMessages);

            PageCount = introductionSearchResult.Meta.PageCount;
            TotalItemCount = introductionSearchResult.Meta.TotalItemCount;
            IntroductionSearchResults.AddRange(introductionSearchResult.Data);

            var foundResultsTest = $"Found {introductionSearchResult.Meta.TotalItemCount:#,##0} results";
            await App.Current.MainPage.DisplaySnackbar(foundResultsTest);
            var toast = Toast.Make(foundResultsTest);       // not very friendly.. get rid of .Net and display towards top?
            await toast.Show();
            //https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/alerts/toast?tabs=android
        }
        finally
        {
            SetIsBusy(false).RaisePropertyChangedFor("IsPaginationVisible");
        }

        return this;
    }
}

public interface IIntroductionSearchViewModel : IBaseViewModel
{
    bool HasSearched { get; set; }

    string SearchText { get; set; }

    int PageNumber { get; }

    int PageSize { get; }

    int PageCount { get; }

    int TotalItemCount { get; }

    bool IsPaginationVisible { get; }

    ObservableCollection<IntroductionSearchResultDto> IntroductionSearchResults { get; }

    Task PageNumberChangedAsync(object args);

    IAsyncRelayCommand<object> PageNumberChangedCommand { get; }

    Task<IIntroductionSearchViewModel> SearchAsync();

    IAsyncRelayCommand SearchCommand { get; }
}
