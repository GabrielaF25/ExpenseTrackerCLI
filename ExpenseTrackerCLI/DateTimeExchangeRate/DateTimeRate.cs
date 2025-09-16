namespace ExpenseTrackerCLI.DateTimeExchangeRate;

public class DateTimeRate : IDateTimeRate
{
    public DateTime Rate { get; set; }
    public DateTime SetDateTimeNow()
    {
        Rate = DateTime.Now;
        return Rate;
    }
  
}
