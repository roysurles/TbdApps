
using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using RecipeApp.BlazorWasmBootstrap.Features.Shared.ApiClients;
using RecipeApp.BlazorWasmBootstrap.Features.Shared.Models;
using RecipeApp.Shared.Features.Introduction;

using Tbd.RefitEx;
using Tbd.Shared.Extensions;

namespace RecipeApp.BlazorWasmBootstrap.Features.Details
{
    public class DetailsPageViewModel : BaseViewModel, IDetailsPageViewModel
    {
        protected readonly IIntroductionV1_0ApiClient _introductionV1_0ApiClient;
        protected readonly ILogger<DetailsPageViewModel> _logger;
        protected Guid _introductionId = Guid.Empty;

        public DetailsPageViewModel(IIntroductionV1_0ApiClient introductionV1_0ApiClient
            , ILogger<DetailsPageViewModel> logger)
        {
            _introductionV1_0ApiClient = introductionV1_0ApiClient;
            _logger = logger;
        }

        public bool IsValidIntroductionIdParameter { get; protected set; } = true;

        public IntroductionDto Introduction { get; protected set; } =
            new IntroductionDto();

        public async Task<IDetailsPageViewModel> InitializeAsync(string introductionId)
        {
            _logger.LogInformation($"{nameof(DetailsPageViewModel)}({introductionId})");

            IsValidIntroductionIdParameter = true;

            if (string.IsNullOrWhiteSpace(introductionId))
                return SetIntroductionToNewDto();

            if (Guid.TryParse(introductionId, out Guid _introductionId).Equals(false))
            {
                IsValidIntroductionIdParameter = false;
                AddInformationMessage("The Id for this page is incorrect.  Please navigate to the Home page and try again.", $"{nameof(DetailsPageViewModel)}.{nameof(InitializeAsync)}", HttpStatusCode.BadRequest.ToInt());
                return this;
            }

            if (Equals(Guid.Empty, _introductionId))
                return SetIntroductionToNewDto();

            var getIntroductionTask = RefitExStaticMethods.TryInvokeApiAsync(
                () => _introductionV1_0ApiClient.GetAsync(_introductionId), ApiResultMessages);

            await Task.WhenAll(getIntroductionTask);
            Introduction = getIntroductionTask.Result.Data;

            return this;
        }

        public async Task<IDetailsPageViewModel> SaveIntroductionAsync()
        {
            _logger.LogInformation($"{nameof(SaveIntroductionAsync)}()");

            ClearApiResultMessages();

            if (Introduction.TryValidateObject(ApiResultMessages).Equals(false))
                return this;

            var saveIntroductionTask = Introduction.IsNew
                ? RefitExStaticMethods.TryInvokeApiAsync(() => _introductionV1_0ApiClient.InsertAsync(Introduction), ApiResultMessages)
                : RefitExStaticMethods.TryInvokeApiAsync(() => _introductionV1_0ApiClient.UpdateAsync(Introduction), ApiResultMessages);

            await saveIntroductionTask;
            Introduction = saveIntroductionTask.Result.Data;

            return this;
        }

        protected IDetailsPageViewModel SetIntroductionToNewDto()
        {
            Introduction = new IntroductionDto();
            return this;
        }
    }

    public interface IDetailsPageViewModel : IBaseViewModel
    {
        bool IsValidIntroductionIdParameter { get; }

        IntroductionDto Introduction { get; }

        Task<IDetailsPageViewModel> InitializeAsync(string introductionId);

        Task<IDetailsPageViewModel> SaveIntroductionAsync();
    }
}
