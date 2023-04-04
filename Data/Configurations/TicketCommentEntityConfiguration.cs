using ERP.Shared.Ticketing;
using ERP.Ticketing.HttpApi.Features.Ticketing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Ticketing.HttpApi.Data.Configurations;

public class TicketCommentEntityConfiguration: IEntityTypeConfiguration<TicketComment>
{
    public void Configure(EntityTypeBuilder<TicketComment> builder)
    {
        builder.HasQueryFilter(x => x.DeletedAt == null);
        
        builder.HasIndex(x => x.DeletedAt);
        
        builder.HasIndex(x => x.ShamsiCreatedAt);
        builder.Property(x => x.ShamsiCreatedAt)
            .HasMaxLength(TicketConstants.MaxShamsiLength);
    }
}