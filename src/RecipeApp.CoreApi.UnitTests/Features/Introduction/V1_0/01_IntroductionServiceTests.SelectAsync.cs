
using System;
using System.Net;
using System.Threading.Tasks;

using FluentAssertions;

using Moq;

using RecipeApp.Shared.Features.Introduction;

namespace RecipeApp.CoreApi.UnitTests.Features.Introduction.V1_0
{
    // TODO:  Get this working....
    public partial class IntroductionServiceTests
    {
        [Xunit.Theory(DisplayName = "IntroductionServiceTests.Select")]
        [Xunit.InlineData("00000000-0000-0000-0000-000000000000", HttpStatusCode.BadRequest, "Id is required.")]
        [Xunit.InlineData("eb95c593-69b2-4483-8fc3-4f74726a317e", HttpStatusCode.NotFound, "No data found.")]
        [Xunit.InlineData("eb95c593-69b2-4483-8fc3-4f74726a317e", HttpStatusCode.OK, null)]
        public async Task Select_Should_Return_Correct_StatusCode(Guid id
            , HttpStatusCode expectedHttpStatusCode
            , string expectedMessage)
        {
            // Arrange
            var returnIntroductionDto = expectedHttpStatusCode == HttpStatusCode.OK
                ? new IntroductionDto()
                : null;

            _introductionRepositoryMock
                .Setup(x => x.SelectAsync(It.IsAny<Guid>()))
                .ReturnsAsync(returnIntroductionDto);

            // Act
            var actualApiResult = await _introductionService.SelectAsync(id).ConfigureAwait(false);

            // Assert
            actualApiResult.HttpStatusCode.Should().Be(expectedHttpStatusCode);
            if (actualApiResult.Messages.Count > 0)
                actualApiResult.Messages[0].Message.Should().Be(expectedMessage);
        }
    }
}
