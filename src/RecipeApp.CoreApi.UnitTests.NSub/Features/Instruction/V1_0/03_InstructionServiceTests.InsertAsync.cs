namespace RecipeApp.CoreApi.UnitTests.NSub.Features.Instruction.V1_0;

public partial class InstructionServiceTests
{
    [Theory(DisplayName = "InstructionServiceTests.Insert")]
    [InlineData("00000000-0000-0000-0000-000000000000", null, HttpStatusCode.BadRequest, new string[] { "Introduction Id is required." })]
    [InlineData("eb95c593-69b2-4483-8fc3-4f74726a317e", null, HttpStatusCode.BadRequest, new string[] { "The Description field is required." })]
    [InlineData("eb95c593-69b2-4483-8fc3-4f74726a317e", "", HttpStatusCode.BadRequest, new string[] { "The Description field is required." })]
    [InlineData("eb95c593-69b2-4483-8fc3-4f74726a317e", "Description", HttpStatusCode.Created, new string[] { })]
    public async Task Insert_Should_Return_Correct_StatusCode(Guid introductionId
        , string? description
        , HttpStatusCode expectedHttpStatusCode
        , string[] expectedMessages)
    {
        // Arrange
        var instructionDto = new InstructionDto { IntroductionId = introductionId, Description = description };

        _instructionRepositoryMock.InsertAsync(Arg.Any<InstructionDto>(), Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(instructionDto);

        // Act
        var actualApiResult = await _instructionService.InsertAsync(instructionDto, null, new CancellationToken());

        // Assert
        actualApiResult.HttpStatusCode.Should().Be(expectedHttpStatusCode);
        actualApiResult.Messages.Count.Should().Be(expectedMessages.Length);
        foreach (var expectedMessage in expectedMessages)
            actualApiResult.Messages.Should().Contain(m => Equals(m.Message, expectedMessage));
    }
}
