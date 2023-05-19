using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Extensions.Logging;

namespace RecipeApp.Maui;

public class MainPageViewModel : ObservableObject, IMainPageViewModel
{
    protected readonly IHttpClientFactory _httpClientFactory;
    protected readonly ILogger<MainPageViewModel> _logger;

    public MainPageViewModel(IHttpClientFactory httpClientFactory, ILogger<MainPageViewModel> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;

        var httpClient = _httpClientFactory.CreateClient("CoreApi");
    }
}

public interface IMainPageViewModel
{

}
