using RecipeApp.Maui.Features.Shared;

namespace RecipeApp.Maui;

public partial class MainPage : ContentPage
{
    protected readonly IMainPageViewModel _mainPageViewModel;
    protected readonly ILogger<MainPage> _logger;
    protected readonly IHttpClientFactory _httpClientFactory;
    protected readonly HttpClient _coreApiHttpClient;
    protected readonly IIntroductionApiClientV1_0 _introductionApiClientV1_0;

    //int count = 0;

    public MainPage(IMainPageViewModel mainPageViewModel, ILogger<MainPage> logger, IHttpClientFactory httpClientFactory, IIntroductionApiClientV1_0 introductionApiClientV1_0)
    {
        InitializeComponent();
        BindingContext = _mainPageViewModel = mainPageViewModel;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _coreApiHttpClient = _httpClientFactory.CreateClient(Constants.HttpClientNames.CoreApi);
        _introductionApiClientV1_0 = introductionApiClientV1_0;
    }

    //private async void OnCounterClicked(object sender, EventArgs e)
    //{
    //    //count++;

    //    //if (count == 1)
    //    //    CounterBtn.Text = $"Clicked {count} time";
    //    //else
    //    //    CounterBtn.Text = $"Clicked {count} times";

    //    //SemanticScreenReader.Announce(CounterBtn.Text);

    //    //var introductionId = new Guid("32531D1D-DB95-437C-864D-6432A3913620");

    //    // Injecting Refit client
    //    //try
    //    //{
    //    //    var apiResult = await _introductionApiClientV1_0.GetAsync(introductionId);
    //    //}
    //    //catch (Exception ex)
    //    //{
    //    //    var message = ex.Message;
    //    //    throw;
    //    //}

    //    // Injecting HttpClient
    //    //try
    //    //{
    //    //    var httpsResponse = await _coreApiHttpClient.GetAsync($"/api/v1.0/Introduction/{introductionId}");
    //    //    var httpsData = await httpsResponse.Content.ReadAsStringAsync();
    //    //}
    //    //catch (Exception ex)
    //    //{
    //    //    var message = ex.Message;
    //    //    throw;
    //    //}

    //    // Manually creating HttpClient
    //    //try
    //    //{
    //    //    var baseDomain = DeviceInfo.Platform == DevicePlatform.Android
    //    //        ? "10.0.2.2"
    //    //        : "localhost";

    //    //    HttpsClientHandlerService handler = new HttpsClientHandlerService();
    //    //    HttpClient httpsClient = new HttpClient(handler.GetPlatformMessageHandler());
    //    //    var httpsResponse = await httpsClient.GetAsync(new Uri($"https://{baseDomain}:44350/api/v1.0/Introduction/{introductionId}"));
    //    //    var httpsData = await httpsResponse.Content.ReadAsStringAsync();
    //    //}
    //    //catch (Exception ex)
    //    //{
    //    //    var message = ex.Message;
    //    //    throw;
    //    //}
    //}
}

