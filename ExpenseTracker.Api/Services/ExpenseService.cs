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
}