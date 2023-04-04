using Microsoft.AspNetCore.Identity;

namespace ERP.Ticketing.HttpApi.Features.Users;

public class Role: IdentityRole<Guid>
{
    public Role()
    {
        Permissions = new List<Permission>();
        Users = new List<User>();
    }
    
    public string Title { get; set; }

    public ICollection<Permission> Permissions { get; set; }
    public ICollection<User> Users { get; set; }
}