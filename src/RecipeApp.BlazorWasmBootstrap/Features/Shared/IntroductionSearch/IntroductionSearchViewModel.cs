using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using RecipeApp.BlazorWasmBootstrap.Features.Shared.Models;
using RecipeApp.Shared.Features.Introduction;

using Tbd.Shared.Extensions;

namespace RecipeApp.BlazorWasmBootstrap.Features.Shared.IntroductionSearch
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

        public event EventHandler StateHasChangedEvent;

        public bool HasSearched { get; protected set; }

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
            IntroductionSearchResults.AddRange(response.Data);
            HasSearched = true;
            StateHasChangedEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    public interface IIntroductionSearchViewModel : IBaseViewModel
    {
        event EventHandler StateHasChangedEvent;

        public bool HasSearched { get; }

        IntroductionSearchRequestDto IntroductionSearchRequestDto { get; }

        ObservableCollection<IntroductionSearchResultDto> IntroductionSearchResults { get; }

        Task SearchAsync();
    }
}
