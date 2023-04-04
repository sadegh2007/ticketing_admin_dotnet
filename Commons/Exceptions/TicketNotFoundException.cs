using Opw.HttpExceptions;

namespace ERP.Ticketing.HttpApi.Commons.Exceptions;

public class TicketNotFoundException: NotFoundException
{
    public TicketNotFoundException(string message = "تیکت مورد نظر یافت نشد.") : base(message)
    {
    }
}