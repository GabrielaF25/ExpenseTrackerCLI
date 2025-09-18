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


    public async Task<ResultResponse<Expense>> ConvertExpenseCurrencyFromRon(int id, CurrencyType currencyType, CancellationToken ct = default)
    {
        var expenseToChangeCurrency = await _services.GetExpenseById(id, ct);

        if (expenseToChangeCurrency == null)
        {
            return ResultResponse<Expense>.Failure($"The expense with id {id} does not exist!", ErrorType.NotFound);
        }

        var result = _exchangeRateProvider.GetValue(currencyType);

        expenseToChangeCurrency.Amount = Math.Round(expenseToChangeCurrency.Amount / result, 4);
        expenseToChangeCurrency.Currency = currencyType;
        expenseToChangeCurrency.FixRateDate = _dateTimeRate.SetDateTimeNow();

        var resultFromUpdate = await _services.Update(expenseToChangeCurrency, ct);
        if (!resultFromUpdate.IsSuccess)
        {
            return resultFromUpdate;
        }

        return ResultResponse<Expense>.Success(expenseToChangeCurrency);
    }

    public async Task<ResultResponse<Expense>> ConvertExpenseCurrencyToRon(int id, CancellationToken ct = default)
    {
        var expenseToChangeCurrency = await _services.GetExpenseById(id, ct);

        if (expenseToChangeCurrency == null)
        {
            return ResultResponse<Expense>.Failure($"The expense with id {id} does not exist!", ErrorType.NotFound);
        }

        var result = _exchangeRateProvider.GetValue(expenseToChangeCurrency.Currency);

        expenseToChangeCurrency.Amount = Math.Round(expenseToChangeCurrency.Amount * result, 4);
        expenseToChangeCurrency.Currency = CurrencyType.Ron;
        expenseToChangeCurrency.FixRateDate = _dateTimeRate.SetDateTimeNow();

        var resultFromUpdate = await _services.Update(expenseToChangeCurrency, ct);
        if (!resultFromUpdate.IsSuccess)
        {
            return resultFromUpdate;
        }

        return ResultResponse<Expense>.Success();
    }
}
