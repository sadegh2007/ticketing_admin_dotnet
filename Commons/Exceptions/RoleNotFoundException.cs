using Opw.HttpExceptions;

namespace ERP.Ticketing.HttpApi.Commons.Exceptions;

public class RoleNotFoundException: NotFoundException
{
    public RoleNotFoundException(string message = "این نقش پیدا نشد."): base(message)
    {
        
    }
}