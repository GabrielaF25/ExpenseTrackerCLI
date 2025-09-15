using ExpenseProjectNUnitTests.DataBaseHelper;
using ExpenseProjectNUnitTests.TestData;
using ExpenseTrackerCLI.Entities;
using ExpenseTrackerCLI.ExpensesDatabase;
using ExpenseTrackerCLI.Repositories;

namespace ExpenseProjectNUnitTests
{
    public class Tests
    {
        private IExpensesRepository _repository;
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
        public void AddExpense_WhenCalled_ShouldAddExpenseInDb()
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
            _repository.AddExpense(expense);

            //Assert
            Assert.That(_context.Expenses.ToList(), Does.Contain(expense));
        }

        [TestCaseSource(typeof(TestDataCases), nameof(TestDataCases.TestCaseDataEmptyListExpenses))]
        [TestCaseSource(typeof(TestDataCases), nameof(TestDataCases.TestCaseDataExpenses))]
        public void GetAllExpenses_WhenCalldes_ReturnAllExpenses(List<Expense> listFromParameter)
        {
            //Arrange
            _context.Expenses.AddRange(listFromParameter);

            //Act
            var listFromDb = _repository.GetAllExpenses();

            //Assert
            Assert.That(listFromDb.Count(), Is.EqualTo(_context.Expenses.Count()));
        }

        [TestCaseSource(typeof(TestDataCases), nameof(TestDataCases.TestCaseDataExpenses))]
        public void RemoveExpense_WhenCalled_ShouldRemoveExpenseInDb(List<Expense> expenses)
        {
            //Arrange
            _context.Expenses.AddRange(expenses);
            var expenseToRemove = expenses[1];
            
            //Act
            _repository.RemoveExpense(expenseToRemove);

            //Assert
            Assert.That(_context.Expenses, Does.Not.Contain(expenseToRemove));
            Assert.That(_context.Expenses.Count, Is.EqualTo(expenses.Count - 1));
        }

        [TestCaseSource(typeof(TestDataCases), nameof(TestDataCases.TestCaseDataExpenses))]
        public void GetExpenseById_WhenCalled_ShouldReturnRequiredExpese(List<Expense> expenseListFromParameter)
        {
            //Arrange
            _context.Expenses.AddRange(expenseListFromParameter);
            _context.SaveChanges();
            var expense = expenseListFromParameter[1];

            //Act
            var expenseFromDb = _repository.GetExpenseById(2);

            //Assert

            Assert.That(expenseFromDb, Is.EqualTo(expense));
        }

        [TestCaseSource(typeof(TestDataCases), nameof(TestDataCases.TestCaseDataExpenses))]
        public void GetExpenseById_WhenCalledInexistentId_ShouldReturnNull(List<Expense> expenseListFromParameter)
        {
            //Arrange
            _context.Expenses.AddRange(expenseListFromParameter);

            //Act
            var expenseFromDb = _repository.GetExpenseById(7);

            //Assert
            Assert.That(expenseFromDb, Is.Null);
        }

        [TestCaseSource(typeof(TestDataCases), nameof(TestDataCases.TestCaseDataExpenses))]
        public void UpdateExpense_WhenCalled_UpdateExpenseInDb(List<Expense> expenseListFromParameter)
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
                CreatedExpense = new DateTime(2014, 06, 19)
            };

            //Act
            _repository.UpdateExpense(expenseForUpdate);

            //Assert

            var expense = _repository.GetExpenseById(2);

            Assert.IsNotNull(expense);

            Assert.Multiple(() =>
            { 
                Assert.That(expense.Title, Is.EqualTo(expenseForUpdate.Title));
                Assert.That(expense.Description, Is.EqualTo(expenseForUpdate.Description));
                Assert.That(expense.Amount, Is.EqualTo(expenseForUpdate.Amount));
                Assert.That(expense.ExpenseType, Is.EqualTo(expenseForUpdate.ExpenseType));
                Assert.That(expense.CreatedExpense, Is.EqualTo(expenseForUpdate.CreatedExpense));

            });   
        }
    }
}