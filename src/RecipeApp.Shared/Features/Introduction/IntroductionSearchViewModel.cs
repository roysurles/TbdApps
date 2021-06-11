using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using RecipeApp.Shared.Features.Introduction;
using RecipeApp.Shared.Models;

using Tbd.RefitEx;
using Tbd.Shared.ApiResult;

namespace RecipeApp.BlazorWasmBootstrap.Features.Shared.IntroductionSearch
{
    public class IntroductionSearchViewModel : BaseViewModel, IIntroductionSearchViewModel
    {
        protected readonly IIntroductionApiClientV1_0 _introductionpiClientV1_0;
        protected readonly ILogger<IntroductionSearchViewModel> _logger;

        public IntroductionSearchViewModel(IIntroductionApiClientV1_0 introductionpiClientV1_0
            , ILogger<IntroductionSearchViewModel> logger)
        {
            _introductionpiClientV1_0 = introductionpiClientV1_0;
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

        [SuppressMessage("Design", "RCS1090:Add call to 'ConfigureAwait' (or vice versa).", Justification = "<Pending>")]
        public async Task SearchAsync(int pageNumber = 1, int pageSize = 10)
        {
            _logger.LogInformation($"{nameof(SearchAsync)}({pageNumber}, {pageSize})");

            ClearApiResultMessages();

            IntroductionSearchRequestDto.SetPagination(pageNumber, pageSize);
            IntroductionSearchResult = await RefitExStaticMethods.TryInvokeApiAsync(
                () => _introductionpiClientV1_0.SearchAsync(IntroductionSearchRequestDto), ApiResultMessages);

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
