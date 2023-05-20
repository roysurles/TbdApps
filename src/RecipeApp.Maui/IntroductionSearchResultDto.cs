namespace RecipeApp.Maui;

public class IntroductionSearchResultDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Comment { get; set; }

    public int IngredientsCount { get; set; }

    public int InstructionsCount { get; set; }

    // For MAUI only...
    public string IngredientsAndInstructionsCountsAsString =>
        $"{IngredientsCount:#,##0} ingredients; {InstructionsCount:#,##0} instructions;";
}
