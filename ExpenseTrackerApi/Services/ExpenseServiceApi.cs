using AutoMapper;
using ExpenseTrackerApi.Dto;
using ExpenseTrackerCLI.Common;
using ExpenseTrackerCLI.Entities;
using ExpenseTrackerCLI.Services;

namespace ExpenseTrackerApi.Services;

public class ExpenseServiceApi : IExpenseServiceApi
{
    private readonly IExpensesServices _expensesServices;
    private readonly IMapper _mapper;
    public ExpenseServiceApi(IExpensesServices expensesServices, IMapper mapper)
    {
        _expensesServices = expensesServices;
        _mapper = mapper;
    }
    public IEnumerable<ExpenseDto> GetExpenses()
    {
        var expenses = _expensesServices.GetAllExpenses();

        var expensesToReturn = _mapper.Map<IEnumerable<ExpenseDto>>(expenses);

        return expensesToReturn;
    }

    public ExpenseDto? GetExpenseById(int id)
    {
        var expense = _expensesServices.GetExpenseById(id);

        var expenseToReturn = _mapper.Map<ExpenseDto>(expense);

        return expenseToReturn;
    }

    public ResultResponse RemoveExpense(int id)
    {
        var expenseFromDb = GetExpenseById(id);
        if (expenseFromDb is null)
        {
            return ResultResponse.Failure($"The expense with id: {id} was not found.");
        }
        ResultResponse result = _expensesServices.RemoveExpenses(id);

        return result;
    }

    public ResultResponse AddExpense(ExpenseForCreationDto forCreationDto)
    {
        var expenseForD = _mapper.Map<Expense>(forCreationDto);

        ResultResponse result = _expensesServices.AddExpenses(expenseForD);
        
        return result;
    }

    public ResultResponse UpdateExpense(int id, ExpenseForCreationDto forCreationDto)
    {
        var expenseExists = GetExpenseById(id);
        if (expenseExists is null)
        {
            return ResultResponse.Failure($"The expense with id: {id} was not found.");
        }

        var expenseForDB = _mapper.Map<Expense>(forCreationDto);

        expenseForDB.Id = id;
        ResultResponse result = _expensesServices.Update(expenseForDB);

        return result;
    }

    public ResultResponse ConvertExpenseCurrencyFromRonTo(int id, CurrencyType currencyType)
    {
        var expenseExists = GetExpenseById(id);
        if (expenseExists is null)
        {
            return ResultResponse.Failure($"The expense with id: {id} was not found.");
        }

        var result = _expensesServices.ConvertExpenseCurrencyFromRon(id, currencyType);

        return result;
    }

    public ResultResponse ConvertExpenseCurrencyFromToRon(int id)
    {
        var expenseExists = GetExpenseById(id);
        if (expenseExists != null)
        {
            return ResultResponse.Failure($"The expense with id: {id} was not found.");
        }

        var result = _expensesServices.ConvertExpenseCurrencyToRon(id);

        return result;
    }

}
