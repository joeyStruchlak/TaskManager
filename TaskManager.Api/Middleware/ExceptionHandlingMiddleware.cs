using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace TaskManager.Api.Middleware;

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
        catch (ValidationException ex)
        {
            _logger.LogWarning("Validation failed: {Errors}", ex.Message);
            await HandleValidationException(context, ex);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Resource not found: {Message}", ex.Message);
            await HandleProblemDetails(context, StatusCodes.Status404NotFound, "Not Found", ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Business rule violation: {Message}", ex.Message);
            await HandleProblemDetails(context, StatusCodes.Status409Conflict, "Conflict", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await HandleProblemDetails(context, StatusCodes.Status500InternalServerError, "Internal Server Error", "An unexpected error occurred.");
        }
    }

    // RFC 7807 Problem Details — the industry standard error response format
    private static async Task HandleProblemDetails(HttpContext context, int statusCode, string title, string detail)
    {
        if (context.Response.HasStarted) return;

        var problem = new ProblemDetails
        {
            Type = $"https://httpstatuses.com/{statusCode}",
            Title = title,
            Status = statusCode,
            Detail = detail,
            Instance = context.Request.Path
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }

    // Validation gets special treatment — multiple errors at once
    private static async Task HandleValidationException(HttpContext context, ValidationException ex)
    {
        if (context.Response.HasStarted) return;

        var problem = new ValidationProblemDetails(
            ex.Errors
              .GroupBy(e => e.PropertyName)
              .ToDictionary(
                  g => g.Key,
                  g => g.Select(e => e.ErrorMessage).ToArray()
              ))
        {
            Type = "https://httpstatuses.com/400",
            Title = "Validation Failed",
            Status = StatusCodes.Status400BadRequest,
            Instance = context.Request.Path
        };

        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}