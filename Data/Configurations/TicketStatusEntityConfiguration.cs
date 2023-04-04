using ERP.Shared.Ticketing;
using ERP.Ticketing.HttpApi.Features.Ticketing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Ticketing.HttpApi.Data.Configurations;

public class TicketStatusEntityConfiguration: IEntityTypeConfiguration<TicketStatus>
{
    public void Configure(EntityTypeBuilder<TicketStatus> builder)
    {
        builder.Property(x => x.Title)
            .HasMaxLength(TicketConstants.MaxTitleLength)
            .IsRequired();
        
        builder.Property(x => x.Name)
            .HasMaxLength(TicketStatusConstants.MaxNameLength)
            .IsRequired();
        
        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}