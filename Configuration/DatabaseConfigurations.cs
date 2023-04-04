using ERP.Ticketing.HttpApi.Data;
using ERP.Ticketing.HttpApi.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace ERP.Ticketing.HttpApi.Configuration;

public static class DatabaseConfigurations
{
    public static IServiceCollection AddAppDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<AppDbContext>((provider, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Default")!);
            options.AddInterceptors(
                new EntityHelperSaveChangeInterceptor(provider.GetRequiredService<IHttpContextAccessor>()));
        });

        return services;
    }

    public static IApplicationBuilder UseDbMigrate(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        appDb.Database.Migrate();

        return app;
    }
}