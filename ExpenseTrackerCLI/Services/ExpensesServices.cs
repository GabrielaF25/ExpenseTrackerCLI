using ExpenseTrackerCLI.Common;
using ExpenseTrackerCLI.Entities;
using ExpenseTrackerCLI.ExchangeRate;
using ExpenseTrackerCLI.Repositories;
using FluentValidation;

namespace ExpenseTrackerCLI.Services;

public class ExpensesServices(IExpensesRepository repository, IValidator<Expense?> validator, IExchangeRateProvider exchangeRateProvider) : IExpensesServices
{
    private readonly IExpensesRepository _repository = repository;
    private readonly IValidator<Expense?> _validator = validator;
    private readonly IExchangeRateProvider _exchangeRateProvider = exchangeRateProvider;
    public ResultResponse  AddExpenses(Expense expenseToAdd)
    {
        if(expenseToAdd == null)
        {
            return ResultResponse.Failure("Expense is null");
        }

        var validationResult = _validator.Validate(expenseToAdd);
        if (!validationResult.IsValid) return ResultResponse.Failure(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

        _repository.AddExpense(expenseToAdd);
        return ResultResponse.Success(expenseToAdd);
    }

    public ResultResponse RemoveExpenses(int idFromParameter)
    {
        var expenseToRemove = _repository.GetExpenseById(idFromParameter);
        if (expenseToRemove == null)
        {
            return ResultResponse.Failure("Sorry! Expense not found in database.");
        }
        _repository.RemoveExpense(expenseToRemove);
        return ResultResponse.Success();
    }

    public ResultResponse Update(Expense expenseToUpdate) 
    {
        if (expenseToUpdate == null)
        {
            return ResultResponse.Failure("Expense is null!");
        }

        var expenseFromDb = _repository.GetExpenseById(expenseToUpdate.Id);
        if (expenseFromDb == null) 
        {
            return ResultResponse.Failure("Expense not found!");
        }

        if(!string.IsNullOrWhiteSpace(expenseToUpdate.Title)) 
        {
            expenseFromDb.Title = expenseToUpdate.Title;
        }

        if(!string.IsNullOrWhiteSpace(expenseToUpdate.Description)) 
        {
            expenseFromDb.Description = expenseToUpdate.Description;
        }

        expenseFromDb.ExpenseType = expenseToUpdate.ExpenseType;
        if(expenseToUpdate.Amount != 0)
        {
        expenseFromDb.Amount = expenseToUpdate.Amount;
        }
        expenseFromDb.Currency = expenseToUpdate.Currency;
        expenseFromDb.BaseCurrency = expenseToUpdate.BaseCurrency;

        _repository.UpdateExpense(expenseFromDb);
        return ResultResponse.Success();
    }

    public IEnumerable<Expense> GetAllExpenses()
    {
       return _repository.GetAllExpenses();
    }

    public Expense? GetExpenseById(int id)
    {
       return _repository.GetExpenseById(id);
    }

    public ResultResponse ConvertExpenseCurrencyFromRon(int id, CurrencyType currencyType)
    {
        var expenseToChangeCurrency = _repository.GetExpenseById(id);

        if (expenseToChangeCurrency == null)
        {
            return ResultResponse.Failure($"The expense with id {id} does not exist!");
        }

        var result = _exchangeRateProvider.GetValue(currencyType);

        expenseToChangeCurrency.Amount = Math.Round(expenseToChangeCurrency.Amount / result, 4);
        expenseToChangeCurrency.Currency = currencyType;
        expenseToChangeCurrency.FixRateDate = DateTimeOffset.Now;
        
        var resultFromUpdate = Update(expenseToChangeCurrency);
        if (!resultFromUpdate.IsSuccess)
        {
            return resultFromUpdate;
        }

        return ResultResponse.Success(expenseToChangeCurrency);
    }

    public ResultResponse ConvertExpenseCurrencyToRon(int id)
    {
        var expenseToChangeCurrency = _repository.GetExpenseById(id);

        if (expenseToChangeCurrency == null)
        {
            return ResultResponse.Failure($"The expense with id {id} does not exist!");
        }

        var result = _exchangeRateProvider.GetValue(expenseToChangeCurrency.Currency);

        expenseToChangeCurrency.Amount = Math.Round(expenseToChangeCurrency.Amount * result, 4);
        expenseToChangeCurrency.Currency = CurrencyType.Ron;
        expenseToChangeCurrency.FixRateDate = DateTimeOffset.Now;

        var resultFromUpdate = Update(expenseToChangeCurrency);
        if (!resultFromUpdate.IsSuccess)
        {
            return resultFromUpdate;
        }

        return ResultResponse.Success();
    }
}
