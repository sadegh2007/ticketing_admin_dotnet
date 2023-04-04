using ERP.Shared.Ticketing;
using ERP.Ticketing.HttpApi.Features.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Ticketing.HttpApi.Data.Configurations;

public class TenantEntityConfiguration: IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasIndex(x => x.DeletedAt);

        builder.Property(x => x.Title)
            .HasMaxLength(TicketConstants.MaxTitleLength);
        
        builder.Property(x => x.Id)
            .HasMaxLength(TicketConstants.MaxTitleLength);
        
        // builder.HasMany(x => x.Users)
        //     .WithMany(x => x.Tenants);

        // builder.HasOne(x => x.Creator);
        // builder.HasOne(x => x.DeletedBy);
        
        builder.HasQueryFilter(x => x.DeletedAt == null);
    }
}