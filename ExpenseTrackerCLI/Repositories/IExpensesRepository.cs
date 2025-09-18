using ExpenseTrackerCLI.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ExpenseTrackerCLI.Repositories;

public interface IExpensesRepository
{
    Task RemoveExpense(Expense expense, CancellationToken ct = default);
    Task AddExpense(Expense expense, CancellationToken ct = default);
    Task UpdateExpense(Expense expense, CancellationToken ct = default);
    Task<IEnumerable<Expense>> GetAllExpenses(CancellationToken ct = default);
    Task<Expense?> GetExpenseById(int id, CancellationToken ct = default);
}
