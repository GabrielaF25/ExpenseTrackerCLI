using AutoMapper;
using ExpenseTrackerApi.Dto;
using ExpenseTrackerApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.Controllers
{
    [Route("api/[controller]")]
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
        public ActionResult<IEnumerable<ExpenseDto>> GetExpense()
        {
            var expenses = expenseService.GetExpenses();

            return Ok(expenses);
        }

        [HttpGet("{id}", Name = "GetExpenseById")]
        public ActionResult<ExpenseDto> GetExpense(int id)
        {
            var exenseDto = expenseService.GetExpenseById(id);
            if (exenseDto is null)
            {
                return NotFound();
            }
            return Ok(exenseDto);
        }

        [HttpPost]
        public ActionResult<ExpenseDto> AddExpense(ExpenseForCreationDto expenseDtoFromParameter)
        {
            var result = expenseService.AddExpense(expenseDtoFromParameter);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            var expenseDto = _mapper.Map<ExpenseDto>(result.Expense);
            return CreatedAtRoute("GetExpenseById", new { result.Expense!.Id }, expenseDto);
        }


        [HttpPut("{id}")]
        public ActionResult<ExpenseDto> UpdateExpense(int id, ExpenseForCreationDto expenseDtoFromParameter)
        {
            var result = expenseService.UpdateExpense(id, expenseDtoFromParameter);

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
        [HttpDelete("{id}")]
        public ActionResult DeleteExpense(int id)
        {
            var result = expenseService.RemoveExpense(id);
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
}
