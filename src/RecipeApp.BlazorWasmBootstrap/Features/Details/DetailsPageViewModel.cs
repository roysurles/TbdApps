
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using RecipeApp.BlazorWasmBootstrap.Features.Ingredient;
using RecipeApp.BlazorWasmBootstrap.Features.Instruction;
using RecipeApp.BlazorWasmBootstrap.Features.Shared.ApiClients;
using RecipeApp.BlazorWasmBootstrap.Features.Shared.Models;
using RecipeApp.Shared.Features.Instruction;
using RecipeApp.Shared.Features.Introduction;

using Tbd.RefitEx;
using Tbd.Shared.Extensions;

namespace RecipeApp.BlazorWasmBootstrap.Features.Details
{
    public class DetailsPageViewModel : BaseViewModel, IDetailsPageViewModel
    {
        protected readonly IIntroductionApiClientV1_0 _introductionpiClientV1_0;
        protected readonly ILogger<DetailsPageViewModel> _logger;
        protected Guid _introductionId = Guid.Empty;

        public DetailsPageViewModel(IIntroductionApiClientV1_0 introductionpiClientV1_0
            , IIngredientViewModel ingredientViewModel
            , IInstructionViewModel instructionViewModel
            , ILogger<DetailsPageViewModel> logger)
        {
            _introductionpiClientV1_0 = introductionpiClientV1_0;
            IngredientViewModel = ingredientViewModel;
            InstructionViewModel = instructionViewModel;
            _logger = logger;
        }

        public bool IsValidIntroductionIdParameter { get; protected set; } = true;

        public IntroductionDto Introduction { get; protected set; } =
            new IntroductionDto();

        public IIngredientViewModel IngredientViewModel { get; }

        public IInstructionViewModel InstructionViewModel { get; }

        public ObservableCollection<InstructionDto> Instructions { get; protected set; } =
            new ObservableCollection<InstructionDto>();

        public async Task<IDetailsPageViewModel> InitializeAsync(string introductionId)
        {
            _logger.LogInformation($"{nameof(DetailsPageViewModel)}({introductionId})");

            IsValidIntroductionIdParameter = true;

            if (string.IsNullOrWhiteSpace(introductionId))
                return SetIntroductionToNewDto();

            if (Guid.TryParse(introductionId, out Guid parsedGuid).Equals(false))
            {
                IsValidIntroductionIdParameter = false;
                AddInformationMessage("The Id for this page is incorrect.  Please navigate to the Home page and try again.", $"{nameof(DetailsPageViewModel)}.{nameof(InitializeAsync)}", HttpStatusCode.BadRequest.ToInt());
                return this;
            }
            _introductionId = parsedGuid;

            if (Equals(Guid.Empty, _introductionId))
                return SetIntroductionToNewDto();

            var getIntroductionTask = RefitExStaticMethods.TryInvokeApiAsync(
                () => _introductionpiClientV1_0.GetAsync(_introductionId), ApiResultMessages);

            var initializeIngredientViewModelTask = IngredientViewModel.InitializeAsync(_introductionId);
            var initializeInstructionViewModelTask = InstructionViewModel.InitializeAsync(_introductionId);

            await Task.WhenAll(getIntroductionTask, initializeIngredientViewModelTask, initializeInstructionViewModelTask);

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
                ? RefitExStaticMethods.TryInvokeApiAsync(() => _introductionpiClientV1_0.InsertAsync(Introduction), ApiResultMessages)
                : RefitExStaticMethods.TryInvokeApiAsync(() => _introductionpiClientV1_0.UpdateAsync(Introduction), ApiResultMessages);

            await saveIntroductionTask;
            // TODO:  need snackbar or stacking alerts
            if (saveIntroductionTask.Result.IsSuccessHttpStatusCode)
                AddInformationMessage("Introduction saved successfully!", $"{nameof(DetailsPageViewModel)}.{nameof(SaveIntroductionAsync)}", 200);
            Introduction = saveIntroductionTask.Result.Data;

            return this;
        }

        public async Task<IDetailsPageViewModel> DeleteIntroductionAsync()
        {
            _logger.LogInformation($"{nameof(DeleteIntroductionAsync)}()");

            ClearApiResultMessages();

            if (Introduction?.IsNew == true)
            {
                AddInformationMessage("There is nothing to Delete!", $"{nameof(DetailsPageViewModel)}.{nameof(DeleteIntroductionAsync)}");
                return this;
            }

            var apiResult = await RefitExStaticMethods.TryInvokeApiAsync(() => _introductionpiClientV1_0.DeleteAsync(Introduction.Id), ApiResultMessages);
            if (apiResult.IsSuccessHttpStatusCode)
                AddInformationMessage("Introduction deleted successfully!", $"{nameof(DetailsPageViewModel)}.{nameof(DeleteIntroductionAsync)}", 200);

            return SetIntroductionToNewDto();
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

        IIngredientViewModel IngredientViewModel { get; }

        IInstructionViewModel InstructionViewModel { get; }

        Task<IDetailsPageViewModel> InitializeAsync(string introductionId);

        Task<IDetailsPageViewModel> SaveIntroductionAsync();

        Task<IDetailsPageViewModel> DeleteIntroductionAsync();
    }
}
