using ERP.Ticketing.HttpApi.Data.Common.Helpers;
using ERP.Ticketing.HttpApi.Features.Users;

namespace ERP.Ticketing.HttpApi.Features.Categories;

public class Category: IModel, ICreator, ICreateTime, IUpdateTime, IShamsiCreatedAt
{
    public Guid Id { get; set; }

    public string Title { get; set; }
    public Guid CreatorId { get; set; }
    public User Creator { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string ShamsiCreatedAt { get; set; }
}