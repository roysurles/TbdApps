using FluentAssertions;

using Moq;

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Xunit;

namespace RecipeApp.CoreApi.UnitTests.Features.Ingredient.V1_0
{
    public partial class IngredientServiceTests
    {
        [Theory(DisplayName = "IngredientServiceTests.Delete")]
        [InlineData("00000000-0000-0000-0000-000000000000", HttpStatusCode.BadRequest, new string[] { "Id is required." })]
        [InlineData("eb95c593-69b2-4483-8fc3-4f74726a317e", HttpStatusCode.OK, new string[] { })]
        public async Task Delete_Should_Return_Correct_StatusCode(Guid id
            , HttpStatusCode expectedHttpStatusCode
            , string[] expectedMessages)
        {
            // Arrange
            _ingredientRepositoryMock
                .Setup(x => x.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var actualApiResult = await _ingredientService.DeleteAsync(id, new CancellationToken()).ConfigureAwait(false);

            // Assert
            actualApiResult.HttpStatusCode.Should().Be(expectedHttpStatusCode);
            actualApiResult.Messages.Count.Should().Be(expectedMessages.Length);
            foreach (var expectedMessage in expectedMessages)
                actualApiResult.Messages.Should().Contain(m => Equals(m.Message, expectedMessage));
        }
    }
}
