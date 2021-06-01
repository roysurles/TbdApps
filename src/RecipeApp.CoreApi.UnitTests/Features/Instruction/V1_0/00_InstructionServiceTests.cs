using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Moq;

using RecipeApp.CoreApi.Features.Instruction.V1_0;

namespace RecipeApp.CoreApi.UnitTests.Features.Instruction.V1_0
{
    public partial class InstructionServiceTests
    {
        protected readonly ServiceProvider _serviceProvider;
        protected readonly ILogger<InstructionServiceV1_0> _instructionServiceLogger;
        protected readonly Mock<IInstructionRepositoryV1_0> _instructionRepositoryMock;
        protected readonly IInstructionServiceV1_0 _instructionService;

        public InstructionServiceTests(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _instructionServiceLogger = new LoggerFactory().CreateLogger<InstructionServiceV1_0>();

            _instructionRepositoryMock = new Mock<IInstructionRepositoryV1_0>();

            _instructionService = new InstructionServiceV1_0(_serviceProvider
                , _instructionServiceLogger
                , _instructionRepositoryMock.Object);
        }
    }
}
