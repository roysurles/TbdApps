
using System.Net;
using System.Threading.Tasks;

using FluentAssertions;

using Moq;

using RecipeApp.Shared.Features.Introduction;

using Xunit;

namespace RecipeApp.CoreApi.UnitTests.Features.Introduction.V1_0
{
    public partial class IntroductionServiceTests
    {
        [Theory(DisplayName = "IntroductionServiceTests.Insert")]
        [InlineData(null, HttpStatusCode.BadRequest, new string[] { "The Title field is required." })]
        [InlineData("", HttpStatusCode.BadRequest, new string[] { "The Title field is required." })]
        [InlineData("New Recipe", HttpStatusCode.Created, new string[] { })]
        public async Task Insert_Should_Return_Correct_StatusCode(string title
            , HttpStatusCode expectedHttpStatusCode
            , string[] expectedMessages)
        {
            // Arrange
            var introductionDto = new IntroductionDto { Title = title };

            _introductionRepositoryMock
                .Setup(x => x.InsertAsync(It.IsAny<IntroductionDto>(), It.IsAny<string>()))
                .ReturnsAsync(introductionDto);

            // Act
            var actualApiResult = await _introductionService.InsertAsync(introductionDto, null).ConfigureAwait(false);

            // Assert
            actualApiResult.HttpStatusCode.Should().Be(expectedHttpStatusCode);
            actualApiResult.Messages.Count.Should().Be(expectedMessages.Length);
            foreach (var expectedMessage in expectedMessages)
                actualApiResult.Messages.Should().Contain(m => Equals(m.Message, expectedMessage));
        }
    }
}
