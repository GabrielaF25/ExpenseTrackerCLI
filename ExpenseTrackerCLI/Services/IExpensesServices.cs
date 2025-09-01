using ExpenseTrackerCLI.Common;
using ExpenseTrackerCLI.Entities;

namespace ExpenseTrackerCLI.Services;

public interface IExpensesServices
{
    ResultClass AddExpenses(Expense expenseToAdd);
    ResultClass RemoveExpenses(int idFromParameter);
    ResultClass Update(Expense expenseToUpdate);
    IEnumerable<Expense> GetAllExpenses();
    Expense? GetExpenseById(int id);
}
