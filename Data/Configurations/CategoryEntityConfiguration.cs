using ERP.Shared.Ticketing;
using ERP.Ticketing.HttpApi.Features.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Ticketing.HttpApi.Data.Configurations;

public class CategoryEntityConfiguration: IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(x => x.Title)
            .HasMaxLength(TicketConstants.MaxTitleLength);

        builder.HasIndex(x => x.ShamsiCreatedAt);
        builder.Property(x => x.ShamsiCreatedAt)
            .HasMaxLength(TicketConstants.MaxShamsiLength);
    }
}