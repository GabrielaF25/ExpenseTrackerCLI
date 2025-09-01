namespace ExpenseTrackerCLI.Common;

public class ResultClass
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;

    public static ResultClass Success() => new ResultClass { IsSuccess = true };
    public static ResultClass Failure(string message) => new ResultClass { IsSuccess =false, Message = message };
}
