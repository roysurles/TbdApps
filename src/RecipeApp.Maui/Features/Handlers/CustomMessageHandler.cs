namespace RecipeApp.Maui.Features.Handlers;

public class CustomMessageHandler : DelegatingHandler
{
    // TODO:  ISessionViewModel
    //protected readonly ISessionViewModel _sessionViewModel;

    //public CustomMessageHandler(ISessionViewModel sessionViewModel) =>
    //    _sessionViewModel = sessionViewModel;

    public CustomMessageHandler()
    {

    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // TODO:  need session instead of Guid.NewGuid()
        request.Headers.Remove("X-Correlation-ID");
        request.Headers.Add("X-Correlation-ID", Guid.NewGuid().ToString());
        //request.Headers.Add("X-Correlation-ID", _sessionViewModel.TraceId.ToString());
        return base.SendAsync(request, cancellationToken);
    }
}
