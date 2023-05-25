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
}

public interface IInstructionViewModel : IBaseViewModel
{
    ObservableCollection<InstructionDto> Instructions { get; }

    Task<IInstructionViewModel> InitializeAsync(Guid introductionId);
}
