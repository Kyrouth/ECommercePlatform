using Application.Users.Login;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.Users;

public sealed class Login : IEndpoint
{
    public sealed record Request(string Email, string Password);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("Test", async ([FromBody] Request request, [FromServices] ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new LoginUserCommand(request.Email, request.Password);

            if(string.IsNullOrWhiteSpace(request.Email))
            {
                return Results.BadRequest("The email can not be empty");
            }


            Result<LoginUserResponse> result = await sender.Send(command, cancellationToken);

            return Results.Ok(result.Value);
        })
        .WithTags(Tags.Users);
    }
}
