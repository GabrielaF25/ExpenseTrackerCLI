using ExpenseTrackerCLI.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ExpenseTrackerCLI.Repositories;

public interface IExpensesRepository
{
    void RemoveExpense(Expense expense);
    void AddExpense(Expense expense);
    void UpdateExpense(Expense expense);
    IEnumerable<Expense> GetAllExpenses();
    Expense? GetExpenseById(int id);
}
