using Microsoft.AspNetCore.Mvc;

namespace MinimalApi.Api;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError("{error}", ex.ToString());

            var problemDetails = new ProblemDetails
            {
                Detail = "An error has occured, check api log file for details.",
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server error"
            };
            context.Response.StatusCode = problemDetails.Status.Value;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
