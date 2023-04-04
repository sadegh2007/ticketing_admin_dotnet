using Opw.HttpExceptions;

namespace ERP.Ticketing.HttpApi.Commons.Exceptions;

public class UserNotFoundException: NotFoundException
{
    public UserNotFoundException(string message = "این کاربر وجود ندارد."): base(message)
    {
        
    }
}