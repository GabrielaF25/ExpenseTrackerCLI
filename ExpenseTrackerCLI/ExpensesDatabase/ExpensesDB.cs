using ExpenseTrackerCLI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerCLI.ExpensesDatabase;

public class ExpensesDB : DbContext
{
    public ExpensesDB(DbContextOptions<ExpensesDB> options) : base(options) { }
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Expense>()
            .Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<Expense>()
            .Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Expense>()
            .Property(e => e.ExpenseType)
            .IsRequired()
            .HasConversion<string>();

        modelBuilder.Entity<Expense>()
            .Property(e => e.CreatedExpense)
            .IsRequired();

        modelBuilder.Entity<Expense>()
            .Property(e => e.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Expense>()
           .Property(e => e.BaseCurrency)
           .IsRequired();
    }
}
