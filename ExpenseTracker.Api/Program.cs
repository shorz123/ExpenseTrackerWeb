using ExpenseTracker.Api.Data;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Api.Models;

//create app config object
var builder = WebApplication.CreateBuilder(args);

//Gather API endpoint information for Swagger 
builder.Services.AddEndpointsApiExplorer();

//Add Swagger services
builder.Services.AddSwaggerGen();

// Register database context with SQL Server
builder.Services.AddDbContext<ExpenseDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ExpenseDatabase")));

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

//get all
app.MapGet("/expenses", async (ExpenseDbContext db) =>
{
    return await db.Expenses.ToListAsync();
});

//get by id 
app.MapGet("/expenses/{id}", async (int id, ExpenseDbContext db) =>
{
    var expense = await db.Expenses.FindAsync(id);

    return expense is not null
        ? Results.Ok(expense)
        : Results.NotFound();
});


//post
app.MapPost("/expenses", async (Expense expense, ExpenseDbContext db) =>
{
    db.Expenses.Add(expense);
    await db.SaveChangesAsync();

    return Results.Created($"/expenses/{expense.Id}", expense);
});

//update
app.MapPut("/expenses/{id}", async (int id, Expense updatedExpense, ExpenseDbContext db) =>
{
    var expense = await db.Expenses.FindAsync(id);

    if (expense is null)
    {
        return Results.NotFound();
    }

    expense.Title = updatedExpense.Title;
    expense.Amount = updatedExpense.Amount;
    expense.Date = updatedExpense.Date;

    await db.SaveChangesAsync();

    return Results.Ok(expense);
});


//delete
app.MapDelete("/expenses/{id}", async (int id, ExpenseDbContext db) =>
{
    var expense = await db.Expenses.FindAsync(id);

    if (expense is null)
    {
        return Results.NotFound();
    }

    db.Expenses.Remove(expense);
    await db.SaveChangesAsync();

    return Results.NoContent();
});



//Start app
app.Run();

