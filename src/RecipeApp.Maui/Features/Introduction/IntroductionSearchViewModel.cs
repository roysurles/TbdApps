using CommunityToolkit.Maui.Alerts;

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
    public ObservableCollection<IntroductionSearchResultDto> introductionSearchResults = new();

    [RelayCommand]
    public async Task<IIntroductionSearchViewModel> SearchAsync()
    {
        try
        {
            _logger.LogInformation("{methodName}({pageNumber}, {pageSize})", nameof(SearchAsync), PageNumber, PageSize);
            HasSearched = true;
            ResetForNextOperation();
            WeakReferenceMessenger.Default.Send(new IsBusyValueChangedMessage(IsBusy));
            IntroductionSearchResults.Clear();

            var cleanSearchText = string.Equals(SearchText?.ToString().Trim(), "-") ? string.Empty : SearchText?.ToString().Trim();

            _introductionSearchRequestDto.SetSearchText(cleanSearchText)
                                         .SetPagination(PageNumber, PageSize)
                .OrderByClause.Clear()
                              .AddOrderByAscending(p => p.Title)
                              .AddOrderByDescending(p => p.InstructionsCount);

            var introductionSearchResult = await RefitExStaticMethods.TryInvokeApiAsync(
                () => _introductionApiClientV1_0.SearchAsync(_introductionSearchRequestDto), ApiResultMessages);

            IntroductionSearchResults.AddRange(introductionSearchResult.Data);

            var foundResultsTest = $"Found {introductionSearchResult.Meta.TotalItemCount:#,##0} results";
            await App.Current.MainPage.DisplaySnackbar(foundResultsTest);
            var toast = Toast.Make(foundResultsTest);       // not very friendly.. get rid of .Net and display towards top?
            await toast.Show();
            //https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/alerts/toast?tabs=android
        }
        finally
        {
            IsBusy = false;
            WeakReferenceMessenger.Default.Send(new IsBusyValueChangedMessage(IsBusy));
        }

        return this;
    }
}

public interface IIntroductionSearchViewModel : IBaseViewModel
{
    bool HasSearched { get; set; }

    string SearchText { get; set; }

    int PageNumber { get; set; }

    int PageSize { get; set; }

    ObservableCollection<IntroductionSearchResultDto> IntroductionSearchResults { get; }

    Task<IIntroductionSearchViewModel> SearchAsync();

    IAsyncRelayCommand SearchCommand { get; }
}
