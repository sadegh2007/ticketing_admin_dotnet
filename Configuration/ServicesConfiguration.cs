using ERP.Ticketing.HttpApi.Commons.Middlewares;
using ERP.Ticketing.HttpApi.Commons.Providers;
using ERP.Ticketing.HttpApi.Features.Categories;
using ERP.Ticketing.HttpApi.Features.Dashboard;
using ERP.Ticketing.HttpApi.Features.Departments;
using ERP.Ticketing.HttpApi.Features.Notifications;
using ERP.Ticketing.HttpApi.Features.Tenants;
using ERP.Ticketing.HttpApi.Features.Ticketing;
using ERP.Ticketing.HttpApi.Features.Users;
using ERP.Ticketing.HttpApi.Services;
using ERP.Ticketing.HttpApi.Services.Smslr;
using Microsoft.AspNetCore.Authorization;

namespace ERP.Ticketing.HttpApi.Configuration;

public static class ServicesConfiguration
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<ISmsSenderService, SmsSenderService>();
        services.AddScoped<UserService>();
        services.AddScoped<TicketingService>();
        services.AddScoped<TicketingCommentService>();
        services.AddScoped<TicketingUserService>();
        services.AddScoped<DepartmentService>();
        services.AddScoped<CategoryService>();
        services.AddScoped<DashboardService>();
        services.AddScoped<NotificationService>();
        services.AddScoped<RoleService>();
        services.AddScoped<PermissionService>();
        services.AddScoped<TenantService>();

        services.AddSingleton<UserConnectionService>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationProvider>();

        return services;
    }
}