using ExpenseTrackerCLI.Entities;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerApi.Dto;

public class ExpenseForCreationDto
{
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public decimal Amount { get; set; }
    public CurrencyType Currency { get; set; } = CurrencyType.Ron;
    public CurrencyType BaseCurrency { get; set; } = CurrencyType.Ron;

    [Required]
    public ExpenseType ExpenseType { get; set; }
    public DateTimeOffset? FixRateDate { get; set; }

    [Required]
    public DateTimeOffset CreatedExpense { get; set; }
}
