using EducationPlatform.Application.Abstractions.Authentication;
using EducationPlatform.Application.Abstractions.Persistence;
using EducationPlatform.Infrastructure.Options;
using EducationPlatform.Infrastructure.Persistence;
using EducationPlatform.Infrastructure.Services.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EducationPlatform.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

        var connectionString = configuration.GetConnectionString("EducationPlatform") 
                                ?? throw new InvalidOperationException("Connection string 'EducationPlatform' bulunamadı.");

        services.AddDbContext<EducationPlatformDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(typeof(EducationPlatformDbContext).Assembly.FullName);
                sqlOptions.EnableRetryOnFailure();
            });
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<EducationPlatformDbContext>());

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IPasswordHasher, Sha256PasswordHasher>();

        return services;
    }
}
