using ExpenseTracker.Api.Data;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Api.Models;
using ExpenseTracker.Api.Dtos;

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

//allow cors
        builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

//build application
var app = builder.Build();

//allow cors
app.UseCors("AllowReactApp");

//sEnable Swagger in dev emv
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirect HTTP to HTTPS
app.UseHttpsRedirection();

// get all
app.MapGet("/expenses", async (ExpenseDbContext db) =>
{
    return await db.Expenses
        .Select(expense => new ExpenseDto
        {
            Id = expense.Id,
            Title = expense.Title,
            Amount = expense.Amount,
            Date = expense.Date
        })
        .ToListAsync();
});

// get by id
app.MapGet("/expenses/{id}", async (int id, ExpenseDbContext db) =>
{
    var expense = await db.Expenses.FindAsync(id);

    return expense is not null
        ? Results.Ok(new ExpenseDto
        {
            Id = expense.Id,
            Title = expense.Title,
            Amount = expense.Amount,
            Date = expense.Date
        })
        : Results.NotFound();
});

// post
app.MapPost("/expenses", async (CreateExpenseDto dto, ExpenseDbContext db) =>
{
    var expense = new Expense
    {
        Title = dto.Title,
        Amount = dto.Amount,
        Date = dto.Date
    };

    db.Expenses.Add(expense);
    await db.SaveChangesAsync();

    return Results.Created($"/expenses/{expense.Id}", new ExpenseDto
    {
        Id = expense.Id,
        Title = expense.Title,
        Amount = expense.Amount,
        Date = expense.Date
    });
});

// update
app.MapPut("/expenses/{id}", async (int id, UpdateExpenseDto dto, ExpenseDbContext db) =>
{
    var expense = await db.Expenses.FindAsync(id);

    if (expense is null)
    {
        return Results.NotFound();
    }

    expense.Title = dto.Title;
    expense.Amount = dto.Amount;
    expense.Date = dto.Date;

    await db.SaveChangesAsync();

    return Results.Ok(new ExpenseDto
    {
        Id = expense.Id,
        Title = expense.Title,
        Amount = expense.Amount,
        Date = expense.Date
    });
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

