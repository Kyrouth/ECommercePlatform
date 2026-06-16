using Application.Authentication.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.Authentication;

public sealed class Register : AEndpoint
{
    public sealed record RegisterUserRequest(Guid ClientId, Guid VerificationTokenId);
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/authentication/register", async (
            [FromBody] RegisterUserRequest request,
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

            var command = new RegisterUserCommand(request.ClientId, request.VerificationTokenId);

            var result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Results.Ok();
        })
        .AllowAnonymous()
        .WithTags(Tags.Authentication)
        .WithName("Register")
        .WithSummary("Register user")
        .WithDescription("Register a new user with verified session");
    }
}