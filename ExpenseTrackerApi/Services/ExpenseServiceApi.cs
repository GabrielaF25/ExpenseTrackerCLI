using AutoMapper;
using ExpenseTrackerApi.Dto;
using ExpenseTrackerCLI.Common;
using ExpenseTrackerCLI.Entities;
using ExpenseTrackerCLI.Services.ExpenseChange;
using ExpenseTrackerCLI.Services.ExpenseService;

namespace ExpenseTrackerApi.Services;

public class ExpenseServiceApi : IExpenseServiceApi
{
    private readonly IExpensesServices _expensesServices;
    private readonly IExpenseExchangeService _expenseExchangeService;
    private readonly IMapper _mapper;
    public ExpenseServiceApi(IExpensesServices expensesServices,IExpenseExchangeService expenseExchangeService, IMapper mapper)
    {
        _expensesServices = expensesServices;
        _expenseExchangeService = expenseExchangeService;
        _mapper = mapper;
    }
    public async Task<IEnumerable<ExpenseDto>> GetExpenses()
    {
        var expenses = await _expensesServices.GetAllExpenses();

        var expensesToReturn = _mapper.Map<IEnumerable<ExpenseDto>>(expenses);

        return expensesToReturn;
    }

    public async Task<ExpenseDto?> GetExpenseById(int id)
    {
        var expense = await _expensesServices.GetExpenseById(id);

        var expenseToReturn = _mapper.Map<ExpenseDto>(expense);

        return expenseToReturn;
    }

    public async Task<ResultResponse<Expense>> RemoveExpense(int id)
    {
        var expenseFromDb = await GetExpenseById(id);
        if (expenseFromDb is null)
        {
            return ResultResponse<Expense>.Failure($"The expense with id: {id} was not found.", ErrorType.NotFound);
        }
        ResultResponse<Expense> result = await _expensesServices.RemoveExpenses(id);

        return result;
    }

    public async  Task<ResultResponse<Expense>> AddExpense(ExpenseForCreationDto forCreationDto)
    {
        if (forCreationDto is null)
            return ResultResponse<Expense>.Failure("Payload is null.", ErrorType.NullObject);

        var expenseForD = _mapper.Map<Expense>(forCreationDto);

        ResultResponse<Expense> result = await _expensesServices.AddExpenses(expenseForD);
        
        return result;
    }

    public async Task<ResultResponse<Expense>> UpdateExpense(int id, ExpenseForCreationDto forCreationDto)
    {
        if (forCreationDto is null)
            return ResultResponse<Expense>.Failure("Payload is null.", ErrorType.NullObject);

        var expenseExists =  await GetExpenseById(id);
        if (expenseExists is null)
        {
            return ResultResponse<Expense>.Failure($"The expense with id: {id} was not found.", ErrorType.NotFound);
        }

        var expenseForDB = _mapper.Map<Expense>(forCreationDto);

        expenseForDB.Id = id;
        ResultResponse<Expense> result = await _expensesServices.Update(expenseForDB);

        return result;
    }

    public async Task<ResultResponse<Expense>> ConvertExpenseCurrencyFromRonTo(int id, CurrencyType currencyType)
    {
        var expenseExists = await GetExpenseById(id);
        if (expenseExists is null)
        {
            return ResultResponse<Expense>.Failure($"The expense with id: {id} was not found.", ErrorType.NotFound);
        }

        var result = await _expenseExchangeService.ConvertExpenseCurrencyFromRon(id, currencyType);

        return result;
    }

    public async Task<ResultResponse<Expense>> ConvertExpenseCurrencyFromToRon(int id)
    {
        var expenseExists = await GetExpenseById(id);
        if (expenseExists is null)
        {
            return ResultResponse<Expense>.Failure($"The expense with id: {id} was not found.", ErrorType.NotFound);
        }

        var result = await _expenseExchangeService.ConvertExpenseCurrencyToRon(id);

        return result;
    }

    public async Task<PagedResult<ExpenseDto>> GetExpensesPaged(int page, int pageSize)
    {
        var expenses = await _expensesServices.GetAllExpenses();

        var query = expenses.OrderByDescending(e => e.CreatedExpense)
            .ThenByDescending(e => e.Id);

        var total = expenses.Count();

        var items = expenses
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
                
        var itemsDto = _mapper.Map<IReadOnlyList<ExpenseDto>>(items);

        return new PagedResult<ExpenseDto>
        {
            Items = itemsDto,
            Page = page,
            PageSize = pageSize,
            TotalItems = total
        };
    }
}
