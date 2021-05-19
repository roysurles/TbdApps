using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using RecipeApp.BlazorWasmBootstrap.Features.Shared.Models;
using RecipeApp.Shared.Features.Introduction;

namespace RecipeApp.BlazorWasmBootstrap.Features.Shared.Services.IntroductionSearch
{
    public class IntroductionSearchViewModel : BaseViewModel, IIntroductionSearchViewModel
    {
        protected readonly IIntroductionV1_0ApiClient _introductionV1_0ApiClient;
        protected readonly ILogger<IntroductionSearchViewModel> _logger;

        public IntroductionSearchViewModel(IIntroductionV1_0ApiClient introductionV1_0ApiClient
            , ILogger<IntroductionSearchViewModel> logger)
        {
            _introductionV1_0ApiClient = introductionV1_0ApiClient;
            _logger = logger;
        }

        public IntroductionSearchRequestDto IntroductionSearchRequestDto { get; } =
            new IntroductionSearchRequestDto();

        public ObservableCollection<IntroductionSearchResultDto> IntroductionSearchResults { get; } =
            new ObservableCollection<IntroductionSearchResultDto>();

        public async Task SearchAsync()
        {
            _logger.LogInformation(nameof(SearchAsync));

            ClearApiResultMessages();
            IntroductionSearchResults.Clear();

            var response = await _introductionV1_0ApiClient.SearchAsync(IntroductionSearchRequestDto);
            ApiResultMessages.AddRange(response.Messages);
            // TODO:  move this to an extension:  AddRange
            foreach (var item in response.Data)
                IntroductionSearchResults.Add(item);
        }
    }

    public interface IIntroductionSearchViewModel : IBaseViewModel
    {
        IntroductionSearchRequestDto IntroductionSearchRequestDto { get; }

        ObservableCollection<IntroductionSearchResultDto> IntroductionSearchResults { get; }

        Task SearchAsync();
    }
}
