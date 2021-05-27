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
        protected readonly ILogger<IntroductionV1_0Service> _introductionServiceLogger;
        protected readonly Mock<IIntroductionV1_0Repository> _introductionRepositoryMock;
        protected readonly IIntroductionV1_0Service _introductionService;

        public IntroductionServiceTests(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            // TODO: Cleanup
            //_httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            //_httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());
            //_httpContextAccessorMock.Setup(x => x.HttpContext.RequestServices).Returns(_serviceProvider);

            _introductionServiceLogger = new LoggerFactory().CreateLogger<IntroductionV1_0Service>();

            _introductionRepositoryMock = new Mock<IIntroductionV1_0Repository>();

            _introductionService = new IntroductionV1_0Service(_serviceProvider
                , _introductionServiceLogger
                , _introductionRepositoryMock.Object);
        }
    }
}
