using ExpenseTrackerCLI.Common;
using ExpenseTrackerCLI.Entities;

namespace ExpenseTrackerCLI.Services.ExpenseChange;

public  interface IExpenseExchangeService
{
    Task<ResultResponse<Expense>> ConvertExpenseCurrencyToRon(int id, CancellationToken ct = default);
    Task<ResultResponse<Expense>> ConvertExpenseCurrencyFromRon(int id, CurrencyType currencyType, CancellationToken ct = default);
}
