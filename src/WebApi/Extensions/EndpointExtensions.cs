using System.Reflection;
using WebApi.Endpoints;

namespace WebApi.Extensions;

public static class EndpointExtensions
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpointType = typeof(IEndpoint);

        var endpoints = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(x =>
                endpointType.IsAssignableFrom(x) &&
                !x.IsInterface &&
                !x.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<IEndpoint>();

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(app);
        }
    }
}