using ExpenseTracker.Api.Data;
using ExpenseTracker.Api.Dtos;
using ExpenseTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Services;

public class ExpenseService
{
    private readonly ExpenseDbContext _db;

    public ExpenseService(ExpenseDbContext db)
    {
        _db = db;
    }

    public async Task<List<ExpenseDto>> GetAllExpensesAsync()
    {
        return await _db.Expenses
            .Select(expense => new ExpenseDto
            {
                Id = expense.Id,
                Title = expense.Title,
                Amount = expense.Amount,
                Date = expense.Date
            })
            .ToListAsync();
    }

    public async Task<ExpenseDto?> GetExpenseByIdAsync(int id)
    {
        var expense = await _db.Expenses.FindAsync(id);

        if (expense is null)
        {
            return null;
        }

        return ToDto(expense);
    }

    public async Task<(bool Success, string? ErrorMessage, ExpenseDto? Expense)> CreateExpenseAsync(CreateExpenseDto dto)
    {
        var validationError = ValidateExpense(dto.Title, dto.Amount);

        if (validationError is not null)
        {
            return (false, validationError, null);
        }

        var expense = new Expense
        {
            Title = dto.Title,
            Amount = dto.Amount,
            Date = dto.Date
        };

        _db.Expenses.Add(expense);
        await _db.SaveChangesAsync();

        return (true, null, ToDto(expense));
    }

    public async Task<(bool Success, string? ErrorMessage, ExpenseDto? Expense)> UpdateExpenseAsync(int id, UpdateExpenseDto dto)
    {
        var expense = await _db.Expenses.FindAsync(id);

        if (expense is null)
        {
            return (false, "Expense not found.", null);
        }

        var validationError = ValidateExpense(dto.Title, dto.Amount);

        if (validationError is not null)
        {
            return (false, validationError, null);
        }

        expense.Title = dto.Title;
        expense.Amount = dto.Amount;
        expense.Date = dto.Date;

        await _db.SaveChangesAsync();

        return (true, null, ToDto(expense));
    }

    public async Task<bool> DeleteExpenseAsync(int id)
    {
        var expense = await _db.Expenses.FindAsync(id);

        if (expense is null)
        {
            return false;
        }

        _db.Expenses.Remove(expense);
        await _db.SaveChangesAsync();

        return true;
    }
// Deletes every expense from the database. 
    public async Task DeleteAllExpensesAsync()
{
    await _db.Expenses.ExecuteDeleteAsync();
}

    private static string? ValidateExpense(string title, decimal amount)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return "Title is required.";
        }

        if (amount <= 0)
        {
            return "Amount must be greater than zero.";
        }

        return null;
    }

    private static ExpenseDto ToDto(Expense expense)
    {
        return new ExpenseDto
        {
            Id = expense.Id,
            Title = expense.Title,
            Amount = expense.Amount,
            Date = expense.Date
        };
    }

    public async Task<List<ExpenseDto>> SeedExpensesAsync()
{
    var seedExpenses = new List<Expense>
    {
new()
{
    Title = "Costco",
    Amount = 186.42m,
    Date = new DateOnly(2026, 7, 1)
},
new()
{
    Title = "Safeway",
    Amount = 72.35m,
    Date = new DateOnly(2026, 7, 2)
},
new()
{
    Title = "Chevron",
    Amount = 58.17m,
    Date = new DateOnly(2026, 7, 3)
},
new()
{
    Title = "Starbucks",
    Amount = 8.95m,
    Date = new DateOnly(2026, 7, 4)
},
new()
{
    Title = "Fred Meyer",
    Amount = 94.61m,
    Date = new DateOnly(2026, 7, 5)
},
new()
{
    Title = "Target",
    Amount = 63.28m,
    Date = new DateOnly(2026, 7, 6)
},
new()
{
    Title = "Chipotle",
    Amount = 24.83m,
    Date = new DateOnly(2026, 7, 7)
},
new()
{
    Title = "Home Depot",
    Amount = 48.57m,
    Date = new DateOnly(2026, 7, 8)
},
new()
{
    Title = "Trader Joe's",
    Amount = 57.44m,
    Date = new DateOnly(2026, 7, 9)
},
new()
{
    Title = "Shell",
    Amount = 61.73m,
    Date = new DateOnly(2026, 7, 10)
},
new()
{
    Title = "Walmart",
    Amount = 118.09m,
    Date = new DateOnly(2026, 7, 11)
},
new()
{
    Title = "MOD Pizza",
    Amount = 29.87m,
    Date = new DateOnly(2026, 7, 12)
},
new()
{
    Title = "WinCo Foods",
    Amount = 143.76m,
    Date = new DateOnly(2026, 7, 13)
},
new()
{
    Title = "McDonald's",
    Amount = 13.42m,
    Date = new DateOnly(2026, 7, 14)
},
new()
{
    Title = "REI",
    Amount = 89.99m,
    Date = new DateOnly(2026, 7, 15)
},
new()
{
    Title = "QFC",
    Amount = 66.14m,
    Date = new DateOnly(2026, 7, 16)
},
new()
{
    Title = "Panda Express",
    Amount = 22.71m,
    Date = new DateOnly(2026, 7, 17)
},
new()
{
    Title = "Lowe's",
    Amount = 41.56m,
    Date = new DateOnly(2026, 7, 18)
},
new()
{
    Title = "Safeway",
    Amount = 81.93m,
    Date = new DateOnly(2026, 7, 19)
},
new()
{
    Title = "Costco Gas",
    Amount = 54.88m,
    Date = new DateOnly(2026, 7, 20)
},
    };

    _db.Expenses.AddRange(seedExpenses);
    await _db.SaveChangesAsync();

    return seedExpenses
        .Select(expense => new ExpenseDto
        {
            Id = expense.Id,
            Title = expense.Title,
            Amount = expense.Amount,
            Date = expense.Date
        })
        .ToList();
}
}