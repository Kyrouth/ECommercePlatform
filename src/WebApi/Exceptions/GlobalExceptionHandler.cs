using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Exceptions;

internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception occurred");

        if (httpContext.Response.HasStarted)
        {
            logger.LogWarning("Response already started");
            return false;
        }

        var (statusCode, message) = MapException(exception);

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = message
        };
        
        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

        private static (int statusCode, string message) MapException(Exception ex)
    {
        return ex switch
        {
            BadHttpRequestException =>
                (StatusCodes.Status400BadRequest, "Invalid request body"),

            System.Text.Json.JsonException =>
                (StatusCodes.Status400BadRequest, "Malformed JSON"),

            InvalidOperationException =>
                (StatusCodes.Status400BadRequest, "Invalid request"),

            _ =>
                (StatusCodes.Status500InternalServerError, "Server failure")
        };
    }
}
