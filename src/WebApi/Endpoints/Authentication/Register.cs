using Application.Authentication.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.Authentication;

public sealed class Register : AEndpoint
{
    public sealed record RegisterAuthenticationRequest(
        Guid VerificationTokenId,
        string FirstName,
        string LastName,
        string Username
    );
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/authentication/register", async (
            [FromBody] RegisterAuthenticationRequest request,
            HttpContext http,
            [FromServices] ISender sender,
            CancellationToken cancellationToken
        ) =>
        {
            // Check that the user is auth or not
            if (http?.User?.Identity?.IsAuthenticated ?? false)
            {
                return Results.Forbid();
            }

            var command = new RegisterAuthenticationCommand(
                request.VerificationTokenId,
                request.FirstName,
                request.LastName,
                request.Username
            );

            var result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Results.Ok(result.Value);
        })
        .AllowAnonymous()
        .WithTags(Tags.Authentication)
        .WithName("Register")
        .WithSummary("Register user")
        .WithDescription("Register a new user with VerificationTokenId");
    }
}