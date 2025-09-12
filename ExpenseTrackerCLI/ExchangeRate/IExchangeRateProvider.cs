using ExpenseTrackerCLI.Entities;
namespace ExpenseTrackerCLI.ExchangeRate;

public interface IExchangeRateProvider
{
    decimal GetValue(CurrencyType currency);
}
