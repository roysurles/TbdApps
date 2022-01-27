using Bunit;

using Microsoft.Extensions.Logging;

using Moq;

using RecipeApp.BlazorWasmBootstrap.Features.Instruction;
using RecipeApp.BlazorWasmBootstrap.UnitTests.Shared.Extensions;

using Xunit;
using Xunit.Abstractions;

namespace RecipeApp.BlazorWasmBootstrap.UnitTests.Features.Instruction
{
    public class InstructionTableEditComponentTests : TestContext
    {
        protected readonly ITestOutputHelper _output;

        public InstructionTableEditComponentTests(ITestOutputHelper output) =>
            _output = output;

        [Fact]
        public void Should_Render()
        {
            // Arrange
            JSInterop.Mode = JSRuntimeMode.Loose;
            Services.AddDefaultServices();

            var mockInstructionApiClientV1_0 = new Mock<IInstructionApiClientV1_0>();
            var mockLogger = new Mock<ILogger<InstructionViewModel>>();

            // Act
            var cut = RenderComponent<InstructionTableEditComponent>(parameters =>
                parameters.Add(p => p.InstructionViewModel, new InstructionViewModel(mockInstructionApiClientV1_0.Object, mockLogger.Object)));

            // Assert
            var divHtml = cut.Find("div");
        }
    }
}
