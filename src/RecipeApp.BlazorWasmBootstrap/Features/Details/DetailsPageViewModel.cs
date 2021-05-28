
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using RecipeApp.BlazorWasmBootstrap.Features.Shared.ApiClients;
using RecipeApp.BlazorWasmBootstrap.Features.Shared.Models;
using RecipeApp.Shared.Features.Ingredient;
using RecipeApp.Shared.Features.Instruction;
using RecipeApp.Shared.Features.Introduction;

using Tbd.RefitEx;
using Tbd.Shared.Extensions;

namespace RecipeApp.BlazorWasmBootstrap.Features.Details
{
    public class DetailsPageViewModel : BaseViewModel, IDetailsPageViewModel
    {
        protected readonly IIntroductionV1_0ApiClient _introductionV1_0ApiClient;
        protected readonly IIngredientV1_0ApiClient _ingredientV1_0ApiClient;
        protected readonly IInstructionV1_0ApiClient _instructionV1_0ApiClient;
        protected readonly ILogger<DetailsPageViewModel> _logger;
        protected Guid _introductionId = Guid.Empty;

        public DetailsPageViewModel(IIntroductionV1_0ApiClient introductionV1_0ApiClient
            , IIngredientV1_0ApiClient ingredientV1_0ApiClient
            , IInstructionV1_0ApiClient instructionV1_0ApiClient
            , ILogger<DetailsPageViewModel> logger)
        {
            _introductionV1_0ApiClient = introductionV1_0ApiClient;
            _ingredientV1_0ApiClient = ingredientV1_0ApiClient;
            _instructionV1_0ApiClient = instructionV1_0ApiClient;
            _logger = logger;
        }

        public bool IsValidIntroductionIdParameter { get; protected set; } = true;

        public IntroductionDto Introduction { get; protected set; } =
            new IntroductionDto();

        public ObservableCollection<IngredientDto> Ingredients { get; protected set; } =
            new ObservableCollection<IngredientDto>();

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
                () => _introductionV1_0ApiClient.GetAsync(_introductionId), ApiResultMessages);
            var getIngredientsTask = RefitExStaticMethods.TryInvokeApiAsync(
                () => _ingredientV1_0ApiClient.GetAllForIntroductionIdAsync(_introductionId), ApiResultMessages);
            var getInstructionsTask = RefitExStaticMethods.TryInvokeApiAsync(
                () => _instructionV1_0ApiClient.GetAllForIntroductionIdAsync(_introductionId), ApiResultMessages);

            await Task.WhenAll(getIntroductionTask, getIngredientsTask, getInstructionsTask);

            Introduction = getIntroductionTask.Result.Data;

            Ingredients.Clear();
            Ingredients.AddRange(getIngredientsTask.Result.Data);

            Instructions.Clear();
            Instructions.AddRange(getInstructionsTask.Result.Data);

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

            var apiResult = await RefitExStaticMethods.TryInvokeApiAsync(() => _introductionV1_0ApiClient.DeleteAsync(Introduction.Id), ApiResultMessages);
            if (apiResult.IsSuccessHttpStatusCode)
                AddInformationMessage("Introduction deleted successfully!", $"{nameof(DetailsPageViewModel)}.{nameof(DeleteIntroductionAsync)}", 200);

            return SetIntroductionToNewDto();
        }

        public IDetailsPageViewModel AddIngredient()
        {
            _logger.LogInformation($"{nameof(AddIngredient)}()");

            ClearApiResultMessages();

            Ingredients.Add(new IngredientDto { IntroductionId = Introduction.Id });

            return this;
        }

        public async Task<IDetailsPageViewModel> SaveIngredientAsync(IngredientDto ingredientDto)
        {
            _logger.LogInformation($"{nameof(SaveIngredientAsync)}()");

            ClearApiResultMessages();

            if (ingredientDto.TryValidateObject(ApiResultMessages).Equals(false))
                return this;

            var index = Ingredients.IndexOf(ingredientDto);

            var saveIngredientTask = ingredientDto.IsNew
                ? RefitExStaticMethods.TryInvokeApiAsync(() => _ingredientV1_0ApiClient.InsertAsync(ingredientDto), ApiResultMessages)
                : RefitExStaticMethods.TryInvokeApiAsync(() => _ingredientV1_0ApiClient.UpdateAsync(ingredientDto), ApiResultMessages);

            await saveIngredientTask;
            // TODO:  need snackbar or stacking alerts
            if (saveIngredientTask.Result.IsSuccessHttpStatusCode)
            {
                Ingredients[index] = saveIngredientTask.Result.Data;
                AddInformationMessage("Ingredient saved successfully!", $"{nameof(DetailsPageViewModel)}.{nameof(SaveIntroductionAsync)}", 200);
            }

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

        ObservableCollection<IngredientDto> Ingredients { get; }

        ObservableCollection<InstructionDto> Instructions { get; }

        Task<IDetailsPageViewModel> InitializeAsync(string introductionId);

        Task<IDetailsPageViewModel> SaveIntroductionAsync();

        Task<IDetailsPageViewModel> DeleteIntroductionAsync();

        IDetailsPageViewModel AddIngredient();

        Task<IDetailsPageViewModel> SaveIngredientAsync(IngredientDto ingredientDto);
    }
}
