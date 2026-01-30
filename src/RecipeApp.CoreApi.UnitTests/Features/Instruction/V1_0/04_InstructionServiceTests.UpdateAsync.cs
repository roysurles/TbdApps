namespace RecipeApp.CoreApi.UnitTests.Features.Instruction.V1_0;

public partial class InstructionServiceTests : IClassFixture<TestClassFixture>
{
    [Theory(DisplayName = "InstructionServiceTests.Update")]
    [InlineData("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000", null, HttpStatusCode.BadRequest, new string[] { "Introduction Id is required." })]
    [InlineData("eb95c593-69b2-4483-8fc3-4f74726a317e", "00000000-0000-0000-0000-000000000000", null, HttpStatusCode.BadRequest, new string[] { "Id is required." })]
    [InlineData("eb95c593-69b2-4483-8fc3-4f74726a317e", "eb95c593-69b2-4483-8fc3-4f74726a317e", null, HttpStatusCode.BadRequest, new string[] { "The Description field is required." })]
    [InlineData("eb95c593-69b2-4483-8fc3-4f74726a317e", "eb95c593-69b2-4483-8fc3-4f74726a317e", "", HttpStatusCode.BadRequest, new string[] { "The Description field is required." })]
    [InlineData("eb95c593-69b2-4483-8fc3-4f74726a317e", "eb95c593-69b2-4483-8fc3-4f74726a317e", "Description", HttpStatusCode.OK, new string[] { })]
    public async Task Update_Should_Return_Correct_StatusCode(Guid introductionId
        , Guid instructionId
        , string description
        , HttpStatusCode expectedHttpStatusCode
        , string[] expectedMessages)
    {
        // Arrange
        var instructionDto = new InstructionDto { Id = instructionId, IntroductionId = introductionId, Description = description };

        _instructionRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<InstructionDto>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(instructionDto);

        // Act
        var actualApiResult = await _instructionService.UpdateAsync(instructionDto, null, new CancellationToken());

        // Assert
        actualApiResult.HttpStatusCode.Should().Be(expectedHttpStatusCode);
        actualApiResult.Messages.Count.Should().Be(expectedMessages.Length);
        foreach (var expectedMessage in expectedMessages)
            actualApiResult.Messages.Should().Contain(m => Equals(m.Message, expectedMessage));
    }
}
