namespace RecipeApp.CoreApi.UnitTests.Features.Introduction.V1_0;

public partial class IntroductionServiceTests
{
    [Theory(DisplayName = "IntroductionServiceTests.Delete")]
    [InlineData("00000000-0000-0000-0000-000000000000", HttpStatusCode.BadRequest, "Id is required.")]
    [InlineData("eb95c593-69b2-4483-8fc3-4f74726a317e", HttpStatusCode.OK, null)]
    public async Task Delete_Should_Return_Correct_StatusCode(Guid id
        , HttpStatusCode expectedHttpStatusCode
        , string expectedMessage)
    {
        // Arrange
        _introductionRepositoryMock
            .Setup(x => x.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var actualApiResult = await _introductionService.DeleteAsync(id, new CancellationToken());

        // Assert
        actualApiResult.HttpStatusCode.Should().Be(expectedHttpStatusCode);
        if (actualApiResult.Messages.Count > 0)
            actualApiResult.Messages[0].Message.Should().Be(expectedMessage);
    }
}
