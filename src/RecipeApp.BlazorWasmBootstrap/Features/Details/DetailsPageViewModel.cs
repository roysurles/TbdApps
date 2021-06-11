
using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using RecipeApp.BlazorWasmBootstrap.Features.Ingredient;
using RecipeApp.BlazorWasmBootstrap.Features.Instruction;
using RecipeApp.BlazorWasmBootstrap.Features.Introduction;
using RecipeApp.Shared.Models;

namespace RecipeApp.BlazorWasmBootstrap.Features.Details
{
    public class DetailsPageViewModel : BaseViewModel, IDetailsPageViewModel
    {
        protected readonly ILogger<DetailsPageViewModel> _logger;
        protected Guid _introductionId = Guid.Empty;

        public DetailsPageViewModel(IIntroductionViewModel introductionViewModel
            , IIngredientViewModel ingredientViewModel
            , IInstructionViewModel instructionViewModel
            , ILogger<DetailsPageViewModel> logger)
        {
            IntroductionViewModel = introductionViewModel;
            IngredientViewModel = ingredientViewModel;
            InstructionViewModel = instructionViewModel;
            _logger = logger;
        }

        public IIntroductionViewModel IntroductionViewModel { get; }

        public IIngredientViewModel IngredientViewModel { get; }

        public IInstructionViewModel InstructionViewModel { get; }

        public async Task<IDetailsPageViewModel> InitializeAsync(Guid introductionId)
        {
            _logger.LogInformation($"{nameof(InitializeAsync)}({introductionId})");

            _introductionId = introductionId;

            var initializeIntroductionViewModelTask = IntroductionViewModel.InitializeAsync(_introductionId);
            var initializeIngredientViewModelTask = IngredientViewModel.InitializeAsync(_introductionId);
            var initializeInstructionViewModelTask = InstructionViewModel.InitializeAsync(_introductionId);

            await Task.WhenAll(initializeIntroductionViewModelTask, initializeIngredientViewModelTask, initializeInstructionViewModelTask);

            return this;
        }

        public async Task<IDetailsPageViewModel> InitializeIngredientsAndInstructionsAsync(Guid introductionId)
        {
            _logger.LogInformation($"{nameof(InitializeIngredientsAndInstructionsAsync)}({introductionId})");

            _introductionId = introductionId;

            var initializeIngredientViewModelTask = IngredientViewModel.InitializeAsync(_introductionId);
            var initializeInstructionViewModelTask = InstructionViewModel.InitializeAsync(_introductionId);

            await Task.WhenAll(initializeIngredientViewModelTask, initializeInstructionViewModelTask);

            return this;
        }
    }

    public interface IDetailsPageViewModel : IBaseViewModel
    {
        IIntroductionViewModel IntroductionViewModel { get; }

        IIngredientViewModel IngredientViewModel { get; }

        IInstructionViewModel InstructionViewModel { get; }

        Task<IDetailsPageViewModel> InitializeAsync(Guid introductionId);

        Task<IDetailsPageViewModel> InitializeIngredientsAndInstructionsAsync(Guid introductionId);
    }
}
