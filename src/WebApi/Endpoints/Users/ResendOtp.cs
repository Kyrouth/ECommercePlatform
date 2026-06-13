using Application.Users.ResendOtp;
using Application.Users.SendOtp;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.Users;

public sealed class ResendOtp : AEndpoint
{
    public sealed record Request(Guid ClientId);
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/users/resend-otp", async (
            [FromBody] Request request,
            HttpContext http,
            [FromServices] ISender sender,
            CancellationToken cancellationToken
        ) =>
        {
            // Check that the user is auth or not
            if(http?.User?.Identity?.IsAuthenticated ?? false)
            {
                return Results.Forbid();
            }

            var command = new ResendOtpUserCommand(request.ClientId);

            var result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Results.Ok();
        })
        .WithTags(Tags.Users); //TODO Documenting
    }
}