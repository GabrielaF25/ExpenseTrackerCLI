using ExpenseTrackerApi.Profiles;
using ExpenseTrackerApi.Services;
using ExpenseTrackerCLI.Entities;
using ExpenseTrackerCLI.ExchangeRate;
using ExpenseTrackerCLI.ExpensesDatabase;
using ExpenseTrackerCLI.Repositories;
using ExpenseTrackerCLI.Services;
using ExpenseTrackerCLI.ValidateEntities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ExpensesDB>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("ExpensesDB"))
);
builder.Services.AddScoped<IExpensesRepository, ExpensesRepository>();
builder.Services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
builder.Services.AddScoped<IExpensesServices, ExpensesServices>();
builder.Services.AddScoped<IExpenseServiceApi, ExpenseServiceApi>();
builder.Services.AddScoped<IValidator<Expense>, ValidateExpenses>();
builder.Services.AddAutoMapper(typeof(ExpenseProfile).Assembly);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
