using System.Collections.Generic;

namespace RecipeApp.Shared.Features.Instruction
{
    public class InstructionsDto
    {
        public List<InstructionDto> Instructions { get; set; } =
            new List<InstructionDto>();
    }
}
