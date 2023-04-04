namespace ERP.Ticketing.HttpApi.Commons.Exceptions;

public class CategoryAssignedToTicketException: Exception
{
    public CategoryAssignedToTicketException(string message = "این دسته بندی به تیکت نسبت داده شده و قابل حذف نمی باشد."): base(message)
    {
        
    }
}