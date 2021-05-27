
using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using RecipeApp.BlazorWasmBootstrap.Features.Shared.ApiClients;
using RecipeApp.BlazorWasmBootstrap.Features.Shared.Models;
using RecipeApp.Shared.Features.Introduction;

using Tbd.Shared.Extensions;

namespace RecipeApp.BlazorWasmBootstrap.Features.Details
{
    public class DetailsPageViewModel : BaseViewModel, IDetailsPageViewModel
    {
        protected readonly IIntroductionV1_0ApiClient _introductionV1_0ApiClient;
        protected readonly ILogger<DetailsPageViewModel> _logger;
        protected Guid _introductionId;

        public DetailsPageViewModel(IIntroductionV1_0ApiClient introductionV1_0ApiClient
            , ILogger<DetailsPageViewModel> logger)
        {
            _introductionV1_0ApiClient = introductionV1_0ApiClient;
            _logger = logger;
        }

        public IntroductionDto Introduction { get; protected set; } =
            new IntroductionDto();

        public async Task<IDetailsPageViewModel> InitializeAsync(string introductionId)
        {
            _logger.LogInformation($"{nameof(DetailsPageViewModel)}({introductionId})");

            if (string.IsNullOrWhiteSpace(introductionId) || Guid.TryParse(introductionId, out Guid parsedGuid).Equals(false))
            {
                AddInformationMessage("The Id for this page is incorrect.  Please navigate to the Home page and try again.", $"{nameof(DetailsPageViewModel)}.{nameof(InitializeAsync)}", HttpStatusCode.BadRequest.ToInt());
                return this;
            }

            if (Equals(Guid.Empty, _introductionId))
            {
                Introduction = new IntroductionDto();
                return this;
            }

            var getIntroductionTask = _introductionV1_0ApiClient.GetAsync(_introductionId);

            await Task.WhenAll(getIntroductionTask);

            AddMessages(getIntroductionTask.Result.Messages);

            return this;
        }

        public async Task<IDetailsPageViewModel> SaveIntroductionAsync()
        {
            _logger.LogInformation($"{nameof(SaveIntroductionAsync)}()");

            ClearApiResultMessages();

            if (Introduction.TryValidateObject(ApiResultMessages).Equals(false))
                return this;

            var saveIntroductionTask = Introduction.IsNew
                ? _introductionV1_0ApiClient.InsertAsync(Introduction)
                : _introductionV1_0ApiClient.UpdateAsync(Introduction);

            await saveIntroductionTask;

            AddMessages(saveIntroductionTask.Result.Messages);
            Introduction = saveIntroductionTask.Result.Data;

            return this;
        }
    }

    public interface IDetailsPageViewModel : IBaseViewModel
    {
        public IntroductionDto Introduction { get; }

        Task<IDetailsPageViewModel> InitializeAsync(string introductionId);

        Task<IDetailsPageViewModel> SaveIntroductionAsync();
    }
}
