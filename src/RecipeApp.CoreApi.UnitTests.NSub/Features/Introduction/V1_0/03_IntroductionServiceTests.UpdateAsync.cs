namespace RecipeApp.CoreApi.UnitTests.NSub.Features.Introduction.V1_0;

public partial class IntroductionServiceTests
{
    [Theory(DisplayName = "IntroductionServiceTests.Update")]
    [InlineData("00000000-0000-0000-0000-000000000000", null, HttpStatusCode.BadRequest, new string[] { "Id is required." })]
    [InlineData("eb95c593-69b2-4483-8fc3-4f74726a317e", null, HttpStatusCode.BadRequest, new string[] { "The Title field is required." })]
    [InlineData("eb95c593-69b2-4483-8fc3-4f74726a317e", "", HttpStatusCode.BadRequest, new string[] { "The Title field is required." })]
    [InlineData("eb95c593-69b2-4483-8fc3-4f74726a317e", "New Recipe", HttpStatusCode.OK, new string[] { })]
    public async Task Update_Should_Return_Correct_StatusCode(Guid introductionId
        , string? title
        , HttpStatusCode expectedHttpStatusCode
        , string[] expectedMessages)
    {
        // Arrange
        var introductionDto = new IntroductionDto { Id = introductionId, Title = title };

        _introductionRepositoryMock.InsertAsync(Arg.Any<IntroductionDto>(), Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(introductionDto);

        // Act
        var actualApiResult = await _introductionService.UpdateAsync(introductionDto, null, new CancellationToken());

        // Assert
        actualApiResult.HttpStatusCode.Should().Be(expectedHttpStatusCode);
        actualApiResult.Messages.Count.Should().Be(expectedMessages.Length);
        foreach (var expectedMessage in expectedMessages)
            actualApiResult.Messages.Should().Contain(m => Equals(m.Message, expectedMessage));
    }
}
