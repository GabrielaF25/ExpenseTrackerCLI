using ExpenseTrackerCLI.Entities;

namespace ExpenseTrackerCLI.ExchangeRate;

public class ExchangeRateProvider : IExchangeRateProvider
{
    public Dictionary<CurrencyType, decimal> ExchangeRate { get; set; } 
    public ExchangeRateProvider()
    {
        ExchangeRate = new Dictionary<CurrencyType, decimal>()
        {
            [CurrencyType.Ron] = 1m,
            [CurrencyType.Lire] = 5.84m,
            [CurrencyType.Euro] = 5.07m,
            [CurrencyType.Dollar] = 4.33m
        };
    }

    public decimal GetValue(CurrencyType currency)
    {
        if (!ExchangeRate.TryGetValue(currency, out decimal result))
        {
            throw new ArgumentException($"This currency: {currency} is not supported");
        }
        return result;
    }
}
