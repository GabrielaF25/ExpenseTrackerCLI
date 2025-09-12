using ExpenseTrackerCLI.Entities;

namespace ExpenseTrackerCLI.Common;

public class ResultResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public Expense? Expense { get; private set; }  
    public static ResultResponse Success(Expense? expense = null) => new() { IsSuccess = true, Expense = expense  };
    public static ResultResponse Failure(string message) => new() { IsSuccess = false, Message = message };
}
