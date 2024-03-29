﻿namespace RecipeApp.CoreApi.UnitTests.Features.Ingredient.V1_0;

public partial class IngredientServiceTests
{
    [Theory(DisplayName = "IngredientServiceTests.Insert")]
    [InlineData("00000000-0000-0000-0000-000000000000", null, null, HttpStatusCode.BadRequest, new string[] { "Introduction Id is required." })]
    [InlineData("eb95c593-69b2-4483-8fc3-4f74726a317e", null, null, HttpStatusCode.BadRequest, new string[] { "The Measurement field is required.", "The Description field is required." })]
    [InlineData("eb95c593-69b2-4483-8fc3-4f74726a317e", "", "", HttpStatusCode.BadRequest, new string[] { "The Measurement field is required.", "The Description field is required." })]
    [InlineData("eb95c593-69b2-4483-8fc3-4f74726a317e", "Measurement", "Description", HttpStatusCode.Created, new string[] { })]
    public async Task Insert_Should_Return_Correct_StatusCode(Guid introductionId
        , string measurement
        , string description
        , HttpStatusCode expectedHttpStatusCode
        , string[] expectedMessages)
    {
        // Arrange
        var ingredientDto = new IngredientDto { IntroductionId = introductionId, Measurement = measurement, Description = description };

        _ingredientRepositoryMock
            .Setup(x => x.InsertAsync(It.IsAny<IngredientDto>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ingredientDto);

        // Act
        var actualApiResult = await _ingredientService.InsertAsync(ingredientDto, null, new CancellationToken());

        // Assert
        actualApiResult.HttpStatusCode.Should().Be(expectedHttpStatusCode);
        actualApiResult.Messages.Count.Should().Be(expectedMessages.Length);
        foreach (var expectedMessage in expectedMessages)
            actualApiResult.Messages.Should().Contain(m => Equals(m.Message, expectedMessage));
    }
}
