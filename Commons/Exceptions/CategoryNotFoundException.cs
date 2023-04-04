using Opw.HttpExceptions;

namespace ERP.Ticketing.HttpApi.Commons.Exceptions;

public class CategoryNotFoundException: NotFoundException
{
    public CategoryNotFoundException(string message = "این دسته بندی وجود ندارد."): base(message)
    {
        
    }
}