using Application;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using Serilog;
using WebApi.Exceptions;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddOpenApi();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("ECommerce API")
            .ShowOperationId()
            .SortTagsAlphabetically()
            .SortOperationsByMethod();
    });
}


app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

// app.UseAuthorization();
app.UseExceptionHandler();

app.MapEndpoints();

app.Run();
