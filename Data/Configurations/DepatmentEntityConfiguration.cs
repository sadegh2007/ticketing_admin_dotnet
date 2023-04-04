using ERP.Shared.Ticketing;
using ERP.Ticketing.HttpApi.Features.Departments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Ticketing.HttpApi.Data.Configurations;

public class DepatmentEntityConfiguration: IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.Property(x => x.Title)
            .HasMaxLength(TicketConstants.MaxTitleLength)
            .IsRequired();
        
        builder.HasIndex(x => x.ShamsiCreatedAt);
        builder.Property(x => x.ShamsiCreatedAt)
            .HasMaxLength(TicketConstants.MaxShamsiLength);
    }
}