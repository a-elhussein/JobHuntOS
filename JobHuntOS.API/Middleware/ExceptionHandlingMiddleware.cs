using System.Net;
using System.Text.Json;

namespace JobHuntOS.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occured");
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            KeyNotFoundException => new ErrorResponse(
                (int)HttpStatusCode.NotFound,
                "Resource not found",
                exception.Message),

            ArgumentException => new ErrorResponse(
                (int)HttpStatusCode.BadRequest,
                "Invalid request",
                exception.Message),

            _ => new ErrorResponse(
                (int)HttpStatusCode.InternalServerError,
                "An unexpected error occurred",
                "Please try again later")
        };

        context.Response.StatusCode = response.StatusCode;

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}

public record ErrorResponse(int StatusCode, string Message, string Detail);
