using ExpenseTrackerCLI.Entities;
using ExpenseTrackerCLI.ExpensesDatabase;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace ExpenseTrackerCLI.Repositories;

public class ExpensesRepository(ExpensesDB context) : IExpensesRepository
{
    private readonly ExpensesDB _context = context;

    public async Task AddExpense(Expense expense, CancellationToken ct = default)
    {
       await  _context.Expenses.AddAsync(expense, ct);
       await  _context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<Expense>> GetAllExpenses(CancellationToken ct = default) => await _context.Expenses.AsNoTracking().ToListAsync(ct);

    public async Task  RemoveExpense(Expense expense, CancellationToken ct = default)
    {
        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync(ct);
    }

    public async Task  UpdateExpense(Expense expenseFromParameter, CancellationToken ct = default)
    {
        var expenseFromDB = await _context.Expenses.FirstOrDefaultAsync(ex => ex.Id ==  expenseFromParameter.Id, ct);

        expenseFromDB!.Title = expenseFromParameter.Title;
        expenseFromDB.Description = expenseFromParameter.Description;
        expenseFromDB.Amount = expenseFromParameter.Amount;
        expenseFromDB.ExpenseType = expenseFromParameter.ExpenseType;
        expenseFromDB.CreatedExpense = expenseFromParameter.CreatedExpense;
        expenseFromDB.Currency = expenseFromParameter.Currency;
        expenseFromDB.BaseCurrency = expenseFromParameter.BaseCurrency;
        expenseFromDB.FixRateDate = expenseFromParameter.FixRateDate;

        await _context.SaveChangesAsync(ct);
    }

    public async Task<Expense?> GetExpenseById(int id, CancellationToken ct = default)
    {
        return await _context.Expenses.FirstOrDefaultAsync( e => e.Id == id , ct);
    }
}
