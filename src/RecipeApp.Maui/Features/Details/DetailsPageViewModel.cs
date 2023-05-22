namespace RecipeApp.Maui.Features.Details;

[QueryProperty("IntroductionId", "IntroductionId")]
public partial class DetailsPageViewModel : ObservableObject, IDetailsPageViewModel
{
    protected readonly IIntroductionApiClientV1_0 _introductionApiClientV1_0;
    protected readonly ILogger<DetailsPageViewModel> _logger;

    public DetailsPageViewModel(IIntroductionApiClientV1_0 introductionApiClientV1_0, ILogger<DetailsPageViewModel> logger)
    {
        _introductionApiClientV1_0 = introductionApiClientV1_0;
        _logger = logger;
    }

    [ObservableProperty]
    [SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Utilizing ObservableProperty attribute")]
    public string introductionId;
}

public interface IDetailsPageViewModel
{
    string IntroductionId { get; }
}

