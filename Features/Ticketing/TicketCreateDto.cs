using System.Collections.ObjectModel;
using ERP.Shared.Ticketing;

namespace ERP.Ticketing.HttpApi.Features.Ticketing;

public class TicketCreateDto
{
    public string Title { get; set; }
    public Collection<Guid>? UserIds { get; set; }
    public Collection<Guid>? Categories { get; set; }
    public Guid? DepartmentId { get; set; }
    public string Message { get; set; }

    public TicketPriority? Priority { get; set; }
    
    public IEnumerable<IFormFile>? Files { get; set; }
}