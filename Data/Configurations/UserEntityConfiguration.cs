using ERP.Ticketing.HttpApi.Features.Tenants;
using ERP.Ticketing.HttpApi.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Ticketing.HttpApi.Data.Configurations;

public class UserEntityConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasQueryFilter(x => x.DeletedAt == null);
        
        // builder.HasMany(x => x.Roles)
        //     .WithMany(x => x.Users);
        
        // builder.HasMany(x => x.Tenants)
        //     .WithMany(x => x.Users)
        //     .UsingEntity<TenantUser>();
    }
}