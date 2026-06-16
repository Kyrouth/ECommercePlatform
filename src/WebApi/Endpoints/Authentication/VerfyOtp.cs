using Application.Authentication.VerifyOtp;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.Authentication;

public sealed class VerifyOtp : AEndpoint
{
    public sealed record VerifyOtpAuthenticationRequest(Guid ClientId, string Otp);
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/authentication/verify-otp", async (
            [FromBody] VerifyOtpAuthenticationRequest request,
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


            var command = new VerifyOtpAuthenticationCommand(request.ClientId, request.Otp);

            var result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Results.Ok(result.Value);
        })
        .AllowAnonymous()
        .WithTags(Tags.Authentication)
        .WithName("VerifyOtpUser")
        .WithSummary("Verify OTP for authentication")
        .WithDescription("Verifies the OTP sent to the user's phone number and returns a verification result used for authentication flow.");
    }
}