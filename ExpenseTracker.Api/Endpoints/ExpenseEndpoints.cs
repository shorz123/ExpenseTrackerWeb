using ExpenseTracker.Api.Dtos;
using ExpenseTracker.Api.Services;

namespace ExpenseTracker.Api.Endpoints;

public static class ExpenseEndpoints
{
    public static void MapExpenseEndpoints(this WebApplication app)
    {
        var v1 = app.MapGroup("/api/v1/expenses");

        // get all
        v1.MapGet("/", async (ExpenseService service) =>
        {
            var expenses = await service.GetAllExpensesAsync();

            return Results.Ok(expenses);
        });

        // get by id
        v1.MapGet("/{id:int}", async (int id, ExpenseService service) =>
        {
            var expense = await service.GetExpenseByIdAsync(id);

            return expense is not null
                ? Results.Ok(expense)
                : Results.NotFound();
        });

        // post
        v1.MapPost("/", async (
            CreateExpenseDto dto,
            ExpenseService service) =>
        {
            var result = await service.CreateExpenseAsync(dto);

            if (!result.Success)
            {
                return Results.BadRequest(result.ErrorMessage);
            }

            return Results.Created(
                $"/api/v1/expenses/{result.Expense!.Id}",
                result.Expense);
        })
        .RequireRateLimiting("write-policy");

        // update
        v1.MapPut("/{id:int}", async (
            int id,
            UpdateExpenseDto dto,
            ExpenseService service) =>
        {
            var result = await service.UpdateExpenseAsync(id, dto);

            if (!result.Success)
            {
                return result.ErrorMessage == "Expense not found."
                    ? Results.NotFound()
                    : Results.BadRequest(result.ErrorMessage);
            }

            return Results.Ok(result.Expense);
        })
        .RequireRateLimiting("write-policy");

        // add demo expenses
        v1.MapPost("/seed", async (ExpenseService service) =>
        {
            var expenses = await service.SeedExpensesAsync();

            return Results.Ok(expenses);
        })
        .RequireRateLimiting("write-policy");

        // delete one expense
        v1.MapDelete("/{id:int}", async (
            int id,
            ExpenseService service) =>
        {
            var deleted = await service.DeleteExpenseAsync(id);

            return deleted
                ? Results.NoContent()
                : Results.NotFound();
        })
        .RequireRateLimiting("write-policy");

        // delete every expense
        v1.MapDelete("/all", async (ExpenseService service) =>
        {
            await service.DeleteAllExpensesAsync();

            return Results.NoContent();
        })
        .RequireRateLimiting("write-policy");
    }
}