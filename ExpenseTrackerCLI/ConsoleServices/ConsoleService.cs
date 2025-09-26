using ExpenseTrackerCLI.Entities;
using ExpenseTrackerCLI.ExpensesDatabase;
using System.Runtime.CompilerServices;

namespace ExpenseTrackerCLI.ConsoleServices;

public class ConsoleService : IConsoleService
{
    public async Task<string> Read() => (await Console.In.ReadLineAsync())?.Trim() ?? string.Empty ;

    public async Task Write(string message)
    {
        await Console.Out.WriteLineAsync(message);
    }
    public async Task  Menu()
    {
        await this.Write("Expense Tracker");
        await this.Write("1. Add Expense");
        await this.Write("2. View Expenses");
        await this.Write("3. Update Expense");
        await this.Write("4. Delete Expense");
        await this.Write("5. ConvertExpenseCurrency");
        await this.Write("6. Exit.");
        await this.Write("Select an option:");
    }

    public async Task<string> GetValueString(string message)
    {
        await this.Write(message);
        return await this.Read();
    }
    public async Task DisplayExpense(Expense expense)
    {
        var dt = expense.CreatedExpense.ToLocalTime().ToString("yyyy-MM-dd HH:mm");

        await  this.Write($"Id: {expense.Id} \n Created datetime: {dt} \n " +
            $"Expense type: {expense.ExpenseType,-22} \n" +
            $" Title & Description: {expense.Title} - {expense.Description} \n" +
            $" Amount: {expense.Amount} {expense.Currency}");

        await this.Write("-----------------------------------------------------------");
    }
}
