﻿namespace RecipeApp.CoreApi.UnitTests.Features.Instruction.V1_0;

public partial class InstructionServiceTests
{
    [Theory(DisplayName = "InstructionServiceTests.Select")]
    [InlineData("00000000-0000-0000-0000-000000000000", HttpStatusCode.BadRequest, new string[] { "Id is required." })]
    [InlineData("eb95c593-69b2-4483-8fc3-4f74726a317e", HttpStatusCode.NotFound, new string[] { "No data found." })]
    [InlineData("eb95c593-69b2-4483-8fc3-4f74726a317e", HttpStatusCode.OK, new string[] { })]
    public async Task Select_Should_Return_Correct_StatusCode(Guid id
        , HttpStatusCode expectedHttpStatusCode
        , string[] expectedMessages)
    {
        // Arrange
        var returnInstructionDto = Equals(expectedHttpStatusCode, HttpStatusCode.OK)
            ? new InstructionDto()
            : null;

        _instructionRepositoryMock
            .Setup(x => x.SelectAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnInstructionDto);

        // Act
        var actualApiResult = await _instructionService.SelectAsync(id, new CancellationToken());

        // Assert
        actualApiResult.HttpStatusCode.Should().Be(expectedHttpStatusCode);
        actualApiResult.Messages.Count.Should().Be(expectedMessages.Length);
        foreach (var expectedMessage in expectedMessages)
            actualApiResult.Messages.Should().Contain(m => Equals(m.Message, expectedMessage));
    }
}
