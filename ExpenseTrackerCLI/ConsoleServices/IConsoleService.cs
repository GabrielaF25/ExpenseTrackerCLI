using ExpenseTrackerCLI.Entities;

namespace ExpenseTrackerCLI.ConsoleServices;

public interface IConsoleService
{
    void Write(string message);
    string Read();
    void Menu();
    string GetValueString(string message);
    void DisplayExpense(Expense expense);
}
