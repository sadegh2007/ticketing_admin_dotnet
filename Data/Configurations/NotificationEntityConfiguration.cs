using ERP.Shared.Ticketing;
using ERP.Ticketing.HttpApi.Features.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Ticketing.HttpApi.Data.Configurations;

public class NotificationEntityConfiguration: IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.Property(x => x.Title)
            .HasMaxLength(TicketConstants.MaxTitleLength);

        builder.Property(x => x.Data)
            .HasColumnType("jsonb");
        
        // builder.OwnsOne(x => x.Data, ba => ba.ToJson());
    }
}