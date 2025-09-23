using ExpenseTrackerApi.Services;
using ExpenseTrackerCLI.Common;
using ExpenseTrackerCLI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.Controllers;

[Route("api/Exchange")]
[ApiController]
[Authorize]
public class ExchangeExpenseController : ControllerBase
{
    private readonly IExpenseServiceApi  _expenseServiceApi;

    public ExchangeExpenseController(IExpenseServiceApi expenseServiceApi)
    { 
        _expenseServiceApi = expenseServiceApi;
    }

    [HttpPut("{id}/exchange/from-ron-to/{currencyType}")]
    public async Task<ActionResult> ExchangeFromRonTo([FromRoute]int id, [FromRoute]CurrencyType currencyType)
    {
        var result = await _expenseServiceApi.ConvertExpenseCurrencyFromRonTo(id, currencyType);

        if (!result.IsSuccess)
        {
            return result.Error is ErrorType.NotFound ? NotFound($"{result.Message}") : BadRequest(result.Message);
        }

        return NoContent();
    }
    [HttpPut("{id}/exchange/to-ron")]
    public async Task<ActionResult> ExchangeToRon([FromRoute] int id)
    {
        var result = await _expenseServiceApi.ConvertExpenseCurrencyFromToRon(id);

        if (!result.IsSuccess)
        {
            return result.Error is ErrorType.NotFound ? NotFound($"{result.Message}") : BadRequest(result.Message);
        }

        return NoContent();
    }
}
