using System;

namespace RecipeApp.Shared.Features.Introduction
{
    public class IntroductionSearchResultDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Comment { get; set; }

        public int IngredientsCount { get; set; }

        public int InstructionsCount { get; set; }
    }
}
