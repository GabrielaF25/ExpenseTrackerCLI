using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ExpenseTrackerCLI.ExpensesDatabase;

public class ExpensesFactory : IDesignTimeDbContextFactory<ExpensesDB>
{
    public ExpensesDB CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json")
              .Build();

        var optionBuilder = new DbContextOptionsBuilder<ExpensesDB>();
        optionBuilder.UseSqlServer(config.GetConnectionString("ExpensesDB"));

        return new ExpensesDB(optionBuilder.Options);
    }
}
