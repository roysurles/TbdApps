using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using RecipeApp.BlazorWasmBootstrap.Features.Shared.ApiClients;
using RecipeApp.BlazorWasmBootstrap.Features.Shared.Models;
using RecipeApp.Shared.Features.Introduction;

using Tbd.RefitEx;
using Tbd.Shared.ApiResult;

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
            new IntroductionSearchRequestDto { PageNumber = 1, PageSize = 10 };

        public IApiResultModel<IEnumerable<IntroductionSearchResultDto>> IntroductionSearchResult { get; protected set; } =
            new ApiResultModel<IEnumerable<IntroductionSearchResultDto>>()
            .SetMeta(1, 10, 0)
            .SetData(new List<IntroductionSearchResultDto>());

        public void OnStateHasChanged() =>
            StateHasChangedEvent?.Invoke(this, EventArgs.Empty);

        public void SetBusyFlag(bool isBusy)
        {
            IsBusy = isBusy;
            OnStateHasChanged();
        }

        public async Task SearchAsync(int pageNumber = 1, int pageSize = 10)
        {
            _logger.LogInformation($"{nameof(SearchAsync)}({pageNumber}, {pageSize})");

            ClearApiResultMessages();

            IntroductionSearchRequestDto.SetPagination(pageNumber, pageSize);
            IntroductionSearchResult = await RefitExStaticMethods.TryInvokeApiAsync(
                () => _introductionV1_0ApiClient.SearchAsync(IntroductionSearchRequestDto), ApiResultMessages);

            HasSearched = true;
        }
    }

    public interface IIntroductionSearchViewModel : IBaseViewModel
    {
        event EventHandler StateHasChangedEvent;

        public bool HasSearched { get; }

        IntroductionSearchRequestDto IntroductionSearchRequestDto { get; }

        IApiResultModel<IEnumerable<IntroductionSearchResultDto>> IntroductionSearchResult { get; }

        void OnStateHasChanged();

        void SetBusyFlag(bool isBusy);

        Task SearchAsync(int pageNumber = 1, int pageSize = 10);
    }
}
