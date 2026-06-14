using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.Messaging;
using Application.Common.Interfaces.Repositories;
using Infrastructure.Authentication;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserDeviceRepository, UserDeviceRepository>();
        services.AddScoped<IPhoneVerificationSessionRepository, PhoneVerificationSessionRepository>();

        var secret = configuration["Otp:Secret"];

        if (string.IsNullOrWhiteSpace(secret))
            throw new Exception("OTP Secret is missing");

        services.AddSingleton<IOtpHasher>(new HmacOtpHasher(secret));

        services.AddSingleton<IMessageSender, ConsoleSmsSender>();


        services.AddSingleton<IClockProvider, SystemClock>();

        return services;
    }
}