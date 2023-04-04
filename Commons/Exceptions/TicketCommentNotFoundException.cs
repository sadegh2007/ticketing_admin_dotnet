using Opw.HttpExceptions;

namespace ERP.Ticketing.HttpApi.Commons.Exceptions;

public class TicketCommentNotFoundException: NotFoundException
{
    public TicketCommentNotFoundException(string message = "کامنت مورد نظر یافت نشد."): base(message)
    {
        
    }
}