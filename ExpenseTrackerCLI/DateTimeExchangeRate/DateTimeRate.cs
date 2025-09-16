namespace ExpenseTrackerCLI.DateTimeExchangeRate;

public class DateTimeRate : IDateTimeRate
{
    public DateTime SetDateTimeNow()
    {
        return DateTime.Now;
    }  
}
