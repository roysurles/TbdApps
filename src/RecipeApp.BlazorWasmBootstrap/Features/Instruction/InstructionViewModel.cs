

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using RecipeApp.BlazorWasmBootstrap.Features.Shared.Models;
using RecipeApp.Shared.Features.Instruction;

using Tbd.RefitEx;
using Tbd.Shared.Extensions;

namespace RecipeApp.BlazorWasmBootstrap.Features.Instruction
{
    public class InstructionViewModel : BaseViewModel, IInstructionViewModel
    {
        protected readonly IInstructionApiClientV1_0 _instructionApiClientV1_0;
        protected readonly ILogger<InstructionViewModel> _logger;

        public InstructionViewModel(IInstructionApiClientV1_0 instructionApiClientV1_0
            , ILogger<InstructionViewModel> logger)
        {
            _instructionApiClientV1_0 = instructionApiClientV1_0;
            _logger = logger;
        }

        public Guid IntroductionId { get; protected set; }

        public bool IsIntroductionNew =>
            Equals(Guid.Empty, IntroductionId);

        public ObservableCollection<InstructionDto> Instructions { get; protected set; } =
            new ObservableCollection<InstructionDto>();

        public async Task<IInstructionViewModel> InitializeAsync(Guid introductionId)
        {
            _logger.LogInformation($"{nameof(InstructionViewModel)}({introductionId})");

            IntroductionId = introductionId;

            var response = await RefitExStaticMethods.TryInvokeApiAsync(
                () => _instructionApiClientV1_0.GetAllForIntroductionIdAsync(IntroductionId), ApiResultMessages);

            Instructions.Clear();
            Instructions.AddRange(response.Data);

            return this;
        }

        public IInstructionViewModel AddInstruction()
        {
            _logger.LogInformation($"{nameof(AddInstruction)}()");

            ClearApiResultMessages();

            Instructions.Add(new InstructionDto { IntroductionId = IntroductionId });

            return this;
        }

        public async Task<IInstructionViewModel> SaveInstructionAsync(InstructionDto instructionDto)
        {
            _logger.LogInformation($"{nameof(SaveInstructionAsync)}({nameof(instructionDto)})");

            ClearApiResultMessages();

            if (instructionDto.TryValidateObject(ApiResultMessages).Equals(false))
                return this;

            var index = Instructions.IndexOf(instructionDto);

            var saveIngredientTask = instructionDto.IsNew
                ? RefitExStaticMethods.TryInvokeApiAsync(() => _instructionApiClientV1_0.InsertAsync(instructionDto), ApiResultMessages)
                : RefitExStaticMethods.TryInvokeApiAsync(() => _instructionApiClientV1_0.UpdateAsync(instructionDto), ApiResultMessages);

            await saveIngredientTask;

            // TODO:  need snackbar or stacking alerts
            if (saveIngredientTask.Result.IsSuccessHttpStatusCode)
            {
                Instructions[index] = saveIngredientTask.Result.Data;
                AddInformationMessage("Ingredient saved successfully!", $"{nameof(InstructionViewModel)}.{nameof(SaveInstructionAsync)}", 200);
            }

            return this;
        }

        public async Task<IInstructionViewModel> DeleteInstructionAsync(InstructionDto instructionDto)
        {
            _logger.LogInformation($"{nameof(DeleteInstructionAsync)}({nameof(instructionDto)})");

            ClearApiResultMessages();

            var index = Instructions.IndexOf(instructionDto);

            var response = await RefitExStaticMethods.TryInvokeApiAsync(() => _instructionApiClientV1_0.DeleteAsync(instructionDto.Id), ApiResultMessages);

            if (response.IsSuccessHttpStatusCode)
            {
                Instructions.RemoveAt(index);
                AddInformationMessage("Ingredient deleted successfully!", $"{nameof(InstructionViewModel)}.{nameof(DeleteInstructionAsync)}", 200);
            }

            return this;
        }
    }

    public interface IInstructionViewModel : IBaseViewModel
    {
        Guid IntroductionId { get; }

        bool IsIntroductionNew { get; }

        ObservableCollection<InstructionDto> Instructions { get; }

        Task<IInstructionViewModel> InitializeAsync(Guid introductionId);

        IInstructionViewModel AddInstruction();

        Task<IInstructionViewModel> SaveInstructionAsync(InstructionDto instructionDto);

        Task<IInstructionViewModel> DeleteInstructionAsync(InstructionDto instructionDto);
    }
}
