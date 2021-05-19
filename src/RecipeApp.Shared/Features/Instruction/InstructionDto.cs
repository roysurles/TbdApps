using System;
using System.ComponentModel.DataAnnotations;

using RecipeApp.Shared.Features.Dto;

namespace RecipeApp.Shared.Features.Instruction
{
    public class InstructionDto : BaseDto
    {
        public Guid IntroductionId { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        public string Description { get; set; }
    }
}
