using MediatR;

using Microsoft.Extensions.Logging;

using RecipeApp.CoreApi.Features.Ingredient.Cqrs;

using System.Threading;
using System.Threading.Tasks;

namespace RecipeApp.CoreApi.Features.Ingredient.V1_0.CqrsMedDapp
{
    public class GetIngredientByIdQueryHandlerNotificationHandler : INotificationHandler<GetIngredientByIdQueryHandlerNotification>
    {
        protected readonly ILogger<GetIngredientByIdQueryHandlerNotificationHandler> _logger;

        public GetIngredientByIdQueryHandlerNotificationHandler(ILogger<GetIngredientByIdQueryHandlerNotificationHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(GetIngredientByIdQueryHandlerNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{methodName}", nameof(Handle));

            // TODO:  interrogate notification.ApiResultModel for further processing

            return Task.CompletedTask;
        }
    }
}
