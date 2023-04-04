using Microsoft.AspNetCore.Identity;

namespace ERP.Ticketing.HttpApi.Commons.Exceptions;

public class IdentityException: Exception
{
    public IdentityException(IdentityResult result) : base(result.Errors.First().Description)
    {
    }
}