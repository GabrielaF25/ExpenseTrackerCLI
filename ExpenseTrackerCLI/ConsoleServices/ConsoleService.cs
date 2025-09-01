using ExpenseTrackerCLI.Entities;
using ExpenseTrackerCLI.ExpensesDatabase;

namespace ExpenseTrackerCLI.ConsoleServices;

public class ConsoleService : IConsoleService
{
    public string Read() => Console.ReadLine() ?? string.Empty ;

    public void Write(string message)
    {
        Console.WriteLine(message);
    }
    public void Menu()
    {
        this.Write("Expense Tracker");
        this.Write("1. Add Expense");
        this.Write("2. View Expenses");
        this.Write("3. Update Expense");
        this.Write("4. Delete Expense");
        this.Write("5. Exit");
        this.Write("Select an option:");
    }

    public string GetValueString(string message)
    {
        this.Write(message);
        return this.Read();
    }
    public void DisplayExpense(Expense expense)
    {

            var amount = expense.Amount.ToString("C", System.Globalization.CultureInfo.CurrentCulture);
            var dt = expense.CreatedExpense.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
            this.Write($"{expense.Id} | {dt} | {expense.ExpenseType,-22} | {expense.Title} - {expense.Description} : {amount}");
    }
}
