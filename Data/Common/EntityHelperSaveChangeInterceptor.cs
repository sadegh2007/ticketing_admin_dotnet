using System.Globalization;
using ERP.Ticketing.HttpApi.Commons.Extensions;
using ERP.Ticketing.HttpApi.Data.Common.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ERP.Ticketing.HttpApi.Data.Common;

public class EntityHelperSaveChangeInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _contextAccessor;

    public EntityHelperSaveChangeInterceptor(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }
    
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        foreach (var entityEntry in eventData.Context?.ChangeTracker.Entries()!)
        {
            switch (entityEntry.State)
            {
                case EntityState.Added:
                {
                    // TODO: Need to implement user id
                    if (entityEntry.Entity is ICreator creator) {
                        creator.CreatorId = _contextAccessor.HttpContext!.User.GetUserId();
                    }
                    
                    if (entityEntry.Entity is ICreateTime createTime)
                        createTime.CreatedAt = DateTime.UtcNow;

                    if (entityEntry.Entity is IShamsiCreatedAt shamsiCreatedAt)
                    {
                        var persianCalendar = new PersianCalendar();
                        var today = DateTime.UtcNow;
                        
                        shamsiCreatedAt.ShamsiCreatedAt =
                            $"{persianCalendar.GetYear(today)}-{persianCalendar.GetMonth(today).ToString().PadLeft(2, '0')}-{persianCalendar.GetDayOfMonth(today).ToString().PadLeft(2, '0')} {today.Hour.ToString().PadLeft(2, '0')}:{today.Minute.ToString().PadLeft(2, '0')}";
                    }

                    if (entityEntry.Entity is ITenantable tenantable)
                    {
                        tenantable.TenantId = _contextAccessor.HttpContext!.GetTenantId()!;
                    }
                    
                    break;
                }
                case EntityState.Modified:
                {
                    if (entityEntry.Entity is IUpdateTime createTime)
                        createTime.UpdatedAt = DateTime.UtcNow;
                    break;
                }
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}