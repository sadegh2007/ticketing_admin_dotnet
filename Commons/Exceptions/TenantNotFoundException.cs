using Opw.HttpExceptions;

namespace ERP.Ticketing.HttpApi.Commons.Exceptions;

public class TenantNotFoundException: NotFoundException
{
    public TenantNotFoundException(string message = "تننت مورد نظر یافت نشد."): base(message)
    {
        
    }
}