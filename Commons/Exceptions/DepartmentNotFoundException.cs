using Opw.HttpExceptions;

namespace ERP.Ticketing.HttpApi.Commons.Exceptions;

public class DepartmentNotFoundException: NotFoundException
{
    public DepartmentNotFoundException(string message = "این دپارتمان وجود ندارد."): base(message)
    {
        
    }
}