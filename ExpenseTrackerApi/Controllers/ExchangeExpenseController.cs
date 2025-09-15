using ExpenseTrackerApi.Dto;
using ExpenseTrackerApi.Services;
using ExpenseTrackerCLI.Entities;
using ExpenseTrackerCLI.ExchangeRate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExchangeExpenseController : ControllerBase
{
    private readonly IExpenseServiceApi  _expenseServiceApi;

    public ExchangeExpenseController(IExpenseServiceApi expenseServiceApi)
    { 
        _expenseServiceApi = expenseServiceApi;
    }

    [HttpPut("Exchange/FromRonTo")]
    public ActionResult ExchangeFromRonTo(int id, CurrencyType currencyType)
    {
        var result = _expenseServiceApi.ConvertExpenseCurrencyFromRonTo(id, currencyType);
        if (!result.IsSuccess)
        {
            if(result.Message.Contains($"The expense with id {id} does not exist!"))
            {
                return NotFound($"{ result.Message}");
            }
            return BadRequest(result.Message);
        }

        return NoContent();
    }
    [HttpPut("Exchange/ToRon")]
    public ActionResult ExchangeToRon(int id)
    {
        var result = _expenseServiceApi.ConvertExpenseCurrencyFromToRon(id);
        if (!result.IsSuccess)
        {
            if (result.Message.Contains($"The expense with id {id} does not exist!"))
            {
                return NotFound($"{result.Message}");
            }
            return BadRequest(result.Message);
        }

        return NoContent();
    }
}
