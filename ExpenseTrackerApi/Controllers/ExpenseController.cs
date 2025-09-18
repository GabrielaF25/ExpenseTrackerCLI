using AutoMapper;
using ExpenseTrackerApi.Dto;
using ExpenseTrackerApi.Services;
using ExpenseTrackerCLI.Common;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.Controllers
{
    [Route("api/Expense")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseServiceApi expenseService;
        private readonly IMapper _mapper;
        public ExpenseController(IExpenseServiceApi service, IMapper mapper)
        {
            expenseService = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpense()
        {
            var expenses = await expenseService.GetExpenses();

            return Ok(expenses);
        }

        [HttpGet("{id}", Name = "GetExpenseById")]
        public async Task<ActionResult<ExpenseDto>> GetExpense(int id)
        {
            var exenseDto = await expenseService.GetExpenseById(id);
            if (exenseDto is null)
            {
                return NotFound();
            }
            return Ok(exenseDto);
        }

        [HttpPost]
        public async Task<ActionResult<ExpenseDto>> AddExpense(ExpenseForCreationDto expenseDtoFromParameter)
        {
            var result = await expenseService.AddExpense(expenseDtoFromParameter);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            var expenseDto = _mapper.Map<ExpenseDto>(result.Data);
            return CreatedAtRoute("GetExpenseById", new { id = result.Data!.Id }, expenseDto);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<ExpenseDto>> UpdateExpense(int id, ExpenseForCreationDto expenseDtoFromParameter)
        {
            var result = await expenseService.UpdateExpense(id, expenseDtoFromParameter);

            if (!result.IsSuccess)
            {
                return result.Error is ErrorType.NotFound ? NotFound($"{result.Message}") : BadRequest(result.Message);
            }

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExpense(int id)
        {
            var result = await expenseService.RemoveExpense(id);

            if (!result.IsSuccess)
            {
                return result.Error is ErrorType.NotFound ? NotFound($"{result.Message}") : BadRequest(result.Message);
            }

            return NoContent();
        }
    }
}
