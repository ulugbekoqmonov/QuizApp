using Application.Interfaces;
using WebUI.Services;

namespace WebUI;

public static class Startup
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService,CurrentUserService>();
        services.AddHttpContextAccessor();
        services.AddControllersWithViews();
        services.AddRazorPages();

        return services;
    }
}
