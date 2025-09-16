using ExpenseTrackerCLI.Common;
using ExpenseTrackerCLI.DateTimeExchangeRate;
using ExpenseTrackerCLI.Entities;
using ExpenseTrackerCLI.ExchangeRate;
using ExpenseTrackerCLI.Services.ExpenseService;

namespace ExpenseTrackerCLI.Services.ExpenseChange;

public class ExpenseExchangeService(IExpensesServices services, IExchangeRateProvider exchangeRateProvider, IDateTimeRate dateTimeRate) : IExpenseExchangeService
{
    private readonly IExpensesServices _services = services;
    private readonly IExchangeRateProvider _exchangeRateProvider = exchangeRateProvider;
    private readonly IDateTimeRate _dateTimeRate = dateTimeRate;


    public ResultResponse ConvertExpenseCurrencyFromRon(int id, CurrencyType currencyType)
    {
        var expenseToChangeCurrency = _services.GetExpenseById(id);

        if (expenseToChangeCurrency == null)
        {
            return ResultResponse.Failure($"The expense with id {id} does not exist!");
        }

        var result = _exchangeRateProvider.GetValue(currencyType);

        expenseToChangeCurrency.Amount = Math.Round(expenseToChangeCurrency.Amount / result, 4);
        expenseToChangeCurrency.Currency = currencyType;
        expenseToChangeCurrency.FixRateDate = _dateTimeRate.SetDateTimeNow();

        var resultFromUpdate = _services.Update(expenseToChangeCurrency);
        if (!resultFromUpdate.IsSuccess)
        {
            return resultFromUpdate;
        }

        return ResultResponse.Success(expenseToChangeCurrency);
    }

    public ResultResponse ConvertExpenseCurrencyToRon(int id)
    {
        var expenseToChangeCurrency = _services.GetExpenseById(id);

        if (expenseToChangeCurrency == null)
        {
            return ResultResponse.Failure($"The expense with id {id} does not exist!");
        }

        var result = _exchangeRateProvider.GetValue(expenseToChangeCurrency.Currency);

        expenseToChangeCurrency.Amount = Math.Round(expenseToChangeCurrency.Amount * result, 4);
        expenseToChangeCurrency.Currency = CurrencyType.Ron;
        expenseToChangeCurrency.FixRateDate = _dateTimeRate.SetDateTimeNow();

        var resultFromUpdate = _services.Update(expenseToChangeCurrency);
        if (!resultFromUpdate.IsSuccess)
        {
            return resultFromUpdate;
        }

        return ResultResponse.Success();
    }
}
