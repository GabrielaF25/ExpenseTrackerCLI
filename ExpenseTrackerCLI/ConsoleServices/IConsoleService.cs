using ExpenseTrackerCLI.Entities;

namespace ExpenseTrackerCLI.ConsoleServices;

public interface IConsoleService
{
    Task Write(string message);
    Task<string> Read();
    Task Menu();
    Task<string> GetValueString(string message);
    Task DisplayExpense(Expense expense);
}
