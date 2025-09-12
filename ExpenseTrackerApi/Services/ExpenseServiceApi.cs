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
        ResultResponse result = _expensesServices.RemoveExpenses(id);

        return result;
    }

    public ResultResponse AddExpense(ExpenseForCreationDto forCreationDto)
    {
        var bookForD = _mapper.Map<Expense>(forCreationDto);

        ResultResponse result = _expensesServices.AddExpenses(bookForD);

        var bookDto = _mapper.Map<ExpenseDto>(result.Expense);

        return result;
    }

    public ResultResponse UpdateExpense(int id, ExpenseForCreationDto forCreationDto)
    {
        var bookForD = _mapper.Map<Expense>(forCreationDto);

        bookForD.Id = id;
        ResultResponse result = _expensesServices.Update(bookForD);

        var bookDto = _mapper.Map<ExpenseDto>(result.Expense);

        return result;
    }

}
