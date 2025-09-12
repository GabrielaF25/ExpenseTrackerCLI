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
        this.Write("5. ConvertExpenseCurrency");
        this.Write("6. Exit.");
        this.Write("Select an option:");
    }

    public string GetValueString(string message)
    {
        this.Write(message);
        return this.Read();
    }
    public void DisplayExpense(Expense expense)
    {
        var dt = expense.CreatedExpense.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
        this.Write($"Id: {expense.Id} \n Created datetime: {dt} \n Expense type: {expense.ExpenseType,-22} \n" +
            $" Title & Description: {expense.Title} - {expense.Description} \n Amount: {expense.Amount} {expense.Currency}");
        this.Write("-----------------------------------------------------------");
    }
}
