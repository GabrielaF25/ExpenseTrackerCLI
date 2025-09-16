using ExpenseTrackerCLI.Common;
using ExpenseTrackerCLI.Entities;
using ExpenseTrackerCLI.Repositories;
using FluentValidation;

namespace ExpenseTrackerCLI.Services.ExpenseService;

public class ExpensesServices(IExpensesRepository repository, IValidator<Expense?> validator) : IExpensesServices
{
    private readonly IExpensesRepository _repository = repository;
    private readonly IValidator<Expense?> _validator = validator;
    public ResultResponse  AddExpenses(Expense expenseToAdd)
    {
        if(expenseToAdd == null)
        {
            return ResultResponse.Failure("Expense is null!");
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
            return ResultResponse.Failure("Sorry! Expense not found.");
        }
        _repository.RemoveExpense(expenseToRemove);
        return ResultResponse.Success();
    }

    public ResultResponse Update(Expense expenseToUpdate) 
    {
        if (expenseToUpdate == null)
            return ResultResponse.Failure("Expense is null!");

        var expenseFromDb = _repository.GetExpenseById(expenseToUpdate.Id);

        if (expenseFromDb == null) 
            return ResultResponse.Failure("Expense not found!");

        if(string.IsNullOrWhiteSpace(expenseToUpdate.Title)) 
            expenseToUpdate.Title = expenseFromDb.Title;

        if(string.IsNullOrWhiteSpace(expenseToUpdate.Description)) 
            expenseToUpdate.Description = expenseFromDb.Description;

        if (expenseToUpdate.Amount == 0)  
            expenseToUpdate.Amount = expenseFromDb.Amount;

        _repository.UpdateExpense(expenseToUpdate);

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
}
