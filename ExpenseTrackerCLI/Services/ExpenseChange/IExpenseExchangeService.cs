using ExpenseTrackerCLI.Common;
using ExpenseTrackerCLI.Entities;

namespace ExpenseTrackerCLI.Services.ExpenseChange;

public  interface IExpenseExchangeService
{
    ResultResponse ConvertExpenseCurrencyToRon(int id);
    ResultResponse ConvertExpenseCurrencyFromRon(int id, CurrencyType currencyType);
}
