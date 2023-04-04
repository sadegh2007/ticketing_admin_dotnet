using ERP.Ticketing.HttpApi.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Ticketing.HttpApi.Data.Configurations;

public class RoleConfiguration: IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasMany(x => x.Permissions)
            .WithMany(x => x.Roles);
        
        // builder.HasMany(x => x.Users)
        //     .WithMany(x => x.Roles);
    }
}