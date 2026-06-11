using Application.Users.SendOtp;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.Users;

public sealed class SendOtp : AEndpoint
{
    public sealed record Request(Guid DeviceId, string PhoneNumber);
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/users/send-otp", async ([FromBody] Request request, [FromServices] ISender sender, CancellationToken cancellationToken) =>
        {
         // Check that the user is auth or not   

            var command = new SendOtpUserCommand(request.DeviceId, request.PhoneNumber);

            var result = await sender.Send(command, cancellationToken);

            if(result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Results.Ok(result);
        });
    }
}