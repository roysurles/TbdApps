using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using RecipeApp.Shared.Features.Instruction;

using Refit;

using Tbd.Shared.ApiResult;

namespace RecipeApp.BlazorWasmBootstrap.Features.Shared.ApiClients
{
    public interface IInstructionV1_0ApiClient
    {
        [Get("/api/v1.0/Instruction/{id}")]
        Task<ApiResultModel<InstructionDto>> GetAsync(Guid id);

        [Get("/api/v1.0/Instruction/AllForIntroductionId/{introductionId}")]
        Task<ApiResultModel<IEnumerable<InstructionDto>>> GetAllForIntroductionIdAsync(Guid introductionId);

        [Post("/api/v1.0/Instruction")]
        Task<ApiResultModel<InstructionDto>> InsertAsync([Body] InstructionDto instructionDto);

        [Put("/api/v1.0/Instruction")]
        Task<ApiResultModel<InstructionDto>> UpdateAsync([Body] InstructionDto instructionDto);

        [Delete("/api/v1.0/Instruction/{id}")]
        Task<ApiResultModel<int>> DeleteAsync(Guid id);
    }
}
