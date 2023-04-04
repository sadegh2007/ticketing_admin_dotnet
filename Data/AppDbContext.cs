using ERP.Ticketing.HttpApi.Features.Categories;
using ERP.Ticketing.HttpApi.Features.Departments;
using ERP.Ticketing.HttpApi.Features.Notifications;
using ERP.Ticketing.HttpApi.Features.Tenants;
using ERP.Ticketing.HttpApi.Features.Ticketing;
using ERP.Ticketing.HttpApi.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ERP.Ticketing.HttpApi.Data;

public class AppDbContext : DbContext
{
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantUser> TenantUsers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketCategory> TicketCategories { get; set; }
    public DbSet<TicketUser> TicketUsers { get; set; }
    public DbSet<TicketUserHistory> TicketUserHistories { get; set; }
    public DbSet<TicketDepartmentHistory> TicketDepartmentHistories { get; set; }
    public DbSet<TicketComment> TicketComments { get; set; }
    public DbSet<TicketCommentSeen> TicketCommentSeens { get; set; }
    public DbSet<TicketStatus> TicketStatuses { get; set; }
    public DbSet<TicketStatusHistory> TicketStatusHistories { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<DepartmentUser> DepartmentUsers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    private const string Cs = "Server=localhost;Port=5432;Database=db-ticketing;User Id=;Password=;Include Error Detail=True";

    public AppDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseNpgsql(Cs);

        return new AppDbContext(builder.Options);
    }
}