using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PropertyPro.Application.Common.Exceptions;

namespace PropertyPro.Api.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "An unhandled exception occurred.");

        var statusCode = exception switch
        {
            ValidationException =>
                StatusCodes.Status400BadRequest,

            NotFoundException =>
                StatusCodes.Status404NotFound,

            ConflictException =>
                StatusCodes.Status409Conflict,

            UnauthorizedException =>
                StatusCodes.Status401Unauthorized,

            _ =>
                StatusCodes.Status500InternalServerError
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,

            Title = exception switch
            {
                ValidationException =>
                    "Validation Failed",

                NotFoundException =>
                    "Resource Not Found",

                ConflictException =>
                    "Conflict",

                UnauthorizedException =>
                    "Unauthorized",

                _ =>
                    "An unexpected error occurred."
            },

            Detail = statusCode == 500
                ? "An unexpected error occurred."
                : exception.Message
        };

        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions["errors"] =
                validationException.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        group => group.Key,
                        group => group
                            .Select(x => x.ErrorMessage)
                            .ToArray());
        }

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}