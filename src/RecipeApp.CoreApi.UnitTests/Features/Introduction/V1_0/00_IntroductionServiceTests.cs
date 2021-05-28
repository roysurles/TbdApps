using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Moq;

using RecipeApp.CoreApi.Features.Introduction.V1_0;

namespace RecipeApp.CoreApi.UnitTests.Features.Introduction.V1_0
{
    public partial class IntroductionServiceTests
    {
        protected readonly ServiceProvider _serviceProvider;
        protected readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        protected readonly ILogger<IntroductionServiceV1_0> _introductionServiceLogger;
        protected readonly Mock<IIntroductionRepositoryV1_0> _introductionRepositoryMock;
        protected readonly IIntroductionServiceV1_0 _introductionService;

        public IntroductionServiceTests(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            // TODO: Cleanup
            //_httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            //_httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());
            //_httpContextAccessorMock.Setup(x => x.HttpContext.RequestServices).Returns(_serviceProvider);

            _introductionServiceLogger = new LoggerFactory().CreateLogger<IntroductionServiceV1_0>();

            _introductionRepositoryMock = new Mock<IIntroductionRepositoryV1_0>();

            _introductionService = new IntroductionServiceV1_0(_serviceProvider
                , _introductionServiceLogger
                , _introductionRepositoryMock.Object);
        }
    }
}
