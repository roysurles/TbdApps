using System;

namespace RecipeApp.Shared.Features.Instruction
{
    public static class InstructionDtosExtensions
    {
        public static object ToInsertParameters(this InstructionDto instructionDto
            , Guid id, string createdById, DateTime? createdOnUtc)
        {
            return new
            {
                Id = instructionDto.Id = id,
                instructionDto.IntroductionId,
                instructionDto.Description,
                CreatedById = instructionDto.CreatedById = createdById,
                createdOnUtc = instructionDto.CreatedOnUtc = createdOnUtc
            };
        }

        public static object ToUpdateParameters(this InstructionDto instructionDto
            , string updatedById, DateTime? updatedOnUtc)
        {
            return new
            {
                instructionDto.Id,
                instructionDto.Description,
                updatedById = instructionDto.UpdatedById = updatedById,
                UpdatedOnUtc = instructionDto.UpdatedOnUtc = updatedOnUtc
            };
        }
    }
}
