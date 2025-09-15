using ExpenseTrackerApi.Dto;
using ExpenseTrackerCLI.Common;
using ExpenseTrackerCLI.Entities;

namespace ExpenseTrackerApi.Services;

public interface IExpenseServiceApi
{
    IEnumerable<ExpenseDto> GetExpenses();
    ExpenseDto? GetExpenseById(int id);
    ResultResponse RemoveExpense(int id);
    ResultResponse AddExpense(ExpenseForCreationDto forCreationDto);
    ResultResponse UpdateExpense(int id, ExpenseForCreationDto forCreationDto);
    ResultResponse ConvertExpenseCurrencyFromRonTo(int id, CurrencyType currencyType);
    ResultResponse ConvertExpenseCurrencyFromToRon(int id);
}
