using ERP.Ticketing.HttpApi.Features.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Ticketing.HttpApi.Data.Configurations;

public class TenantUserEntityConfiguration: IEntityTypeConfiguration<TenantUser>
{
    public void Configure(EntityTypeBuilder<TenantUser> builder)
    {
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => x.UserId);
    }
}