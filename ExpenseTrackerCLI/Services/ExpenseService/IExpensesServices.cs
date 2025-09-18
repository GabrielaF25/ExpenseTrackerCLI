using ExpenseTrackerCLI.Common;
using ExpenseTrackerCLI.Entities;

namespace ExpenseTrackerCLI.Services.ExpenseService;

public interface IExpensesServices
{
    Task<ResultResponse<Expense>> AddExpenses(Expense expenseToAdd, CancellationToken ct = default);
    Task<ResultResponse<Expense>> RemoveExpenses(int idFromParameter, CancellationToken ct = default);
    Task<ResultResponse<Expense>> Update(Expense expenseToUpdate, CancellationToken ct = default);
    Task<IEnumerable<Expense>> GetAllExpenses(CancellationToken ct = default);
    Task<Expense?> GetExpenseById(int id, CancellationToken ct = default);
   
}
