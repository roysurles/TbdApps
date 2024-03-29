﻿namespace RecipeApp.CoreApi.UnitTests.Features.Introduction.V1_0;

public partial class IntroductionServiceTests
{
    protected readonly ServiceProvider _serviceProvider;
    protected readonly ILogger<IntroductionServiceV1_0> _introductionServiceLogger;
    protected readonly Mock<IIntroductionRepositoryV1_0> _introductionRepositoryMock;
    protected readonly IIntroductionServiceV1_0 _introductionService;

    public IntroductionServiceTests(ServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        _introductionServiceLogger = new LoggerFactory().CreateLogger<IntroductionServiceV1_0>();

        _introductionRepositoryMock = new Mock<IIntroductionRepositoryV1_0>();

        _introductionService = new IntroductionServiceV1_0(_serviceProvider
            , _introductionServiceLogger
            , _introductionRepositoryMock.Object);
    }
}
