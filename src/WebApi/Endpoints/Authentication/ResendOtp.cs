using Application.Authentication.ResendOtp;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.Authentication;

public sealed class ResendOtp : AEndpoint
{
    public sealed record ResendOtpAuthenticationRequest(Guid ClientId);
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/authentication/resend-otp", async (
            [FromBody] ResendOtpAuthenticationRequest request,
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

            var command = new ResendOtpAuthenticationCommand(request.ClientId);

            var result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Results.Ok();
        })
        .AllowAnonymous()
        .WithTags(Tags.Authentication)
        .WithName("ResendOtp")
        .WithSummary("Resend OTP code")
        .WithDescription("Resend a new one-time password (OTP) if the previous one has expired or is no longer valid.");
    }
}