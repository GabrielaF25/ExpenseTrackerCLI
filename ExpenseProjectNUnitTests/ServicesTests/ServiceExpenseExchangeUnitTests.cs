using ExpenseTrackerCLI.Common;
using ExpenseTrackerCLI.DateTimeExchangeRate;
using ExpenseTrackerCLI.Entities;
using ExpenseTrackerCLI.ExchangeRate;
using ExpenseTrackerCLI.Services.ExpenseChange;
using ExpenseTrackerCLI.Services.ExpenseService;
using Moq;

namespace ExpenseProjectNUnitTests.ServicesTests;

public class ServiceExpenseExchangeUnitTests
{
    private Mock<IExpensesServices> _mockServices;
    private Mock<IExchangeRateProvider> _mockProvider;
    private Mock<IDateTimeRate> _mockdate;
    private ExpenseExchangeService _service;

    [SetUp]
    public void Setup()
    {
        _mockdate = new Mock<IDateTimeRate>();
        _mockServices = new Mock<IExpensesServices>();
        _mockProvider = new Mock<IExchangeRateProvider>();
        _service = new ExpenseExchangeService(_mockServices.Object, _mockProvider.Object, _mockdate.Object);
    }

    #region ConvertExpenseCurrencyFromRon
    [Test]
    public async Task ConvertExpenseCurrencyFromRon_WhenCalled_ChangeExpenseCurrency()
    {
        //Arrange
        var expense = new Expense()
        {
            Title = "Test",
            Description = "Description Test",
            Amount = 150,
            ExpenseType = ExpenseType.MarketingExpenses,
            CreatedExpense = DateTime.Now,
            Currency = CurrencyType.Ron,
            BaseCurrency = CurrencyType.Ron
        };

        var currencyType = CurrencyType.Euro;

        _mockServices.Setup(x => x.GetExpenseById(1, default)).ReturnsAsync(expense);
        _mockProvider.Setup(x => x.GetValue(currencyType)).Returns(5.07m);
        _mockServices.Setup(x => x.Update(expense, default)).ReturnsAsync(ResultResponse<Expense>.Success());
        _mockdate.Setup(x => x.SetDateTimeNow()).Returns(DateTime.Now);

        //Act
        var result = await _service.ConvertExpenseCurrencyFromRon(1, currencyType);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(expense.Amount, Is.EqualTo(29.5858));
            Assert.That(expense.Currency, Is.EqualTo(currencyType));
            Assert.IsTrue(result.IsSuccess);
        });
    }
    [Test]
    public async Task ConvertExpenseCurrencyFromRon_WhenCalled_ShoulReturnFailedMessageUpdate()
    {
        //Arrange
        var currencyType = CurrencyType.Euro;

        _mockServices.Setup(x => x.GetExpenseById(1, default)).ReturnsAsync(new Expense());
        _mockProvider.Setup(x => x.GetValue(currencyType)).Returns(5.07m);
        _mockServices.Setup(x => x.Update(It.IsAny<Expense>(), default)).ReturnsAsync(ResultResponse<Expense>.Failure("Sorry! Something went wrong!", ErrorType.ErrorValidation));
        _mockdate.Setup(x => x.SetDateTimeNow()).Returns(DateTime.Now);

        //Act
        var result = await _service.ConvertExpenseCurrencyFromRon(1, currencyType);

        //Assert{
        Assert.IsFalse(result.IsSuccess);
        Assert.That(result.Message, Is.EqualTo("Sorry! Something went wrong!"));
    }

    [Test]
    public async Task ConvertExpenseCurrencyFromRon_WhenCalled_ReturnFailedMessage()
    {
        //Arrange
        var currencyType = CurrencyType.Euro;

        _mockServices.Setup(x => x.GetExpenseById(1, default)).ReturnsAsync((Expense?)null);

        //Act
        var result = await _service.ConvertExpenseCurrencyFromRon(1, currencyType);

        //Assert
        Assert.False(result.IsSuccess);
        Assert.That(result.Message, Is.EqualTo($"The expense with id {1} does not exist!"));
        _mockServices.Verify(x => x.Update(It.IsAny<Expense>(), default), Times.Never);
    }
    #endregion

    #region ConvertExpenseCurrencyToRon

    [Test]
    public async Task ConvertExpenseCurrencyToRon_WhenCalled_ChangeExpenseCurrency()
    {
        //Arrange
        var expense = new Expense()
        {
            Title = "Test",
            Description = "Description Test",
            Amount = 150,
            ExpenseType = ExpenseType.MarketingExpenses,
            CreatedExpense = DateTime.Now,
            Currency = CurrencyType.Euro,
            BaseCurrency = CurrencyType.Ron
        };

        var currencyType = CurrencyType.Euro;

        _mockServices.Setup(x => x.GetExpenseById(1, default)).ReturnsAsync(expense);
        _mockProvider.Setup(x => x.GetValue(currencyType)).Returns(5.07m);
        _mockServices.Setup(x => x.Update(expense, default)).ReturnsAsync(ResultResponse<Expense>.Success());
        _mockdate.Setup(x => x.SetDateTimeNow()).Returns(DateTime.Now);

        //Act
        var result = await _service.ConvertExpenseCurrencyToRon(1);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(expense.Amount, Is.EqualTo(760.5));
            Assert.That(expense.Currency, Is.EqualTo(CurrencyType.Ron));
            Assert.IsTrue(result.IsSuccess);
        });
    }

    [Test]
    public async Task ConvertExpenseCurrencyToRon_WhenCalled_ShoulReturnFailedMessageUpdate()
    {
        //Arrange
        _mockServices.Setup(x => x.GetExpenseById(1, default)).ReturnsAsync(new Expense());
        _mockProvider.Setup(x => x.GetValue(CurrencyType.Euro)).Returns(5.07m);
        _mockServices.Setup(x => x.Update(It.IsAny<Expense>(), default)).ReturnsAsync(ResultResponse<Expense>.Failure("Sorry! Something went wrong!", ErrorType.ErrorValidation));
        _mockdate.Setup(x => x.SetDateTimeNow()).Returns(DateTime.Now);

        //Act
        var result = await _service.ConvertExpenseCurrencyToRon(1);

        //Assert{
        Assert.IsFalse(result.IsSuccess);
        Assert.That(result.Message, Is.EqualTo("Sorry! Something went wrong!"));
    }

    [Test]
    public async Task ConvertExpenseCurrencyToRon_WhenCalled_ReturnFailedMessage()
    {
        //Arrange
        _mockServices.Setup(x => x.GetExpenseById(1, default)).ReturnsAsync((Expense?)null);

        //Act
        var result = await _service.ConvertExpenseCurrencyToRon(1);

        //Assert
        Assert.False(result.IsSuccess);
        Assert.That(result.Message, Is.EqualTo($"The expense with id {1} does not exist!"));
        _mockServices.Verify(x => x.Update(It.IsAny<Expense>(), default), Times.Never);
    }
    #endregion
}
