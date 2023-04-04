using Microsoft.AspNetCore.Authorization;

namespace ERP.Ticketing.HttpApi.Commons;

public class PermissionRequirement: IAuthorizationRequirement
{
    public string Permission { get; }

    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
}