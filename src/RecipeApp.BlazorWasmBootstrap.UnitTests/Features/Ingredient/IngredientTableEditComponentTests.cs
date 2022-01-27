using Bunit;

using Microsoft.Extensions.Logging;

using Moq;

using RecipeApp.BlazorWasmBootstrap.Features.Ingredient;
using RecipeApp.BlazorWasmBootstrap.UnitTests.Shared.Extensions;

using Xunit;
using Xunit.Abstractions;

namespace RecipeApp.BlazorWasmBootstrap.UnitTests.Features.Ingredient
{
    public class IngredientTableEditComponentTests : TestContext
    {
        protected readonly ITestOutputHelper _output;

        public IngredientTableEditComponentTests(ITestOutputHelper output) =>
            _output = output;

        [Fact]
        public void Should_Render()
        {
            // Arrange
            JSInterop.Mode = JSRuntimeMode.Loose;
            Services.AddDefaultServices();

            var mockIngredientApiClientV1_0 = new Mock<IIngredientApiClientV1_0>();
            var mockLogger = new Mock<ILogger<IngredientViewModel>>();

            // Act
            var cut = RenderComponent<IngredientTableEditComponent>(parameters =>
                parameters.Add(p => p.IngredientViewModel, new IngredientViewModel(mockIngredientApiClientV1_0.Object, mockLogger.Object)));

            // Assert
            var divHtml = cut.Find("div");
        }
    }
}
