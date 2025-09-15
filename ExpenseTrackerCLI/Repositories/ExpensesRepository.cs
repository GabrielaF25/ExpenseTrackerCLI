using ExpenseTrackerCLI.Entities;
using ExpenseTrackerCLI.ExpensesDatabase;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerCLI.Repositories;

public class ExpensesRepository(ExpensesDB context) : IExpensesRepository
{
    private readonly ExpensesDB _context = context;

    public void AddExpense(Expense expense)
    {
        _context.Expenses.Add(expense);
        _context.SaveChanges();
    }

    public IEnumerable<Expense> GetAllExpenses() => [.._context.Expenses.AsNoTracking()];

    public void RemoveExpense(Expense expense)
    {
        _context.Expenses.Remove(expense);
        _context.SaveChanges();
    }

    public void UpdateExpense(Expense expenseFromParameter)
    {
        var expenseFromDB = _context.Expenses.FirstOrDefault(ex => ex.Id ==  expenseFromParameter.Id);

        expenseFromDB!.Title = expenseFromParameter.Title;
        expenseFromDB.Description = expenseFromParameter.Description;
        expenseFromDB.Amount = expenseFromParameter.Amount;
        expenseFromDB.ExpenseType = expenseFromParameter.ExpenseType;
        expenseFromDB.CreatedExpense = expenseFromParameter.CreatedExpense;

        _context.SaveChanges();
    }

    public Expense? GetExpenseById(int id)
    {
        return _context.Expenses.FirstOrDefault( e => e.Id == id );
    }
}
