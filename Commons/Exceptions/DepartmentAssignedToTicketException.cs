namespace ERP.Ticketing.HttpApi.Commons.Exceptions;

public class DepartmentAssignedToTicketException: Exception
{
    public DepartmentAssignedToTicketException(string message = "این دپارتمان به تیکت نسبت داده شده و قابل حذف نمی باشد.") : base(message)
    {
        
    }
}