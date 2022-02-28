
using Microsoft.Extensions.Logging;

using RecipeApp.Shared.Features.Instruction;
using RecipeApp.Shared.Models;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

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

            var saveInstructionTask = instructionDto.IsNew
                ? RefitExStaticMethods.TryInvokeApiAsync(() => _instructionApiClientV1_0.InsertAsync(instructionDto), ApiResultMessages)
                : RefitExStaticMethods.TryInvokeApiAsync(() => _instructionApiClientV1_0.UpdateAsync(instructionDto), ApiResultMessages);

            await saveInstructionTask;

            if (saveInstructionTask.Result.IsSuccessHttpStatusCode)
                Instructions[index] = saveInstructionTask.Result.Data;

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

        public async Task<IInstructionViewModel> MoveInstructionFirstAsync(InstructionDto instructionDto)
        {
            _logger.LogInformation("{MoveInstructionFirstAsync}({instructionDto})", nameof(MoveInstructionFirstAsync), nameof(InstructionDto));

            ClearApiResultMessages();

            if (instructionDto.IsNew)
                return AddInformationMessage("Please save the instruction before moving it.") as IInstructionViewModel;

            if (instructionDto.SortOrder.Equals(1))
                return AddInformationMessage("This instruction is already first.") as IInstructionViewModel;

            if (instructionDto.TryValidateObject(ApiResultMessages).Equals(false))
                return this;

            var currentIndex = Instructions.IndexOf(instructionDto);
            Instructions.Move(currentIndex, 0);

            return await ResequenceInstructionsSortOrderAsync();
        }

        public async Task<IInstructionViewModel> MoveInstructionPreviousAsync(InstructionDto instructionDto)
        {
            _logger.LogInformation("{MoveInstructionPreviousAsync}({instructionDto})", nameof(MoveInstructionPreviousAsync), nameof(InstructionDto));

            ClearApiResultMessages();

            if (instructionDto.IsNew)
                return AddInformationMessage("Please save the instruction before moving it.") as IInstructionViewModel;

            if (instructionDto.SortOrder.Equals(1))
                return AddInformationMessage("This instruction is already first.") as IInstructionViewModel;

            if (instructionDto.TryValidateObject(ApiResultMessages).Equals(false))
                return this;

            var currentIndex = Instructions.IndexOf(instructionDto);
            Instructions.Move(currentIndex, currentIndex - 1);

            return await ResequenceInstructionsSortOrderAsync();
        }

        public async Task<IInstructionViewModel> MoveInstructionNextAsync(InstructionDto instructionDto)
        {
            _logger.LogInformation("{MoveInstructionNextAsync}({instructionDto})", nameof(MoveInstructionNextAsync), nameof(InstructionDto));

            ClearApiResultMessages();

            if (instructionDto.IsNew)
                return AddInformationMessage("Please save the instruction before moving it.") as IInstructionViewModel;

            if (instructionDto.SortOrder.Equals(Instructions.Count))
                return AddInformationMessage("This instruction is already last.") as IInstructionViewModel;

            if (instructionDto.TryValidateObject(ApiResultMessages).Equals(false))
                return this;

            var currentIndex = Instructions.IndexOf(instructionDto);
            Instructions.Move(currentIndex, currentIndex + 1);

            return await ResequenceInstructionsSortOrderAsync();
        }

        public async Task<IInstructionViewModel> MoveInstructionLastAsync(InstructionDto instructionDto)
        {
            _logger.LogInformation("{MoveInstructionLastAsync}({instructionDto})", nameof(MoveInstructionLastAsync), nameof(InstructionDto));

            ClearApiResultMessages();

            if (instructionDto.IsNew)
                return AddInformationMessage("Please save the instruction before moving it.") as IInstructionViewModel;

            if (instructionDto.SortOrder.Equals(Instructions.Count))
                return AddInformationMessage("This instruction is already last.") as IInstructionViewModel;

            if (instructionDto.TryValidateObject(ApiResultMessages).Equals(false))
                return this;

            var currentIndex = Instructions.IndexOf(instructionDto);
            Instructions.Move(currentIndex, Instructions.Count - 1);

            return await ResequenceInstructionsSortOrderAsync();
        }

        protected async Task<IInstructionViewModel> ResequenceInstructionsSortOrderAsync()
        {
            var index = 0;
            foreach (var item in Instructions)
                item.SortOrder = ++index;

            var instructionsDto = new InstructionsDto { Instructions = Instructions.ToList() };
            await RefitExStaticMethods.TryInvokeApiAsync(() => _instructionApiClientV1_0.UpdateMultipleAsync(instructionsDto), ApiResultMessages);

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

        Task<IInstructionViewModel> MoveInstructionFirstAsync(InstructionDto instructionDto);

        Task<IInstructionViewModel> MoveInstructionPreviousAsync(InstructionDto instructionDto);

        Task<IInstructionViewModel> MoveInstructionNextAsync(InstructionDto instructionDto);

        Task<IInstructionViewModel> MoveInstructionLastAsync(InstructionDto instructionDto);
    }
}
