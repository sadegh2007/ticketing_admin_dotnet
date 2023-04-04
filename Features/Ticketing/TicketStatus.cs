using ERP.Ticketing.HttpApi.Data.Common.Helpers;

namespace ERP.Ticketing.HttpApi.Features.Ticketing;

public class TicketStatus: IModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
}