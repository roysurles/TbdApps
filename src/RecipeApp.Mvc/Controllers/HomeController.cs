using RecipeApp.Mvc.Extensions;

namespace RecipeApp.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IHttpContextAccessor httpContextAccessor, ILogger<HomeController> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;

        // https://andrewlock.net/session-state-gdpr-and-non-essential-cookies/
        // https://stackoverflow.com/questions/281881/sessionid-keeps-changing-in-asp-net-mvc-why
        _httpContextAccessor.HttpContext!.Session.SetString("_forceSession", string.Empty);  // this has to be done for session to persist
    }

    public IActionResult Index()
    {
        _logger.LogInformation(nameof(Index));

        /*  Notes:
         *  Cannot use IoC for Blazor ViewModels in Mvc, because its not 1:1
         *  Cannot use Blazor ViewModels in Mvc Session, because they do not have parameterless constructors
         *  _httpContextAccessor.HttpContext!.Connection.Id & _httpContextAccessor.HttpContext!.Session.Id; are not consistent & very flaky
         */

        // This indicates a new session, either from new browser or previous session timed out
        if (string.IsNullOrWhiteSpace(_httpContextAccessor.HttpContext!.Session.GetString("sessionId")))
        {
            _logger.LogInformation("{MethodName} - Starting new session", nameof(Index));

            _httpContextAccessor.HttpContext!.Session.SetString("SessionId", Guid.NewGuid().ToString());
            _httpContextAccessor.HttpContext!.Session.Set("RecipeSearchViewModel", new RecipeSearchViewModel());
        }

        var recipeSearchViewModel = _httpContextAccessor.HttpContext!.Session.Get<RecipeSearchViewModel>("RecipeSearchViewModel");
        PopulateTempData(recipeSearchViewModel.SearchText);

        return View(recipeSearchViewModel);
    }

    public IActionResult Privacy()
    {
        _logger.LogInformation(nameof(Privacy));
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> SearchFromLayoutAsync()
    {
        _logger.LogInformation(nameof(SearchFromLayoutAsync));

        var recipeSearchViewModel = _httpContextAccessor.HttpContext!.Session.Get<RecipeSearchViewModel>("RecipeSearchViewModel");
        if (recipeSearchViewModel is null)
        {
            _logger.LogInformation("{MethodName} - Session has expired", nameof(SearchFromLayoutAsync));
            return RedirectToAction(nameof(Index));
        }

        // This method could be invoked from Search button in header or from grid pagination in index.cshtml,
        // both of which are in different forms
        recipeSearchViewModel.SearchText = HttpContext.Request.Form["SearchTextInput"].ToString();
        if (string.IsNullOrWhiteSpace(recipeSearchViewModel.SearchText))
            recipeSearchViewModel.SearchText = HttpContext.Request.Form["SearchTextInputHidden"].ToString();

        PopulateTempData(recipeSearchViewModel.SearchText);

        recipeSearchViewModel.IntroductionSearchResult = await SearchIntAsync(recipeSearchViewModel);
        _httpContextAccessor.HttpContext!.Session.Set("RecipeSearchViewModel", recipeSearchViewModel);

        return View("Index", recipeSearchViewModel);
    }

    private async Task<ApiResultModel<List<IntroductionSearchResultDto>>> SearchIntAsync(RecipeSearchViewModel recipeSearchViewModel)
    {
        recipeSearchViewModel.HasSearched = true;
        var introductionSearchViewModel = _httpContextAccessor.HttpContext!.RequestServices.GetService<IIntroductionSearchViewModel>();
        introductionSearchViewModel!.IntroductionSearchRequestDto.SearchText = recipeSearchViewModel.SearchText;
        await introductionSearchViewModel.SearchAsync(); // TODO:  pageNumber, pageSize

        return (ApiResultModel<List<IntroductionSearchResultDto>>)introductionSearchViewModel.IntroductionSearchResult;
    }

    private void PopulateTempData(string searchText)
    {
        // Using TempData, because we are keeping inputs across 2 forms in sync
        TempData["SessionId"] = _httpContextAccessor.HttpContext!.Session.GetString("SessionId");
        TempData["SearchText"] = searchText;
    }
}
