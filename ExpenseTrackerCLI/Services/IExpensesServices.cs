using ExpenseTrackerCLI.Common;
using ExpenseTrackerCLI.Entities;

namespace ExpenseTrackerCLI.Services;

public interface IExpensesServices
{
    ResultResponse AddExpenses(Expense expenseToAdd);
    ResultResponse RemoveExpenses(int idFromParameter);
    ResultResponse Update(Expense expenseToUpdate);
    IEnumerable<Expense> GetAllExpenses();
    Expense? GetExpenseById(int id);
    ResultResponse ConvertExpenseCurrencyToRon(int id);
    ResultResponse ConvertExpenseCurrencyFromRon(int id, CurrencyType currencyType);
}
