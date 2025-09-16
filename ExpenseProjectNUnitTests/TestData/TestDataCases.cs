using ExpenseTrackerCLI.Entities;

namespace ExpenseProjectNUnitTests.TestData;

public static class TestDataCases
{
    public static IEnumerable<TestCaseData> TestCaseDataEmptyListExpenses()
    {
        yield return new TestCaseData(
            new List<Expense>() { }
            );
    }

    public static IEnumerable<TestCaseData> TestCaseDataExpenses()
    {
        yield return new TestCaseData(
            new List<Expense>()
            {
                new Expense()
                {
                    Title = "Test",
                    Description = "Description Test",
                    Amount = 150,
                    ExpenseType = ExpenseType.MarketingExpenses,
                    CreatedExpense = DateTime.Now
                },

                new Expense()
                {
                    Title = "Test",
                    Description = "Description Test",
                    Amount = 160,
                    ExpenseType = ExpenseType.LegalExpenses,
                    CreatedExpense = DateTime.Now
                },
                new Expense()
                {
                    Title = "Test",
                    Description = "Description Test",
                    Amount = 170,
                    ExpenseType = ExpenseType.UnknownExpenses,
                    CreatedExpense = DateTime.Now
                },

                new Expense()
                {
                    Title = "Test",
                    Description = "Description Test",
                    Amount = 180,
                    ExpenseType = ExpenseType.AdministrativeExpenses,
                    CreatedExpense = DateTime.Now
                }
            }
        );
    }

    public static IEnumerable<TestCaseData> TestCasesUpdateExpenseService()
    {
        yield return new TestCaseData(
            new List<Expense>()
            {
                new Expense()
                {
                    Id = 1,
                    Title = "",
                    Description = "Test Description",
                    Amount = 12m
                }
            }, "Test Title", "Test Description", 12m
        );

        yield return new TestCaseData(
            new List<Expense>()
            {
                new Expense()
                {
                    Id = 1,
                    Title = "Test Title",
                    Description = "",
                    Amount = 12m
                }
            }, "Test Title", "Test Description", 12m
        );

        yield return new TestCaseData(
            new List<Expense>()
            {
                new Expense()
                {
                    Id = 1,
                    Title = "Test Title",
                    Description = "Test Description",
                    Amount = 0m
                }
            }, "Test Title", "Test Description", 12m
        );

        yield return new TestCaseData(
            new List<Expense>()
            {
                new Expense()
                {   
                    Id = 1,
                    Title = "",
                    Description = "",
                    Amount = 0m
                }
            }, "Test Title", "Test Description", 12m
        );
    }
}
