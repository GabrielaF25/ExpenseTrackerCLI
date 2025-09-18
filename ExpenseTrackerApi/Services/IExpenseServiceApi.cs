using ExpenseTrackerApi.Dto;
using ExpenseTrackerCLI.Common;
using ExpenseTrackerCLI.Entities;

namespace ExpenseTrackerApi.Services;

public interface IExpenseServiceApi
{
    Task<IEnumerable<ExpenseDto>> GetExpenses();
    Task<ExpenseDto?> GetExpenseById(int id);
    Task<ResultResponse<Expense>> RemoveExpense(int id);
    Task<ResultResponse<Expense>> AddExpense(ExpenseForCreationDto forCreationDto);
    Task<ResultResponse<Expense>> UpdateExpense(int id, ExpenseForCreationDto forCreationDto);
    Task<ResultResponse<Expense>> ConvertExpenseCurrencyFromRonTo(int id, CurrencyType currencyType);
    Task<ResultResponse<Expense>> ConvertExpenseCurrencyFromToRon(int id);
}
