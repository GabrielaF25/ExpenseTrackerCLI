namespace ExpenseTrackerCLI.Entities;

public class Expense
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public CurrencyType Currency { get; set; } = CurrencyType.Ron;
    public CurrencyType BaseCurrency { get; set; } = CurrencyType.Ron;
    public ExpenseType ExpenseType { get; set; }
    public DateTimeOffset? FixRateDate { get; set; }
    public DateTimeOffset CreatedExpense { get; set; } 
}
