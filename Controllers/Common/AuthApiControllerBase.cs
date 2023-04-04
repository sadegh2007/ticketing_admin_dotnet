using ERP.Ticketing.HttpApi.Commons.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace ERP.Ticketing.HttpApi.Controllers.Common;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AuthApiControllerBase: ApiControllerBase
{
    private Guid _userId;

    public Guid UserId
    {
        get
        {
            if (_userId.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                _userId = User.GetUserId();
            }

            return _userId;
        }
    }
}