﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeApp.Database.Ef.RecipeDb.Models
{
    [Table("Ingredient")]
    public class IngredientModel
    {
        [Key]
        public Guid Id { get; set; }

        public Guid IntroductionId { get; set; }

        public int SortOrder { get; set; }

        [MaxLength(50)]
        public string Measurement { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        [MaxLength(255)]
        public string CreatedById { get; set; }

        public DateTime? CreatedOnUtc { get; set; }

        [MaxLength(255)]
        public string UpdatedById { get; set; }

        public DateTime? UpdatedOnUtc { get; set; }
    }
}
