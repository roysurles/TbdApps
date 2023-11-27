namespace RecipeApp.CoreApi.Features.Ingredient.V1_0.CqrsMedDapp;

public class GetIngredientsByIntroductionIdQueryHandlerNotificationHandler : INotificationHandler<GetIngredientsByIntroductionIdQueryHandlerNotification>
{
    protected readonly ILogger<GetIngredientsByIntroductionIdQueryHandlerNotificationHandler> _logger;

    public GetIngredientsByIntroductionIdQueryHandlerNotificationHandler(ILogger<GetIngredientsByIntroductionIdQueryHandlerNotificationHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(GetIngredientsByIntroductionIdQueryHandlerNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{methodName}", nameof(Handle));

        // TODO:  interrogate notification.ApiResultModel for further processing

        return Task.CompletedTask;
    }
}
