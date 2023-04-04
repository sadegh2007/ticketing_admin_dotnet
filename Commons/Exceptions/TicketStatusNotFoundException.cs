using Opw.HttpExceptions;

namespace ERP.Ticketing.HttpApi.Commons.Exceptions;

public class TicketStatusNotFoundException: NotFoundException
{
    public TicketStatusNotFoundException(string message = "وضعیت تیکت یافت نشد.") : base(message)
    {
    }
}