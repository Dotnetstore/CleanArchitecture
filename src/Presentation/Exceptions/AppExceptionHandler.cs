using Microsoft.AspNetCore.Diagnostics;

namespace Presentation.Exceptions;

public class AppExceptionHandler : IExceptionHandler
{
    private readonly ILogger<AppExceptionHandler> _logger;

    public AppExceptionHandler(ILogger<AppExceptionHandler> logger)
    {
        _logger = logger;
    }
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        (int statusCode, string errorMessage) = exception switch
        {
            NotImplementedException notImplementedException => (500, notImplementedException.Message),
            _ => (500, "Unknown exception occurred.")
        };

        _logger.LogError(exception, exception.Message);
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(errorMessage, cancellationToken: cancellationToken);

        return true;
    }
}