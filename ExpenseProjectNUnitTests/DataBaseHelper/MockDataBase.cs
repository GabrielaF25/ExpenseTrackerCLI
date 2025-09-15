using ExpenseTrackerCLI.ExpensesDatabase;
using Microsoft.EntityFrameworkCore;

namespace ExpenseProjectNUnitTests.DataBaseHelper;

public static class MockDataBase
{
   public static ExpensesDB CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ExpensesDB>()
                                 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                                 .Options;

        return new ExpensesDB(options);
    }
}
