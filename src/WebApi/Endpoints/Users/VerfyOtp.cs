using Application.Users.VerifyOtp;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.Users;

public sealed class VerifyOtp : AEndpoint
{
    public sealed record VerifyOtpUserRequest(Guid ClientId, string Otp);
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/users/verify-otp", async (
            [FromBody] VerifyOtpUserRequest request,
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


            var command = new VerifyOtpUserCommand(request.ClientId, request.Otp);

            var result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Results.Ok(result.Value);
        })
        .AllowAnonymous()
        .WithTags(Tags.Users, Tags.Authentication)
        .WithName("VerifyOtpUser")
        .WithSummary("Verify OTP for authentication")
        .WithDescription("Verifies the OTP sent to the user's phone number and returns a verification result used for authentication flow.");
    }
}