namespace ERP.Ticketing.HttpApi.Commons.Exceptions;

public class TicketUserAlreadyAddedException: Exception
{
    public TicketUserAlreadyAddedException(string message = "این کاربر قبلا اضافه شده است."): base(message)
    {
        
    }
}