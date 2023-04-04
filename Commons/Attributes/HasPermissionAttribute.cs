using ERP.Ticketing.HttpApi.Features.Users;
using Microsoft.AspNetCore.Authorization;

namespace ERP.Ticketing.HttpApi.Commons.Attributes;

public class HasPermissionAttribute: AuthorizeAttribute
{
    public HasPermissionAttribute(string permission): base(policy: permission)
    {
    }
}