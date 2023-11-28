namespace RecipeApp.CoreApi.UnitTests.Features.Instruction.V1_0;

public partial class InstructionServiceTests
{
    [Theory(DisplayName = "InstructionServiceTests.SelectAllForIntroductionIdAsync")]
    [InlineData("00000000-0000-0000-0000-000000000000", HttpStatusCode.BadRequest, new string[] { "Introduction Id is required." })]
    [InlineData("eb95c593-69b2-4483-8fc3-4f74726a317e", HttpStatusCode.OK, new string[] { "No data found." })]
    [InlineData("eb95c593-69b2-4483-8fc3-4f74726a317e", HttpStatusCode.OK, new string[] { })]
    public async Task SelectAllForIntroductionIdAsync_Should_Return_Correct_StatusCode(Guid id
        , HttpStatusCode expectedHttpStatusCode
        , string[] expectedMessages)
    {
        // Arrange
        var returnInstructionDto = Equals(expectedHttpStatusCode, HttpStatusCode.OK) && expectedMessages.Length.Equals(0)
            ? new List<InstructionDto> { new InstructionDto() }
            : new List<InstructionDto>();

        _instructionRepositoryMock
            .Setup(x => x.SelectAllForIntroductionIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnInstructionDto);

        // Act
        var actualApiResult = await _instructionService.SelectAllForIntroductionIdAsync(id, new CancellationToken());

        // Assert
        actualApiResult.HttpStatusCode.Should().Be(expectedHttpStatusCode);
        actualApiResult.Messages.Count.Should().Be(expectedMessages.Length);
        foreach (var expectedMessage in expectedMessages)
            actualApiResult.Messages.Should().Contain(m => Equals(m.Message, expectedMessage));
    }
}
