using ExpenseTrackerCLI.Common;
using ExpenseTrackerCLI.Entities;
using ExpenseTrackerCLI.Repositories;
using FluentValidation;

namespace ExpenseTrackerCLI.Services;

public class ExpensesServices : IExpensesServices
{
    private readonly IExpensesRepository _repository;
    private readonly IValidator<Expense> _validator;

    public ExpensesServices(IExpensesRepository repository, IValidator<Expense> validator)
    {
       _repository = repository;
        _validator = validator;
    }
     
    public ResultClass  AddExpenses(Expense expenseToAdd)
    {
        if(expenseToAdd == null)
        {
            return ResultClass.Failure("Expense is null");
        }

        var validationResult = _validator.Validate(expenseToAdd);
        if (!validationResult.IsValid) return ResultClass.Failure(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

        _repository.AddExpense(expenseToAdd);
        return ResultClass.Success();
    }

    public ResultClass RemoveExpenses(int idFromParameter)
    {
        var expenseToRemove = _repository.GetExpenseById(idFromParameter);
        if (expenseToRemove == null)
        {
            return ResultClass.Failure("Sorry! Expense not found in database.");
        }
        _repository.RemoveExpense(expenseToRemove);
        return ResultClass.Success();
    }

    public ResultClass Update(Expense expenseToUpdate) 
    {
        if (expenseToUpdate == null)
        {
            return ResultClass.Failure("Expense is null!");
        }

        var expenseFromDb = _repository.GetExpenseById(expenseToUpdate.Id);
        if (expenseFromDb == null) 
        {
            return ResultClass.Failure("Expense not found!");
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

        expenseFromDb.Amount = expenseToUpdate.Amount;

        _repository.UpdateExpense(expenseFromDb);
        return ResultClass.Success();
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
