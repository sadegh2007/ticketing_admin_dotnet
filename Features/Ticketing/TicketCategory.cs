using ERP.Ticketing.HttpApi.Data.Common.Helpers;
using ERP.Ticketing.HttpApi.Features.Categories;
using ERP.Ticketing.HttpApi.Features.Users;

namespace ERP.Ticketing.HttpApi.Features.Ticketing;

public class TicketCategory: ICreateTime, ICreator
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public User Creator { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
    public Ticket Ticket { get; set; }
    public DateTime CreatedAt { get; set; }
}