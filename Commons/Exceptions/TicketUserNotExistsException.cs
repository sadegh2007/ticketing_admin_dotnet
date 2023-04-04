namespace ERP.Ticketing.HttpApi.Commons.Exceptions;

public class TicketUserNotExistsException : Exception
{
    public TicketUserNotExistsException(string message = "این کاربر در این تیکت وجود ندارد."): base(message)
    {
    }
}