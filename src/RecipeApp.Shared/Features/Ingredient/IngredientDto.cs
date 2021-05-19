using System;
using System.ComponentModel.DataAnnotations;

using RecipeApp.Shared.Features.Dto;

namespace RecipeApp.Shared.Models.Ingredient
{
    public class IngredientDto : BaseDto
    {
        public Guid IntroductionId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Measurement cannot exceed 50 characters.")]
        public string Measurement { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        public string Description { get; set; }
    }
}
