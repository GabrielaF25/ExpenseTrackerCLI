using ExpenseTrackerCLI.ConsoleServices;
using ExpenseTrackerCLI.Entities;
using System.Globalization;

namespace ExpenseTrackerCLI.ConsoleApp;

public  class ViewExpensesHelper(IConsoleService consoleService)
{
    private readonly IConsoleService _consoleService = consoleService;

    public bool ChangeFieldAnswer(string field)
    {
        var answer = _consoleService.GetValueString($"Do you want to change this {field}? (y/n):");
        return answer.Equals("y", StringComparison.OrdinalIgnoreCase);
    }

    public  IEnumerable<Expense> ChooseYearFilter(IEnumerable<Expense> expenses)
    {
        var inputYear = _consoleService.GetValueString("Please enter the year:");
        if (!int.TryParse(inputYear, out var year))
        {
            return expenses.Where(d => d.CreatedExpense.Year == DateTimeOffset.Now.Year);
        }
        return expenses.Where(d => d.CreatedExpense.Year == year);
    }

    public  IEnumerable<Expense> ChooseTheMonthFilter(IEnumerable<Expense> expenses, int? selectedYear)
    {
        if (!expenses.Any()) return expenses;

        var now = DateTimeOffset.Now;
        var input = _consoleService.GetValueString("Please enter the month (1-12, Enter = auto):")?.Trim();

        int yearToUse = selectedYear ?? now.Year;

        int monthToUse;
        if (!string.IsNullOrWhiteSpace(input) && int.TryParse(input, out var parsed) && parsed >= 1 && parsed <= 12)
        {
            monthToUse = parsed;
        }
        else
        {
            int cap = yearToUse == now.Year ? now.Month : 12;

            monthToUse = expenses
                .Where(e => e.CreatedExpense.Year == yearToUse && e.CreatedExpense.Month <= cap)
                .Select(e => e.CreatedExpense.Month)
                .DefaultIfEmpty(cap)
                .Max();
            _consoleService.Write($"Entered value for month {input} is not valid. The year {yearToUse} and the month {monthToUse} will be displayed");
        }

        return expenses.Where(d => d.CreatedExpense.Year == yearToUse &&
                                   d.CreatedExpense.Month == monthToUse);
    }

    public IEnumerable<Expense> ChooseExpensesByAmountRange(IEnumerable<Expense> expenses)
    {
        var answerStartRangeOfAmount = _consoleService.GetValueString("Please enter the start of the amount range");
        var answerEndRangeOfAmount = _consoleService.GetValueString("Please enter the end of the amount range");

        decimal startRangeAmount = 0;
        decimal endRangeAmount = 0;

        var culture = CultureInfo.CurrentCulture;

        bool startSucces = !string.IsNullOrWhiteSpace(answerStartRangeOfAmount) && 
            decimal.TryParse(answerStartRangeOfAmount,NumberStyles.Number, culture, out startRangeAmount);
        bool endSucces = !string.IsNullOrWhiteSpace(answerEndRangeOfAmount) &&
            decimal.TryParse(answerEndRangeOfAmount, NumberStyles.Number, culture, out endRangeAmount);

        if (!startSucces)
        {
            _consoleService.Write($"Sorry! But {answerStartRangeOfAmount} is not valid ");
        }

        if (!endSucces) 
        {
            _consoleService.Write($"Sorry! But {answerEndRangeOfAmount} is not valid ");
        }

        if (startSucces && endSucces && startRangeAmount > endRangeAmount)
        {
            (startRangeAmount, endRangeAmount) = (endRangeAmount, startRangeAmount);
        }

        if (startSucces) expenses = expenses.Where(e => e.Amount >= startRangeAmount);
        if (endSucces) expenses = expenses.Where(e => e.Amount <= endRangeAmount);

        return expenses;
    } 
}
