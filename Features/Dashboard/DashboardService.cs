using System.Globalization;
using ERP.Shared.Dashboard;
using ERP.Ticketing.HttpApi.Commons.Extensions;
using ERP.Ticketing.HttpApi.Data;
using Microsoft.EntityFrameworkCore;

namespace ERP.Ticketing.HttpApi.Features.Dashboard;

public class DashboardService
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _contextAccessor;

    public DashboardService(AppDbContext dbContext, IHttpContextAccessor contextAccessor)
    {
        _dbContext = dbContext;
        _contextAccessor = contextAccessor;
    }

    public async Task<DashboardInfoDto> Info(CancellationToken cancellationToken = default)
    {
        var info = new DashboardInfoDto
        {
            TicketsCount = await _dbContext.Tickets.Where(x => x.TenantId == _contextAccessor.HttpContext!.GetTenantId()).IgnoreQueryFilters().CountAsync(cancellationToken),
            DeletedTicketsCount = await _dbContext.Tickets.Where(x => x.TenantId == _contextAccessor.HttpContext!.GetTenantId()).IgnoreQueryFilters().CountAsync(x => x.DeletedAt != null, cancellationToken),
            TicketCommentsCount = await _dbContext.TicketComments.Where(x => x.Ticket.TenantId == _contextAccessor.HttpContext!.GetTenantId()).IgnoreQueryFilters().CountAsync(cancellationToken),
            DeletedTicketCommentsCount = await _dbContext.TicketComments.Where(x => x.Ticket.TenantId == _contextAccessor.HttpContext!.GetTenantId()).IgnoreQueryFilters().CountAsync(x => x.DeletedAt != null, cancellationToken),
            CommentsCountInYearChart = await CommentInYearChart(cancellationToken),
        };

        return info;
    }

    public async Task<ICollection<int>> CommentInYearChart(CancellationToken cancellationToken = default)
    {
        var persianCalendar = new PersianCalendar();
        var currentYear = persianCalendar.GetYear(DateTime.Now).ToString();
        
        var result = new Dictionary<string, int>();
        result.Add($"{currentYear}-01", 0);
        result.Add($"{currentYear}-02", 0);
        result.Add($"{currentYear}-03", 0);
        result.Add($"{currentYear}-04", 0);
        result.Add($"{currentYear}-05", 0);
        result.Add($"{currentYear}-06", 0);
        result.Add($"{currentYear}-07", 0);
        result.Add($"{currentYear}-08", 0);
        result.Add($"{currentYear}-09", 0);
        result.Add($"{currentYear}-10", 0);
        result.Add($"{currentYear}-11", 0);
        result.Add($"{currentYear}-12", 0);

        var comments = await _dbContext.TicketComments
            .Where(x => x.ShamsiCreatedAt.StartsWith(currentYear))
            .Where(x => x.Ticket.TenantId == _contextAccessor.HttpContext!.GetTenantId())
            .GroupBy(x => x.ShamsiCreatedAt.Substring(0, 7))
            .Select(group => new    
            {
                Count = group.Count(),
                ShamsiDate = group.Key
            })
            .OrderBy(x => x.ShamsiDate)
            .IgnoreQueryFilters()
            .ToListAsync(cancellationToken);

        foreach (var comment in comments)
        {
            result[comment.ShamsiDate] = comment.Count;
        }

        Console.WriteLine(result);
        
        return result.Select(x => x.Value).ToList();
    }
}