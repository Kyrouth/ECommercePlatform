using Domain.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints;

public abstract class AEndpoint
{
    public abstract void MapEndpoint(IEndpointRouteBuilder app);

    public IResult HandleFailure(Result result) =>
        result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            IValidationResult validationResult => Results.BadRequest(CreateProblemDetails(
                "Validation Error",
                StatusCodes.Status400BadRequest,
                result.Error,
                validationResult.Errors
            )),
            { Error.Type: ErrorType.NotFound } => Results.NotFound(CreateProblemDetails(
                "Not Found",
                StatusCodes.Status404NotFound,
                result.Error
            )),
            { Error.Type: ErrorType.Conflict } => Results.Conflict(CreateProblemDetails(
                "Conflict",
                StatusCodes.Status409Conflict,
                result.Error
            )),
            _ => Results.BadRequest(CreateProblemDetails(
                "Bad Request",
                StatusCodes.Status400BadRequest,
                result.Error
            ))
        };


    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error,
        Error[]? errors = null
    ) => new()
    {
        Title = title,
        Type = error.Code,
        Detail = error.Description,
        Status = status,
        Extensions = { { nameof(errors), errors } },
    };


}