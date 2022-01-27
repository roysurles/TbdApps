using RecipeApp.Shared.Features.Instruction;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RecipeApp.CoreApi.Features.Instruction.V1_0
{
    public interface IInstructionRepositoryV1_0
    {
        Task<InstructionDto> SelectAsync(Guid id, CancellationToken cancellationToken);

        Task<IEnumerable<InstructionDto>> SelectAllForIntroductionIdAsync(Guid introductionId, CancellationToken cancellationToken);

        Task<InstructionDto> InsertAsync(InstructionDto instructionDto, string createdById, CancellationToken cancellationToken);

        Task<InstructionDto> UpdateAsync(InstructionDto instructionDto, string updatedById, CancellationToken cancellationToken);

        Task<int> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}