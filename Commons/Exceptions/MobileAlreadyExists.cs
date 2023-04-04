namespace ERP.Ticketing.HttpApi.Commons.Exceptions;

public class MobileAlreadyExists: Exception
{
    public MobileAlreadyExists(string message = "این شماره قبلا ثبت شده است."): base(message)
    {
        
    }
}