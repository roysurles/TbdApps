namespace RecipeApp.Shared.Features.Introduction;

public static class IntroductionDtosExtensions
{
    public static object ToInsertParameters(this IntroductionDto introductionDto
        , string createdById = null, DateTime? createdOnUtc = null)
    {
        return new
        {
            introductionDto.Id,
            introductionDto.Title,
            introductionDto.Comment,
            CreatedById = introductionDto.CreatedById = createdById,
            CreatedOnUtc = introductionDto.CreatedOnUtc = createdOnUtc
        };
    }

    public static object ToUpdateParameters(this IntroductionDto introductionDto
        , string updatedById = null, DateTime? updatedOnUtc = null)
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
