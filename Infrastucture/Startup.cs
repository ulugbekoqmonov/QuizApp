using Application.Abstraction;
using Application.Interfaces;
using Infrastructure.DataAccess.Interceptors;
using Infrastructure.DataAccess.Repository;
using Infrastructure.Services;
using Infrastucture.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DbConnection")));
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<IUserRepository, UserRepository>();        
        services.AddScoped<IUserTokenRepository, UserTokenRepository>();
        services.AddScoped<IPermissionRepository,PermissionRepository>();
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddScoped<IDateTime,DateTimeService>();
        
        return services;
    }
}
