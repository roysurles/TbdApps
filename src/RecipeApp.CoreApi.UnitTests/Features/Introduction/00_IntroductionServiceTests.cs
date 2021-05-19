using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Moq;

using RecipeApp.CoreApi.Features.Introduction;

namespace RecipeApp.CoreApi.UnitTests.Features.Introduction
{
    public partial class IntroductionServiceTests
    {
        protected readonly ServiceProvider _serviceProvider;
        protected readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        protected readonly ILogger<IntroductionService> _introductionServiceLogger;
        protected readonly Mock<IIntroductionRepository> _introductionRepositoryMock;
        protected readonly IIntroductionService _introductionService;

        public IntroductionServiceTests(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            // TODO: Cleanup
            //_httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            //_httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());
            //_httpContextAccessorMock.Setup(x => x.HttpContext.RequestServices).Returns(_serviceProvider);

            _introductionServiceLogger = new LoggerFactory().CreateLogger<IntroductionService>();

            _introductionRepositoryMock = new Mock<IIntroductionRepository>();

            _introductionService = new IntroductionService(_serviceProvider
                , _introductionServiceLogger
                , _introductionRepositoryMock.Object);
        }
    }
}
