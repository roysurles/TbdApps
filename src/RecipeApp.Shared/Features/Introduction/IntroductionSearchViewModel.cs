using Microsoft.Extensions.Logging;

using RecipeApp.Shared.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Tbd.RefitEx;
using Tbd.Shared.ApiResult;

namespace RecipeApp.Shared.Features.Introduction
{
    public class IntroductionSearchViewModel : BaseViewModel, IIntroductionSearchViewModel
    {
        protected readonly IIntroductionApiClientV1_0 _introductionApiClientV1_0;
        protected readonly IIntroductionApiClientNativeV1_0 _introductionApiClientNativeV1_0;
        protected readonly ILogger<IntroductionSearchViewModel> _logger;

        public IntroductionSearchViewModel(IIntroductionApiClientV1_0 introductionpiClientV1_0
            , IIntroductionApiClientNativeV1_0 introductionApiClientNativeV1_0
            , ILogger<IntroductionSearchViewModel> logger)
        {
            _introductionApiClientV1_0 = introductionpiClientV1_0;
            _introductionApiClientNativeV1_0 = introductionApiClientNativeV1_0;
            _logger = logger;
        }

        public event EventHandler StateHasChangedEvent;

        public bool HasSearched { get; protected set; }

        public IntroductionSearchRequestDto IntroductionSearchRequestDto { get; } =
            new IntroductionSearchRequestDto { PageNumber = 1, PageSize = 10 };

        public IApiResultModel<List<IntroductionSearchResultDto>> IntroductionSearchResult { get; protected set; } =
            new ApiResultModel<List<IntroductionSearchResultDto>>()
            .SetMeta(1, 10, 0)
            .SetData(new List<IntroductionSearchResultDto>());

        public string FilterText { get; set; }

        public IApiResultModel<List<IntroductionSearchResultDto>> FilteredIntroductionSearchResult
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FilterText))
                    return IntroductionSearchResult;

                var filteredData = IntroductionSearchResult.Data.Where(item => item.Title.Contains(FilterText, StringComparison.InvariantCultureIgnoreCase)).ToList();
                return new ApiResultModel<List<IntroductionSearchResultDto>>()
                    .SetData(filteredData)
                    .SetMeta(IntroductionSearchResult.Meta);
            }
        }

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

            IntroductionSearchRequestDto.OrderByClause.Clear();
            IntroductionSearchRequestDto.SetPagination(pageNumber, pageSize)
                .OrderByClause
                    .AddOrderByAscending(p => p.Title)
                    .AddOrderByDescending(p => p.InstructionsCount);

            // NOTE:  example of using native c# typed api client
            //IntroductionSearchResult = await _introductionApiClientNativeV1_0.SearchAsync(IntroductionSearchRequestDto);

            // NOTE:  example of using refit api client
            IntroductionSearchResult = await RefitExStaticMethods.TryInvokeApiAsync(
                () => _introductionApiClientV1_0.SearchAsync(IntroductionSearchRequestDto), ApiResultMessages);

            HasSearched = true;
        }
    }

    public interface IIntroductionSearchViewModel : IBaseViewModel
    {
        event EventHandler StateHasChangedEvent;

        public bool HasSearched { get; }

        IntroductionSearchRequestDto IntroductionSearchRequestDto { get; }

        IApiResultModel<List<IntroductionSearchResultDto>> IntroductionSearchResult { get; }

        string FilterText { get; set; }

        IApiResultModel<List<IntroductionSearchResultDto>> FilteredIntroductionSearchResult { get; }

        void OnStateHasChanged();

        void SetBusyFlag(bool isBusy);

        Task SearchAsync(int pageNumber = 1, int pageSize = 10);
    }
}
