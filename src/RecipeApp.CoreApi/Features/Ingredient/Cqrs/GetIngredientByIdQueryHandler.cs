namespace RecipeApp.CoreApi.Features.Ingredient.Cqrs;

public class GetIngredientByIdQueryHandler : IRequestHandler<GetIngredientByIdQuery, IApiResultModel<IngredientDto>>
{
    protected readonly IServiceProvider _serviceProvider;
    protected readonly IMediator _mediator;
    protected readonly ILogger<GetIngredientByIdQueryHandler> _logger;
    protected readonly IIngredientRepositoryV1_0 _ingredientRepository;

    public GetIngredientByIdQueryHandler(IServiceProvider serviceProvider
        , IMediator mediator
        , ILogger<GetIngredientByIdQueryHandler> logger
        , IIngredientRepositoryV1_0 ingredientRepository)
    {
        _serviceProvider = serviceProvider;
        _mediator = mediator;
        _logger = logger;
        _ingredientRepository = ingredientRepository;
    }

    public async Task<IApiResultModel<IngredientDto>> Handle(GetIngredientByIdQuery request, CancellationToken cancellationToken)
    {
        var memberName = $"{nameof(GetIngredientByIdQueryHandler)}.{nameof(Handle)}";
        _logger.LogInformation("{methodName}({id})", nameof(GetIngredientByIdQueryHandler), request.Id);

        var apiResult = _serviceProvider.CreateApiResultModel<IngredientDto>();

        _ = request.Id == Guid.Empty
            ? apiResult.SetHttpStatusCode(HttpStatusCode.BadRequest)
                .AddErrorMessage("Id is required.", memberName, HttpStatusCode.BadRequest)
            : apiResult.SetHttpStatusCode(HttpStatusCode.OK)
                .SetData(await _ingredientRepository.SelectAsync(request.Id, cancellationToken))
                .VerifyDataIsNotNull(ApiResultMessageModelTypeEnumeration.Error, source: $"{memberName}");

        await _mediator.Publish(new GetIngredientByIdQueryHandlerNotification { ApiResultModel = apiResult }, cancellationToken);

        return apiResult;
    }
}
