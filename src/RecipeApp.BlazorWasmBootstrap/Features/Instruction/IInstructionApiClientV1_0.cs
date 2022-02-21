using RecipeApp.Shared.Features.Instruction;

using Refit;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Tbd.Shared.ApiResult;

namespace RecipeApp.BlazorWasmBootstrap.Features.Instruction
{
    public interface IInstructionApiClientV1_0
    {
        [Get("/api/v1.0/Instruction/{id}")]
        Task<ApiResultModel<InstructionDto>> GetAsync(Guid id);

        [Get("/api/v1.0/Instruction/AllForIntroductionId/{introductionId}")]
        Task<ApiResultModel<List<InstructionDto>>> GetAllForIntroductionIdAsync(Guid introductionId);

        [Post("/api/v1.0/Instruction")]
        Task<ApiResultModel<InstructionDto>> InsertAsync([Body] InstructionDto instructionDto);

        [Put("/api/v1.0/Instruction")]
        Task<ApiResultModel<InstructionDto>> UpdateAsync([Body] InstructionDto instructionDto);

        [Put("/api/v1.0/Instruction/Multiple")]
        Task<ApiResultModel<int>> UpdateMultipleAsync([Body] InstructionsDto instructionsDto);

        [Delete("/api/v1.0/Instruction/{id}")]
        Task<ApiResultModel<int>> DeleteAsync(Guid id);
    }
}
