using ERP.Shared.Ticketing;
using ERP.Ticketing.HttpApi.Features.Ticketing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Ticketing.HttpApi.Data.Configurations;

public class TicketEntityConfiguration: IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasQueryFilter(x => x.DeletedAt == null);
        
        builder.HasIndex(x => x.DeletedAt);
        
        builder.Property(x => x.Title)
            .HasMaxLength(TicketConstants.MaxTitleLength)
            .IsRequired();

        builder.HasMany(x => x.Comments)
            .WithOne(x => x.Ticket);
        
        builder.HasMany(x => x.Users)
            .WithOne(x => x.Ticket);

        builder.HasMany(x => x.Categories)
            .WithOne(x => x.Ticket);

        builder.Property(x => x.Number)
            .ValueGeneratedOnAdd();
        
        builder.HasIndex(x => x.ShamsiCreatedAt);
        builder.Property(x => x.ShamsiCreatedAt)
            .HasMaxLength(TicketConstants.MaxShamsiLength);
    }
}