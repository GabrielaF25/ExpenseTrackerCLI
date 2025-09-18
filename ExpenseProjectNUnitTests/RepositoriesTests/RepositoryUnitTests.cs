using ExpenseProjectNUnitTests.DataBaseHelper;
using ExpenseProjectNUnitTests.TestData;
using ExpenseTrackerCLI.Entities;
using ExpenseTrackerCLI.ExpensesDatabase;
using ExpenseTrackerCLI.Repositories;

namespace ExpenseProjectNUnitTests.RepositoriesTests
{
    public class RepositoryUnitTests
    {
        private ExpensesRepository _repository;
        private ExpensesDB _context;
        [SetUp]
        public void Setup()
        {
            _context = MockDataBase.CreateInMemoryContext();
            _repository = new ExpensesRepository(_context);

        }

        [TearDown]
        public void Teardown()
        {
            _context.Dispose();
        }
        
        [Test]
        public async Task AddExpense_WhenCalled_ShouldAddExpenseInDb()
        {
            //Arrange
            var expense = new Expense()
            {
                Title = "Test",
                Description = "Description Test",
                Amount = 150,
                ExpenseType = ExpenseType.MarketingExpenses,
                CreatedExpense = DateTime.Now
            };

            //Act
            await _repository.AddExpense(expense);

            //Assert
            Assert.That(_context.Expenses.ToList(), Does.Contain(expense));
        }

        [TestCaseSource(typeof(TestDataCases), nameof(TestDataCases.TestCaseDataEmptyListExpenses))]
        [TestCaseSource(typeof(TestDataCases), nameof(TestDataCases.TestCaseDataExpenses))]
        public async Task GetAllExpenses_WhenCalldes_ReturnAllExpenses(List<Expense> listFromParameter)
        {
            //Arrange
            _context.Expenses.AddRange(listFromParameter);

            //Act
            var listFromDb = await _repository.GetAllExpenses();

            //Assert
            Assert.That(listFromDb.Count(), Is.EqualTo(_context.Expenses.Count()));
        }

        [TestCaseSource(typeof(TestDataCases), nameof(TestDataCases.TestCaseDataExpenses))]
        public async Task RemoveExpense_WhenCalled_ShouldRemoveExpenseInDb(List<Expense> expenses)
        {
            //Arrange
            _context.Expenses.AddRange(expenses);
            var expenseToRemove = expenses[1];
            
            //Act
            await _repository.RemoveExpense(expenseToRemove);

            //Assert
            Assert.That(_context.Expenses, Does.Not.Contain(expenseToRemove));
            Assert.That(_context.Expenses.Count, Is.EqualTo(expenses.Count - 1));
        }

        [TestCaseSource(typeof(TestDataCases), nameof(TestDataCases.TestCaseDataExpenses))]
        public async Task GetExpenseById_WhenCalled_ShouldReturnRequiredExpese(List<Expense> expenseListFromParameter)
        {
            //Arrange
            _context.Expenses.AddRange(expenseListFromParameter);
            _context.SaveChanges();
            var expense = expenseListFromParameter[1];

            //Act
            var expenseFromDb = await _repository.GetExpenseById(2);

            //Assert

            Assert.That(expenseFromDb, Is.EqualTo(expense));
        }

        [TestCaseSource(typeof(TestDataCases), nameof(TestDataCases.TestCaseDataExpenses))]
        public async Task GetExpenseById_WhenCalledInexistentId_ShouldReturnNull(List<Expense> expenseListFromParameter)
        {
            //Arrange
            _context.Expenses.AddRange(expenseListFromParameter);

            //Act
            var expenseFromDb = await _repository.GetExpenseById(7);

            //Assert
            Assert.That(expenseFromDb, Is.Null);
        }

        [TestCaseSource(typeof(TestDataCases), nameof(TestDataCases.TestCaseDataExpenses))]
        public async Task UpdateExpense_WhenCalled_UpdateExpenseInDb(List<Expense> expenseListFromParameter)
        {
            //Arrange
            _context.Expenses.AddRange(expenseListFromParameter);
            _context.SaveChanges();
            var expenseForUpdate = new Expense()
            {
                Id = 2,
                Title = "Updated Test",
                Description = "Updated Description",
                Amount = 100,
                ExpenseType = ExpenseType.UnknownExpenses,
                Currency = CurrencyType.Dollar,
                BaseCurrency = CurrencyType.Euro,
                CreatedExpense = new DateTime(2014, 06, 19),
                FixRateDate = new DateTime(2013, 04, 19)
            };

            //Act
            await _repository.UpdateExpense(expenseForUpdate);

            //Assert

            var expense = await _repository.GetExpenseById(2);

            Assert.IsNotNull(expense);

            Assert.Multiple(() =>
            { 
                Assert.That(expense.Title, Is.EqualTo(expenseForUpdate.Title));
                Assert.That(expense.Description, Is.EqualTo(expenseForUpdate.Description));
                Assert.That(expense.Amount, Is.EqualTo(expenseForUpdate.Amount));
                Assert.That(expense.ExpenseType, Is.EqualTo(expenseForUpdate.ExpenseType));
                Assert.That(expense.CreatedExpense, Is.EqualTo(expenseForUpdate.CreatedExpense));
                Assert.That(expense.FixRateDate, Is.EqualTo(expenseForUpdate.FixRateDate));
                Assert.That(expense.Currency, Is.EqualTo(expenseForUpdate.Currency));
                Assert.That(expense.BaseCurrency, Is.EqualTo(expenseForUpdate.BaseCurrency));

            }); 
            var expensesFromDb = _context.Expenses.Where( e => e.Id != 2 ).ToList();
            foreach(var items in expensesFromDb)
            {
                Assert.Multiple(() =>
                { 
                    Assert.That(items.Title, Is.Not.EqualTo(expenseForUpdate.Title));
                    Assert.That(items.Description, Is.Not.EqualTo(expenseForUpdate.Description));
                });
            }
        }
    }
}