using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using PhoneBook.Core.Domain.Common;
using PhoneBook.Core.Contracts.Common.Validation;
using PhoneBook.Application.Commands.Common.Exceptions;

namespace PhoneBook.Endpoints.Api.Infrastructure;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;

    public GlobalExceptionHandler(IProblemDetailsService problemDetailsService) 
        => _problemDetailsService = problemDetailsService;

    public async ValueTask<bool> TryHandleAsync(
    HttpContext httpContext,
    Exception exception,
    CancellationToken cancellationToken)
    {
        var problemDetails = CreateProblemDetails(
            httpContext,
            exception);

        httpContext.Response.StatusCode =
            problemDetails.Status
            ?? StatusCodes.Status500InternalServerError;

        return await _problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetails,
                Exception = exception
            });
    }

    private static ProblemDetails CreateProblemDetails(
        HttpContext httpContext,
        Exception exception)
    {
        ProblemDetails problemDetails;

        switch (exception)
        {
            case ValidationException validationException:
                problemDetails = CreateValidationProblemDetails(httpContext, validationException.Errors);
                break;

            case ContactNotFoundException:
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Contact not found.",
                    Detail = exception.Message,
                    Type = "https://httpstatuses.com/404"
                };
                break;

            case DomainException:
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Domain validation failed.",
                    Detail = exception.Message,
                    Type = "https://httpstatuses.com/400"
                };
                break;

            case KeyNotFoundException:
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Resource not found.",
                    Detail = exception.Message,
                    Type = "https://httpstatuses.com/404"
                };
                break;

            case InvalidOperationException:
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "The requested operation conflicts with the current resource state.",
                    Detail = exception.Message,
                    Type = "https://httpstatuses.com/409"
                };
                break;

            default:
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "An unexpected error occurred.",
                    Detail = "An unexpected error occurred while processing the request.",
                    Type = "https://httpstatuses.com/500"
                };
                break;
        }

        problemDetails.Instance = httpContext.Request.Path;

        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

        return problemDetails;
    }

    private static ValidationProblemDetails CreateValidationProblemDetails(
        HttpContext httpContext,
        IReadOnlyList<ValidationError> errors)
    {
        var groupedErrors = errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Select(error => error.ErrorMessage)
                    .ToArray());

        var problemDetails = new ValidationProblemDetails(
            groupedErrors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation failed.",
            Type = "https://httpstatuses.com/400",
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] =
            httpContext.TraceIdentifier;

        return problemDetails;
    }
}