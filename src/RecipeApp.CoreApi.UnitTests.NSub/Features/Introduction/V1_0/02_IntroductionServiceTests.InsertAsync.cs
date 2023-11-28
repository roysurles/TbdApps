namespace RecipeApp.CoreApi.UnitTests.NSub.Features.Introduction.V1_0;

public partial class IntroductionServiceTests
{
    [Theory(DisplayName = "IntroductionServiceTests.Insert")]
    [InlineData(null, HttpStatusCode.BadRequest, new string[] { "The Title field is required." })]
    [InlineData("", HttpStatusCode.BadRequest, new string[] { "The Title field is required." })]
    [InlineData("New Recipe", HttpStatusCode.Created, new string[] { })]
    public async Task Insert_Should_Return_Correct_StatusCode(string? title
        , HttpStatusCode expectedHttpStatusCode
        , string[] expectedMessages)
    {
        // Arrange
        var introductionDto = new IntroductionDto { Title = title };

        _introductionRepositoryMock.InsertAsync(Arg.Any<IntroductionDto>(), Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(introductionDto);

        // Act
        var actualApiResult = await _introductionService.InsertAsync(introductionDto, null, new CancellationToken());

        // Assert
        actualApiResult.HttpStatusCode.Should().Be(expectedHttpStatusCode);
        actualApiResult.Messages.Count.Should().Be(expectedMessages.Length);
        foreach (var expectedMessage in expectedMessages)
            actualApiResult.Messages.Should().Contain(m => Equals(m.Message, expectedMessage));
    }
}
