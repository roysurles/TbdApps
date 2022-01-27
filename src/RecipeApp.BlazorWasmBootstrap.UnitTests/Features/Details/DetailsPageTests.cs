using Bunit;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Moq;

using RecipeApp.BlazorWasmBootstrap.Features.Details;
using RecipeApp.BlazorWasmBootstrap.Features.Ingredient;
using RecipeApp.BlazorWasmBootstrap.Features.Instruction;
using RecipeApp.BlazorWasmBootstrap.UnitTests.Shared.Extensions;
using RecipeApp.Shared.Features.Introduction;

using Xunit;
using Xunit.Abstractions;

namespace RecipeApp.BlazorWasmBootstrap.UnitTests.Features.Details
{
    public class DetailsPageTests : TestContext
    {
        protected readonly ITestOutputHelper _output;

        public DetailsPageTests(ITestOutputHelper output) =>
            _output = output;

        [Fact]
        public void Should_Render()
        {
            // Arrange
            JSInterop.Mode = JSRuntimeMode.Loose;
            Services.AddDefaultServices();

            var mockIntroductionApiClientV1_0 = new Mock<IIntroductionApiClientV1_0>();
            var mockIntroductionViewModelLogger = new Mock<ILogger<IntroductionViewModel>>();
            Services.AddTransient<IIntroductionViewModel>(_ => new IntroductionViewModel(mockIntroductionApiClientV1_0.Object
                , mockIntroductionViewModelLogger.Object));

            var mockIngredientApiClientV1_0 = new Mock<IIngredientApiClientV1_0>();
            var mockIngredientViewModelLogger = new Mock<ILogger<IngredientViewModel>>();
            Services.AddTransient<IIngredientViewModel>(_ => new IngredientViewModel(mockIngredientApiClientV1_0.Object
                , mockIngredientViewModelLogger.Object));

            var mockInstructionApiClientV1_0 = new Mock<IInstructionApiClientV1_0>();
            var mockInstructionViewModelLogger = new Mock<ILogger<InstructionViewModel>>();
            Services.AddTransient<IInstructionViewModel>(_ => new InstructionViewModel(mockInstructionApiClientV1_0.Object
                , mockInstructionViewModelLogger.Object));

            var mockDetailsPageViewModelLogger = new Mock<ILogger<DetailsPageViewModel>>();
            Services.AddTransient<ILogger<DetailsPageViewModel>>(_ => mockDetailsPageViewModelLogger.Object);

            Services.AddTransient<IDetailsPageViewModel, DetailsPageViewModel>();

            // Act
            var cut = RenderComponent<DetailsPage>();

            // Assert
            var divHtml = cut.Find("div");
        }
    }
}
