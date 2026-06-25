using ExpenseTracker.Api.Data;
using ExpenseTracker.Api.Dtos;
using ExpenseTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Endpoints;

public static class ExpenseEndpoints
{
    public static void MapExpenseEndpoints(this WebApplication app)
    {
        var v1 = app.MapGroup("/api/v1/expenses");

        // get all
        v1.MapGet("/", async (ExpenseDbContext db) =>
        {
            return await db.Expenses
                .Select(expense => new ExpenseDto
                {
                    Id = expense.Id,
                    Title = expense.Title,
                    Amount = expense.Amount,
                    Date = expense.Date
                })
                //runs sql query
                .ToListAsync();
        });

        // get by id
        v1.MapGet("/{id}", async (int id, ExpenseDbContext db) =>
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
        v1.MapPost("/", async (CreateExpenseDto dto, ExpenseDbContext db) =>
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
            return Results.BadRequest("Title is required.");
            }

            if (dto.Amount <= 0)
            {
            return Results.BadRequest("Amount must be greater than zero.");
            }
            var expense = new Expense
            {
                Title = dto.Title,
                Amount = dto.Amount,
                Date = dto.Date
            };

            db.Expenses.Add(expense);
            await db.SaveChangesAsync();

            return Results.Created($"/api/v1/expenses/{expense.Id}", new ExpenseDto
            {
                Id = expense.Id,
                Title = expense.Title,
                Amount = expense.Amount,
                Date = expense.Date
            });
        });

        // update
        v1.MapPut("/{id}", async (int id, UpdateExpenseDto dto, ExpenseDbContext db) =>
        {
            var expense = await db.Expenses.FindAsync(id);

            if (expense is null)
            {
                return Results.NotFound();
            }
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
            return Results.BadRequest("Title is required.");
            }

            if (dto.Amount <= 0)
            {
            return Results.BadRequest("Amount must be greater than zero.");
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

        // delete
        v1.MapDelete("/{id}", async (int id, ExpenseDbContext db) =>
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
    }
}