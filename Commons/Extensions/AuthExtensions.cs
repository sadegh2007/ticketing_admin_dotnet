using System.Security.Claims;
using ERP.Ticketing.HttpApi.Data.Common.Helpers;

namespace ERP.Ticketing.HttpApi.Commons.Extensions;

public static class AuthExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var userId = principal.FindFirst("sub") ?? principal.FindFirst(ClaimTypes.NameIdentifier);
        return Guid.Parse(userId.Value);
    }

    public static string? GetTenantId(this HttpContext context)
    {
        string? tenant = null;
        if (context.Request.Headers.TryGetValue("x-tenant", out var tenants))
        {
            tenant = tenants.FirstOrDefault();
        }

        return tenant;
    }

    public static IQueryable<T> FilterTenant<T>(this IQueryable<T> query, string tenantId) where T : ITenantable {
        return query.Where(x => x.TenantId == tenantId);
    }
}