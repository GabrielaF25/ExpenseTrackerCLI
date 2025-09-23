using ExpenseTrackerCLI.Common;
using ExpenseTrackerCLI.Entities;
using ExpenseTrackerCLI.Repositories;
using FluentValidation;

namespace ExpenseTrackerCLI.Services.ExpenseService;

public class ExpensesServices(IExpensesRepository repository, IValidator<Expense?> validator) : IExpensesServices
{
    private readonly IExpensesRepository _repository = repository;
    private readonly IValidator<Expense?> _validator = validator;
    public async Task<ResultResponse<Expense>> AddExpenses(Expense expenseToAdd, CancellationToken ct = default)
    {
        if(expenseToAdd == null)
        {
            return ResultResponse<Expense>.Failure("Sorry! Expense is null!", ErrorType.NullObject);
        }

        var validationResult = _validator.Validate(expenseToAdd);
        if (!validationResult.IsValid) return ResultResponse<Expense>.Failure(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)), ErrorType.ErrorValidation);

        await _repository.AddExpense(expenseToAdd, ct);
        return ResultResponse<Expense>.Success(expenseToAdd);
    }

    public async Task<ResultResponse<Expense>> RemoveExpenses(int idFromParameter, CancellationToken ct = default)
    {
        var expenseToRemove = await _repository.GetExpenseById(idFromParameter, ct);
        if (expenseToRemove == null)
        {
            return ResultResponse<Expense>.Failure("Sorry! Expense not found.", ErrorType.NotFound);
        }
        await _repository.RemoveExpense(expenseToRemove, ct);
        return ResultResponse<Expense>.Success();
    }

    public async Task<ResultResponse<Expense>> Update(Expense expenseToUpdate, CancellationToken ct = default) 
    {
        if (expenseToUpdate == null)
            return ResultResponse<Expense>.Failure("Sorry! Expense is null!", ErrorType.NullObject);

        var expenseFromDb = await _repository.GetExpenseById(expenseToUpdate.Id, ct);

        if (expenseFromDb == null) 
            return ResultResponse<Expense>.Failure("Sorry! Expense not found!", ErrorType.NotFound);

        if(string.IsNullOrWhiteSpace(expenseToUpdate.Title)) 
            expenseToUpdate.Title = expenseFromDb.Title;

        if(string.IsNullOrWhiteSpace(expenseToUpdate.Description)) 
            expenseToUpdate.Description = expenseFromDb.Description;

        if (expenseToUpdate.Amount == 0)  
            expenseToUpdate.Amount = expenseFromDb.Amount;

        await _repository.UpdateExpense(expenseToUpdate, ct);

        return ResultResponse<Expense>.Success();
    }

    public async Task<IEnumerable<Expense>> GetAllExpenses(CancellationToken ct = default)
    {
       return await _repository.GetAllExpenses(ct);
    }

    public async Task<Expense?> GetExpenseById(int id, CancellationToken ct = default)
    {
       return await _repository.GetExpenseById(id, ct);
    }
}
