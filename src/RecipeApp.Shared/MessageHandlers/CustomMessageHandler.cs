﻿namespace RecipeApp.Shared.MessageHandlers;

public class CustomMessageHandler : DelegatingHandler
{
    protected readonly ISessionViewModel _sessionViewModel;

    public CustomMessageHandler(ISessionViewModel sessionViewModel) =>
        _sessionViewModel = sessionViewModel;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Remove("X-Correlation-ID");
        request.Headers.Add("X-Correlation-ID", _sessionViewModel.TraceId.ToString());
        return base.SendAsync(request, cancellationToken);
    }
}
