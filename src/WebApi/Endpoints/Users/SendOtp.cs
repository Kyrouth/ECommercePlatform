using Application.Users.SendOtp;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.Users;

public sealed class SendOtp : AEndpoint
{
    public sealed record SendOtpUserRequest(Guid ClientId, string PhoneNumber);
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/users/send-otp", async (
            [FromBody] SendOtpUserRequest request,
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

            var userAgent = http?.Request.Headers.UserAgent.ToString();

            var ipAddress = http?.Connection.RemoteIpAddress?.ToString();

            var command = new SendOtpUserCommand(request.ClientId, request.PhoneNumber, userAgent, ipAddress);

            var result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Results.Ok();
        })
        .AllowAnonymous()
        .WithTags(Tags.Users, Tags.Authentication)
        .WithName("SendOtpUser")
        .WithSummary("Send OTP for authentication")
        .WithDescription("Sends a one-time password (OTP) to the provided phone number for authentication or registration flow.");
    }
}