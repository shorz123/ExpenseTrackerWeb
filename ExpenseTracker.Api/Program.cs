using ExpenseTrackerWeb.Api.Data;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Api.Models;

//create app config object
var builder = WebApplication.CreateBuilder(args);

//Gather API endpoint information for Swagger 
builder.Services.AddEndpointsApiExplorer();

//Add Swagger services
builder.Services.AddSwaggerGen();

//build application
var app = builder.Build();

//sEnable Swagger in dev emv
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirect HTTP to HTTPS
app.UseHttpsRedirection();


var expenses = new List<Expense>
{
    new Expense
    {
        Id = 1,
        Title = "Gas",
        Amount = 50.00m,
        Date = DateTime.Today
    },
    new Expense
    {
        Id = 2,
        Title = "Groceries",
        Amount = 120.00m,
        Date = DateTime.Today
    }
};


app.MapGet("/expenses", () =>
{
    return expenses;
});


app.MapGet("/expenses/{id}", (int id) =>
{
    var expense = expenses.FirstOrDefault(e => e.Id == id);

    if (expense == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(expense);
});

app.MapPost("/expenses", (Expense expense) =>
{
    expenses.Add(expense);

    return Results.Created($"/expenses/{expense.Id}", expense);
});

app.MapDelete("/expenses/{id}", (int id) =>
{
    var expense = expenses.FirstOrDefault(e => e.Id == id);

    if (expense == null)
    {
        return Results.NotFound();
    }

    expenses.Remove(expense);

    return Results.NoContent();
});

app.MapPut("/expenses/{id}", (int id, Expense updatedExpense) =>
{
    var expense = expenses.FirstOrDefault(e => e.Id == id);

    if (expense == null)
    {
        return Results.NotFound();
    }

    expense.Title = updatedExpense.Title;
    expense.Amount = updatedExpense.Amount;
    expense.Date = updatedExpense.Date;

    return Results.Ok(expense);
});



//Start app
app.Run();

