
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
        protected Guid _introductionId = Guid.Empty;

        public InstructionViewModel(IInstructionApiClientV1_0 instructionApiClientV1_0
            , ILogger<InstructionViewModel> logger)
        {
            _instructionApiClientV1_0 = instructionApiClientV1_0;
            _logger = logger;
        }

        public bool IsIntroductionNew =>
            Equals(Guid.Empty, _introductionId);

        public ObservableCollection<InstructionDto> Instructions { get; protected set; } =
            new ObservableCollection<InstructionDto>();

        public async Task<IInstructionViewModel> InitializeAsync(Guid introductionId)
        {
            _logger.LogInformation($"{nameof(InstructionViewModel)}({introductionId})");

            ApiResultMessages.Clear();
            Instructions.Clear();
            _introductionId = introductionId;

            if (Equals(Guid.Empty, _introductionId))
                return this;

            var response = await RefitExStaticMethods.TryInvokeApiAsync(
                () => _instructionApiClientV1_0.GetAllForIntroductionIdAsync(introductionId), ApiResultMessages);

            Instructions.AddRange(response.Data);

            return this;
        }

        public IInstructionViewModel AddInstruction()
        {
            _logger.LogInformation($"{nameof(AddInstruction)}()");

            ClearApiResultMessages();

            Instructions.Add(new InstructionDto { IntroductionId = _introductionId });

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

            if (saveIngredientTask.Result.IsSuccessHttpStatusCode)
                Instructions[index] = saveIngredientTask.Result.Data;

            return this;
        }

        public async Task<IInstructionViewModel> DeleteInstructionAsync(InstructionDto instructionDto)
        {
            _logger.LogInformation($"{nameof(DeleteInstructionAsync)}({nameof(instructionDto)})");

            ClearApiResultMessages();

            var index = Instructions.IndexOf(instructionDto);

            var response = await RefitExStaticMethods.TryInvokeApiAsync(() => _instructionApiClientV1_0.DeleteAsync(instructionDto.Id), ApiResultMessages);

            if (response.IsSuccessHttpStatusCode)
                Instructions.RemoveAt(index);

            return this;
        }
    }

    public interface IInstructionViewModel : IBaseViewModel
    {
        bool IsIntroductionNew { get; }

        ObservableCollection<InstructionDto> Instructions { get; }

        Task<IInstructionViewModel> InitializeAsync(Guid introductionId);

        IInstructionViewModel AddInstruction();

        Task<IInstructionViewModel> SaveInstructionAsync(InstructionDto instructionDto);

        Task<IInstructionViewModel> DeleteInstructionAsync(InstructionDto instructionDto);
    }
}
