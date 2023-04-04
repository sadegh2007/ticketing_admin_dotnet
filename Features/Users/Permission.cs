using ERP.Ticketing.HttpApi.Data.Common.Helpers;

namespace ERP.Ticketing.HttpApi.Features.Users;

public class Permission: IModel, ICreateTime
{
    public Permission()
    {
        Roles = new List<Role>();
    }
    
    public Guid Id { get; set; }
    public string? Route { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }

    public ICollection<Role> Roles { get; set; }
    public DateTime CreatedAt { get; set; }
}