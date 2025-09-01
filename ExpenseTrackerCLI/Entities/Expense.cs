namespace ExpenseTrackerCLI.Entities;

public class Expense
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public ExpenseType ExpenseType { get; set; }
    public DateTimeOffset CreatedExpense { get; set; } 

}
