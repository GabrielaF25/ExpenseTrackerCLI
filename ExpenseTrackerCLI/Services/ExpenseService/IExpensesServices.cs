using ExpenseTrackerCLI.Common;
using ExpenseTrackerCLI.Entities;

namespace ExpenseTrackerCLI.Services.ExpenseService;

public interface IExpensesServices
{
    ResultResponse AddExpenses(Expense expenseToAdd);
    ResultResponse RemoveExpenses(int idFromParameter);
    ResultResponse Update(Expense expenseToUpdate);
    IEnumerable<Expense> GetAllExpenses();
    Expense? GetExpenseById(int id);
   
}
