using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using FluentValidation;

namespace ExpenseTrackerApi.ExceptionHandler;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled exception. TraceId = {0}", httpContext.TraceIdentifier);

        var (status, title, detail, type) = MapException(exception);

        ProblemDetails problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path,
            Type = type

        };

        problem.Extensions["traceId"] = httpContext.TraceIdentifier;

        httpContext.Response.StatusCode = status ?? StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }

    private static (int? status, string title, string? detail, string type) MapException(Exception ex)
    {
        if (ex is KeyNotFoundException)
            return (StatusCodes.Status404NotFound, "Resource not found", ex.Message, "about:blank");

 
        if (ex is ArgumentException or ArgumentNullException or FormatException)
            return (StatusCodes.Status400BadRequest, "Bad request", ex.Message, "about:blank");

        if (ex is ValidationException fv)
        {
           
            var detail = string.Join("; ", fv.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
            return (StatusCodes.Status400BadRequest, "Validation failed", detail, "https://datatracker.ietf.org/doc/html/rfc7807");
        }

       
        if (ex is UnauthorizedAccessException)
            return (StatusCodes.Status401Unauthorized, "Unauthorized", ex.Message, "about:blank");

        
        if (ex is DbUpdateConcurrencyException)
            return (StatusCodes.Status409Conflict, "Concurrency conflict", ex.Message, "about:blank");

      
        if (ex is DbUpdateException)
            return (StatusCodes.Status500InternalServerError, "Database error", Short(ex.Message, 400), "about:blank");

        return (StatusCodes.Status500InternalServerError, "Unexpected server error", Short(ex.Message, 400), "about:blank");

        static string? Short(string? s, int max) =>
            string.IsNullOrWhiteSpace(s) ? s : (s.Length <= max ? s : s[..max] + "…");
    }
}
