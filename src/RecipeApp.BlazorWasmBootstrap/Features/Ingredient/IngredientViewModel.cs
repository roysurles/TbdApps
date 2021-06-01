﻿
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using RecipeApp.BlazorWasmBootstrap.Features.Shared.Models;
using RecipeApp.Shared.Features.Ingredient;

using Tbd.RefitEx;
using Tbd.Shared.Extensions;

namespace RecipeApp.BlazorWasmBootstrap.Features.Ingredient
{
    public class IngredientViewModel : BaseViewModel, IIngredientViewModel
    {
        protected readonly IIngredientApiClientV1_0 _ingredientApiClientV1_0;
        protected readonly ILogger<IngredientViewModel> _logger;

        public IngredientViewModel(IIngredientApiClientV1_0 ingredientApiClientV1_0
            , ILogger<IngredientViewModel> logger)
        {
            _ingredientApiClientV1_0 = ingredientApiClientV1_0;
            _logger = logger;
        }

        public Guid IntroductionId { get; protected set; }

        public bool IsIntroductionNew =>
            Equals(Guid.Empty, IntroductionId);

        public ObservableCollection<IngredientDto> Ingredients { get; protected set; } =
            new ObservableCollection<IngredientDto>();

        public async Task<IIngredientViewModel> InitializeAsync(Guid introductionId)
        {
            _logger.LogInformation($"{nameof(IngredientViewModel)}({introductionId})");

            IntroductionId = introductionId;

            var response = await RefitExStaticMethods.TryInvokeApiAsync(
                () => _ingredientApiClientV1_0.GetAllForIntroductionIdAsync(IntroductionId), ApiResultMessages);

            Ingredients.Clear();
            Ingredients.AddRange(response.Data);

            return this;
        }

        public IIngredientViewModel AddIngredient()
        {
            _logger.LogInformation($"{nameof(AddIngredient)}()");

            ClearApiResultMessages();

            Ingredients.Add(new IngredientDto { IntroductionId = IntroductionId });

            return this;
        }

        public async Task<IIngredientViewModel> SaveIngredientAsync(IngredientDto ingredientDto)
        {
            _logger.LogInformation($"{nameof(SaveIngredientAsync)}({nameof(ingredientDto)})");

            ClearApiResultMessages();

            if (ingredientDto.TryValidateObject(ApiResultMessages).Equals(false))
                return this;

            var index = Ingredients.IndexOf(ingredientDto);

            var saveIngredientTask = ingredientDto.IsNew
                ? RefitExStaticMethods.TryInvokeApiAsync(() => _ingredientApiClientV1_0.InsertAsync(ingredientDto), ApiResultMessages)
                : RefitExStaticMethods.TryInvokeApiAsync(() => _ingredientApiClientV1_0.UpdateAsync(ingredientDto), ApiResultMessages);

            await saveIngredientTask;

            // TODO:  need snackbar or stacking alerts
            if (saveIngredientTask.Result.IsSuccessHttpStatusCode)
            {
                Ingredients[index] = saveIngredientTask.Result.Data;
                AddInformationMessage("Ingredient saved successfully!", $"{nameof(IngredientViewModel)}.{nameof(SaveIngredientAsync)}", 200);
            }

            return this;
        }

        public async Task<IIngredientViewModel> DeleteIngredientAsync(IngredientDto ingredientDto)
        {
            _logger.LogInformation($"{nameof(DeleteIngredientAsync)}({nameof(ingredientDto)})");

            ClearApiResultMessages();

            var index = Ingredients.IndexOf(ingredientDto);

            var response = await RefitExStaticMethods.TryInvokeApiAsync(() => _ingredientApiClientV1_0.DeleteAsync(ingredientDto.Id), ApiResultMessages);

            if (response.IsSuccessHttpStatusCode)
            {
                Ingredients.RemoveAt(index);
                AddInformationMessage("Ingredient deleted successfully!", $"{nameof(IngredientViewModel)}.{nameof(DeleteIngredientAsync)}", 200);
            }

            return this;
        }
    }

    public interface IIngredientViewModel : IBaseViewModel
    {
        Guid IntroductionId { get; }

        bool IsIntroductionNew { get; }

        ObservableCollection<IngredientDto> Ingredients { get; }

        Task<IIngredientViewModel> InitializeAsync(Guid introductionId);

        IIngredientViewModel AddIngredient();

        Task<IIngredientViewModel> SaveIngredientAsync(IngredientDto ingredientDto);

        Task<IIngredientViewModel> DeleteIngredientAsync(IngredientDto ingredientDto);
    }
}
