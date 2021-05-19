using System;

namespace RecipeApp.Shared.Features.Introduction
{
    public static class IntroductionDtosExtensions
    {
        public static object ToInsertParameters(this IntroductionDto introductionDto
            , Guid id, string createdById, DateTime? createdOnUtc)
        {
            return new
            {
                Id = introductionDto.Id = id,
                introductionDto.Title,
                introductionDto.Comment,
                CreatedById = introductionDto.CreatedById = createdById,
                createdOnUtc = introductionDto.CreatedOnUtc = createdOnUtc
            };
        }

        public static object ToUpdateParameters(this IntroductionDto introductionDto
            , string updatedById, DateTime? updatedOnUtc)
        {
            return new
            {
                introductionDto.Id,
                introductionDto.Title,
                introductionDto.Comment,
                updatedById = introductionDto.UpdatedById = updatedById,
                UpdatedOnUtc = introductionDto.UpdatedOnUtc = updatedOnUtc
            };
        }
    }
}
